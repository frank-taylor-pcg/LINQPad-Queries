<Query Kind="Program" />

// Mediator pattern
// https://www.dofactory.com/net/mediator-design-pattern

void Main()
{
	Chatroom chatroom = new Chatroom();

	Participant george = new Beatle("George");
	Participant paul = new Beatle("Paul");
	Participant ringo = new Beatle("Ringo");
	Participant john = new Beatle("John");
	Participant yoko = new NonBeatle("Yoko");

	chatroom.Register(george);
	chatroom.Register(paul);
	chatroom.Register(ringo);
	chatroom.Register(john);
	chatroom.Register(yoko);

	yoko.Send("John", "Hi, John!");
	paul.Send("Ringo", "All you need is love");
	ringo.Send("George", "My sweet Lord");
	paul.Send("John", "Can't buy me love");
	john.Send("Yoko", "My sweet love");
}

abstract class AbstractChatroom
{
	public abstract void Register(Participant participant);
	public abstract void Send(string from, string to, string message);
}

class Chatroom : AbstractChatroom
{
	private Dictionary<string, Participant> _participants = new Dictionary<string, Participant>();
	public override void Register(Participant participant)
	{
		if (!_participants.ContainsValue(participant))
		{
			_participants[participant.Name] = participant;
		}
		participant.Chatroom = this;
	}

	public override void Send(string from, string to, string message)
	{
		Participant participant = _participants[to];
		
		if (participant != null)
			participant.Receive(from, message);
	}
}

class Participant
{
	public Chatroom Chatroom { get; set; }
	public string Name { get; set; }
	public Participant(string name) => Name = name;
	public void Send(string to, string message) => Chatroom.Send(Name, to, message);
	public virtual void Receive(string from, string message) => Console.WriteLine($"{from} to {Name}: '{message}'");
}

class Beatle : Participant
{
	public Beatle(string name) : base(name) {}
	public override void Receive(string from, string message)
	{
		Console.WriteLine("To a Beatle: ");
		base.Receive(from, message);
	}
}

class NonBeatle : Participant
{
	public NonBeatle(string name) : base(name) { }
	public override void Receive(string from, string message)
	{
		Console.WriteLine("To a non-Beatle: ");
		base.Receive(from, message);
	}
}