<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	ShowCase();
}

private void ShowCase()
{
	Gradient gradient = new()
	{
		ID = "grad1",
		A = new Point(0, 0),
		B = new Point(100, 100)
	};
	gradient.Stops.Add(new GradientStop() { Color = "rgb(255,255,0)", Offset = 0, Opacity = 255 });
	gradient.Stops.Add(new GradientStop() { Color = "rgb(255,0,0)", Offset = 100, Opacity = 255 });

	Gradient gradient2 = new()
	{
		ID = "grad2",
		B = new Point(0, 0),
		A = new Point(100, 100)
	};
	gradient2.Stops.Add(new GradientStop() { Color = "rgb(0,255,255)", Offset = 0, Opacity = 255 });
	gradient2.Stops.Add(new GradientStop() { Color = "rgb(0,0,255)", Offset = 100, Opacity = 255 });

	Circle circle = new Circle();
	circle.Point = new Point(64, 64);
	circle.Style = new Style();
	circle.Radius = 64;
	circle.Style.FillColor = gradient.URL;

	Circle inner = new Circle();
	inner.Point = new Point(64, 64);
	inner.Style = new Style();
	inner.Radius = 40;
	inner.Style.FillColor = gradient2.URL;

	StringBuilder sbSvg = new StringBuilder();
	sbSvg.AppendLine("<defs>");
	sbSvg.Append(gradient.ToString());
	sbSvg.Append(gradient2.ToString());
	sbSvg.AppendLine("</defs>");
	sbSvg.AppendLine(circle.ToString());
	sbSvg.AppendLine(inner.ToString());
	
	Line line = new Line(new Point(0, 0), new Point(10, 10));
	sbSvg.AppendLine(line.ToString());
	
	PointList pointsA = new();
	PointList pointsB = new();
	
	int numPoints = 8;
	for (int i = 0; i < numPoints; i++)
	{
		double angle = i * (360 / numPoints);
		double radians = (angle * Math.PI) / 180.0;
		
		int x = (int)(Math.Cos(radians) * 32);
		int y = (int)(Math.Sin(radians) * 32);
		
		pointsA.Points.Add(new(x + 200, y + 40));
		pointsB.Points.Add(new(x + 300, y + 40));
	}

	Polygon polygon = new()
	{
		PointList = pointsA,
	};
	polygon.Style.FillColor = "green";
	sbSvg.AppendLine(polygon.ToString());


	// Doesn't connect the first and last points
	PolyLine polyline = new()
	{
		PointList = pointsB,
	};
	polyline.Style.FillColor = "red";
	sbSvg.AppendLine(polyline.ToString());
	
	Text text = new()
	{
		Content = "Example Text",
		Alignment = TextAlignId.MIDDLE,
		Position = new(500, 40),
		Style = new Style(1, "white", "blue")
	};
	
	sbSvg.AppendLine(text.ToString());

	sbSvg.ToString().Dump();
	new Svg(sbSvg.ToString(), 1000, 1000).Dump();
}

public enum TextAlignId
{
	START,
	MIDDLE,
	END,
}

public interface ISVGElement { }

public class SVGBuilder
{
	private StringBuilder sb = new();
	public int Width { get; set; } = 500;
	public int Height { get; set; } = 500;
	
	public SVGBuilder(int width, int height)
	{
		Width = width;
		Height = height;
	}

	public void Add(ISVGElement element) => sb.AppendLine(element.ToString());
	public void Add(string raw) => sb.AppendLine(raw);
	
	object ToDump() => new Svg(sb.ToString(), Width, Height);
}

public class Point : ISVGElement
{
	public int X { get; set; }
	public int Y { get; set; }
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
}

public class PointList : ISVGElement
{
	public List<Point> Points { get; set; } = new();
	
	public void Add(Point point) => Points.Add(point);
	public void Add(int x, int y) => Points.Add(new Point(x, y));

