using EDAssistant.Dialogs;
using EDAssistant.ViewModels;
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

namespace EDAssistant
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();

		public MainWindow()
		{
			this.DataContext = ViewModel;
			this.Loaded += MainWindow_Loaded;
			InitializeComponent();
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			ViewModel.StartWatching();
		}

		bool _autoScroll = true;
		private void ListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			var sv = e.OriginalSource as ScrollViewer;
			if (e.ExtentHeightChange == 0)
			{   // Content unchanged : user scroll event
				if (sv.VerticalOffset == sv.ScrollableHeight)
				{   // Scroll bar is in bottom
					// Set auto-scroll mode
					_autoScroll = true;
				}
				else
				{   // Scroll bar isn't in bottom
					// Unset auto-scroll mode
					_autoScroll = false;
				}
			}

			// Content scroll event : auto-scroll eventually
			if (_autoScroll && e.ExtentHeightChange != 0)
			{   // Content changed and auto-scroll mode set
				// Autoscroll
				sv.ScrollToVerticalOffset(sv.ExtentHeight);
			}
		}

		private void TargetSystemButton_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(ViewModel.TargetSystem);
		}

		private void CurrentSystemButton_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(ViewModel.CurrentSystem);
		}

		private void CurrentStationButton_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(ViewModel.CurrentStation);
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.Logs.Clear();
		}

		private void ApiKeyButton_Click(object sender, RoutedEventArgs e)
		{
			if (ApiKeyDialog.TryGetKey(out var key))
			{
				Properties.Settings.Default.ApiKey = key;
				ViewModel.HasApiKey = true;
			}
		}
	}
}
