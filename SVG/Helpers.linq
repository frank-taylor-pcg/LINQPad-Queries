<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	Gradient gradient = new Gradient();
	gradient.ID = "grad1";
	gradient.X1 = 0;
	gradient.Y1 = 0;
	gradient.X2 = 100;
	gradient.Y2 = 0;
	gradient.Stops.Add(new GradientStop() { Color = "rgb(255,255,0)", Offset = 0, Opacity = 1 });
	gradient.Stops.Add(new GradientStop() { Color = "rgb(255,0,0)", Offset = 100, Opacity = 1 });

	Circle circle = new Circle();
	circle.Position = new Position(32, 32);
	circle.Style = new Style();
	circle.Radius = 32;
	circle.Style.FillColor = gradient.URL;
	
	Circle inner = new Circle();
	inner.Position = new Position(32, 32);
	inner.Style = new Style();
	inner.Radius = 24;
	inner.Style.FillColor = gradient.URL;

	StringBuilder sbSvg = new StringBuilder();
	sbSvg.AppendLine("<defs>");
	sbSvg.Append(gradient.ToString());
	sbSvg.AppendLine("</defs>");
	sbSvg.Append(circle.ToString());
	sbSvg.Append(inner.ToString());
	
	sbSvg.ToString().Dump();
	new Svg(sbSvg.ToString(), 100, 100).Dump();
}

public class Position
{
	public int X { get; set; }
	public int Y { get; set; }
	public Position(int x, int y)
	{
		X = x;
		Y = y;
	}
}

public class GradientStop
{
	public int Offset { get; set; }
	public string Color { get; set; }
	public float Opacity { get; set; }

	public override string ToString() => $"<stop offset='{Offset}' style='stop-color:{Color};stop-opacity:{Opacity}%' />";
}
public class Gradient
{
	public string ID { get; set; }
	public List<GradientStop> Stops {get; set; } = new List<GradientStop>();
	public int X1 { get; set; }
	public int Y1 { get; set; }
	public int X2 { get; set; }
	public int Y2 { get; set; }

	public string URL => $"url(#{ID})";

	public override string ToString()
	{
		StringBuilder sbResult = new StringBuilder();
		sbResult.AppendLine($"<linearGradient id='{ID}' x1='{X1}%' y1='{Y1}%' x2='{X2}%' y2='{Y2}%'>");
		foreach (GradientStop stop in Stops)
		{
			sbResult.AppendLine(stop.ToString());
		}
		sbResult.AppendLine("</linearGradient>");
		return sbResult.ToString();
	}
}

public class Style
{
	public int StrokeWidth { get; set; }
	public string StrokeColor { get; set; }
	public string FillColor { get; set; }
	
	public Style(int strokeWidth = 1, string strokeColor = "white", string fillColor = "transparent")
	{
		StrokeWidth = strokeWidth;
		StrokeColor = strokeColor;
		FillColor = fillColor;
	}

	public override string ToString() => $"style='stroke-width:{StrokeWidth};stroke:{StrokeColor};fill:{FillColor}'";
}

public class Circle
{
	public Position Position { get; set; }
	public Style Style { get; set; }
	public int Radius { get; set; }
	public override string ToString() => $"<circle cx='{Position.X}' cy='{Position.Y}' r='{Radius}' {Style} />";
}

public class Ellipse
{
	public Position Position { get; set; }
	public Style Style { get; set; }
	public int Radius { get; set; }
}
