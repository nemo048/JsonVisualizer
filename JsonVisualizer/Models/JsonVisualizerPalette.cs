using JsonVisualizer.Settings;
using SkiaSharp;

namespace JsonVisualizer.Models;

internal sealed class JsonVisualizerPalette : IDisposable
{
    internal SKPaint DelimiterPaint { get; }
    internal SKPaint NumberPaint { get; }
    internal SKPaint StringPaint { get; }
    internal SKPaint BooleanPaint { get; }
    internal SKPaint NullPaint { get; }
    internal SKColor BackgroundColor { get; }
    
    internal JsonVisualizerPalette(JsonVisualizerSettings settings)
    {
        DelimiterPaint = GetPaint(settings.PaletteSettings.DelimiterColor, settings);
        NumberPaint = GetPaint(settings.PaletteSettings.NumberColor, settings);
        StringPaint = GetPaint(settings.PaletteSettings.StringColor, settings);
        BooleanPaint = GetPaint(settings.PaletteSettings.BooleanColor, settings);
        NullPaint = GetPaint(settings.PaletteSettings.NullColor, settings);
        BackgroundColor = GetColor(settings.PaletteSettings.BackgroundColor);
    }
    
    private static SKPaint GetPaint(string color, JsonVisualizerSettings settings)
    {
        return new SKPaint
        {
            Color = GetColor(color),
            TextSize = settings.TextSize,
            Typeface = settings.Typeface,
            IsAntialias = settings.IsAntialias
        };
    }

    private static SKColor GetColor(string hexColor)
    {
        return SKColor.Parse(hexColor);
    }

    public void Dispose()
    {
        DelimiterPaint.Dispose();
        NumberPaint.Dispose();
        StringPaint.Dispose();
        BooleanPaint.Dispose();
        NullPaint.Dispose();
    }
}