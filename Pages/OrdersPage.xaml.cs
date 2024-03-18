using DishesApplication.Tools;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Font = iTextSharp.text.Font;

namespace DishesApplication.Pages
{
	public partial class OrdersPage : Page
	{
		public OrdersPage()
		{
			InitializeComponent();
			DishesApplicationDB.SetOrdersDataToListView(lvOrders);
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}

		private void btnOrderInfoPage(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new OrderDetailsPage((sender as Button).DataContext as Orders));
		}

		private void btnGeneratePdfFile(object sender, RoutedEventArgs e)
		{
			Document document = new Document();
			PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("./Reports/Отчет_Заказы клиентов.pdf", FileMode.Create));
			document.Open();

			BaseFont baseFont = BaseFont.CreateFont("C:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
			Font font = new Font(baseFont, 12, Font.NORMAL);
			Font boldFont = new Font(baseFont, 12, Font.BOLD);
			Font titleBoldFont = new Font(baseFont, 18, Font.BOLD);

			Paragraph titleOrders = new Paragraph("Заказы клиентов", titleBoldFont);
			titleOrders.Alignment = Element.ALIGN_CENTER; // Выравнивание по центру
			titleOrders.SpacingAfter = 30f; // Расстояние после заголовка
			document.Add(titleOrders);

			List<Orders> orders = DishesApplicationDB.GetAllOrders();

			PdfPTable tableOrders = new PdfPTable(7);
			tableOrders.SetWidthPercentage(new float[] { 50, 100, 90, 90, 80, 50, 100 }, PageSize.A4);
			tableOrders.LockedWidth = true;
			tableOrders.DefaultCell.BorderWidth = 1;

			tableOrders.AddCell(new Phrase("№ Заказа", boldFont));
			tableOrders.AddCell(new Phrase("Данные клиента", boldFont));
			tableOrders.AddCell(new Phrase("Дата заказа", boldFont));
			tableOrders.AddCell(new Phrase("Дата доставки", boldFont));
			tableOrders.AddCell(new Phrase("Статус", boldFont));
			tableOrders.AddCell(new Phrase("Код выдачи", boldFont));
			tableOrders.AddCell(new Phrase("Пункт выдачи", boldFont));

			foreach (Orders item in orders)
			{
				Users user = DishesApplicationDBEntities.GetContext().Users.FirstOrDefault(u => u.UserId == item.UserId);
				PickupPointAddresses pickupPointAddress = DishesApplicationDBEntities.GetContext().PickupPointAddresses.FirstOrDefault(p => p.PointId == item.OrderPickupPointId);
				StatusOrders statusOrder = DishesApplicationDBEntities.GetContext().StatusOrders.FirstOrDefault(s => s.StatusId == item.OrderStatus);

				PdfPCell orderIdCell = new PdfPCell(new Phrase(item.OrderId.ToString(), boldFont));
				orderIdCell.HorizontalAlignment = Element.ALIGN_CENTER;
				orderIdCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				tableOrders.AddCell(orderIdCell);
				tableOrders.AddCell(new Phrase($"ФИО:\n{user.Fio}\n\nЛогин:\n{user.Login}", font));
				tableOrders.AddCell(new Phrase(item.OrderDate.ToString(), font));
				tableOrders.AddCell(new Phrase(item.OrderDeliveryDate.ToString(), font));
				tableOrders.AddCell(new Phrase(statusOrder.Status, font));
				tableOrders.AddCell(new Phrase(item.Code.ToString(), font));
				tableOrders.AddCell(new Phrase(pickupPointAddress.PickupPointAddress, font));
			}

			SetCellPadding(tableOrders, 2);
			document.Add(tableOrders);
			document.NewPage();

			//----------------------------------------------Продукты в заказах клиентов----------------------------------------------------

			Paragraph titleProductsFromOrder = new Paragraph("Продукты в заказах клиентов", titleBoldFont);
			titleProductsFromOrder.Alignment = Element.ALIGN_CENTER; // Выравнивание по центру
			titleProductsFromOrder.SpacingAfter = 30f; // Расстояние после заголовка
			document.Add(titleProductsFromOrder);

			List<OrderProducts> orderProducts = DishesApplicationDB.GetAllOrderProducts();

			PdfPTable tableProducts = new PdfPTable(7);
			tableProducts.SetWidthPercentage(new float[] { 50, 80, 90, 100, 80, 55, 80 }, PageSize.A4);
			tableProducts.LockedWidth = true;
			tableProducts.DefaultCell.BorderWidth = 1;

			tableProducts.AddCell(new Phrase("№ Заказа", boldFont));
			tableProducts.AddCell(new Phrase("Артикул продукта", boldFont));
			tableProducts.AddCell(new Phrase("Название продукта", boldFont));
			tableProducts.AddCell(new Phrase("Производитель", boldFont));
			tableProducts.AddCell(new Phrase("Цена", boldFont));
			tableProducts.AddCell(new Phrase("Остаток на складе", boldFont));
			tableProducts.AddCell(new Phrase("Кол-во шт в заказе", boldFont));

			foreach (OrderProducts item in orderProducts)
			{
				Products product = DishesApplicationDB.GetAllProducts().FirstOrDefault(p => p.ProductArticleNumber == item.ProductArticleNumber);
				Manufacturers manufacturer = DishesApplicationDB.GetAllManufacturers().FirstOrDefault(m => m.ManufacturerId == item.Products.ManufacturerId);

				PdfPCell orderIdCell = new PdfPCell(new Phrase(item.OrderId.ToString(), boldFont));
				orderIdCell.HorizontalAlignment = Element.ALIGN_CENTER;
				orderIdCell.VerticalAlignment = Element.ALIGN_MIDDLE;

				tableProducts.AddCell(orderIdCell);
				tableProducts.AddCell(new Phrase(product.ProductArticleNumber, font));
				tableProducts.AddCell(new Phrase(product.ProductName, font));
				tableProducts.AddCell(new Phrase(manufacturer.Name, font));
				tableProducts.AddCell(new Phrase(product.ProductCost.ToString(), font));
				tableProducts.AddCell(new Phrase(product.ProductQuantityInStock.ToString(), font));
				tableProducts.AddCell(new Phrase(item.CountProduct.ToString(), font));
			}

			SetCellPadding(tableProducts, 2);
			document.Add(tableProducts);
			document.Close();
			MessageBox.Show("PDF файл успешно сохранен!");
		}

		private void SetCellPadding(PdfPTable table, int padding)
		{
			for (int row = 0; row < table.Rows.Count; row++)
			{
				PdfPRow tableRow = table.GetRow(row);

				for (int col = 0; col < tableRow.GetCells().Length; col++)
				{
					PdfPCell cell = tableRow.GetCells()[col];
					cell.Padding = padding;
				}
			}
		}
	}
}
