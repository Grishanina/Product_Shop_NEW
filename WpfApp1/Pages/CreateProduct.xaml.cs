using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
	/// Логика взаимодействия для CreateProduct.xaml
	/// </summary>
	public partial class CreateProduct : Page
	{
		
		T_Product PRO;  // объект, в котором будет хранится данные о новом или отредактированном коте
		bool flagUpdate = false; // для определения, создаем мы новый объект или редактируем старый
		//string path;  // путь к картинке

		public void uploadFields()  // метод для заполнения списков
		{
			cmbManufact.ItemsSource = ClassBase.BD.T_Manufacturer.ToList();
			cmbManufact.SelectedValuePath = "id_manufacturer";
			cmbManufact.DisplayMemberPath = "Manufacturer";

			cmbProvider.ItemsSource = ClassBase.BD.T_Provider.ToList();
			cmbProvider.SelectedValuePath = "id_provider";
			cmbProvider.DisplayMemberPath = "Provider";

			cmbType.ItemsSource = ClassBase.BD.T_Type.ToList();
			cmbType.SelectedValuePath = "id_type";
			cmbType.DisplayMemberPath = "Type";

			cmbDiscount.ItemsSource = ClassBase.BD.T_Discount.ToList();
			cmbDiscount.SelectedValuePath = "id_discount";
			cmbDiscount.DisplayMemberPath = "Discount";

			lbSostav.ItemsSource = ClassBase.BD.T_Sostav.ToList();
			lbSostav.SelectedValuePath = "id_sostav";
			lbSostav.DisplayMemberPath = "Sostav";

		}

		// конструктор для редактирования данных о продукте ( с аргументом, который хранит информацию о продукте, которого хотим отредактировать)
		public CreateProduct(T_Product product)
		{
			InitializeComponent();
			uploadFields(); // заполняем списки
			flagUpdate = true;  // отметка о том, что кота редактируем
			PRO = product;  // ассоциируем выше созданный глобавльный объект с объектом в кострукторе для дальнейшего редактирования этих данных
			tbName.Text = product.Title;  
			cmbManufact.SelectedIndex = product.id_manufacturer - 1;  
			cmbProvider.SelectedIndex = product.id_provider - 1;
			cmbType.SelectedIndex = product.id_type - 1;
			cmbDiscount.SelectedIndex = product.id_discount - 1;
			tbPrice.Text = Convert.ToString(product.Price);

			// находим черты характера того кота, которого мы редактируем:
			List<T_Sostav> tC = ClassBase.BD.T_Sostav.Where(x => x.id_sostav == product.id_product).ToList();

			// цикл для выделения черт характера кота в общем списке:
			foreach (T_Sostav t in lbSostav.Items)
			{
				if (tC.FirstOrDefault(x => x.id_sostav == t.id_sostav) != null)
				{
					lbSostav.SelectedItems.Add(t);
				}
			}

			// вывод картинки

			//if (product.Photo != null)
			//{
			//	BitmapImage img = new BitmapImage(new Uri(product.Photo, UriKind.RelativeOrAbsolute));
			//	photoProduct.Source = img;
			//}
		}

		// конструктор для создания нового кота (без аргументов)
		public CreateProduct()
		{
			InitializeComponent();
			uploadFields();  // заполняем списки
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			try
			{

				// если флаг равен false, то создаем объект для добавления кота
				if (flagUpdate == false)
				{
					PRO = new T_Product();
				}
				// заполняем поля таблицы CatTable
				PRO.Title = tbName.Text;
				PRO.id_manufacturer = cmbManufact.SelectedIndex + 1;
				PRO.id_provider = cmbProvider.SelectedIndex + 1;
				PRO.id_type = cmbType.SelectedIndex + 1;
				PRO.id_discount = cmbDiscount.SelectedIndex + 1;
				PRO.Price = Convert.ToInt32(tbPrice.Text);
				//PRO.Photo = path;

				// если флаг равен false, то добавляем объект в базу
				if (flagUpdate == false)
				{
					ClassBase.BD.T_Product.Add(PRO);
				}
				// BaseClass.tBE.SaveChanges();

				// Для заполнения таблицы TraitsCats нужно организовать цикл, так как черт характера у кота может быть несколько
				// Цикл будет организовывать по чертам характера, которые выделены в списке

				// находим список черт характера кота:
				List<T_Sostav_Product> sostav = ClassBase.BD.T_Sostav_Product.Where(x => PRO.id_product == x.id_product).ToList();

				// если список не пустой, удаляем из него все черты характера  этого кота
				if (sostav.Count > 0)
				{
					foreach (T_Sostav_Product s in sostav)
					{
						ClassBase.BD.T_Sostav_Product.Remove(s);
					}
				}

				// перезаписываем черты кота (или добавляем черты для нового кота)
				foreach (T_Sostav_Product s in lbSostav.SelectedItems)
				{
					T_Sostav_Product SP = new T_Sostav_Product()  // объект для записи в таблицу TraitsCat
					{
						id_product = PRO.id_product,
						id_sostav = s.id_sostav
					};
					ClassBase.BD.T_Sostav_Product.Add(SP);
				}
				//  BaseClass.tBE.SaveChanges();

				ClassBase.BD.SaveChanges();
				MessageBox.Show("Информация добавлена");
			}
			catch
			{
				MessageBox.Show("Что-то пошло не по плану");
			}
		}

		private void btnPhto_Click(object sender, RoutedEventArgs e)  // добавление фото кота с помощью диалогового окна
		{
		//	OpenFileDialog OFD = new OpenFileDialog();  // создаем объект диалогового окна
		//	OFD.ShowDialog();  // открываем диалоговое окно
		//	path = OFD.FileName;  // извлекаем полный путь к картинке
		//	string[] arrayPath = path.Split('\\');  // разделяем путь к картинке в массив
		//	path = "\\" + arrayPath[arrayPath.Length - 2] + "\\" + arrayPath[arrayPath.Length - 1];  // записываем в бд путь, начиная с имени папки
		//																							 // MessageBox.Show(path);
		}

		private void btnHom_Click(object sender, RoutedEventArgs e)
		{
			Class1.Mfrm.Navigate(new Product());
		}
	}
}

