using System.Text.Json;
using JsonVisualizer.Models;
using JsonVisualizer.Settings;
using SkiaSharp;

namespace JsonVisualizer.Helpers;

public sealed class JsonDrawer : IDisposable
{
    private readonly Stream _jsonStream;
    private readonly bool _disposeStream;
    private readonly float _lineHeight;
    private readonly float _indentSize;
    private readonly float _whitespaceSize;
    private readonly float _padding;
    private readonly JsonVisualizerPalette _palette;

    public JsonDrawer(
        Stream jsonStream, 
        JsonVisualizerSettings settings,
        bool disposeStream = false)
    {
        _jsonStream = jsonStream;
        _disposeStream = disposeStream;

        _palette = new JsonVisualizerPalette(settings);

        _lineHeight = CalculateLineHeight(_palette.StringPaint);
        _whitespaceSize = CalculateWhiteSpaceSize(_palette.StringPaint);
        _indentSize = _whitespaceSize * 2;
        _padding = settings.Padding;
    }

    private static float CalculateWhiteSpaceSize(SKPaint stringPaint)
    {
        return stringPaint.MeasureText(' '.ToString());
    }

    public JsonDrawer(Stream jsonStream, bool disposeStream = false) : 
        this(jsonStream, JsonVisualizerSettings.Default, disposeStream)
    {
        
    }
    
    private static float CalculateLineHeight(SKPaint paint)
    {
        SKFontMetrics metrics = paint.FontMetrics;
        
        float ascent = Math.Abs(metrics.Ascent);
        float descent = Math.Abs(metrics.Descent);
        float leading = metrics.Leading;
        float lineHeight = ascent + descent + leading;
        
        return lineHeight;
    }

    public async Task DrawJsonAsync(Stream targetStream, CancellationToken cancellationToken = default)
    {
        using (JsonDocument jsonDocument = await JsonDocument
                   .ParseAsync(_jsonStream, cancellationToken: cancellationToken).ConfigureAwait(false))
        using (SKBitmap bitmap = GetBitmap(jsonDocument))
        using (SKCanvas canvas = new(bitmap))
        using (DrawingCursor drawingCursor = new(canvas, _whitespaceSize, _indentSize, _lineHeight, startX: _padding, startY: _lineHeight))
        using (JsonCursorEnumerator jsonCursorEnumerator = new(drawingCursor, _palette))
        {
            canvas.DrawColor(_palette.BackgroundColor);

            jsonCursorEnumerator.Enumerate(jsonDocument.RootElement);

            using (SKImage skImage = SKImage.FromBitmap(bitmap))
            using (SKData skData = skImage.Encode(SKEncodedImageFormat.Png, 100))
            await using (Stream dataStream = skData.AsStream())
            {
                await dataStream.CopyToAsync(targetStream, cancellationToken);
            }
        }
    }

    private SKBitmap GetBitmap(JsonDocument jsonDocument)
    {
        using (SizeCalculationCursor sizeCalculationCursor = new(_whitespaceSize, _indentSize, _lineHeight, startX: _padding, startY: _lineHeight, _padding))
        using (JsonCursorEnumerator jsonCursorEnumerator = new(sizeCalculationCursor, _palette))
        {
            jsonCursorEnumerator.Enumerate(jsonDocument.RootElement);
            
            return new SKBitmap(sizeCalculationCursor.GetWidth(), sizeCalculationCursor.GetHeight());
        }
    }

    public void Dispose()
    {
        if (_disposeStream)
        {
            _jsonStream.Dispose();
        }
        
        _palette.Dispose();
    }
}