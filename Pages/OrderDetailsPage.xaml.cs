using DishesApplication.Tools;
using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для OrderDetailsPage.xaml
	/// </summary>
	public partial class OrderDetailsPage : Page
	{
		private Orders _currentOrder = new Orders();
		public OrderDetailsPage(Orders selectedOrder)
		{
			InitializeComponent();
			DishesApplicationDB.SetProductsDataToListView(lvOrderProducts);
			lvOrderProducts.ItemsSource = DishesApplicationDB.GetAllOrderProducts(selectedOrder);
			if (selectedOrder != null)
			{
				_currentOrder = selectedOrder;
			}
			DataContext = _currentOrder;
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}

		private void btnDeleteFromOrder(object sender, RoutedEventArgs e)
		{
			OrderProducts product = (OrderProducts)((Button)sender).DataContext;

			if (MessageBox.Show("Вы точно хотите удалить этот товар?", "Удаление товара",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				try
				{
					using (var context = new DishesApplicationDBEntities())
					{
						var orderProductToRemove = context.OrderProducts
								.FirstOrDefault(op => op.OrderId == _currentOrder.OrderId && op.ProductArticleNumber == product.ProductArticleNumber);

						context.OrderProducts.Remove(orderProductToRemove);

						context.SaveChanges();
						MessageBox.Show("Товар успешно удален!");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			lvOrderProducts.ItemsSource = null;
			lvOrderProducts.ItemsSource = DishesApplicationDB.GetAllOrderProducts(_currentOrder);
		}
	}
}
