using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDAssistant.Models.LogEvents
{
	public class FsdTargetLogEvent : LogEvent
	{
		public string Name { get; set; }
		public int RemainingJumpsInRoute { get; set; }
		public override string Message => $"Jumping to {Name} with {RemainingJumpsInRoute} jumps left";
	}
}
