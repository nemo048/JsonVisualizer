using System.Text.Json;
using System.Text.Json.Serialization;
using JsonVisualizer.Settings;

namespace JsonVisualizer.Converters;

public class JsonVisualizerPaletteConverter : JsonConverter<JsonVisualizerPaletteSettings>
{
    public override JsonVisualizerPaletteSettings Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token.");
        }

        string? backgroundColor = null;
        string? stringColor = null;
        string? numberColor = null;
        string? booleanColor = null;
        string? delimiterColor = null;
        string? nullColor = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string? propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "backgroundColor":
                        backgroundColor = reader.GetString();
                        break;
                    case "stringColor":
                        stringColor = reader.GetString();
                        break;
                    case "numberColor":
                        numberColor = reader.GetString();
                        break;
                    case "booleanColor":
                        booleanColor = reader.GetString();
                        break;
                    case "delimiterColor":
                        delimiterColor = reader.GetString();
                        break;
                    case "nullColor":
                        nullColor = reader.GetString();
                        break;
                    default:
                        throw new JsonException($"Unknown property: {propertyName}");
                }
            }
        }

        return new JsonVisualizerPaletteSettings(
            backgroundColor,
            stringColor,
            numberColor,
            booleanColor,
            delimiterColor,
            nullColor);
    }

    public override void Write(Utf8JsonWriter writer, JsonVisualizerPaletteSettings value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("backgroundColor", value.BackgroundColor);
        writer.WriteString("stringColor", value.StringColor);
        writer.WriteString("numberColor", value.NumberColor);
        writer.WriteString("booleanColor", value.BooleanColor);
        writer.WriteString("delimiterColor", value.DelimiterColor);
        writer.WriteString("nullColor", value.NullColor);
        writer.WriteEndObject();
    }
}
