using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDAssistant.Models.LogEvents
{
	public class LoadGameLogEvent : LogEvent
	{
		public string Commander { get; set; }
		[JsonPropertyName("Ship_Localised")]
		public string Ship { get; set; }
		public string ShipName { get; set; }
		public int ShipID { get; set; }
		public override string Message => $"CMDR {Commander} in {Ship} [{ShipName}]";
	}
}
