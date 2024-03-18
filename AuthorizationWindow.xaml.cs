using DishesApplication.Tools;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;

namespace DishesApplication
{
	public enum Role
	{
		Admin = 1,
		Manager = 2,
		Client = 3,
		Guest = 4
	}

	public partial class MainWindow : Window
	{
		DishesApplicationDBEntities db = new DishesApplicationDBEntities();

		private DateTime CanAutorizeDateTimeUtc = DateTime.UtcNow;
		private String capthcaValue;

		public MainWindow()
		{
			InitializeComponent();
			captchaTextBox.Source = GenerateCapthca();
			DataContext = this;
		}

		private void SignInGuest(object sender, RoutedEventArgs e)
		{
			BaseWindow window = new BaseWindow(Role.Guest);
			window.Show();
			Close();
		}

		private void SignInUser(object sender, RoutedEventArgs e)
		{
			string login = loginInput.Text;
			string password = passwordInput.Password;
			string capthca = captchaInput.Text;

			if (DateTime.UtcNow < CanAutorizeDateTimeUtc)
			{
				int timeBlockedInSeconds = (CanAutorizeDateTimeUtc - DateTime.UtcNow).Seconds;
				MessageBox.Show($"Повторите попытку через {timeBlockedInSeconds} секунд", "Ошибка", MessageBoxButton.OK);
				return;
			}

			if (String.IsNullOrWhiteSpace(capthca))
			{
				MessageBox.Show("Заполните CAPTCHA", "Ошибка", MessageBoxButton.OK);
				return;
			}

			if (capthca != capthcaValue)
			{
				MessageBox.Show("Вы некорректно заполнили CAPTCHA", "Ошибка", MessageBoxButton.OK);
				CanAutorizeDateTimeUtc = DateTime.UtcNow.AddSeconds(10);
				captchaTextBox.Source = GenerateCapthca();
				return;
			}

			Users user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
			if (user == null)
			{
				MessageBox.Show("Логин или пароль введен неверно", "Ошибка", MessageBoxButton.OK);
				return;
			}

			Storage.SystemUser = user;
			Role role = Role.Guest;
			if (user.UserRoleId == 1)
			{
				role = Role.Admin;
			}
			else if (user.UserRoleId == 2)
			{
				role = Role.Manager;
			}
			else if (user.UserRoleId == 3)
			{
				role = Role.Client;
			}
			BaseWindow window = new BaseWindow(role);
			window.Show();
			Close();
		}

		private ImageSource GenerateCapthca()
		{
			String randomString = GenerateRandomString();
			capthcaValue = randomString;
			Bitmap captcha = GenerateCaptchaImage(randomString, 200, 70);

			return ConvertBitmapToImageSource(captcha);
		}

		private ImageSource ConvertBitmapToImageSource(Bitmap bitmap)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				// Сохраняем Bitmap в формате Bmp в MemoryStream
				bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

				// Создаем BitmapImage и загружаем его из MemoryStream
				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.EndInit();
				bitmapImage.Freeze(); // Фиксируем изображение для использования в различных потоках

				return bitmapImage;
			}
		}

		private string GenerateRandomString()
		{
			string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%&*+";

			Random random = new Random();
			StringBuilder stringBuilder = new StringBuilder();

			for (int i = 0; i < 4; i++)
			{
				int index = random.Next(characters.Length);
				stringBuilder.Append(characters[index]);
			}

			return stringBuilder.ToString();
		}

		static Bitmap GenerateCaptchaImage(string captchaText, int width, int height)
		{
			Bitmap captchaImage = new Bitmap(width, height);
			using (Graphics graphics = Graphics.FromImage(captchaImage))
			{
				// Задаем белый фон
				graphics.FillRectangle(Brushes.White, 0, 0, width, height);

				Random random = new Random();

				// Рисуем символы CAPTCHA
				for (int i = 0; i < captchaText.Length; i++)
				{
					char captchaChar = captchaText[i];

					// Устанавливаем случайный цвет для символа
					Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
					SolidBrush brush = new SolidBrush(randomColor);

					// Устанавливаем случайный угол наклона для символа
					float angle = random.Next(-20, 20);

					// Устанавливаем случайную позицию для символа
					float x = i * (width / captchaText.Length) + random.Next(5, 15);
					float y = random.Next(10, height - 30);

					// Перечеркиваем или налагаем символы друг на друга
					if (random.Next(2) == 0)
					{
						graphics.TranslateTransform(x, y);
						graphics.RotateTransform(angle);
						graphics.DrawString(captchaChar.ToString(), new Font("Arial", 20), brush, 0, 0);
						graphics.ResetTransform();
					}
					else
					{
						graphics.DrawString(captchaChar.ToString(), new Font("Arial", 20), brush, x, y);
					}
				}

				// Добавляем случайные линии для улучшения защиты от OCR
				for (int i = 0; i < 5; i++)
				{
					System.Drawing.Pen pen = new System.Drawing.Pen(Color.FromArgb(random.Next(256), random.Next(256), random.Next(256)), 2);
					graphics.DrawLine(pen, random.Next(width), random.Next(height), random.Next(width), random.Next(height));
				}
			}
			return captchaImage;
		}

		private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				SignInUser(sender, e);
			}
		}
	}
}
