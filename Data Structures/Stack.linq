<Query Kind="Program" />

void Main()
{
	int iValue;
	StringBuilder sb = new();
	
	Stack<int> iStack = new Stack<int>().Dump("Create a new integer stack");
	
	"".Dump("Push 20 elements to the stack");
	Enumerable.Range(0, 20).ToList().ForEach(x => iStack.Push(x));
	
	for (int i = 0; i < 10; i++)
	{
		iStack.Pop(out iValue);
		sb.AppendLine(iValue.ToString());
	}
	sb.ToString().Dump("Pop and display the top 10 elements");

	iStack.Peek(out iValue);
	iValue.ToString().Dump("Display the next element");

	sb.Clear();
	while (iStack.Pop(out iValue))
	{
		sb.AppendLine(iValue.ToString());
	}
	sb.ToString().Dump("Pop and display the remaining elements");
}

record Node<T>(T Data, Node<T> Next);
public record Result<T>(bool Success, T Data);

/// <summary>A last-in, first-out (LIFO) structure implemented without using arrays or the standard library</summary>
public class Stack<T>
{
	private Node<T> top;

	/// <summary>Returns true when the stack is empty</summary>
	public bool IsEmpty() => (top is null);
	
	/// <summary>Pushes the provided element to the top of the stack</summary>
	public void Push(T element)
	{
		Node<T> node = new(element, top);
		top = node;
	}
	
	/// <summary>Pops the top-most element from the stack</summary>
	public bool Pop(out T element)
	{
		bool result = Peek(out element);
		top = top?.Next;
		return result;
	}
	
	/// <summary>Peeks at the top-most element on the stack</summary>
	public bool Peek(out T element)
	{
		if (top is null)
		{
			element = default(T);
			return false;
		}
		element = top.Data;
		return true;
	}
}

