<Query Kind="Program" />

void Main()
{
	MyTest();
}

void MyTest()
{
	LinkedList<int> list = new();
	list.Count.Dump("Size after initialization");

	Enumerable.Range(0, 10).ToList().ForEach(x => list.Add(x));
	list.Count.Dump("Size after adding 10 elements");

	FormattedDump(list, "Initial elements");

	list.Contains(3).Dump("Contains 3");
	list.Contains(11).Dump("Contains 11");

	// Building a second list to add to the first one
	LinkedList<int> second = new();
	Enumerable.Range(20, 10).ToList().ForEach(x => second.Add(x));

	// Add it to the end of the first one
	list.AddRange(second);

	FormattedDump(list, "Elements after adding second list to end of first");

	// Inserting the second list into the first one again, but this time between the 1 and the 2
	list.InsertRange(2, second);

	FormattedDump(list, "Elements after inserting the second list between elements 1 and 2");

	list.IndexOf(20).Dump("Get the first occurrence of the element 20");

	list.LastIndexOf(20).Dump("Get the first occurrence of the element 20");
	
	FormattedDump(list.GetRange(5, 10), "Get 10 elements starting at the 5th");
	
	FormattedDump(list.GetRange(list.Count - 5, 10), "Get the last 5 elements, requesting more than the list contains");
}

void FormattedDump<T>(LinkedList<T> list, string message)
{
	StringBuilder sb = new();
	list.ForEach(x => sb.Append($" {x} "));
	sb.ToString().Dump(message);
}

public class LinkedList<T>
{
	// Reference to the first element
	private Node<T> head;
	
	// Reference to the last element
	private Node<T> tail;
	
	// Reference to the previous element iterated to
	private Node<T> previous;

	// Reference to the current element iterated to
	private Node<T> current;
	
	// The number of elements in the collection.
	public int Count { get; set; } = 0;

	#region Add single element
	/// <summary>Adds an object to the beginning of the LinkedList<T></summary>
	public void Prepend(T element)
	{
		// Create the new node, set its Next element to the current head (can be null)
		Node<T> node = new() { Data = element, Next = head };

		// Set head to point to the new node
		head = node;
		
		Count++;
	}

	/// <summary>Adds an object to the end of the LinkedList<T></summary>
	public void Append(T element)
	{
		Node<T> current;

		// If head is null, then we have no elements.  Just insert at the beginning
		if (head is null)
		{
			Prepend(element);
			tail = head;
		}
		// Otherwise, start at the head and step through the list until there is no
		// Next to traverse to.  Create the new node and attach it as the Next for
		// the last node.
		else
		{
			current = head;

			while (current.Next is not null)
			{
				current = current.Next;
			}

			Node<T> node = new() { Data = element };
			current.Next = node;
			tail = node;
			Count++;
		}		
	}

	/// <summary>Adds an object to the end of the LinkedList<T></summary>
	public void Add(T element) => Append(element);

	/// <summary>Inserts an element into the LinkedList<T> at the specified index.</summary>
	public void Insert(T element, int index)
	{
		IterateToIndex(index);

		// Create a new node attaching its Next property to the current element's Next property
		Node<T> node = new() { Data = element, Next = current.Next };

		// Correct the current node's Next property to point to our new node, finalizing the insertion
		current.Next = node;
		
		Count++;
	}
	#endregion Add single element
	
	#region Remove single element
	/// <summary>Removes the first occurrence of a specific object from the LinkedList<T>.</summary>
	public bool Remove(T element)
	{
		bool result = false;
		
		int index = IndexOf(element);
		if (index != -1)
		{
			// Adjust the referencs to skip the current element, removing it from the list
			previous.Next = current.Next;
			result = true;
		}
		
		return result;
	}
	#endregion Remove single element

	#region Range actions
	
	public void AddRange(LinkedList<T> list)
	{
		// Make a copy of the list so that the list to add isn't affected in any way
		LinkedList<T> copy = list.Copy();
		tail.Next = list.head;
		tail = list.tail;
		
		Count += list.Count;
	}
	
