<Query Kind="Program" />

// Prototype Pattern
// https://www.dofactory.com/net/prototype-design-pattern

void Main()
{
	ColorManager manager = new ColorManager();
	
	manager["red"] = new Color(255, 0, 0);
	manager["green"] = new Color(0, 255, 0);
	manager["blue"] = new Color(0, 0, 255);

	manager["angry"] = new Color(255, 54, 0);
	manager["peace"] = new Color(128, 211, 128);
	manager["flame"] = new Color(211, 34, 20);

	Color color1 = manager["red"].Clone() as Color;
	Color color2 = manager["peace"].Clone() as Color;
	Color color3 = manager["flame"].Clone() as Color;
}

abstract class ColorPrototype
{
	public abstract ColorPrototype Clone();
}

class Color : ColorPrototype
{
	private int _red;
	private int _green;
	private int _blue;
	
	public Color(int r, int g, int b)
	{
		_red = r;
		_green = g;
		_blue = b;
	}
	
	public override ColorPrototype Clone()
	{
		Console.WriteLine($"Cloning color RGB: {_red,3}, {_green,3}, {_blue,3}");
		return MemberwiseClone() as ColorPrototype;
	}
}
class ColorManager
{
	private Dictionary<string, ColorPrototype> _colors = new Dictionary<string, ColorPrototype>();
	public ColorPrototype this[string key]
	{
		get { return _colors[key]; }
		set { _colors.Add(key, value); }
	}
}