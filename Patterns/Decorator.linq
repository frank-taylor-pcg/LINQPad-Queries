<Query Kind="Program" />

// Decorator Pattern
// https://www.dofactory.com/net/decorator-design-pattern

void Main()
{
	Book book = new Book("Worley", "Inside ASP.NET", 10);
	book.Display();
	Video video = new Video("Spielberg", "Jaws", 23, 92);
	video.Display();
	
	Console.WriteLine("\nMaking video borrowable:");
	
	Borrowable borrowVideo = new Borrowable(video);
	borrowVideo.BorrowItem("Customer #1");
	borrowVideo.BorrowItem("Customer #2");
	borrowVideo.Display();
}

abstract class LibraryItem
{
	public int NumCopies { get; set; }
	public abstract void Display();
}

class Book : LibraryItem
{
	private string _author;
	private string _title;
	public Book(string author, string title, int numCopies)
	{
		_author = author;
		_title = title;
		NumCopies = numCopies;
	}
	public override void Display()
	{
		Console.WriteLine($"\nBook ------ ");
		Console.WriteLine($" Author: {_author}");
		Console.WriteLine($" Title: {_title}");
		Console.WriteLine($" # Copies: {NumCopies}");
	}
}

class Video : LibraryItem
{
	private string _director;
	private string _title;
	private int _playTime;
	public Video(string director, string title, int numCopies, int playTime)
	{
		_director = director;
		_title = title;
		NumCopies = numCopies;
		_playTime = playTime;
	}
	public override void Display()
	{
		Console.WriteLine($"\nVideo ----- ");
		Console.WriteLine($" Director: {_director}");
		Console.WriteLine($" Title: {_title}");
		Console.WriteLine($" # Copies: {NumCopies}");
		Console.WriteLine($" Playtime: {_playTime}");
	}
}

abstract class Decorator : LibraryItem
{
	protected LibraryItem libraryItem;
	
	public Decorator(LibraryItem item)
	{
		libraryItem = item;
	}
	public override void Display()
	{
		libraryItem.Display();
	}
}

class Borrowable : Decorator
{
	protected List<string> borrowers = new List<string>();
	public Borrowable(LibraryItem item) : base(item) {}
	public void BorrowItem(string name)
	{
		borrowers.Add(name);
		libraryItem.NumCopies--;
	}
	public void ReturnItem(string name)
	{
		borrowers.Remove(name);
		libraryItem.NumCopies++;
	}
	public override void Display()
	{
		base.Display();
		borrowers.ForEach(b => Console.WriteLine($" borrower: {b}"));
	}
}