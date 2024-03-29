<Query Kind="Program" />

// Composite Pattern
// https://www.dofactory.com/net/composite-design-pattern

void Main()
{
	CompositeElement root = new CompositeElement("Picture");
	root.Add(new PrimitiveElement("Red Line"));
	root.Add(new PrimitiveElement("Blue Circle"));
	root.Add(new PrimitiveElement("Green Box"));
	
	CompositeElement comp = new CompositeElement("Two Circles");
	comp.Add(new PrimitiveElement("Black Circle"));
	comp.Add(new PrimitiveElement("White Circle"));
	root.Add(comp);
	
	PrimitiveElement pe = new PrimitiveElement("Yellow Line");
	root.Add(pe);
	root.Display(1);
	root.Remove(pe);
	
	Console.WriteLine("\n");
	
	root.Display(1);
}

abstract class DrawingElement
{
	protected string _name;
	public DrawingElement(string name) => this._name = name;
	public abstract void Add(DrawingElement d);
	public abstract void Remove(DrawingElement d);
	public abstract void Display(int indent);
}

class PrimitiveElement : DrawingElement
{
	public PrimitiveElement(string name) : base(name) {}
	public override void Add(DrawingElement d) => Console.WriteLine("Cannot add to a PrimitiveElement");
	public override void Remove(DrawingElement d) => Console.WriteLine("Cannot remove from a PrimitiveElement");
	public override void Display(int indent) => Console.WriteLine($"{new string('-', indent)} {_name}");
}

class CompositeElement : DrawingElement
{
	private List<DrawingElement> _elements = new List<DrawingElement>();
	public CompositeElement(string name) : base(name) { }
	public override void Add(DrawingElement d) => _elements.Add(d);
	public override void Remove(DrawingElement d) => _elements.Remove(d);
	public override void Display(int indent)
	{
		Console.WriteLine($"{new string('-', indent)}+ {_name}");
		_elements.ForEach(x => x.Display(indent + 2));
	}
}