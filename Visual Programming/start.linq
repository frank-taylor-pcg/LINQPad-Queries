<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	Node node = new Node()
	{
		Color = "crimson",
		Name = "This is a test"
	};
	node.Inputs.Add(new InputPin() { TypeClassification = TypeId.BOOLEAN, Name = "Boolean" });
	node.Inputs.Add(new InputPin() { TypeClassification = TypeId.CHARACTER, Name = "Character" });
	node.Inputs.Add(new InputPin() { TypeClassification = TypeId.INTEGER, Name = "Integer" });

	node.Outputs.Add(new OutputPin() { TypeClassification = TypeId.REAL, Name = "Real" });
	node.Outputs.Add(new OutputPin() { TypeClassification = TypeId.TEXT, Name = "Text" });
	node.Outputs.Add(new OutputPin() { TypeClassification = TypeId.COMPLEX, Name = "Complex" });
	
	node.Draw();
}

#region Enums
enum TypeId
{
	BOOLEAN,
	CHARACTER,
	INTEGER,
	REAL,
	TEXT,
	COMPLEX,
}
#endregion Enums

#region Pins
class Pin
{
	public TypeId TypeClassification { get; set; }
	public string Name { get; set; }
	
	private string DetermineColor()
	{
		string result = "white";
		switch (TypeClassification)
		{
			case TypeId.BOOLEAN: result = "red"; break;
			case TypeId.CHARACTER: result = "palevioletred"; break;
			case TypeId.INTEGER: result = "royalblue"; break;
			case TypeId.REAL: result = "greenyellow"; break;
			case TypeId.TEXT: result = "mediumpurple"; break;
			case TypeId.COMPLEX: result = "cyan"; break;
		}
		return result;
	}
	
	public virtual string GenerateSVG(int x, int y)
	{
		StringBuilder result = new StringBuilder();
		result.Append($"<circle cx='{x}' cy='{y}' r='3' stroke-width='2' stroke='{ DetermineColor() }' />");
		return result.ToString();
	}

	public void Draw(int x = 4, int y = 4)
	{
		Svg svg = new Svg(GenerateSVG(x, y), 5, 5).Dump();
	}
}
class InputPin : Pin
{
	public override string GenerateSVG(int x, int y)
	{
		StringBuilder result = new StringBuilder();

		result.Append($"<text x='{x + 10}' y='{y + 3}' text-anchor='start' stroke='white' font-size='smaller'>{Name}</text>");
		result.Append(base.GenerateSVG(x, y));

		return result.ToString();
	}
}
class OutputPin : Pin
{
	public override string GenerateSVG(int x, int y)
	{
		StringBuilder result = new StringBuilder();

		result.Append($"<text x='{x - 10}' y='{y + 3}' text-anchor='end' stroke='white' font-size='smaller'>{Name}</text>");
		result.Append(base.GenerateSVG(x, y));

		return result.ToString();
	}
}
#endregion Pins

#region Nodes
class Node
{
	public List<InputPin> Inputs { get; set; } = new List<InputPin>();
	public List<OutputPin> Outputs { get; set; } = new List<OutputPin>();
	public string Color { get; set; }
	public string Name { get; set; }
	
	public string GenerateSVG(int x, int y)
	{
		StringBuilder result = new StringBuilder();
		int iMaxPins = Math.Max(Inputs.Count, Outputs.Count);
		int iMaxInputLength = Inputs.Max(x => x.Name.Length);
		int iMaxOutputLength = Outputs.Max(x => x.Name.Length);
		int iHeight = (iMaxPins + 1) * 20;
		int iWidth = (iMaxInputLength + iMaxOutputLength) * 6 + 40;

		result.Append($"<rect x='{x}' y='{y}' width='{iWidth}' height='{iHeight}' rx='6' stroke='white' stroke-width='1' fill='{Color}' />");
		
		int py = y + 30;
		foreach (Pin input in Inputs)
		{
			result.Append(input.GenerateSVG(x, py));
			py += 20;
		}
		
		py = y + 30;
		foreach (Pin output in Outputs)
		{
			result.Append(output.GenerateSVG(x + iWidth, py));
			py += 20;
		}

		result.Append($"<text x='{iWidth / 2}' y='15' text-anchor='middle' stroke='white'>{Name}</text>");
		result.Append($"<line x1='{x}' y1='{y + 20}' x2='{x + iWidth}' y2='{y + 20}' stroke='white' />");
		
		return result.ToString();
	}
	
	public void Draw(int x = 0, int y = 0)
	{
		Svg svg = new Svg(GenerateSVG(x, y), 200, 200).Dump();
	}
}
#endregion Nodes