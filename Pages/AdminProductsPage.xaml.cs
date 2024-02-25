using DishesApplication.Tools;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DishesApplication.Pages
{
	/// <summary>
	/// Логика взаимодействия для AdminProductsPage.xaml
	/// </summary>
	public partial class AdminProductsPage : Page
	{
		public AdminProductsPage()
		{
			InitializeComponent();
			DishesApplicationDB.SetDataToListView(lvProducts);
			DishesApplicationDB.FillComboBoxFilter(cbFilter);
			DishesApplicationDB.FillComboBoxSorting(cbSort);
		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, lvProducts);
		}

		private void btnExit(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();
			window.Show();
			Window parentWindow = Window.GetWindow(this);
			Storage.SystemUser = null;
			parentWindow.Close();
		}

		private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, lvProducts);
		}

		private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, lvProducts);
		}

		private void tbPoisk_TextChanged(object sender, TextChangedEventArgs e)
		{
			DishesApplicationDB.UpdateProducts(cbFilter, cbSort, tbPoisk, outputQuantityProducts, allQuantityProducts, lvProducts);
		}
	}
}
