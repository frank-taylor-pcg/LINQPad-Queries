<Query Kind="Program" />

public enum TextAlignId
{
	START,
	MIDDLE,
	END,
}

public static class SVGHelper
{
	public static string Line(int x1, int y1, int x2, int y2, string stroke = "white", int stroke_width = 2)
		=> $"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' stroke='{stroke}' stroke-width='{stroke_width}' />";

	public static string Circle(int x, int y, int r, string stroke = "white", string fill = "transparent", int stroke_width = 2)
		=> $"<circle cx='{x}' cy='{y}' r='{r}' stroke='{stroke}' fill='{fill}' stroke-width='{stroke_width}' />";
		
	private static string GetTextAnchor(TextAlignId alignId)
	{
		string result = null;
		
		switch (alignId)
		{
			case TextAlignId.START: result = "start"; break;
			case TextAlignId.MIDDLE: result = "middle"; break;
			case TextAlignId.END: result = "end"; break;
		}

		return result;
	}

	public static string Text(int x, int y, string text, TextAlignId anchor, string stroke = "white", string fill = "transparent", int stroke_width = 2)
		=> $"<text x='{x}' y='{y}' text-anchor='{GetTextAnchor(anchor)}' stroke='{stroke}' fill='{fill}' stroke-width='{stroke_width}'>{text}</text>";
}