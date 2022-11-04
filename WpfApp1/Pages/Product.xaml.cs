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
    /// Логика взаимодействия для Product.xaml
    /// </summary>
    public partial class Product : Page
    {
        public Product()
        {
            InitializeComponent();
            listProduct.ItemsSource = ClassBase.BD.T_Product.ToList();
        }


		// Список состава продукта
        private void SostavPR_Loaded(object sender, RoutedEventArgs e)
        {
			TextBlock tb = (TextBlock)sender;
			int index = Convert.ToInt32(tb.Uid);

			List<T_Sostav_Product> TC = ClassBase.BD.T_Sostav_Product.Where(x => x.id_product == index).ToList();

			string str = "";

			foreach (T_Sostav_Product tc in TC)
			{
				str += tc.T_Sostav.Sostav + ", ";
			}

			tb.Text = "Состав: " + str.Substring(0, str.Length - 2) + ".";
		}


		// Расчет цены товара с учетом скидки (и без нее)
        private void PricePT_Loaded(object sender, RoutedEventArgs e)
        {
			TextBlock tb = (TextBlock)sender;
			int index = Convert.ToInt32(tb.Uid);

			List<T_Product> TP = ClassBase.BD.T_Product.Where(x => x.id_product == index).ToList();

			double sum = 0;
			double pr = 0;

			foreach (T_Product prd in TP)
			{
				sum += Convert.ToDouble(prd.Price - (prd.Price * (prd.T_Discount.Discount / 100)));
				pr += Convert.ToDouble(prd.Price);
			}

			tb.Text = "Цена со скидкой: " + sum.ToString("F2") + " руб. (Без скидки: " + pr.ToString("F2") + " руб.)";
		}


		// Ограничение знаков после запятой у значения скидки
		private void DiscountPT_Loaded(object sender, RoutedEventArgs e)
		{
			TextBlock tb = (TextBlock)sender;
			int index = Convert.ToInt32(tb.Uid);

			List<T_Product> TP = ClassBase.BD.T_Product.Where(x => x.id_product == index).ToList();

			int dis = 0;

			foreach (T_Product p in TP)
			{

				dis += Convert.ToInt32(p.T_Discount.Discount);
			}

			tb.Text = "Скидка: " + dis.ToString() + " %";
		}
	}
}
