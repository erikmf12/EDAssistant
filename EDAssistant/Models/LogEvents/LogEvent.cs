using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDAssistant.Models.LogEvents
{
	public abstract class LogEvent
	{
		public DateTime Timestamp { get; set; }
		public EventType Event { get; set; }

		public abstract string Message { get; }
	}
}
