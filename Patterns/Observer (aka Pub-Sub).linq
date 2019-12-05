<Query Kind="Program" />

// Observer Pattern, aka Pub-Sub (Publisher-Subscriber)
// https://www.dofactory.com/net/observer-design-pattern

void Main()
{
	Subject store = new Subject();
	List<Observer> customers = new List<Observer>();
	
	for (int i = 0; i < 10; i++)
	{
		Observer observer = new Observer($"Observer {i}");
		store.Subscribe(observer);
		customers.Add(observer);
	}
	
	store.Inventory++;
	
	Console.WriteLine("Some customers unsubscribing from spam emails");
	
	int index = 0;
	foreach (Observer customer in customers)
	{
		if (index % 2 == 1)
		{
			store.Unsubscribe(customer);
		}
		index++;
	}
	
	store.Inventory++;
}

public class Subject : ISubject
{
	private List<Observer> observers = new List<Observer>();
	private int _inventory;
	public int Inventory
	{
		get { return _inventory; }
		set
		{
			// Only notify the observers if the inventory has increased
			if (value > _inventory)
				Notify();
			_inventory = value;
		}
	}
	public void Subscribe(Observer observer) => observers.Add(observer);
	public void Unsubscribe(Observer observer) => observers.Remove(observer);
	public void Notify() { observers.ForEach(x => x.Update()); }
}

public interface ISubject
{
	void Subscribe(Observer observer);
	void Unsubscribe(Observer observer);
	void Notify();
}

public class Observer : IObserver
{
	public string Name { get; private set; }
	public Observer(string name) { Name = name; }
	public void Update() { Console.WriteLine($"{Name}: A new product has arrived at the store"); }
}

public interface IObserver
{
	void Update();
}