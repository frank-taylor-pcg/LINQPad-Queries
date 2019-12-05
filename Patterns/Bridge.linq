<Query Kind="Program" />

// Bridge pattern
// https://www.dofactory.com/net/bridge-design-pattern
// Don't fully have a grasp on this one. Seems simple enough, just can't imagine where I'd use it.

void Main()
{
	Customer customers = new Customer("Chicago");

	customers.Data = new CustomerData();

	customers.Show();
	customers.Next();
	customers.Show();
	customers.Next();
	customers.Show();
	customers.Add("Henry Velasquez");

	customers.ShowAll();
}

class CustomerBase
{
	protected string group;
	public CustomerBase(string group) { this.group = group; }
	private DataObject _data;
	public DataObject Data { get { return _data; } set { _data = value; } }
	public virtual void Next() => Data.NextRecord();
	public virtual void Prior() => Data.PriorRecord();
	public virtual void Add(string customer) => Data.AddRecord(customer);
	public virtual void Delete(string customer) => Data.DeleteRecord(customer);
	public virtual void Show() => Data.ShowRecord();
	public virtual void ShowAll()
	{
		Console.WriteLine($"Customer Group: {group}");
		_data.ShowAllRecords();
	}
}

class Customer : CustomerBase
{
	public Customer(string group) : base(group) {}
	public override void ShowAll()
	{
		Console.WriteLine();
		Console.WriteLine("".PadLeft(40, '-'));
		base.ShowAll();
		Console.WriteLine("".PadLeft(40, '-'));
	}
}

abstract class DataObject
{
	public abstract void NextRecord();
	public abstract void PriorRecord();
	public abstract void AddRecord(string name);
	public abstract void DeleteRecord(string name);
	public abstract void ShowRecord();
	public abstract void ShowAllRecords();
}

class CustomerData : DataObject
{
	private List<string> _customers = new List<string>();
	private int _current = 0;
	
	public CustomerData()
	{
		// Loaded from a database 
		_customers.Add("Jim Jones");
		_customers.Add("Samual Jackson");
		_customers.Add("Allen Good");
		_customers.Add("Ann Stills");
		_customers.Add("Lisa Giolani");
	}

	public override void AddRecord(string name) => _customers.Add(name);
	public override void DeleteRecord(string name) => _customers.Remove(name);
	public override void NextRecord()
	{
		if (_current < _customers.Count - 1)
			_current++;
	}
	public override void PriorRecord()
	{
		if (_current > 0)
			_current--;
	}
	public override void ShowRecord() => Console.WriteLine(_customers[_current]);
	public override void ShowAllRecords()
	{
		foreach (string customer in _customers)
		{
			Console.WriteLine(" " + customer);
		}
	}
}