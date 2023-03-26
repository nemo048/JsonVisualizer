using SkiaSharp;

namespace JsonVisualizer.Models;

internal interface ICursor : IDisposable
{
    void DrawText(string text, SKPaint paint);
    void DrawIndentation(int indentationLevel);
    void DrawWhiteSpace();
    void DrawLineBreak();
    void DrawComma(SKPaint paint);
}
