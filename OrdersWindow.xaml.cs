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
using System.Windows.Shapes;

using System.Collections.ObjectModel;


namespace Glovo
{
	public partial class OrdersWindow : Window
	{
		public OrdersWindow()
		{
			InitializeComponent();
		}

		public ObservableCollection<Order> Orders { get; set; }

		// У цьому методі приймається список замовлень
		public void SetOrders(ObservableCollection<Order> orders)
		{
			Orders = orders;
			OrdersList.ItemsSource = Orders;
		}
	}
}
