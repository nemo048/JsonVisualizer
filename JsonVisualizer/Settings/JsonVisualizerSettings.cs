﻿using System.Reflection;
using System.Text.Json.Serialization;
using SkiaSharp;

namespace JsonVisualizer.Settings;

public sealed class JsonVisualizerSettings
{
    public JsonVisualizerPaletteSettings PaletteSettings { get; }
    public float TextSize { get; }
    public float Padding { get; }
    public SKTypeface Typeface { get; }
    public bool IsAntialias { get; }

    public JsonVisualizerSettings(
        JsonVisualizerPaletteSettings paletteSettings,
        float textSize,
        float padding,
        SKTypeface typeface,
        bool isAntialias)
    {
        PaletteSettings = paletteSettings;
        TextSize = textSize;
        Padding = padding;
        Typeface = typeface;
        IsAntialias = isAntialias;
    }
    
    public JsonVisualizerSettings(
        float textSize,
        float padding,
        SKTypeface typeface,
        bool isAntialias) : 
        this(JsonVisualizerPaletteSettings.Default,
            textSize,
            padding,
            typeface,
            isAntialias)
    {
    }
    
    public JsonVisualizerSettings With(
        JsonVisualizerPaletteSettings? palette = null,
        float? textSize = null,
        float? padding = null,
        SKTypeface? typeface = null,
        bool? isAntialias = null)
    {
        return new JsonVisualizerSettings(
            palette ?? PaletteSettings,
            textSize ?? TextSize,
            padding ?? Padding,
            typeface ?? Typeface,
            isAntialias ?? IsAntialias);
    }

    #region Default

    public static readonly JsonVisualizerSettings Default =
        new(textSize: 32.0f,
            padding: 10.0f,
            GetDefaultTypeface(),
            isAntialias: true);

    private static SKTypeface GetDefaultTypeface()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        using (Stream resourceStream = assembly.GetManifestResourceStream(@"JsonVisualizer.Fonts.JetBrainsMonoNL-Medium.ttf"))
        {
            return SKTypeface.FromStream(resourceStream);
        }
    }

    #endregion
}

public sealed class JsonVisualizerPaletteSettings
{
    public string BackgroundColor { get; }
    public string StringColor { get; }
    public string NumberColor { get; }
    public string BooleanColor { get; }
    public string DelimiterColor { get; }
    public string NullColor { get; }

    public JsonVisualizerPaletteSettings(
        string? backgroundColor,
        string? stringColor,
        string? numberColor,
        string? booleanColor,
        string? delimiterColor,
        string? nullColor)
    {
        BackgroundColor = backgroundColor ?? Default.BackgroundColor;
        StringColor = stringColor ?? Default.StringColor;
        NumberColor = numberColor ?? Default.NumberColor;
        BooleanColor = booleanColor ?? Default.BooleanColor;
        DelimiterColor = delimiterColor ?? Default.DelimiterColor;
        NullColor = nullColor ?? Default.NullColor;
    }

    #region Default

    public static readonly JsonVisualizerPaletteSettings Default =
        new(backgroundColor: "#262a2b",
            stringColor: "#87ff8d",
            numberColor: "#cb686d",
            booleanColor: "#a180ff",
            delimiterColor: "#e7e8e3",
            nullColor: "#fd9f54");

    #endregion
}