	public void InsertRange(int index, LinkedList<T> list)
	{
		// You can't really insert nothing, so just return
		if (list is null) return;

		// Make a copy of the list so that the list to add isn't affected in any way
		LinkedList<T> copy = list.Copy();
		
		// Iterate to the supplied index
		IterateToIndex(index);

		// If the index specified matches the length, then this should call AddRange()
		if (index == Count)
		{
			AddRange(copy);
		}
		else
		{
			// The previously iterate element should now point to the new list's head.
			previous.Next = copy.head;

			// This was the source of the infinite loop bug
			// The new list's tail should now point to the currently iterated element to finalize the insertion
			copy.tail.Next = current;
		}

		// If the range was inserted at the beginning, we need to update the head
		if (index == 0)
		{
			head = copy.head;
		}
		
		Count += list.Count;
	}
	
	public LinkedList<T> GetRange(int index, int count)
	{
		LinkedList<T> result = new();
		
		IterateToIndex(index);
		
		int i = 0;
		
		while (i++ < count && current is not null)
		{
			result.Add(current.Data);
			current = current.Next;
		}
		
		return result;
	}
	
	#endregion Range actions
	
	#region Iterators
	
	public LinkedList<T> Copy()
	{
		LinkedList<T> result = new();

		current = head;
		
		// My ForEach function didn't work here -- the variable in the lambda expression couldn't access the Data property
		while (current is not null)
		{
			result.Add(current.Data);
			current = current.Next;
		}
		
		return result;
	}
	
	/// <summary>Determines whether an element is in the LinkedList<T></summary>
	public bool Contains(T element)
	{
		Node<T> current = head;

		while (current != null)
		{
			if (current.Data.Equals(element))
			{
				return true;
			}
			current = current.Next;
		}

		return false;
	}
	
	/// <summary>Returns the zero-based index of the first occurrence of a value in the LinkedList<T> or in a portion of it.</summary>
	public int IndexOf(T element)
	{
		int index = 0;
		current = head;
		
		while (current != null)
		{
			if (current.Data.Equals(element))
			{
				return index;
			}
			current = current.Next;
			index++;
		}

		return -1;
	}

	/// <summary>Returns the zero-based index of the first occurrence of a value in the LinkedList<T> or in a portion of it.</summary>
	public int LastIndexOf(T element)
	{
		int result = -1;
		int index = 0;
		current = head;

		while (current != null)
		{
			if (current.Data.Equals(element))
			{
				 result = index;
			}
			current = current.Next;
			index++;
		}

		return result;
	}

	/// <summary>Performs the specified action on each element of the LinkedList<T>.</summary>
	public void ForEach(Action<T> action)
	{
		current = head;
		
		while (current != null)
		{
			action(current.Data);
			current = current.Next;
		}
	}
	
	#endregion Iterators

	#region Indexer

	public T this[int index]
	{
		get => GetValue(index);
		set => SetValue(index, value);
	}

	#endregion Indexer

	#region Helper Functions

	// Steps through the list until we reach the desired index.  Sets our local current field to point to it.
	private void IterateToIndex(int index)
	{
		previous = null;
		current = head;
		int i = 0;

		while (i < index)
		{
			previous = current;
			current = current.Next;
			i++;
		}

		// If the index requested matches the index we reached then return
		if (i == index) return;

		// Otherwise, we need to report that the supplied index doesn't exist
		throw new IndexOutOfRangeException();
	}

	private T GetValue(int index)
	{
		IterateToIndex(index);
		return current.Data;
	}

	private void SetValue(int index, T value)
	{
		IterateToIndex(index);
		current.Data = value;
	}

	#endregion Helper Functions
}

private class Node<T>
{
	public T Data { get; set; }

	// Start with a singly-linked list. A node only knows about the next node, not the previous node
	// public Node Previous { get; set; }
	
	public Node<T> Next { get; set; }
}
