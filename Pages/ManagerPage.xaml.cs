using DishesApplication.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для ManagerPage.xaml
	/// </summary>
	public partial class ManagerPage : Page
	{
		public ManagerPage()
		{
			InitializeComponent();
			DishesApplicationDB.SetDataToListView(lvProducts);
			DishesApplicationDB.FillComboBoxFilter(cbFilter);
			DishesApplicationDB.FillComboBoxSorting(cbSort);
		}

		private void btnAddBasket(object sender, RoutedEventArgs e)
		{

        }

		private void btnBasket(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new BasketPage(null));
		}

		private void btnOrders(object sender, RoutedEventArgs e)
		{

		}
		private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, lvProducts);
		}

		private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, lvProducts);
		}

		private void tbPoisk_TextChanged(object sender, TextChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, lvProducts);
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();
			window.Show();
			Window parentWindow = Window.GetWindow(this);
			Storage.SystemUser = null;
			parentWindow.Close();
		}
	}
}