	public override string ToString()
	{
		StringBuilder sbResult = new StringBuilder();

		foreach (Point point in Points)
		{
			sbResult.Append($"{point.X},{point.Y} ");
		}

		return sbResult.ToString();
	}
	
	public PointList Offset(int x, int y)
	{
		PointList result = new();
		foreach (Point p in Points)
		{
			result.Add(p.X + x, p.Y + y);
		}
		return result;
	}
}

public class Polygon : ISVGElement
{
	public PointList PointList { get; set; } = new();
	public Style Style { get; set; } = new();
	public override string ToString() => $"<polygon points='{PointList}' {Style}/>";
	
	public Polygon() { }
	public Polygon(PointList p) { PointList = p; }
}

public class PolyLine : ISVGElement
{
	public PointList PointList { get; set; } = new();
	public Style Style { get; set; } = new();
	public override string ToString() => $"<polyline points='{PointList}' {Style}/>";
}

public class GradientStop : ISVGElement
{
	public int Offset { get; set; }
	public string Color { get; set; }
	public float Opacity { get; set; }

	public override string ToString() => $"<stop offset='{Offset}' style='stop-color:{Color};stop-opacity:{Opacity}%' />";
}

public class Gradient : ISVGElement
{
	public string ID { get; set; }
	public List<GradientStop> Stops { get; set; } = new List<GradientStop>();
	public Point A { get; set; }
	public Point B { get; set; }
	public string URL => $"url(#{ID})";

	public override string ToString()
	{
		StringBuilder sbResult = new StringBuilder();
		sbResult.AppendLine($"<linearGradient id='{ID}' x1='{A.X}%' y1='{A.Y}%' x2='{B.X}%' y2='{B.Y}%'>");
		foreach (GradientStop stop in Stops)
		{
			sbResult.AppendLine(stop.ToString());
		}
		sbResult.AppendLine("</linearGradient>");
		return sbResult.ToString();
	}
}

public class Style : ISVGElement
{
	public int StrokeWidth { get; set; } = 1;
	public string StrokeColor { get; set; } = "white";
	public string FillColor { get; set; } = "transparent";

	public Style(int strokeWidth = 1, string strokeColor = "white", string fillColor = "transparent")
	{
		StrokeWidth = strokeWidth;
		StrokeColor = strokeColor;
		FillColor = fillColor;
	}

	public override string ToString() => $"style='stroke-width:{StrokeWidth};stroke:{StrokeColor};fill:{FillColor}'";
}

public class Line : ISVGElement
{
	public Point A { get; set; }
	public Point B { get; set; }
	public Style Style { get; set; } = new();
	
	public Line(Point a, Point b)
	{
		A = a;
		B = b;
	}
	
	public override string ToString() => $"<line x1='{A.X}' y1='{A.Y}' x2='{B.X}' y2='{B.Y}' {Style} />";
}

public class Circle : ISVGElement
{
	public Point Point { get; set; }
	public Style Style { get; set; } = new();
	public int Radius { get; set; }
	public override string ToString() => $"<circle cx='{Point.X}' cy='{Point.Y}' r='{Radius}' {Style} />";
}

public class Ellipse : ISVGElement
{
	public Point Point { get; set; }
	public Style Style { get; set; } = new();
	public int Radius { get; set; }
}

public class Text : ISVGElement
{
	public string Content { get; set; }
	public Point Position { get; set; }
	public TextAlignId Alignment { get; set; }
	public Style Style { get; set; } = new();
	public int FontSize { get; set; } = 48;
	
	public Text(string text = null)
	{
		Content = text;
	}

	public override string ToString() => $"<text x='{Position.X}' y='{Position.Y}' text-anchor='{GetTextAnchor(Alignment)}' {Style} font-size='{FontSize}'>{Content}</text>";

	private string GetTextAnchor(TextAlignId alignId) =>
		alignId switch
		{
			TextAlignId.START => "start",
			TextAlignId.MIDDLE => "middle",
			TextAlignId.END => "end",
			_ => null
		};
}