<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	SVGComplexAttribute style = SVGFactory.CreateStyle(3, "white");
	SVGElement circle = SVGFactory.CreateCircle(32, 32, 32);
	SVGElement ellipse = SVGFactory.CreateEllipse(64, 96, 64, 32);
//	SVGElement defs = SVGFactory.CreateDefs();
	SVGElement image = SVGFactory.CreateImage(0, 125, 100, 100, @"http://icons.iconarchive.com/icons/blackvariant/button-ui-system-apps/128/Automator-2-icon.png");
		
	SVGElement group = SVGFactory.CreateGroup(style);
	group.Children.Add(circle);
	group.Children.Add(ellipse);
	group.Children.Add(image);
	
//	Util.HorizontalRun(true, defs.ToString(), group.ToString()).Dump();
	
	StringBuilder sbCanvas = new StringBuilder();
	sbCanvas.Append(group);	
//	sbCanvas.ToString().Dump();
	
	new Svg(sbCanvas.ToString(), 500, 500).Dump();
	
//	new Svg(circle.ToString(), 100, 100).Dump();
//	new Svg(ellipse.ToString(), 100, 100).Dump();
}

public static class SVGFactory
{
	public static SVGComplexAttribute CreateStyle(int stroke_width, string stroke = null, string fill = null)
	{
		SVGComplexAttribute result = new SVGComplexAttribute("style");

		result.Properties.Add(new SVGProperty("stroke-width") { Value = stroke_width.ToString() });
		if (stroke != null) result.Properties.Add(new SVGProperty("stroke") { Value = stroke });
		if (fill != null) result.Properties.Add(new SVGProperty("fill") { Value = fill });

		return result;
	}
	public static SVGElement CreateCircle(int x, int y, int r, SVGComplexAttribute style = null)
	{
		SVGElement result = new SVGElement("circle");
		result.SimpleAttributes.Add(new SVGAttribute("cx", x.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("cy", y.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("r", r.ToString()));
		if (style != null) result.ComplexAttributes.Add(style);
		return result;
	}
	public static SVGElement CreateEllipse(int x, int y, int rx, int ry, SVGComplexAttribute style = null)
	{
		SVGElement result = new SVGElement("ellipse");
		result.SimpleAttributes.Add(new SVGAttribute("cx", x.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("cy", y.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("rx", rx.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("ry", ry.ToString()));
		if (style != null) result.ComplexAttributes.Add(style);
		return result;
	}
	public static SVGElement CreateGroup(SVGComplexAttribute style = null)
	{
		SVGElement result = new SVGElement("g");
		if (style != null) result.ComplexAttributes.Add(style);
		return result;
	}
	// Experimental feature, not yet supported in browsers.
	// This one needs more thought. I believe that it requires one or more hatchpath elements as children.
	public static SVGElement CreateHatch(int x, int y, int pitch, int rotate, int hatchUnits, SVGComplexAttribute style = null)
	{
		SVGElement result = new SVGElement("hatch");
		result.SimpleAttributes.Add(new SVGAttribute("x", x.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("y", y.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("pitch", pitch.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("rotate", rotate.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("hatchUnits", hatchUnits.ToString()));
		if (style != null) result.ComplexAttributes.Add(style);
		return result;
	}
	public static SVGElement CreateHatchPath(int stroke_width, string stroke = "white")
	{
		SVGElement result = new SVGElement("hatchpath");
		result.SimpleAttributes.Add(new SVGAttribute("stroke-width", stroke_width.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("stroke", stroke));
		return result;
	}
	public static SVGElement CreateDefs() => new SVGElement("defs");
	// There are a lot of options for preserveAspectRatio. I'll need to think about how to build this out.
	public static SVGElement CreateImage(int x, int y, int width, int height, string href, string preserveAspectRatio = "none")
	{
		SVGElement result = new SVGElement("image");
		result.SimpleAttributes.Add(new SVGAttribute("x", x.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("y", y.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("width", width.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("height", height.ToString()));
		result.SimpleAttributes.Add(new SVGAttribute("href", href));
		result.SimpleAttributes.Add(new SVGAttribute("preserveAspectRatio", preserveAspectRatio));
		return result;
	}
}

public class SVGProperty
{
	public string Name { get; set; }
	public string Value { get; set; }
	public SVGProperty(string strName) { Name = strName; }
	public SVGProperty(string strName, string strValue) { Name = strName; Value = strValue; }
	public override string ToString() => $"{Name}:{Value}";
}

public class SVGAttribute : SVGProperty
{
	public SVGAttribute(string strName) : base(strName) { }
	public SVGAttribute(string strName, string strValue) : base(strName, strValue) { }
	public override string ToString() => $"{Name}='{Value}'";
}

public class SVGComplexAttribute
{
	public string Name { get; set; }
	public List<SVGProperty> Properties { get; set; } = new List<SVGProperty>();
	public SVGComplexAttribute(string strName) { Name = strName; }
	public override string ToString() => $"{Name}='{string.Join(";", Properties)}'";
}

public class SVGElement
{
	public string Name { get; set; }
	public List<SVGAttribute> SimpleAttributes = new List<SVGAttribute>();
	public List<SVGComplexAttribute> ComplexAttributes = new List<SVGComplexAttribute>();
	public List<SVGElement> Children = new List<SVGElement>();
	
	public SVGElement(string strName) { Name = strName; }

	public override string ToString()
	{
		StringBuilder sbResult = new StringBuilder();
		sbResult.Append($"<{Name} ");
		sbResult.Append(string.Join(" ", SimpleAttributes));
		sbResult.Append(" ");
		sbResult.Append(string.Join(" ", ComplexAttributes));
		
		if (Children.Count() == 0)
		{
			sbResult.Append("/>");
		}
		else
		{
			sbResult.AppendLine(">");
			sbResult.AppendLine(string.Join("\n", Children));
			sbResult.AppendLine($"</{ Name }>");
		}
		return sbResult.ToString();
	}
}