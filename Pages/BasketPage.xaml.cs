using DishesApplication.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	public class OrderItem
	{
		public Products Product { get; set; }
		public int Count { get; set; }

		public OrderItem(Products products, int count)
		{
			Product = products;
			Count = count;
		}
	}

	public class BasketPageViewModel : INotifyPropertyChanged
	{
		private List<OrderItem> _orderItems = new List<OrderItem>();
		public List<OrderItem> OrderItems
		{
			get { return _orderItems; }
			set
			{
				_orderItems = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

		public BasketPageViewModel(OrderItem[] products)
		{
			_orderItems = products.ToList();
		}
	}

	public partial class BasketPage : Page
	{
		private ProductsPageViewModel _productsViewModel;
		private BasketPageViewModel _basketViewModel;

		public BasketPage(ProductsPageViewModel productsViewModel)
		{
			InitializeComponent();
			_productsViewModel = productsViewModel;
			OrderItem[] orderItems = ConvertToOrderItems(_productsViewModel.BasketProduct.ToArray());

			_basketViewModel = new BasketPageViewModel(orderItems);
			DataContext = _basketViewModel;

			cbPickupPoint.ItemsSource = DishesApplicationDB.GetAllPickupPointAddresses();
		}

		private OrderItem[] ConvertToOrderItems(Products[] products)
		{
			return products.GroupBy(p => p.ProductArticleNumber).Select(group =>
			{
				Int32 count = group.Count();
				var productsGroup = group.Select(g => g).ToArray();
				return new OrderItem(productsGroup.First(), count);
			}).ToArray();
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}

		private void btnDeleteFromBasket(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Вы точно хотите удалить товар из корзины?\nДействие будет невозможно отменить!", "Назад",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				_productsViewModel.BasketProduct.Remove(((OrderItem)((Button)sender).DataContext).Product);
				_basketViewModel.OrderItems = ConvertToOrderItems(_productsViewModel.BasketProduct.ToArray()).ToList();
			}
		}

		private void btnFormOrder(object sender, RoutedEventArgs e)
		{
			if (cbPickupPoint.SelectedIndex == -1)
			{
				MessageBox.Show("Вы не выбрали пункт выдачи товара!", "Внимание");
				return;
			}

			try
			{
				using (var context = new DishesApplicationDBEntities())
				{
					Orders newOrder = new Orders
					{
						OrderStatus = 2,
						OrderDate = DateTime.Now,
						OrderDeliveryDate = DateTime.Now.AddDays(3),
						OrderPickupPointId = (cbPickupPoint.SelectedItem as PickupPointAddresses).PointId,
						UserId = Storage.SystemUser.UserId,
						Code = DishesApplicationDB.GetLastOrderCode() + 1,
					};
					context.Orders.Add(newOrder);
					context.SaveChanges();

					if (_basketViewModel.OrderItems.Count > 0)
					{
						foreach(var orderItem in _basketViewModel.OrderItems)
						{
							OrderProducts newOrderProduct = new OrderProducts
							{
								OrderId = newOrder.OrderId,
								ProductArticleNumber = orderItem.Product.ProductArticleNumber,
								CountProduct = orderItem.Count,
							};

							context.OrderProducts.Add(newOrderProduct);
						}						
						context.SaveChanges();
					}

					MessageBox.Show("Заказ успешно оформлен!");
					_productsViewModel.BasketProduct = new List<Products>();
					NavigationService.GoBack();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}
