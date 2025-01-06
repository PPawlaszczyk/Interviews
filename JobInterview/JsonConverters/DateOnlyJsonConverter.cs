using System.Text.Json.Serialization;
using System.Text.Json;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyyMMdd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string dateString = reader.GetString() ?? throw new JsonException("Invalid date string");
        return DateOnly.ParseExact(dateString, Format);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}