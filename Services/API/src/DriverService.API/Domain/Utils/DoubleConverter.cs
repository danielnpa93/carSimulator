using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DriverService.API.Domain.Utils
{
    public class DoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TryGetDouble(out double value))
            {
                return value;
            }

            string stringValue = reader.GetString();
            if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedValue))
            {
                return parsedValue;
            }

            throw new JsonException($"Unable to convert '{stringValue}' to {typeToConvert}.");
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
