<Query Kind="Program" />

// Memento pattern
// https://www.dofactory.com/net/memento-design-pattern

void Main()
{
	SalesProspect s = new SalesProspect()
	{
		Name = "Noel van Halen",
		Phone = "(412) 256-0990",
		Budget = 25_000.00
	};
	
	ProspectMemory m = new ProspectMemory();
	m.Memento = s.SaveMemento();
	
	s.Name = "Leo Welch";
	s.Phone = "(310) 209-7111";
	s.Budget = 1_000_000.00;
	
	s.RestoreMemento(m.Memento);
}


class SalesProspect
{
	private string _name;
	private string _phone;
	private double _budget;
	
	public string Name
	{
		get { return _name; }
		set
		{
			_name = value;
			Console.WriteLine("Name: " + _name);
		}
	}

	public string Phone
	{
		get { return _phone; }
		set
		{
			_phone = value;
			Console.WriteLine("Phone: " + _phone);
		}
	}

	public double Budget
	{
		get { return _budget; }
		set
		{
			_budget = value;
			Console.WriteLine("Budget: " + _budget);
		}
	}
	
	public Memento SaveMemento()
	{
		Console.WriteLine("\nSaving state --\n");
		return new Memento(_name, _phone, _budget);
	}
	
	public void RestoreMemento(Memento memento)
	{
		Console.WriteLine("\nRestoring state --\n");
		Name = memento.Name;
		Phone = memento.Phone;
		Budget = memento.Budget;
	}
}

class Memento
{
	public string Name { get; set; }
	public string Phone { get; set; }
	public double Budget { get; set; }
	public Memento(string name, string phone, double budget)
	{
		Name = name;
		Phone = phone;
		Budget = budget;
	}
}

class ProspectMemory
{
	public Memento Memento { get; set; }
}