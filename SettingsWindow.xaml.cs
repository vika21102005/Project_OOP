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

using System.Windows;

namespace Glovo
{
	/// <summary>
	/// Логіка взаємодії для SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		public string SelectedRole { get; private set; }
		public string UserName { get; private set; }

		public SettingsWindow(string currentUserName, string currentRole)
		{
			InitializeComponent();

			// Встановлення поточного імені користувача
			UserNameTextBox.Text = currentUserName;

			// Встановлення поточної ролі в ComboBox
			foreach (ComboBoxItem item in RoleComboBox.Items)
			{
				if (item.Content.ToString() == currentRole)
				{
					RoleComboBox.SelectedItem = item;
					break;
				}
			}
		}

		private void SaveSettings_Click(object sender, RoutedEventArgs e)
		{
			if (RoleComboBox.SelectedItem is ComboBoxItem selectedItem)
			{
				SelectedRole = selectedItem.Content.ToString();
			}

			UserName = UserNameTextBox.Text;

			this.DialogResult = true; // Встановлюємо результат діалогу
			this.Close();
		}
	}
}
