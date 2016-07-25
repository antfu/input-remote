using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Antnf.KeyboardRemote.Client.Components
{
	/// <summary>
	/// AddressInput.xaml 的交互逻辑
	/// </summary>
	public partial class AddressInput : Window
	{
		public string Url { get; set; }

		public AddressInput()
		{
			InitializeComponent();
		}

		private void okButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.Url = this.txtUrl.Text;

			new System.Threading.Thread(() =>
			{
				System.Threading.Thread.Sleep(2000);

				this.Dispatcher.BeginInvoke(
					new Action(() =>
					{
						this.DialogResult = true;
					}), null
				);
			}
			).Start();
		}

		private void cancelButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			new System.Threading.Thread(() =>
			{
				System.Threading.Thread.Sleep(2000);

				this.Dispatcher.BeginInvoke(
					new Action(() =>
					{
						this.DialogResult = false;
					}), null
				);
			}
			).Start();
		}

		private void txtUrl_Loaded(object sender, RoutedEventArgs e)
		{
			this.txtUrl.Text = this.Url ?? string.Empty;
		}
	}
}
