<Query Kind="Program">
  <NuGetReference>Akka</NuGetReference>
  <NuGetReference>Akka.Remote</NuGetReference>
  <Namespace>Akka.Actor</Namespace>
  <Namespace>System.Timers</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
	ActorSystem system = ActorSystem.Create("LINQPad");

	IActorRef server = system.CreateServer("Server");
	IActorRef client = system.CreateClient("Client");

	client.Tell(server);
	
	system.WhenTerminated.Wait();
}

public static class Extensions
{
	public static IActorRef CreateServer(this ActorSystem system, string name)
	{
		Props props = Props.Create<ServerActor>();
		return system.ActorOf(props, name);
	}
	
	public static IActorRef CreateClient(this ActorSystem system, string name)
	{
		Props props = Props.Create<ClientActor>();
		return system.ActorOf(props, name);
	}
}

public class ServerActor : TimerActor
{
	public const int MAX_INACTIVE_TIME = 10_000;
	private IActorRef client;
	public int disconnectTimer = MAX_INACTIVE_TIME;
	public int heartbeatsReceived { get; set; } = 0;
	
	public ServerActor() : base()
	{
		Receive<ConnectMessage>(_ => HandleConnectMessage());
		Receive<HeartbeatMessage>(_ => HandleHeartbeatMessage());
	}

	protected override void OnTimerState_Elapsed(object source, ElapsedEventArgs e)
	{
		base.OnTimerState_Elapsed(source, e);
		
		disconnectTimer -= TimerInterval;
		
		if (disconnectTimer == 0)
		{
			client = null;
		}
		
		if (client != null)
		{
			client.Tell(new GuiUpdateMessage());
		}
	}

	private void HandleConnectMessage() => client = Sender;
	private void HandleHeartbeatMessage()
	{
		disconnectTimer = MAX_INACTIVE_TIME;
		heartbeatsReceived++;
	}
}

public class ClientActor : TimerActor
{
	private IActorRef server;
	public int UpdatesReceived { get; set; } = 0;	
	public bool SendHeartbeat = true;
	public Button DisconnectButton = new("Disconnect");
	
	public ClientActor() : base()
	{
		Receive<IActorRef>(x => HandleActorReference(x));
		Receive<GuiUpdateMessage>(x => HandleGuiUpdateMessage(x));
		Receive<ToggleHeartbeat>(_ => HandleToggleHeartbeat());

		DisconnectButton.Click += (sender, args) =>
		{
			"Disconnect button clicked".Dump();
			Self.Tell(new ToggleHeartbeat());
		};
	}

	protected override void OnTimerState_Elapsed(object source, ElapsedEventArgs e)
	{
		base.OnTimerState_Elapsed(source, e);

		if (server != null && SendHeartbeat)
		{
			server.Tell(new HeartbeatMessage());
		}
	}
	
	private void HandleActorReference(IActorRef actor)
	{
		server = actor;
		server.Tell(new ConnectMessage());
	}

	private void HandleGuiUpdateMessage(GuiUpdateMessage message) => UpdatesReceived++;

	private void HandleToggleHeartbeat()
	{
		"Toggle heartbeat received".Dump();
		SendHeartbeat ^= true;
	}
}


public abstract class TimerActor : ReceiveActor
{
	protected int TimerInterval { get; set; }
	private System.Timers.Timer timer;
	protected DumpContainer dc = new();
	
	public TimerActor(int milliseconds = 250)
	{
		TimerInterval = milliseconds;
		
		timer = new(TimerInterval);
		timer.Elapsed += OnTimerState_Elapsed;
		timer.AutoReset = true;
		timer.Start();
		
		dc.Content = this;
		dc.Dump();
	}
	
	protected virtual void OnTimerState_Elapsed(object source, System.Timers.ElapsedEventArgs e)
	{
		dc.Refresh();
	}
}

#region Messages

public record ConnectMessage();
public record GuiUpdateMessage();
public record HeartbeatMessage();
public record ToggleHeartbeat();

#endregion Messages