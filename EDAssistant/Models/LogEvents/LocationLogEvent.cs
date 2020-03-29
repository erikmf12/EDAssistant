using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDAssistant.Models.LogEvents
{
	public class LocationLogEvent : LogEvent
	{
		public string StarSystem { get; set; }
		public override string Message => $"Currently in {StarSystem}";
	}
}
