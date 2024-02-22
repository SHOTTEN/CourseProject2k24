using System;
using System.Collections.Generic;
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

namespace DishesApplication
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public static class Storage
	{
		public static Users SystemUser;
	}
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
		public MainWindow()
		{
			InitializeComponent();

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
	}
}
