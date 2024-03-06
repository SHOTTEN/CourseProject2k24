using DishesApplication.Tools;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для OrdersPage.xaml
	/// </summary>
	public partial class OrdersPage : Page
	{
		public OrdersPage()
		{
			InitializeComponent();
			DishesApplicationDB.SetOrdersDataToListView(lvOrders);
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}

		private void btnOrderInfoPage(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new OrderDetailsPage((sender as Button).DataContext as Orders));
		}
	}
}
