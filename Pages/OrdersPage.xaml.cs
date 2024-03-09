using DishesApplication.Tools;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

			Paragraph title = new Paragraph("Заказы клиентов", titleBoldFont);
			title.Alignment = Element.ALIGN_CENTER; // Выравнивание по центру
			title.SpacingAfter = 30f; // Расстояние после заголовка
			document.Add(title);

			List<Orders> orders = DishesApplicationDB.GetAllOrders();

			PdfPTable table = new PdfPTable(7);
			table.SetWidthPercentage(new float[] { 50, 100, 90, 90, 80, 50, 100 }, PageSize.A4);
			table.LockedWidth = true;
			table.DefaultCell.BorderWidth = 1;
			table.AddCell(new Phrase("№ Заказа", boldFont));
			table.AddCell(new Phrase("Данные клиента", boldFont));
			table.AddCell(new Phrase("Дата заказа", boldFont));
			table.AddCell(new Phrase("Дата доставки", boldFont));
			table.AddCell(new Phrase("Статус", boldFont));
			table.AddCell(new Phrase("Код выдачи", boldFont));
			table.AddCell(new Phrase("Пункт выдачи", boldFont));

			foreach (Orders item in orders)
			{
				Orders order = orders.FirstOrDefault(o => o.OrderId == item.OrderId);
				Users user = DishesApplicationDBEntities.GetContext().Users.FirstOrDefault(u => u.UserId == item.UserId);
				PickupPointAddresses pickupPointAddress = DishesApplicationDBEntities.GetContext().PickupPointAddresses.FirstOrDefault(p => p.PointId == item.OrderPickupPointId);
				StatusOrders statusOrder = DishesApplicationDBEntities.GetContext().StatusOrders.FirstOrDefault(s => s.StatusId == item.OrderStatus);
				
				PdfPCell orderIdCell = new PdfPCell(new Phrase(order.OrderId.ToString(), font));
				orderIdCell.HorizontalAlignment = Element.ALIGN_CENTER;
				orderIdCell.VerticalAlignment = Element.ALIGN_MIDDLE;
				table.AddCell(orderIdCell);
				table.AddCell(new Phrase($"ФИО:\n{user.Fio}\n\nЛогин:\n{user.Login}", font));
				table.AddCell(new Phrase(order.OrderDate.ToString(), font));
				table.AddCell(new Phrase(order.OrderDeliveryDate.ToString(), font));
				table.AddCell(new Phrase(statusOrder.Status, font));
				table.AddCell(new Phrase(order.Code.ToString(), font));
				table.AddCell(new Phrase(pickupPointAddress.PickupPointAddress, font));
			}

			SetCellPadding(table, 2);

			document.Add(table);
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
