using DishesApplication.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DishesApplication.Tools
{
	public static class DishesApplicationDB
	{
		public static void SetProductsDataToListView(ListView listView)
		{
			var data = GetAllProducts();
			listView.ItemsSource = data;
		}

		public static void SetOrdersDataToListView(ListView listView)
		{
			var data = GetAllOrders();
			listView.ItemsSource = data;
		}

		public static List<Products> GetAllProducts()
		{
			
			return DishesApplicationDBEntities.GetContext().Products.AsNoTracking().ToList();
		}

		public static List<Orders> GetAllOrders()
		{
			return DishesApplicationDBEntities.GetContext().Orders.AsNoTracking().ToList();
		}

		public static List<Manufacturers> GetAllManufacturers()
		{
			return DishesApplicationDBEntities.GetContext().Manufacturers.AsNoTracking().ToList();
		}

		public static List<CategoryProducts> GetAllCategoryProducts()
		{
			return DishesApplicationDBEntities.GetContext().CategoryProducts.AsNoTracking().ToList();
		}

		public static List<Providers> GetAllCategoryProviders()
		{
			return DishesApplicationDBEntities.GetContext().Providers.AsNoTracking().ToList();
		}
		public static List<PickupPointAddresses> GetAllPickupPointAddresses()
		{
			return DishesApplicationDBEntities.GetContext().PickupPointAddresses.AsNoTracking().ToList();
		}

		public static Int32 GetLastOrderCode()
		{
			List<Orders> orders = GetAllOrders();
			Orders lastOrder = orders.LastOrDefault();

			if (lastOrder != null)
			{
				return lastOrder.Code;
			}

			return 0;
		}

		public static List<OrderProducts> GetAllOrderProducts(Orders selectedOrder)
		{
			return (from orderProduct in DishesApplicationDBEntities.GetContext().OrderProducts
						 where orderProduct.OrderId == selectedOrder.OrderId
					select orderProduct).ToList();
		}

		public static void FillComboBoxFilter(ComboBox comboBox)
		{
			var allManufacturers = DishesApplicationDBEntities.GetContext().Manufacturers.ToList();
			allManufacturers.Insert(0, new Manufacturers { Name = "Все производители" });
			comboBox.ItemsSource = allManufacturers;
			comboBox.SelectedIndex = 0;
		}

		public static void FillComboBoxSorting(ComboBox comboBox)
		{
			comboBox.Items.Add("По умолчанию");
			comboBox.Items.Add("Цена по убыванию");
			comboBox.Items.Add("Цена по возрастанию");
			comboBox.SelectedIndex = 0;
		}

		public static void UpdateProducts(ComboBox cbFilter, ComboBox cbSort, 
			TextBox tbPoisk, TextBlock outputQuantityProducts, TextBlock allQuantityProducts,
			ProductsPageViewModel viewModel
		)			
		{
			int countProductsQuantity = 0;
			var currentProducts = GetAllProducts();
			int totalProductsQuantity = currentProducts.Count();

			if (cbFilter.SelectedIndex > 0)
			{
				Manufacturers manufacturers = cbFilter.SelectedItem as Manufacturers;
				string filter = manufacturers.Name.ToString();
				currentProducts = currentProducts.Where(p => p.Manufacturers.Name.Contains(filter)).ToList();
				countProductsQuantity = currentProducts.Count();
			}
			else currentProducts = GetAllProducts();
			switch (cbSort.SelectedIndex)
			{
				case 1:
					currentProducts = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
					break;
				case 2:
					currentProducts = currentProducts.OrderBy(p => p.ProductCost).ToList();
					break;
			}
			if (tbPoisk.Text != "")
				currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(tbPoisk.Text.ToLower()) ||
				p.ProductDescription.ToLower().Contains(tbPoisk.Text.ToLower()) || p.Manufacturers.Name.ToLower().Contains(tbPoisk.Text.ToLower()) ||
				p.ProductCost.ToString().Contains(tbPoisk.Text)).ToList();

			countProductsQuantity = currentProducts.Count();
			outputQuantityProducts.Text = $"Найдено товаров: {countProductsQuantity}";
			allQuantityProducts.Text = $" / {totalProductsQuantity} шт";
			viewModel.Products = currentProducts;
		}
	}
}
