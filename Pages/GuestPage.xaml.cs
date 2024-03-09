using DishesApplication.Tools;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	public partial class GuestPage : Page
	{
		ProductsPageViewModel ViewModel { get; }
		public GuestPage()
		{
			InitializeComponent();
			var allProducts = DishesApplicationDB.GetAllProducts();
			var viewModel = new ProductsPageViewModel(allProducts);

			ViewModel = viewModel;
			DataContext = ViewModel;

			DishesApplicationDB.FillComboBoxFilter(cbFilter);
			DishesApplicationDB.FillComboBoxSorting(cbSort);
		}

		private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, ViewModel);
		}

		private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, ViewModel);
		}

		private void tbPoisk_TextChanged(object sender, TextChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, ViewModel);
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			Window parentWindow = Window.GetWindow(this);
			MainWindow window = new MainWindow();
			parentWindow.Close();
			window.Show();
		}
	}
}
