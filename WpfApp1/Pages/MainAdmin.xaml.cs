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

namespace WpfApp1
{
	/// <summary>
	/// Логика взаимодействия для MainAdmin.xaml
	/// </summary>
	public partial class MainAdmin : Page
	{
		public MainAdmin()
		{
			InitializeComponent();
		}

		private void btnSpUsers_Click(object sender, RoutedEventArgs e)
		{
			Class1.Mfrm.Navigate(new Glavnaya());
		}

        private void btnProduct_Click(object sender, RoutedEventArgs e)
        {
			Class1.Mfrm.Navigate(new Product());
		}
    }
}
