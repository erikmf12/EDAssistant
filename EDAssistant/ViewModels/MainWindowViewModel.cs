using EDAssistant.Models;
using EDAssistant.Models.LogEvents;
using EDAssistant.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace EDAssistant.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{

		LogWatcher _watcher;
		private Dispatcher _dispatcher;

		private void Notify([CallerMemberName] string p = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
		}
		private string _currentSystem;
		private string _targetSystem;
		private string _currentStation;
		private int _currentCredits;
		private bool _appStarting = true;
		private string _shipName;
		private int _shipId;
		private string _commander;
		private bool _hasApiKey;

		public event PropertyChangedEventHandler PropertyChanged;
		public ObservableCollection<LogEvent> Logs { get; set; } = new ObservableCollection<LogEvent>();
		public string CurrentSystem
		{
			get => _currentSystem;
			set
			{
				_currentSystem = value;
				Notify();
			}
		}
		public string CurrentStation
		{
			get => _currentStation;
			set
			{
				_currentStation = value;
				Notify();
			}
		}
		public string TargetSystem
		{
			get => _targetSystem;
			set
			{
				_targetSystem = value;
				Notify();
			}
		}
		public int CurrentCredits
		{
			get => _currentCredits;
			set
			{
				_currentCredits = value;
				Notify();
			}
		}

		public bool HasApiKey
		{
			get => _hasApiKey;
			set
			{
				_hasApiKey = value;
				Notify();
			}
		}
		private Timer _timer;

		public MainWindowViewModel()
		{
			GetApiKey();
		}

		public void GetApiKey()
		{
			var key = Properties.Settings.Default.ApiKey;
			if (!string.IsNullOrEmpty(key))
			{
				HasApiKey = true;
			}
		}

		public void StartWatching()
		{
			_dispatcher = Application.Current.MainWindow.Dispatcher;
			Logs.Clear();
			if (_watcher != null)
			{
				_watcher.Stop();
				_watcher.Dispose();
			}
			_watcher = new LogWatcher();
			_watcher.LogAdded += _watcher_LogAdded;
			_watcher.Start();
			_timer = new Timer(AppInitialized, null, 500, Timeout.Infinite);
		}

		private void AppInitialized(object state)
		{
			_appStarting = false;
			InaraUpdater.UpdateLocationAsync(Properties.Settings.Default.ApiKey, CurrentSystem, CurrentStation, _shipName, _shipId).Wait();
		}


		private void _watcher_LogAdded(object sender, LogEvent e)
		{
			switch (e.Event)
			{
				case EventType.Fileheader:
					Invoke(() => Logs.Clear());
					CurrentStation = string.Empty;
					CurrentSystem = string.Empty;
					TargetSystem = "No Target";
					break;
				case EventType.FSDJump:
					CurrentSystem = (e as FsdJumpLogEvent).StarSystem;
					break;
				case EventType.LoadGame:
					var log = (e as LoadGameLogEvent);
					_shipId = log.ShipID;
					_shipName = log.ShipName;
					_commander = log.Commander;
					break;
				case EventType.Location:
					CurrentSystem = (e as LocationLogEvent).StarSystem;
					break;
				case EventType.FSDTarget:
					TargetSystem = (e as FsdTargetLogEvent).Name;
					break;
				case EventType.Docked:
					CurrentStation = (e as DockedLogEvent).StationName;
					if (!_appStarting)
					{
						InaraUpdater.UpdateLocationAsync(Properties.Settings.Default.ApiKey, CurrentSystem, CurrentStation, _shipName, _shipId).Wait();
					}
					break;
				case EventType.Undocked:
					CurrentStation = "Not Docked";
					break;
				default:
					break;
			}

			if (!_appStarting)
			{
				Invoke(() => Logs.Add(e));
			}
		}

		private void Invoke(Action action)
		{
			_dispatcher.Invoke(() => action());
		}
	}
}
