<Query Kind="Program" />

// Adapter Pattern
// https://www.dofactory.com/net/adapter-design-pattern
void Main()
{
	Compound compound = new Compound("Unknown");
	compound.Display();
	compound = new RichCompound("Water");
	compound.Display();
	compound = new RichCompound("Benzene");
	compound.Display();
	compound = new RichCompound("Ethanol");
	compound.Display();
}

class Compound
{
	protected string _chemical;
	protected float _boilingPoint;
	protected float _meltingPoint;
	protected double _molecularWeight;
	protected string _molecularFormula;
	public Compound(string chemical) { _chemical = chemical; }
	public virtual void Display() { Console.WriteLine($"Compound: {_chemical}"); }
}

class RichCompound : Compound
{
	private ChemicalDatabank _bank;
	public RichCompound(string name) : base(name) {}
	public override void Display()
	{
		_bank = new ChemicalDatabank();
		_boilingPoint = _bank.GetCriticalPoint(_chemical, "B");
		_meltingPoint = _bank.GetCriticalPoint(_chemical, "M");
		_molecularWeight = _bank.GetMolecularWeight(_chemical);
		_molecularFormula = _bank.GetMolecularStructure(_chemical);
		
		base.Display();
		Console.WriteLine($" Formula: {_molecularFormula}");
		Console.WriteLine($" Weight: {_molecularWeight}");
		Console.WriteLine($" Melting point: {_meltingPoint}");
		Console.WriteLine($" Boiling point: {_boilingPoint}");
	}
}

// The "Legacy API"
class ChemicalDatabank
{
	public float GetCriticalPoint(string compound, string point)
	{
		if (point == "M")
		{
			switch (compound.ToUpper())
			{
				case "WATER": return 0.0f;
				case "BENZENE": return 5.5f;
				case "ETHANOL": return -114.1f;
				default: return 0f;
			}
		}
		else
		{
			switch (compound.ToUpper())
			{
				case "WATER": return 100.0f;
				case "BENZENE": return 80.1f;
				case "ETHANOL": return 78.3f;
				default: return 0f;
			}
		}
	}
	
	public string GetMolecularStructure(string compound)
	{
		switch (compound.ToUpper())
		{
			case "WATER": return "H20";
			case "BENZENE": return "C6H6";
			case "ETHANOL": return "C2H5OH";
			default: return "";
		}
	}
	
	public double GetMolecularWeight(string compound)
	{
		switch (compound.ToUpper())
		{
			case "WATER": return 18.105;
			case "BENZENE": return 78.1134;
			case "ETHANOL": return 46.0688;
			default: return 0d;
		}
	}
}