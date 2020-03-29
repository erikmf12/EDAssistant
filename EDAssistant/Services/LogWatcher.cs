using EDAssistant.Models;
using EDAssistant.Models.LogEvents;
using EDAssistant.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EDAssistant.Services
{
	public class LogWatcher : IDisposable
	{
		private readonly string _logFolder = Path.Combine("C:/users", Environment.UserName, "Saved Games/Frontier Developments/Elite Dangerous");

		private CancellationTokenSource _cts;
		private bool _reading = false;

		public event EventHandler<LogEvent> LogAdded;

		public void Start()
		{
			_cts = new CancellationTokenSource();
			ReadLog(_cts.Token);
		}

		public void Stop()
		{
			_cts.Cancel();
		}



		private FileInfo GetLatestLog()
		{
			DirectoryInfo dir = new DirectoryInfo(_logFolder);
			return dir.GetFiles("*.log", SearchOption.TopDirectoryOnly).OrderByDescending(x => x.CreationTime).FirstOrDefault(x => x.Name.StartsWith("Journal."));
		}

		private void ReadLog(CancellationToken token)
		{
			_reading = true;
			Task.Factory.StartNew(async () =>
			{
				var latestLog = GetLatestLog();
				var sw = Stopwatch.StartNew();
				using (var fs = latestLog.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				using (var sr = new StreamReader(fs))
				{
					while (!token.IsCancellationRequested)
					{
						var line = await sr.ReadLineAsync();
						if (line != null)
						{
							if (LogEventFactory.TryCreateKnownLog(line, out var log))
							{
								LogAdded?.Invoke(this, log);
							}
						}
						else
						{
							if (sw.ElapsedMilliseconds > 5000)
							{
								if (GetLatestLog().FullName != latestLog.FullName)
								{
									Reset();
								}
								sw.Restart();
							}
							await Task.Delay(100);
						}
					}
				}
			}, token);
			_reading = false;
		}

		private void Reset()
		{
			_cts.Cancel();
			while (_reading) { }
			_cts = new CancellationTokenSource();
			ReadLog(_cts.Token);
		}

		public void Dispose()
		{
			_cts.Cancel();
		}
	}
}
