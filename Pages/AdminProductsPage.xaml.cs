using DishesApplication.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	public class ProductsPageViewModel : INotifyPropertyChanged
	{
		private List<Products> _products = new List<Products>();
		public List<Products> Products
		{
			get { return _products; }
			set
			{
				_products = value;
				OnPropertyChanged();
			}
		}

		private List<Products> _basketProduct = new List<Products>();
		public List<Products> BasketProduct
		{
			get { return _basketProduct; }
			set
			{
				_basketProduct = value;
				OnPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}

		public ProductsPageViewModel(List<Products> products)
		{
			_products = products;
		}
	}

	public partial class AdminProductsPage : Page
	{
		ProductsPageViewModel ViewModel { get; }

		public AdminProductsPage()
		{
			InitializeComponent();
			var allProducts = DishesApplicationDB.GetAllProducts();
			var viewModel = new ProductsPageViewModel(allProducts);

			ViewModel = viewModel;
			DataContext = ViewModel;

			DishesApplicationDB.FillComboBoxFilter(cbFilter);
			DishesApplicationDB.FillComboBoxSorting(cbSort);
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, ViewModel);
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

		private void btnProductAdd(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AddEditProductPage(null));
		}

		private void btnProductDelete(object sender, RoutedEventArgs e)
		{
			Products product = (Products)((Button)sender).DataContext;

			if (MessageBox.Show("Вы точно хотите удалить этот товар?", "Удаление товара",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				try
				{
					using (var context = new DishesApplicationDBEntities())
					{
						string productArticle = product.ProductArticleNumber;

						var orderProducts = context.OrderProducts.Where(op => op.ProductArticleNumber == productArticle);
						if (orderProducts.Count() > 0)
						{
							MessageBox.Show("Произошла ошибка удаления!\n\nНеобходимо менеджеру удалить товары из заказов", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
							return;
						}

						Products productToRemove = context.Products.Find(productArticle);
						context.Products.Remove(productToRemove);

						context.SaveChanges();
						MessageBox.Show("Товар успешно удален!");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

			ViewModel.Products = DishesApplicationDBEntities.GetContext().Products.ToList();

			if (product.ProductPhoto != null && File.Exists(product.LogotipSourse)) File.Delete(product.LogotipSourse);
		}

		private void btnProductEdit(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AddEditProductPage((sender as Button).DataContext as Products));
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
