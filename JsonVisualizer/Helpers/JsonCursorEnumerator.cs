using System.Text.Json;
using JsonVisualizer.Extensions;
using JsonVisualizer.Models;

namespace JsonVisualizer.Helpers;

internal sealed class JsonCursorEnumerator : IDisposable
{
    #region Control chars

    private const string DOUBLE_QUOTE = "\"";
    private const string ARRAY_START = "[";
    private const string ARRAY_END = "]";
    private const string OBJECT_START = "{";
    private const string COLON = ":";
    private const string OBJECT_END = "}";

    #endregion
    
    private readonly ICursor _cursor;
    private readonly JsonVisualizerPalette _palette;

    internal JsonCursorEnumerator(
        ICursor cursor,
        JsonVisualizerPalette palette)
    {
        _cursor = cursor;
        _palette = palette;
    }

    public void Enumerate(JsonElement jsonElement)
    {
        EnumerateJsonElement(
            _cursor, 
            jsonElement, 
            parentElement: default,
            indentationLevel: 0);
    }
    
    private void EnumerateJsonElement(
        ICursor cursor,
        JsonElement jsonElement,
        JsonElement parentElement,
        int indentationLevel)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                EnumerateObject(cursor, jsonElement, parentElement, indentationLevel);
                break;

            case JsonValueKind.Array:
                EnumerateArray(cursor, jsonElement, parentElement, indentationLevel);
                break;

            case JsonValueKind.String:
                ProcessString(cursor, jsonElement, parentElement);
                break;

            case JsonValueKind.Number:
                ProcessNumber(cursor, jsonElement, parentElement);
                break;
            
            case JsonValueKind.True:
            case JsonValueKind.False:
                ProcessBoolean(cursor, jsonElement, parentElement);
                break;
            
            case JsonValueKind.Null:
                ProcessNull(cursor, jsonElement, parentElement);
                break;
            
            case JsonValueKind.Undefined:
                ProcessUndefined(cursor, jsonElement, parentElement);
                break;
        }
    }

    private void ProcessNull(ICursor cursor, JsonElement jsonElement, JsonElement parentElement)
    {
        ProcessSimpleValueStart(cursor, parentElement);

        cursor.DrawText(jsonElement.GetRawText(), _palette.NullPaint);
    }
    
    private void ProcessUndefined(ICursor cursor, JsonElement jsonElement, JsonElement parentElement)
    {
        ProcessSimpleValueStart(cursor, parentElement);

        cursor.DrawText(jsonElement.GetRawText(), _palette.NullPaint);
    }

    private void ProcessBoolean(ICursor cursor, JsonElement jsonElement, JsonElement parentElement)
    {
        ProcessSimpleValueStart(cursor, parentElement);

        cursor.DrawText(jsonElement.GetRawText(), _palette.BooleanPaint);
    }

    private void ProcessNumber(ICursor cursor, JsonElement jsonElement, JsonElement parentElement)
    {
        ProcessSimpleValueStart(cursor, parentElement);

        cursor.DrawText(jsonElement.GetRawText(), _palette.NumberPaint);
    }

    private static void ProcessSimpleValueStart(ICursor drawingCursor, JsonElement parentElement)
    {
        if (parentElement.IsArray())
        {
            return;
        }

        drawingCursor.DrawWhiteSpace();
    }

    private void ProcessString(ICursor cursor, JsonElement jsonElement, JsonElement parentElement)
    {
        ProcessSimpleValueStart(cursor, parentElement);
        
        cursor.DrawText(DOUBLE_QUOTE, _palette.StringPaint);
        cursor.DrawText(jsonElement.GetString() ?? string.Empty, _palette.StringPaint);
        cursor.DrawText(DOUBLE_QUOTE, _palette.StringPaint);
    }

    private void EnumerateArray(ICursor cursor, JsonElement jsonElement, JsonElement parentElement, int indentationLevel)
    {
        if (parentElement.IsObject())
        {
            cursor.DrawWhiteSpace();
        }
        
        cursor.DrawText(ARRAY_START, _palette.DelimiterPaint);

        using (JsonElement.ArrayEnumerator arrayEnumerator = 
               jsonElement.EnumerateArray().GetEnumerator())
        {
            bool hasNext = arrayEnumerator.MoveNext();
            bool isEmpty = true;
            if (hasNext)
            {
                isEmpty = false;
                cursor.DrawLineBreak();
            }
            
            indentationLevel++;
            
            while (hasNext)
            {
                cursor.DrawIndentation(indentationLevel);
                    
                JsonElement arrayItem = arrayEnumerator.Current;
                hasNext = arrayEnumerator.MoveNext();

                EnumerateJsonElement(
                    cursor,
                    arrayItem,
                    jsonElement,
                    indentationLevel);

                if (hasNext)
                {
                    cursor.DrawComma(_palette.DelimiterPaint);
                }
                
                cursor.DrawLineBreak();
            }
            
            indentationLevel--;
            if (!isEmpty)
            {
                cursor.DrawIndentation(indentationLevel);
            }

            cursor.DrawText(ARRAY_END, _palette.DelimiterPaint);
        }
    }

    private void EnumerateObject(ICursor cursor, JsonElement jsonElement, JsonElement parentElement, int indentationLevel)
    {
        if (parentElement.IsObject())
        {
            cursor.DrawWhiteSpace();
        }
        
        cursor.DrawText(OBJECT_START, _palette.DelimiterPaint);

        using (JsonElement.ObjectEnumerator propertiesEnumerator =
               jsonElement.EnumerateObject().GetEnumerator())
        {
            bool hasNext = propertiesEnumerator.MoveNext();
            bool isEmpty = true;
            if (hasNext)
            {
                isEmpty = false;
                cursor.DrawLineBreak();
            }
            
            indentationLevel++;
            
            while (hasNext)
            {
                cursor.DrawIndentation(indentationLevel);
                
                JsonProperty property = propertiesEnumerator.Current;
                hasNext = propertiesEnumerator.MoveNext();
                
                cursor.DrawText(DOUBLE_QUOTE, _palette.DelimiterPaint);
                cursor.DrawText(property.Name, _palette.DelimiterPaint);
                cursor.DrawText(DOUBLE_QUOTE, _palette.DelimiterPaint);
                cursor.DrawText(COLON, _palette.DelimiterPaint);
                
                EnumerateJsonElement(
                    cursor,
                    property.Value,
                    jsonElement,
                    indentationLevel);

                if (hasNext)
                {
                    cursor.DrawComma(_palette.DelimiterPaint);
                }
                
                cursor.DrawLineBreak();
            }
            
            indentationLevel--;
            if (!isEmpty)
            {
                cursor.DrawIndentation(indentationLevel);
            }
        }

        cursor.DrawText(OBJECT_END, _palette.DelimiterPaint);
    }

    public void Dispose()
    {
        _cursor.Dispose();
    }
}