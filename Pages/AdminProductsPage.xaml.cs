using DishesApplication.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для AdminProductsPage.xaml
	/// </summary>
	public partial class AdminProductsPage : Page
	{
		private List<Products> _products = new List<Products>();

		public AdminProductsPage()
		{
			InitializeComponent();
			DishesApplicationDB.SetDataToListView(lvProducts);
			DishesApplicationDB.FillComboBoxFilter(cbFilter);
			DishesApplicationDB.FillComboBoxSorting(cbSort);
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
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
						if(orderProducts.Count() > 0)
						{
							MessageBox.Show("Произошла ошибка удаления!\n\nНеобходимо менеджеру удалить товары из заказов", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
							return;
						}

						Products productForRemove = context.Products.Find(productArticle);
						context.Products.Remove(productForRemove);

						context.SaveChanges();
						MessageBox.Show("Записи успешно удалены!");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			lvProducts.ItemsSource = DishesApplicationDBEntities.GetContext().Products.ToList();
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

			_products.Add(product);
		}

		private void btnOpenBasket(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new BasketPage(_products));
		}
	}
}
