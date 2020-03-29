using EDAssistant.Converters;
using EDAssistant.Models;
using EDAssistant.Models.LogEvents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDAssistant.Services
{
	public static class LogEventFactory
	{
		private static JsonSerializerOptions _options;
		private static JsonSerializerOptions Options
		{
			get
			{
				if(_options == null)
				{
					_options = new JsonSerializerOptions()
					{
						PropertyNameCaseInsensitive = true,
					};
					_options.Converters.Add(new JsonLocalDateConverter());
					_options.Converters.Add(new JsonStringEnumConverter());
				}
				return _options;
			}
		}

		public static bool TryCreateKnownLog(string json, out LogEvent logEvent)
		{
			logEvent = null;
			EventType type;
			try
			{
				var doc = JsonDocument.Parse(json);
				if (!doc.RootElement.TryGetProperty("event", out var prop))
					return false;

				if (!Enum.TryParse(prop.GetString(), out type))
					return false;

				switch (type)
				{
					case EventType.Docked:
					case EventType.Undocked:
						logEvent = JsonSerializer.Deserialize<DockedLogEvent>(json, Options);
						break;
					case EventType.Fileheader:
						logEvent = JsonSerializer.Deserialize<HeaderLogEvent>(json, Options);
						break;
					case EventType.FSDJump:
						logEvent = JsonSerializer.Deserialize<FsdJumpLogEvent>(json, Options);
						break;
					case EventType.Location:
						logEvent = JsonSerializer.Deserialize<LocationLogEvent>(json, Options);
						break;
					case EventType.FSDTarget:
						logEvent = JsonSerializer.Deserialize<FsdTargetLogEvent>(json, Options);
						break;
					case EventType.LoadGame:
						logEvent = JsonSerializer.Deserialize<LoadGameLogEvent>(json, Options);
						break;
					default:
						throw new NotImplementedException("LogEvent: " + prop);
				}
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}
}
