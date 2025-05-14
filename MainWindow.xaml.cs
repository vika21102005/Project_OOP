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

using System.Collections.ObjectModel;
using System.IO;

public class User
{
	public string UserName { get; set; }
	public string Role { get; set; }
}

public enum UserRole
{
	Client,       // Клієнт
	Courier,      // Кур'єр
	Restaurant,   // Ресторан
	Admin         // Адміністратор
}

public class Order
{
	public int OrderId { get; set; }
	public string ClientName { get; set; }
	public string Status { get; set; }
}
namespace Glovo
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ObservableCollection<Order> Orders { get; set; }
		public User CurrentUser { get; set; }
		// Поточна роль користувача
		public string CurrentRole { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			// Створення тестового користувача
			CurrentUser = new User { UserName = "Іван Петренко", Role = UserRole.Client.ToString() };
			CurrentUser.Role = "Клієнт";  
			DataContext = this;

			Orders = new ObservableCollection<Order>
		{
			new Order { OrderId = 1, ClientName = "Іван Петренко", Status = "Очікується" },
			new Order { OrderId = 2, ClientName = "Марія Іванова", Status = "Виконується" }
		};

			DataContext = this;

			ConfigureUI();
		}

		// Метод для налаштування доступу до елементів UI в залежності від ролі
		private void ConfigureUI()
		{
			switch (CurrentUser.Role)
			{
				case "Клієнт":
					EditOrderButton.IsEnabled = false;  // Клієнти не можуть редагувати
					DeleteOrderButton.IsEnabled = false;  // Клієнти не можуть видаляти
					AddOrderButton.IsEnabled = true;  // Клієнти можуть додавати замовлення
					break;

				case "Кур'єр":
					EditOrderButton.IsEnabled = false;  // Кур'єри не можуть редагувати
					DeleteOrderButton.IsEnabled = false;  // Кур'єри не можуть видаляти
					AddOrderButton.IsEnabled = false;  // Кур'єри не можуть додавати
					break;

				case "Ресторан":
					EditOrderButton.IsEnabled = true;  // Ресторани можуть редагувати замовлення
					DeleteOrderButton.IsEnabled = false;  // Ресторани не можуть видаляти
					AddOrderButton.IsEnabled = false;  // Ресторани не можуть додавати
					break;

				case "Адміністратор":
					EditOrderButton.IsEnabled = true;  // Адміністратори можуть редагувати
					DeleteOrderButton.IsEnabled = true;  // Адміністратори можуть видаляти
					AddOrderButton.IsEnabled = true;  // Адміністратори можуть додавати
					break;

				default:
					MessageBox.Show("Невідома роль користувача!");
					break;
			}
		}

		private void AddOrder_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentUser.Role == "Клієнт")
			{
				Orders.Add(new Order { OrderId = Orders.Count + 1, ClientName = CurrentUser.UserName, Status = "Очікується" });
			}
			else
			{
				MessageBox.Show("Тільки клієнти можуть додавати замовлення.");
			}
		}

		private void EditOrder_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentUser.Role == "Адміністратор" || CurrentUser.Role == "Ресторан")
			{
				if (OrdersListView.SelectedItem is Order selectedOrder)
				{
					selectedOrder.Status = "Змінено";
					OrdersListView.Items.Refresh();
				}
			}
			else
			{
				MessageBox.Show("Ви не маєте прав для редагування.");
			}
		}

		private void DeleteOrder_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentUser.Role == "Адміністратор")
			{
				if (OrdersListView.SelectedItem is Order selectedOrder)
				{
					Orders.Remove(selectedOrder);
				}
			}
			else
			{
				MessageBox.Show("Ви не маєте прав для видалення.");
			}
		}

		

	private void Home_Click(object sender, RoutedEventArgs e)
	{
		string filePath = "OrdersExport.txt";

		try
		{
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				writer.WriteLine("Список замовлень:");
				writer.WriteLine("----------------------------");

				foreach (var order in Orders)
				{
					writer.WriteLine($"ID: {order.OrderId}, Клієнт: {order.ClientName}, Статус: {order.Status}");
				}

				writer.WriteLine("----------------------------");
				writer.WriteLine($"Дата експорту: {DateTime.Now}");
			}

			MessageBox.Show($"Список замовлень успішно збережено у файл: {filePath}");
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Помилка під час збереження файлу: {ex.Message}");
		}
	}


	private void Orders_Click(object sender, RoutedEventArgs e)
		{
			// Перевіряємо, чи є вікно для відображення списку замовлень
			OrdersWindow ordersWindow = new OrdersWindow();

			// Передаємо список замовлень у вікно
			ordersWindow.OrdersList.ItemsSource = Orders;

			// Відкриваємо нове вікно
			ordersWindow.Show();
		}

		private void Settings_Click(object sender, RoutedEventArgs e)
		{
			var settingsWindow = new SettingsWindow(CurrentUser.UserName, CurrentUser.Role);
			settingsWindow.ShowDialog();

			bool roleChanged = !string.IsNullOrEmpty(settingsWindow.SelectedRole) && settingsWindow.SelectedRole != CurrentUser.Role;
			bool nameChanged = !string.IsNullOrEmpty(settingsWindow.UserName) && settingsWindow.UserName != CurrentUser.UserName;

			if (roleChanged)
			{
				CurrentUser.Role = settingsWindow.SelectedRole;
				ConfigureUI(); // Оновлення інтерфейсу після зміни ролі
				MessageBox.Show($"Ваша нова роль: {CurrentUser.Role}");
			}

			if (nameChanged)
			{
				CurrentUser.UserName = settingsWindow.UserName;
				MessageBox.Show($"Ваше нове ім'я: {CurrentUser.UserName}");
			}
		}



		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
	}

}

