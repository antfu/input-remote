using InputRemote.Tools;
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

namespace InputRemote.Client.Components
{
	/// <summary>
	/// AddressInput.xaml 的交互逻辑
	/// </summary>
	public partial class AddressInput : Window
	{
		/// <summary>
		/// 获取或设置对话框接受的Url字符串。
		/// </summary>
		public string Url { get; set; }

		private void txtUrl_Loaded(object sender, RoutedEventArgs e)
		{
			this.txtUrl.Text = this.Url ?? string.Empty;
		}

		public AddressInput()
		{
			InitializeComponent();
		}

		private void okButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.Url = this.txtUrl.Text;

			this.CloseDialog(true);
		}

		private void cancelButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			this.CloseDialog(false);
		}

		/// <summary>
		/// 关闭 <see cref="AddressInput"/> 对话框。
		/// </summary>
		/// <param name="dialogResult">关闭 <see cref="AddressInput"/> 对话框时应返回的对话框结果。</param>
		/// <remarks>
		/// 在调用此方法后，将会为 <see cref="AddressInput"/> 对话框的结束动画等待1秒，然后关闭对话框并返回对话框结果。
		/// </remarks>
		private void CloseDialog(bool? dialogResult)
		{
			new System.Threading.Thread(() =>
			{
				System.Threading.Thread.Sleep(1000);

				this.Dispatcher.BeginInvoke(
					new Action(() =>
					{
						this.DialogResult = dialogResult;
					}), null
				);
			}
				).Start();
		}
	}
}
