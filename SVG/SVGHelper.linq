<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

DumpContainer dcScreen = new DumpContainer();
string strCanvas = SVGHelper.Rectangle(0, 0, 1600, 900, 0, null, "darkblue");

class Ball
{
	public int x;
	public int y;
	public string pColor;
	public string sColor;
};

List<Ball> balls = new List<Ball>();

void Main()
{
	dcScreen.Dump();
	InitializeSimulation();

}

public void InitializeSimulation()
{
	Random rand = new Random();
	for (int i = 0; i < 20; i++)
	{
		balls.Add(new Ball()
		{
			x = rand.Next(16, 1600 - 16),
			y = rand.Next(16, 900 - 16),
			pColor = "darkcyan",
			sColor = "cyan",
		});
	}
}

public string RunSimulation()
{
	//while (true)
	{
		string strSim = Draw()

		dcScreen.Content = new Svg(strSim, 1600, 900);
		dcScreen.Refresh();
		Thread.Sleep(10);
	}
}

public string Draw()
{
	StringBuilder sbResult = new StringBuilder();
	sbResult.Append(strCanvas);

	foreach (Ball b in balls)
	{
		sbResult.Append(DrawBubble(b.x, b.y, b.pColor, b.sColor));
	}

	return sbResult.ToString();
}

public string DrawBubble(int x, int y, string primaryColor, string secondaryColor)
{
	StringBuilder sbResult = new StringBuilder();
	
	sbResult.Append(SVGHelper.Circle(x, y, 32, 0, null, primaryColor));
	sbResult.Append(SVGHelper.Circle(x + 12, y - 12, 8, 0, null, secondaryColor));
	
	return sbResult.ToString();
}

public class Point
{
	public int X { get; set; }
	public int Y { get; set; }
}

public static class SVGHelper
{
	public static int StrokeWidth = 1;
	public static string StrokeColor = "white";
	public static string FillColor = "magenta";
	
	private static string GetStyle(int strokeWidth, string strokeColor, string fillColor)
		=> $"stroke-width:{strokeWidth};stroke:{strokeColor ?? StrokeColor};fill:{fillColor ?? FillColor}";

	public static string Rectangle(int x, int y, int width, int height, int strokeWidth = 1, string strokeColor = null, string fillColor = null)
		 => $"<rect x='{x}' y='{y}' width='{width}' height='{height}' style='{ GetStyle(strokeWidth, strokeColor, fillColor) }'/>";

	public static string Circle(int x, int y, int r, int strokeWidth = 1, string strokeColor = null, string fillColor = null)
		=> $"<circle cx='{x}' cy='{y}' r='{r}' style='{ GetStyle(strokeWidth, strokeColor, fillColor) }'/>";

	public static string Ellipse(int x, int y, int rx, int ry, int strokeWidth = 1, string strokeColor = null, string fillColor = null)
		=> $"<circle cx='{x}' cy='{y}' rx='{rx}' ry='{ry}' style='{ GetStyle(strokeWidth, strokeColor, fillColor) }'/>";

	public static string Line(int x1, int y1, int x2, int y2, int strokeWidth = 1, string strokeColor = null, string fillColor = null)
		=> $"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' style='{ GetStyle(strokeWidth, strokeColor, fillColor) }'/>";
	
	private static string PointList(List<Point> points)
	{
		StringBuilder sbResult = new StringBuilder();

		foreach (Point point in points)
		{
			sbResult.Append($"{point.X}, {point.Y} ");
		}

		return sbResult.ToString();
	}
	public static string Polygon(List<Point> points, int strokeWidth = 1, string strokeColor = null, string fillColor = null)
		=> $"<polygon points='{ PointList(points) }' style='{ GetStyle(strokeWidth, strokeColor, fillColor) }'/>";

	public static string PolyLine(List<Point> points, int strokeWidth = 1, string strokeColor = null, string fillColor = null)
		=> $"<polyline points='{ PointList(points) }' style='{ GetStyle(strokeWidth, strokeColor, fillColor) }'/>";

	public static string Text(int x, int y, string strText, 
}