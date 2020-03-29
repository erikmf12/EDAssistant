using EDAssistant.Models.LogEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EDAssistant.Services
{
	public static class InaraUpdater
	{
		private static DateTime _lastRequest;
		public static async Task<bool> UpdateLocationAsync(string apiKey, string systemName, string stationName, string shipType, int shipGameID)
		{
			if (string.IsNullOrEmpty(apiKey) ||
				string.IsNullOrEmpty(systemName))
			{
				return false;
			}

			// purely to be nice to his servers:
			if(DateTime.Now - _lastRequest < TimeSpan.FromSeconds(3))
			{
				return false;
			}

			using (var client = new HttpClient()
			{
				BaseAddress = new Uri("https://inara.cz/"),
			})
			{
				var header = new Header
				{
					APIkey = apiKey,
					appName = "EDAssistant",
					appVersion = "1.0.0",
				};
				InaraLocation location = new InaraLocation(systemName, stationName, shipType, shipGameID);
				InaraRequest request = new InaraRequest()
				{
					header = header,
					events = new List<InaraLocation> { location }
				};

				var content = JsonSerializer.Serialize(request);

				var result = await client.PostAsync("inapi/v1/", new StringContent(content));
				var resultContent = await result.Content.ReadAsStringAsync();
				_lastRequest = DateTime.Now;
				return result.IsSuccessStatusCode;
			}
		}
	}

	class InaraRequest
	{
		public Header header { get; set; }
		public List<InaraLocation> events { get; set; }

	}

	class Header
	{
		public string appName { get; set; }
		public string appVersion { get; set; }
		public string APIkey { get; set; }
		public bool isDeveloped { get => true; }
		public string commanderName { get; set; }
		public string commanderFrontierID { get; set; }
	}

	class InaraLocation
	{
		public InaraLocation(string systemName, string stationName, string shipType, int shipGameID)
		{
			this.eventData = new EventData
			{
				shipGameID = shipGameID,
				shipType = shipType,
				starsystemName = systemName,
				stationName	 = stationName
			};
		}

		public string cventName { get; } = "addCommanderTravelDock";
		public DateTime cventTimestamp { get; } = DateTime.UtcNow;
		public EventData eventData { get; set; }
	}

	class EventData
	{
		public string starsystemName { get; set; }
		public string stationName { get; set; }
		public string shipType { get; set; }
		public int shipGameID { get; set; }
	}
}
