using DishesApplication.Pages;
using DishesApplication.Tools;
using System.Windows;

namespace DishesApplication
{
	public partial class BaseWindow : Window
	{
		DishesApplicationDBEntities db = new DishesApplicationDBEntities();

		public string Fio => GetFio();
		public string UserRole => GetRole();

		public BaseWindow(Role role)
		{
			InitializeComponent();
			DataContext = this;
			switch (role)
			{
				case Role.Admin:
					{
						MainFrame.Content = new AdminProductsPage();
						break;
					}
				case Role.Manager:
					{
						MainFrame.Content = new ManagerPage();
						break;
					}
				case Role.Client:
					{
						MainFrame.Content = new ClientPage();
						break;
					}
				case Role.Guest:
					{
						MainFrame.Content = new GuestPage();
						break;
					}
			}			
		}

		private string GetFio () {
			if (Storage.SystemUser == null) return ""; 

			return Storage.SystemUser.Fio;
		}

		private string GetRole()
		{
			if (Storage.SystemUser == null) return "Гость";
			return Storage.SystemUser.Roles.RoleName;
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (MessageBox.Show("Вы точно хотите вернуться?\nНесохранённые данные будут утеряны", "Выход",
				MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
			{
				e.Cancel = true;
			}
		}
    }
}
