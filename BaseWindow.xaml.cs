using System.Windows;

namespace DishesApplication
{
	/// <summary>
	/// Логика взаимодействия для BaseWindow.xaml
	/// </summary>
	public partial class BaseWindow : Window
	{
		public string Fio { get; set; }
		public BaseWindow()
		{
			Fio = "Гость";
			InitializeComponent();
			DataContext= this;
		}
	}
}
