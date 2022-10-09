<Query Kind="Program" />

void Main()
{
	Queue<int> iQueue = new();
	
	iQueue.Remove().Dump();
}

public class Node<T>
{
	public T Data { get; set; }
	public Node<T> Next { get; set; }
	public Node<T> Previous { get; set; }
	
	public Node(T data, Node<T> previous)
	{
		Data = data;
		Previous = previous;
	}
}

public record Result<T>(bool Success, T Data);

/// <summary>A first-in, first-out (FIFO) structure implemented without using arrays or the standard library</summary>
public class Queue<T>
{
	private Node<T> first;
	private Node<T> last;
	
	/// <summary>Returns true if there are no elements in the queue</summary>
	public bool IsEmpty() => (first is null && last is null);
	
	/// <summary>Inserts a new element</summary>
	public bool Insert(T element)
	{
		bool result = false;

		Node<T> node = new(element, last);
		last.Next = node;
		last = node;

		return result;
	}
	
	/// <summary>Removes the first element. Returns a tuple containing a value of true if an element was retrieved with the element in question.  Otherwise, returns false and the default value of the type T.</summary>
	public Result<T> Remove()
	{
		Result<T> result = Peek();
		first = first.Next;
		return result;
	}

	/// <summary>Peeks at the first element in the queue without removing it.</summary>
	public Result<T> Peek()
	{
		if (first is null)
		{
			return new Result<T>(false, default(T));
		}
		return new Result<T>(true, first.Data);
	}
}

