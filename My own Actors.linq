<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

void Main()
{
	TestActor test = new();
	
	test.Tell(new Hello("World"));
	test.Tell(new Goodbye("World"));
	test.Tell(new BigMessage("Frank", 20, 69.247f));
}

public record Hello(string Name);
public record Goodbye(string Name);
public record BigMessage(string Name, int Quantity, float Value);


public class Actor
{
	private BlockingCollection<object> mailbox = new();
	private Dictionary<Type, Action<object>> handlers = new();
	private Timer timer;
	
	public Actor()
	{
		timer = new Timer(new TimerCallback(CheckMailbox), null, 250, 2500);
	}
	
	private void CheckMailbox(object state)
	{
		if (mailbox.TryTake(out object message))
		{
			handlers[message.GetType()](message);
		}
	}
		
	public void Tell<T>(T message)
	{
		mailbox.TryAdd(message, TimeSpan.FromMilliseconds(250));
	}

	public void Receive<T>(Action<T> action)
	{
		handlers[typeof(T)] = new Action<object>(o => action((T)o));
	}	
}

public class TestActor : Actor
{
	public TestActor()
	{
		Receive<Hello>((x) => HandleHello(x));
		Receive<Goodbye>((x) => HandleGoodbye(x));
		Receive<BigMessage>((x) => HandleBigMessage(x));
	}

	private void HandleHello(Hello msg)
	{
		$"Hello, {msg.Name}!".Dump();
	}
	
	private void HandleGoodbye(Goodbye msg)
	{
		$"Goodbye, {msg.Name}!".Dump();
	}
	
	private void HandleBigMessage(BigMessage msg)
	{
		StringBuilder sb = new();
		sb.AppendLine("This message is bigger and so we have to do more work.");
		sb.AppendLine("First we'll greet the individual that sent it:");
		sb.AppendLine($"  Hello, {msg.Name}");
		sb.AppendLine("Next the message contains a very important quantity:");
		sb.AppendLine($"  {msg.Quantity}");
		sb.AppendLine($"Lastly, the message contains a very important value:");
		sb.AppendLine($"  {msg.Value}");
		sb.ToString().Dump();
	}
}