using System.Text.Json;

namespace JsonVisualizer.Extensions;

internal static class JsonElementExtensions
{
    internal static bool IsObject(this JsonElement parentElement)
    {
        return parentElement.ValueKind is JsonValueKind.Object;
    }
    
    internal static bool IsArray(this JsonElement jsonElement)
    {
        return jsonElement.ValueKind is JsonValueKind.Array;
    }
}