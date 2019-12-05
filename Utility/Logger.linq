<Query Kind="Program" />

void Main()
{
	DumpContainer dc = new DumpContainer();
	Logger logger = new Logger();
	dc.Content = logger;
	dc.Dump();
	
	for (int i = 0; i < 100; i++)
	{
		logger.Add($"Log message #{i}");
		dc.Refresh();
		Thread.Sleep(100);
	}
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