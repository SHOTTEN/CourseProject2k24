using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DishesApplication.Tools
{
	public static class DishesApplicationDB
	{
		public static void SetDataToListView(ListView listView)
		{
			var data = GetAllProduts();
			listView.ItemsSource = data;
		}

		public static List<Products> GetAllProduts()
		{
			return DishesApplicationDBEntities.GetContext().Products.ToList();
		}

		public static List<Manufacturers> GetAllManufacturers()
		{
			return DishesApplicationDBEntities.GetContext().Manufacturers.ToList();
		}

		public static List<CategoryProducts> GetAllCategoryProducts()
		{
			return DishesApplicationDBEntities.GetContext().CategoryProducts.ToList();
		}

		public static List<Providers> GetAllCategoryProviders()
		{
			return DishesApplicationDBEntities.GetContext().Providers.ToList();
		}

		public static Int32 GetLastOrderCode()
		{
			List<Orders> orders = DishesApplicationDBEntities.GetContext().Orders.ToList();
			Orders lastOrder = orders.LastOrDefault();

			if (lastOrder != null)
			{
				return lastOrder.Code;
			}

			return 0;
		}

		public static List<PickupPointAddresses> GetAllPickupPointAddresses()
		{
			return DishesApplicationDBEntities.GetContext().PickupPointAddresses.ToList();
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
			ListView lvProducts
		)			
		{
			int countProductsQuantity = 0;
			var currentProducts = GetAllProduts();
			int totalProductsQuantity = currentProducts.Count();

			if (cbFilter.SelectedIndex > 0)
			{
				Manufacturers manufacturers = cbFilter.SelectedItem as Manufacturers;
				string filter = manufacturers.Name.ToString();
				currentProducts = currentProducts.Where(p => p.Manufacturers.Name.Contains(filter)).ToList();
				countProductsQuantity = currentProducts.Count();
			}
			else currentProducts = GetAllProduts();
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
			lvProducts.ItemsSource = currentProducts;
		}
	}
}
