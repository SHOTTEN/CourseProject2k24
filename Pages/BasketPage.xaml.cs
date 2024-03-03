using DishesApplication.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для BasketPage.xaml
	/// </summary>
	public partial class BasketPage : Page
	{
		private List<Products> _products;
		public BasketPage(List<Products> products)
		{
			InitializeComponent();
			_products = products;
			lvProducts.ItemsSource = products;
			cbPickupPoint.ItemsSource = DishesApplicationDB.GetAllPickupPointAddresses();
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Вы точно хотите вернуться?\nТовары в корзине будут утеряны", "Назад",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				NavigationService.GoBack();
			}
		}

		private void btnDeleteFromBasket(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Вы точно хотите удалить товар из корзины?\nДействие будет невозможно отменить!", "Назад",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				_products.Remove((Products)((Button)sender).DataContext);
				lvProducts.ItemsSource = null;
				lvProducts.ItemsSource = _products;
			}
		}

		private void btnFormOrder(object sender, RoutedEventArgs e)
		{
			if(cbPickupPoint.SelectedIndex == -1)
			{
				MessageBox.Show("Вы не выбрали пункт выдачи товара!", "Внимание");
				return;
			}

			try
			{
				using(var context = new DishesApplicationDBEntities())
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

					if (_products.Count > 0)
					{
						foreach (var group in _products.GroupBy(p => p.ProductArticleNumber))
						{
							int count = group.Count();
							OrderProducts newOrderProduct = new OrderProducts
							{
								OrderId = newOrder.OrderId,
								ProductArticleNumber = group.Key,
								CountProduct = count,
							};

							context.OrderProducts.Add(newOrderProduct);
						}
						context.SaveChanges();
					}

					MessageBox.Show("Заказ успешно оформлен!");
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
