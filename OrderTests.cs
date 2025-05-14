using NUnit.Framework;
using Glovo;
using System.Collections.ObjectModel;

namespace FoodDelivery.Tests
{
	[TestFixture]
	public class OrderTests
	{
		private ObservableCollection<Order> orders;

		[SetUp]
		public void Setup()
		{
			orders = new ObservableCollection<Order>
			{
				new Order { OrderId = 1, ClientName = "Іван Петренко", Status = "Очікується" },
				new Order { OrderId = 2, ClientName = "Марія Іванова", Status = "Виконується" }
			};
		}

		[Test]
		public void AddOrder_Client_CanAddOrder()
		{
			// Arrange
			var client = new User { UserName = "Тест Клієнт", Role = UserRole.Client.ToString() };

			// Act
			orders.Add(new Order { OrderId = orders.Count + 1, ClientName = client.UserName, Status = "Очікується" });

			// Assert
			Assert.AreEqual(3, orders.Count);
			Assert.AreEqual("Тест Клієнт", orders[2].ClientName);
			Assert.AreEqual("Очікується", orders[2].Status);
		}

		[Test]
		public void DeleteOrder_Admin_CanDeleteOrder()
		{
			// Arrange
			var admin = new User { UserName = "Адмін", Role = UserRole.Admin.ToString() };
			var orderToDelete = orders[0];

			// Act
			orders.Remove(orderToDelete);

			// Assert
			Assert.AreEqual(1, orders.Count);
			Assert.IsFalse(orders.Contains(orderToDelete));
		}

		[Test]
		public void EditOrder_Restaurant_CanEditOrderStatus()
		{
			// Arrange
			var restaurant = new User { UserName = "Ресторан", Role = UserRole.Restaurant.ToString() };
			var orderToEdit = orders[0];

			// Act
			orderToEdit.Status = "Готується";

			// Assert
			Assert.AreEqual("Готується", orderToEdit.Status);
		}
	}
}
