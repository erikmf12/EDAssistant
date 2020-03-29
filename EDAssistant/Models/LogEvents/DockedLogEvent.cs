using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDAssistant.Models.LogEvents
{
	public class DockedLogEvent : LogEvent
	{
		public string StationName { get; set; }
		public string StarSystem { get; set; }

		public override string Message
		{
			get => $"Docked at {StationName}";
		}
	}
}