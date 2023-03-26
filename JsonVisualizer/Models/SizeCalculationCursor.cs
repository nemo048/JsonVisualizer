using SkiaSharp;

namespace JsonVisualizer.Models;

internal sealed class SizeCalculationCursor : ICursor
{
    private readonly float _whitespaceSize;
    private readonly float _lineHeight;
    private readonly float _indentationSize;
    private readonly float _padding;
    private readonly float _startX;

    private float X
    {
        get { return _x; }
        set
        {
            _x = value;
            if (value > _width)
            {
                _width = value;
            }
        }
    }

    private float _width;
    private float _x;

    internal int GetWidth()
    {
        return (int) Math.Ceiling(_width + _padding);
    }

    internal int GetHeight()
    {
        return (int) Math.Ceiling(Y + _padding);
    }

    internal float Y { get; private set; }

    internal SizeCalculationCursor(
        float whitespaceSize,
        float indentationSize,
        float lineHeight,
        float startX,
        float startY,
        float padding)
    {
        _whitespaceSize = whitespaceSize;
        _lineHeight = lineHeight;
        _indentationSize = indentationSize;

        _startX = startX;
        _padding = padding;

        Y = startY;
    }

    public void DrawText(string text, SKPaint paint)
    {
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
        X++;
    }

    public void Dispose()
    {
        //nothing to dispose
    }
}