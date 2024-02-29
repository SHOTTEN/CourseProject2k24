using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace DishesApplication.Tools
{
	public static class AddResizingImg
	{
		public static string _logotypePath;
		public static ImageSource AddImg(ImageSource addProductImg)
		{
			string targetFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "imgProducts");
			Directory.CreateDirectory(targetFolderPath);
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Изображения (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|Все файлы (*.*)|*.*";
			bool? result = openFileDialog.ShowDialog();
			if (result == true)
			{
				string sourceFilePath = openFileDialog.FileName;
				string fileName = Path.GetFileName(sourceFilePath);
				string targetFilePath = Path.Combine(targetFolderPath, fileName);
				File.Copy(sourceFilePath, targetFilePath, true);

				_logotypePath = $"{fileName}";

				ImageSourceConverter converter = new ImageSourceConverter();
				addProductImg = (ImageSource)converter.ConvertFromString(sourceFilePath);

				if(addProductImg.Width > 500 && addProductImg.Height > 500)
				{
					MessageBox.Show("Картинка превышает 500x500 пикселей");
					return null;
				}

				MessageBox.Show($"Файл {sourceFilePath} выбран успешно!");

				return addProductImg;
			}

			return null;
		}



		//// Обрезаем изображение и сохраняем измененный размер
		//ResizeImage(sourceFilePath, targetFilePath, newWidth, newHeight);

		//public static void ResizeImage(string sourceFilePath, string fileName, int newWidth, int newHeight)
		//{
		//	using (Image image = Image.FromFile(sourceFilePath))
		//	using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
		//	using (Graphics g = Graphics.FromImage(resizedImage))
		//	{
		//		g.DrawImage(image, 0, 0, newWidth, newHeight);
		//		resizedImage.Save(fileName);
		//	}
		//}
	}
}
