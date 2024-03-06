using DishesApplication.Pages;
using DishesApplication.Tools;
using System.Linq;
using System.Windows;

namespace DishesApplication
{
	/// <summary>
	/// Логика взаимодействия для BaseWindow.xaml
	/// </summary>
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
			if (Storage.SystemUser == null) return "Гость"; 

			return Storage.SystemUser.Fio;
		}

		private string GetRole()
		{
			string role = db.Roles.First(r => r.RoleId == Storage.SystemUser.UserRoleId).RoleName;
			role = $"{role}";
			return role;
		}
	}
}
