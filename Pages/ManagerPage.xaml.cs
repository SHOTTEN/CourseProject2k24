using DishesApplication.Tools;
using System.Linq;
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
			int productQuantityInBasket = ViewModel.BasketProduct.Count(p => p.ProductArticleNumber == product.ProductArticleNumber);

			if (product.ProductQuantityInStock == 0)
			{
				MessageBox.Show("Вы не можете добавить закончившйся товар в корзину!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (productQuantityInBasket >= product.ProductQuantityInStock)
			{
				MessageBox.Show("Вы не можете добавить больше товара, чем есть на складе!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
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
			if (MessageBox.Show("Вы точно хотите выйти?\nНесохранённые данные будут утеряны", "Выход",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
			{
				return;
			}

			BaseWindow parentWindow = (BaseWindow)Window.GetWindow(this);
			MainWindow window = new MainWindow();
			parentWindow.ForcedClose();
			window.Show();
			Storage.SystemUser = null;
		}
	}
}
