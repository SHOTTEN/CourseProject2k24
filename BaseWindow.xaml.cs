using DishesApplication.Pages;
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
						MainFrame.Content = new ClientPage();
						break;
					}
			}
			
		}

		private string GetFio () {
			Users user = Storage.SystemUser;
			if (user == null)
			{
				return "Гость";
			}
			string fio = $"{user.Surname} {user.Name} {user.Patronomic}";
			return fio;
		}

		private string GetRole()
		{
			Users user = Storage.SystemUser;

			string role = db.Roles.First(r => r.RoleId == user.UserRoleId).RoleName;
			role = $"{role}";
			return role;
		}
	}
}
