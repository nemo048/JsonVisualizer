using SkiaSharp;

namespace JsonVisualizer.Models;

internal sealed class DrawingCursor : ICursor
{
    private const string COMMA = ",";
    private readonly SKCanvas _canvas;
    private readonly float _whitespaceSize;
    private readonly float _indentationSize;
    private readonly float _lineHeight;
    private readonly float _startX;
    internal float X { get; private set; }
    internal float Y { get; private set; }

    internal DrawingCursor(SKCanvas canvas,
        float whitespaceSize,
        float indentationSize,
        float lineHeight,
        float startX,
        float startY)
    {
        _canvas = canvas;
        _whitespaceSize = whitespaceSize;
        _indentationSize = indentationSize;
        _lineHeight = lineHeight;
        _indentationSize = _whitespaceSize * 2.0f;

        _startX = startX;
        
        X = startX;
        Y = startY;
    }

    public void DrawText(string text, SKPaint paint)
    {
        _canvas.DrawText(text, X, Y, paint);
        X += paint.MeasureText(text);
    }

    public void DrawIndentation(int indentationLevel)
    {
        X += indentationLevel * _indentationSize;
    }

    public void DrawWhiteSpace()
    {
        X += _whitespaceSize;
    }

    public void DrawLineBreak()
    {
        Y += _lineHeight;
        X = _startX;
    }

    public void DrawComma(SKPaint paint)
    {
        DrawText(COMMA, paint);
    }

    public void Dispose()
    {
        _canvas.Dispose();
    }
}