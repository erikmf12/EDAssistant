using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace EDAssistant.Dialogs
{
	/// <summary>
	/// Interaction logic for ApiKeyDialog.xaml
	/// </summary>
	public partial class ApiKeyDialog : Window, INotifyPropertyChanged
	{
		private string _apiKey;
		public string ApiKey
		{
			get => _apiKey;
			set
			{
				_apiKey = value;
				Notify();
			}
		}

		private bool _userEntered;
		public event PropertyChangedEventHandler PropertyChanged;
		private void Notify([CallerMemberName] string p = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
		}

		public ApiKeyDialog()
		{
			DataContext = this;
			this.Activated += ApiKeyDialog_Activated;
			InitializeComponent();
		}

		private void ApiKeyDialog_Activated(object sender, EventArgs e)
		{
			var key = Properties.Settings.Default.ApiKey;
			if (!string.IsNullOrEmpty(key))
			{
				ApiKey = key;
			}
			else
			{
				TryGetClipboardText();
			}
		}

		private void TryGetClipboardText()
		{
			var text = Clipboard.GetText();
			if (!string.IsNullOrEmpty(text) && text.Length > 10 && text.Length < 100)
			{
				ApiKey = text;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ApiKey = box.Text;
			Properties.Settings.Default.ApiKey = ApiKey;
			Properties.Settings.Default.Save();
			DialogResult = true;
		}

		public static bool TryGetKey(out string key)
		{
			key = null;
			var dialog = new ApiKeyDialog();
			if (dialog.ShowDialog() == true)
			{
				key = dialog.ApiKey;
				return true;
			}
			return false;
		}
	}
}
