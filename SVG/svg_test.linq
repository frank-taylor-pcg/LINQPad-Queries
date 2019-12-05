<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>static UserQuery.SVGHelper</Namespace>
</Query>

#load ".\svg.linq"

void Main()
{
	InputPin ip = new InputPin() { Name = "Testing", TypeId = PinTypeId.UNKNOWN };
	ip.Draw();
	OutputPin op = new OutputPin() { Name = "Testing", TypeId = PinTypeId.COMPLEX };
	op.Draw(50, 0);
	Node n = new Node() { Name = "Test Node", TypeId = NodeTypeId.VARIABLE };
	n.Inputs.Add(new InputPin() { Name = "Boolean", TypeId = PinTypeId.BOOLEAN });
	n.Inputs.Add(new InputPin() { Name = "Character", TypeId = PinTypeId.CHARACTER });
	n.Inputs.Add(new InputPin() { Name = "Integer", TypeId = PinTypeId.INTEGER });

	n.Outputs.Add(new OutputPin() { Name = "Real", TypeId = PinTypeId.REAL });
	n.Outputs.Add(new OutputPin() { Name = "String", TypeId = PinTypeId.STRING });
	n.Outputs.Add(new OutputPin() { Name = "Complex", TypeId = PinTypeId.COMPLEX });
	n.Draw();
}

void DrawGrid()
{
	string svg = $"<hatch id='hatch' hatchUnits='userSpaceOnUse' pitch='5' rotate='135'><hatchpath stroke='#a080ff' stroke-width='2' /></ hatch > ";
}

#region Pins
enum PinTypeId
{
	UNKNOWN,
	BOOLEAN,
	CHARACTER,
	INTEGER,
	REAL,
	STRING,
	COMPLEX,
}

// Not sure if it's necessary to have separate input and output pin types, but just in case I'll abstract this for now
abstract class Pin
{
	protected const int radius = 4;
	public string Name { get; set; }
	public string VariableName { get; set; } // The underlying variable name.  Make it so that it converts the name to pascal case or something...
	public PinTypeId TypeId { get; set; } = PinTypeId.UNKNOWN;
	protected TextAlign TextAnchor { get; set; } = TextAlign.START;
	protected int TextOffset { get; set; } = 0;
	
	private string DetermineColor()
	{
		switch (TypeId)
		{
			case PinTypeId.BOOLEAN : return "red";
			case PinTypeId.CHARACTER : return "orange";
			case PinTypeId.INTEGER : return "blue";
			case PinTypeId.REAL : return "yellowgreen";
			case PinTypeId.STRING : return "mediumpurple";
			case PinTypeId.COMPLEX : return "cyan";
			default : return "gray";
		}
	}
	
	public string GenerateSVG(int x, int y)
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine(Circle(x, y, radius, DetermineColor()));
		sb.AppendLine(Text(x + TextOffset, y + 3, Name, TextAnchor));
		return sb.ToString();
	}
	
	public void Draw(int x = 0, int y = 0)
	{
		Svg svg = new Svg(GenerateSVG(x, y), 1, 1).Dump();
	}
}

class InputPin : Pin
{
	public InputPin() { TextAnchor = TextAlign.START; TextOffset = 10; }
}

class OutputPin : Pin
{
	public OutputPin() { TextAnchor = TextAlign.END; TextOffset = -10; }
}
#endregion Pins

#region Nodes
enum NodeTypeId
{
	UNKNOWN,
	VARIABLE,
	FUNCTION,
	OTHER
}
class Node
{
	public string Name { get; set; }
	public NodeTypeId TypeId { get; set; } = NodeTypeId.UNKNOWN;
	public List<InputPin> Inputs { get; set; } = new List<InputPin>();
	public List<OutputPin> Outputs { get; set; } = new List<OutputPin>();
	
	private string DetermineColor()
	{
		switch (TypeId)
		{
			case NodeTypeId.OTHER: return "maroon";
			case NodeTypeId.VARIABLE: return "darkblue";
			case NodeTypeId.FUNCTION: return "indigo";
			default: return "gray";
		}
	}
	
	private int CalculateWidth()
	{
		int iMaxInputWidth = Inputs.Max(x => x.Name.Length);
		int iMaxOutputWidth = Inputs.Max(x => x.Name.Length);
		return Math.Max(iMaxInputWidth + iMaxOutputWidth, Name.Length) * 6 + 20;
	}
	
	private int CalculateHeight()
	{
		return (Math.Max(Inputs.Count, Outputs.Count) + 1) * 20;
	}
	
	public string GenerateSVG(int x, int y)
	{
		StringBuilder sb = new StringBuilder();
		
		int iWidth = CalculateWidth();
		int iHeight = CalculateHeight();

		sb.AppendLine(Rect(x, y, iWidth, iHeight, 4, "white", DetermineColor()));
		sb.AppendLine(Text(x + (iWidth / 2), y + 13, Name, TextAlign.MIDDLE));
		sb.AppendLine(Line(x, y + 20, x + iWidth, y + 20));
		
		int py = 30;
		foreach (InputPin input in Inputs)
		{
			sb.AppendLine(input.GenerateSVG(x, py));
			py += 20;
		}
		
		py = 30;
		foreach (OutputPin output in Outputs)
		{
			sb.AppendLine(output.GenerateSVG(x + iWidth, py));
			py += 20;
		}

		return sb.ToString();
	}
	
	public void Draw(int x = 0, int y = 0)
	{
		Svg svg = new Svg(GenerateSVG(x, y), 1, 1).Dump();
	}
}
#endregion Nodes