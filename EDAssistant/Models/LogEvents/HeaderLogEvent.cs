using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDAssistant.Models.LogEvents
{
	public class HeaderLogEvent : LogEvent
	{
		public int Part { get; set; }
		public override string Message
		{
			get => $"Started reading log (Part {Part})";
		}
	}
}
