using System.Text.Json;

namespace JobInterviewTests.Handlers
{
    public class DateOnlyJsonConverterTests
    {
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            Converters = { new DateOnlyJsonConverter() }
        };


        [Fact]
        public void should_deserialize_DateOnly_object()
        {
            string json = "\"20250106\"";
            var result = JsonSerializer.Deserialize<DateOnly>(json, jsonOptions);
            var expected = new DateOnly(2025, 1, 6);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void should_serialize_DateOnly_object()
        {
            var date = new DateOnly(2025, 1, 6);
            string json = JsonSerializer.Serialize(date, jsonOptions);
            Assert.Equal("\"20250106\"", json);
        }

        [Fact]
        public void should_throw_exception_when_invalid_string()
        {
            string invalidJson = "\"2025-01-06\"";
            
            Assert.Throws<FormatException>(() => JsonSerializer.Deserialize<DateOnly>(invalidJson, jsonOptions));

            invalidJson = "\"2025016\"";

            Assert.Throws<FormatException>(() => JsonSerializer.Deserialize<DateOnly>(invalidJson, jsonOptions));

        }
    }
}