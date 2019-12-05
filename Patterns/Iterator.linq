<Query Kind="Program" />

// Iterator pattern
// https://www.dofactory.com/net/iterator-design-pattern
// I need to work on this one a bit more, it's not cemented in my mind

void Main()
{
	Collection collection = new Collection();
	for (int i = 0; i < 10; i++)
	{
		collection[i] = new Item($"Item {i}");
	}
	
	Iterator iterator = collection.CreateIterator();
	
	iterator.Step = 2;
	
	Console.WriteLine("Iterating over collection:");
	
	for (Item item = iterator.First(); !iterator.IsDone; item = iterator.Next())
	{
		Console.WriteLine(item.Name);
	}
}

class Item
{
	public string Name { get; set; }
	public Item(string name) { Name = name; }
}

interface IAbstractCollection
{
	Iterator CreateIterator();
}

class Collection : IAbstractCollection
{
	private ArrayList _items = new ArrayList();
	
	public Iterator CreateIterator() => new Iterator(this);
	public int Count => _items.Count;
	public object this[int index]
	{
		get { return _items[index]; }
		set { _items.Add(value); }
	}
}

interface IAbstractIterator
{
	Item First();
	Item Next();
	bool IsDone { get; }
	Item CurrentItem { get; }
}

class Iterator : IAbstractIterator
{
	private Collection _collection;
	private int _current = 0;

	public int Step { get; set; } = 1;
	public Iterator(Collection collection) => _collection = collection;
	public Item CurrentItem => _collection[_current] as Item;
	public bool IsDone => _current >= _collection.Count;

	public Item First()
	{
		_current = 0;
		return _collection[_current] as Item;
	}

	public Item Next()
	{
		_current += Step;
		if (!IsDone)
			return _collection[_current] as Item;
		else
			return null;
	}
}