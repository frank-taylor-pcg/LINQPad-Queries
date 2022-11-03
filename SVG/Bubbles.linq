<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

#load "Utility\SVGHelper"

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