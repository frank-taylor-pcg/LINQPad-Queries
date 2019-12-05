<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	//List<string> paths = new List<string>();
	//paths.Add($"<path d='M 10 80 C 40 10, 65 10, 95, 80 S 150 150, 180 80' stroke='white' fill='transparent' />");
	//paths.Add($"<path d='M 10 80 L 40 10' stroke='red' />");
	//paths.Add($"<path d='M 65 10 L 95 80' stroke='red' />");
	//Svg svg = new Svg(string.Join('\n', paths.ToArray()), 1, 1).Dump();

	new Svg(BuildCurve(10, 10, 200, 90), 1, 1).Dump();
}

string BuildCurve(int x1, int y1, int x2, int y2)
{
	string[] points = new string[6];

	StringBuilder sb = new StringBuilder();
	
	// The midpoint of the curve
	int mx = (x1 + x2) / 2;
	int my = (y1 + y2) / 2;

	// Start here
	points[0] = $"{x1} {y1}";
	// Curve towards this
	points[1] = $"{x1 + 50} {y1}";
	// Then back towards this
	points[2] = $"{x1 + 50} {y2}";
	// End the first half of the curve here (the midpoint)
	points[3] = $"{ mx } { my }";
	// Reflect the previous control point towards this
	points[4] = $"{x2 - 50} {y2}";
	// End the curve here
	points[5] = $"{x2} {y2}";


	sb.Append($"<path d='M {points[0]} C {points[1]}, {points[2]}, {points[3]} S {points[4]}, {points[5]}' stroke='white' fill='transparent' />");

	return sb.ToString();
}
