using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EDAssistant.Converters
{
	public class JsonLocalDateConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var date = reader.GetDateTime();
			return DateTime.SpecifyKind(date, DateTimeKind.Utc).ToLocalTime();
		}

		public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
		{
			throw new NotImplementedException();
		}
	}
}
