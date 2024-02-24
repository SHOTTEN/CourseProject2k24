﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
			var currentProducts = DishesApplicationDBEntities.GetContext().Products.ToList();
			lvProducts.ItemsSource = currentProducts;
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
