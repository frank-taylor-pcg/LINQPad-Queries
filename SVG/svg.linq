<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	// Circle tests
	new Svg(SVGHelper.Circle(0, 0, 4), 4, 4).Dump();
	new Svg(SVGHelper.Circle(0, 0, 4, "cyan"), 4, 4).Dump();
	new Svg(SVGHelper.Circle(0, 0, 4, "red", "yellow"), 4, 4).Dump();

	// Rect tests
	new Svg(SVGHelper.Rect(0, 0, 20, 20), 20, 20).Dump();
	new Svg(SVGHelper.Rect(0, 0, 20, 20, 4), 20, 20).Dump();
	new Svg(SVGHelper.Rect(0, 0, 20, 20, 3, "blue"), 20, 20).Dump();
	new Svg(SVGHelper.Rect(0, 0, 20, 20, 2, "orange", "silver"), 20, 20).Dump();
	new Svg(SVGHelper.Rect(0, 0, 20, 20, 1, "white", "gray", 2), 20, 20).Dump();

	// Line tests
	new Svg(SVGHelper.Line(0, 0, 10, 10), 10, 10).Dump();
	new Svg(SVGHelper.Line(0, 10, 10, 0, "pink"), 10, 10).Dump();
	new Svg(SVGHelper.Line(0, 0, 10, 10, "brown", 3), 10, 10).Dump();

	// Text tests
	new Svg(SVGHelper.Text(0, 0, "Testing"), 1, 1).Dump();
	new Svg(SVGHelper.Text(50, 0, "Testing", SVGHelper.TextAlign.MIDDLE), 1, 1).Dump();
	new Svg(SVGHelper.Text(50, 0, "Testing", SVGHelper.TextAlign.END, "red"), 1, 1).Dump();
}

public static class SVGHelper
{
	public enum TextAlign { START, MIDDLE, END }
	
	public static string Circle(int x, int y, int r, string stroke = "white", string fill = null)
	{
		return $"<circle cx='{x}' cy='{y}' r='{r}' stroke='{stroke}' stroke-width='2' fill='{fill}' />";
	}
	
	public static string Rect(int x, int y, int width, int height, int r = 0, string stroke = "white", string fill = null, int strokeWidth = 1)
	{
		return $"<rect x='{x}' y='{y}' width='{width}' height='{height}' rx='{r}' stroke='{stroke}' fill='{fill}' stroke-width='{strokeWidth}' />";
	}
	
	public static string Line(int x1, int y1, int x2, int y2, string stroke = "white", int strokeWidth = 1)
	{
		return $"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' stroke='{stroke}' stroke-width='{strokeWidth}' />";
	}
	
	public static string Text(int x, int y, string text, TextAlign align = TextAlign.START, string stroke = "white")
	{
		string anchor = null;
		switch (align)
		{
			case TextAlign.START : anchor = "start"; break;
			case TextAlign.MIDDLE: anchor = "middle"; break;
			case TextAlign.END   : anchor = "end"; break;
		}
		return $"<text x='{x}' y='{y}' text-anchor='{anchor}' stroke='{stroke}' font-size='smaller'>{text}</text>";
	}
}
