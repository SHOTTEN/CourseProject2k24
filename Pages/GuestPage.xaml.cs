using DishesApplication.Tools;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для GuestPage.xaml
	/// </summary>
	public partial class GuestPage : Page
	{
		public GuestPage()
		{
			InitializeComponent();
			DishesApplicationDB.SetProductsDataToListView(lvProducts);
			DishesApplicationDB.FillComboBoxFilter(cbFilter);
			DishesApplicationDB.FillComboBoxSorting(cbSort);
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
			parentWindow.Close();
		}
	}
}
