<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	
}

public class Dumpable
{
	protected DumpContainer Container = new DumpContainer();
	public List<Button> Buttons = new List<Button>();
	public Logger Log = new Logger();

	public Dumpable()
	{
		Container.Content = this;
		Container.Dump();
	}

	public void AddButton(string strLabel, Action actClick)
	{
		Button b = new Button(strLabel);
		b.Click += (sender, AssemblyLoadEventArgs) => RefreshingAction(actClick);
		Buttons.Add(b);
		Container.Refresh();
	}

	private void RefreshingAction(Action act)
	{
		act();
		Container.Refresh();
	}

	// This is subject to change
	private object ToDump() => new { Buttons, Log };
}

public class Logger
{
	private const int MAX_MESSAGES = 100;
	private int MessagesToShow = 10;
	private Queue<string> Messages = new Queue<string>();
	
	public void Add(string strMessage)
	{
		Messages.Enqueue(strMessage);
		RemoveOldMessages();
	}
	
	public void SetMessagesToShow(int val) => MessagesToShow = val;
	
	private void RemoveOldMessages()
	{
		while (Messages.Count > MAX_MESSAGES)
		{
			Messages.Dequeue();
		}
	}

	public override string ToString()
	{
		List<string> lastTen = Messages.Skip(Math.Max(0, Messages.Count() - MessagesToShow)).ToList();
		return string.Join("\n", lastTen);
	}
	
	private object ToDump() => ToString();
}