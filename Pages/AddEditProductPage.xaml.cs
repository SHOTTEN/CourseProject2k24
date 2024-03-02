using DishesApplication.Tools;
using System;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для AddProductPage.xaml
	/// </summary>
	public partial class AddEditProductPage : Page
	{
		private Products _currentProduct = new Products();
		private string _logotypePath => AddResizingImg._logotypePath;
		public AddEditProductPage(Products selectedProduct)
		{
			InitializeComponent();
			cbProductCategory.ItemsSource = DishesApplicationDB.GetAllCategoryProducts();
			cbManufacturer.ItemsSource = DishesApplicationDB.GetAllManufacturers();
			cbProvider.ItemsSource = DishesApplicationDB.GetAllCategoryProviders();
			cbProductCategory.SelectedIndex = 0;
			cbManufacturer.SelectedIndex = 0;
			cbProvider.SelectedIndex = 0;

			if (selectedProduct != null)
			{
				_currentProduct = selectedProduct;
			}
			DataContext = _currentProduct;
		}

		private void btnAdd_Image(object sender, RoutedEventArgs e)
		{
			addProductImg.Source = AddResizingImg.AddImg(addProductImg.Source);
		}

		private void btnSave_Product(object sender, RoutedEventArgs e)
		{
			StringBuilder errors = Validation(out Products product);
			if (errors.Length == 0)
			{
				try
				{
					using (var context = new DishesApplicationDBEntities())
					{
						Products findedProduct = context.Products.Find(_currentProduct.ProductArticleNumber);

						if (findedProduct == null)
						{
							Products newProduct = new Products
							{
								ProductArticleNumber = product.ProductArticleNumber,
								ProductName = product.ProductName,
								ProductDescription = product.ProductDescription,
								ProductCategoryId = (cbProductCategory.SelectedItem as CategoryProducts).CategoryId,
								ManufacturerId = (cbManufacturer.SelectedItem as Manufacturers).ManufacturerId,
								ProviderId = (cbProvider.SelectedItem as Providers).ProviderId,
								ProductCost = product.ProductCost,
								ProductQuantityInStock = product.ProductQuantityInStock,
								ProductDiscountAmount = product.ProductDiscountAmount,
								MaxDiscount = product.MaxDiscount,
								CurrentDiscount = product.CurrentDiscount,
								ProductPhoto = $"imgProducts\\{_logotypePath}"
							};
							context.Products.Add(newProduct);
							context.SaveChanges();
							MessageBox.Show("Успешно сохранено");
							NavigationService.GoBack();
						}
						else
						{
							if (findedProduct.ProductArticleNumber != tbProductArticleNumber.Text)
							{
								findedProduct.ProductArticleNumber = tbProductArticleNumber.Text;
							}
							findedProduct.ProductName = tbProductName.Text;
							findedProduct.ProductDescription = tbProductDescription.Text;
							findedProduct.ProductCategoryId = (cbProductCategory.SelectedItem as CategoryProducts).CategoryId;
							findedProduct.ManufacturerId = (cbManufacturer.SelectedItem as Manufacturers).ManufacturerId;
							findedProduct.ProviderId = (cbProvider.SelectedItem as Providers).ProviderId;
							findedProduct.ProductCost = Decimal.Parse(tbProductCost.Text, CultureInfo.InvariantCulture);
							findedProduct.ProductQuantityInStock = Convert.ToInt32(tbProductQuantityInStock.Text);
							findedProduct.ProductDiscountAmount = Convert.ToDecimal(tbProductDiscountAmount.Text, CultureInfo.InvariantCulture);
							findedProduct.MaxDiscount = Convert.ToDecimal(tbMaxDiscount.Text, CultureInfo.InvariantCulture);
							findedProduct.CurrentDiscount = Convert.ToDecimal(tbCurrentDiscount.Text, CultureInfo.InvariantCulture);
							findedProduct.ProductPhoto = $"imgProducts\\{_logotypePath}";
							context.SaveChanges();
							MessageBox.Show("Успешно изменено");
							NavigationService.GoBack();
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			else
			{
				MessageBox.Show($"При заполнении товара " +
				$"произошли следующие проблемы:\n\n{errors}", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Вы точно хотите вернуться?\nВнесённые данные будут утеряны", "Назад",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
			{
				NavigationService.GoBack();
			}
		}

		private StringBuilder Validation(out Products product)
		{
			product = null;
			StringBuilder errors = new StringBuilder();
			if (string.IsNullOrEmpty(tbProductName.Text))
				errors.AppendLine("Вы не указали наименование продукта");
			if (string.IsNullOrEmpty(tbProductArticleNumber.Text))
				errors.AppendLine("Вы не указали артикул продукта");
			if (string.IsNullOrEmpty(tbProductCost.Text))
				errors.AppendLine("Вы не указали цену продукта");
			if (string.IsNullOrEmpty(tbProductQuantityInStock.Text))
				errors.AppendLine("Вы не указали остаток продукта на складе");

			if (tbProductArticleNumber.Text.Length != 6)
				errors.AppendLine("Артикул товара должен иметь 6 символов");

			bool isProductCostParseSuccess = Decimal.TryParse(tbProductCost.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedProductCost);
			if (!isProductCostParseSuccess)
			{
				errors.AppendLine("Некоректное значение числа");
				return errors;
			}

			if (parsedProductCost <= 0)
				errors.AppendLine("Цена не может быть меньше или равной 0");

			bool isProductQuantityInStockParseSuccess = Int32.TryParse(tbProductQuantityInStock.Text, out int parsedQuantityInStock);
			if (parsedQuantityInStock < 0)
				errors.AppendLine("Остаток на складе не может быть отрицательным");

			decimal? parsedDiscountAmount = null;
			decimal? parsedMaxDiscount = null;
			decimal? parsedCurrentDiscount = null;

			if (!string.IsNullOrWhiteSpace(tbProductDiscountAmount.Text))
			{
				bool isProductDiscountAmountParseSuccess = Decimal.TryParse(tbProductDiscountAmount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedDiscountAmountValue);
				if (isProductDiscountAmountParseSuccess)
				{
					parsedDiscountAmount = parsedDiscountAmountValue;
				}
				else
				{
					errors.AppendLine("Некоректное значение скидки");
					return errors;
				}

				if (parsedDiscountAmount < 0 || parsedDiscountAmount >= 99)
				{
					errors.AppendLine("Скидка не может быть отрицательной и не может превышать 99%");
				}
			}

			if (!string.IsNullOrWhiteSpace(tbMaxDiscount.Text))
			{
				bool isMaxDiscountParseSuccess = Decimal.TryParse(tbMaxDiscount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedMaxDiscountValue);
				if (isMaxDiscountParseSuccess)
				{
					parsedMaxDiscount = parsedMaxDiscountValue;
				}
				else
				{
					errors.AppendLine("Некоректное значение максимальной скидки");
					return errors;
				}

				if (parsedMaxDiscount < 0 || parsedMaxDiscount >= 99)
				{
					errors.AppendLine("Максимальная скидка не может быть отрицательной и не может превышать 99%");
				}
			}

			if (!string.IsNullOrWhiteSpace(tbCurrentDiscount.Text))
			{
				bool isCurrentDiscountParseSuccess = Decimal.TryParse(tbCurrentDiscount.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedCurrentDiscountValue);
				if (isCurrentDiscountParseSuccess)
				{
					parsedCurrentDiscount = parsedCurrentDiscountValue;
				}
				else
				{
					errors.AppendLine("Некоректное значение текущей скидки");
					return errors;
				}

				if (parsedCurrentDiscount < 0 || parsedCurrentDiscount >= 99)
				{
					errors.AppendLine("Текущая скидка не может быть отрицательной и не может превышать 99%");
				}
			}

				product = new Products
				{
					ProductArticleNumber = tbProductArticleNumber.Text,
					ProductName = tbProductName.Text,
					ProductDescription = tbProductDescription.Text,
					ProductCategoryId = (cbProductCategory.SelectedItem as CategoryProducts).CategoryId,
					ManufacturerId = (cbManufacturer.SelectedItem as Manufacturers).ManufacturerId,
					ProviderId = (cbProvider.SelectedItem as Providers).ProviderId,
					ProductCost = parsedProductCost,
					ProductQuantityInStock = parsedQuantityInStock,
					ProductDiscountAmount = parsedDiscountAmount,
					MaxDiscount = parsedMaxDiscount,
					CurrentDiscount = parsedCurrentDiscount
				};
			return errors;
		}

		private void tbProductCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (e.Text == ",")
			{
				e.Handled = true;
			}
		}
	}
}
