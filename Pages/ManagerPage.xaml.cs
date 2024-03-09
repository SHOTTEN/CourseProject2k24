using DishesApplication.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DishesApplication.Pages
{
	public partial class ManagerPage : Page
	{
		ProductsPageViewModel ViewModel { get; }
		public ManagerPage()
		{
			InitializeComponent();
			var allProducts = DishesApplicationDB.GetAllProducts();
			var viewModel = new ProductsPageViewModel(allProducts);

			ViewModel = viewModel;
			DataContext = ViewModel;

			DishesApplicationDB.FillComboBoxFilter(cbFilter);
			DishesApplicationDB.FillComboBoxSorting(cbSort);
		}

		private void btnAddProductToBasket(object sender, RoutedEventArgs e)
		{
			Products product = (Products)((Button)sender).DataContext;
			if (product.ProductQuantityInStock == 0)
			{
				MessageBox.Show("Вы не можете добавить закончившйся товар в корзину!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			ViewModel.BasketProduct.Add(product);
		}

		private void btnOpenBasket(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new BasketPage(ViewModel));
		}

		private void btnOrders(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new OrdersPage());
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
			Storage.SystemUser = null;
		}
	}
}
