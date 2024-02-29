﻿using DishesApplication.Tools;
using System;
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

		private void btnAddProductPage(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new AddProductPage(null));
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
	}
}
