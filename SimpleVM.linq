<Query Kind="Program">
  <NuGetReference>Akka</NuGetReference>
  <Namespace>Akka.Actor</Namespace>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	Machine machine = new();

	Button button = new Button("Trigger E-Stop");
	button.Click += (sender, args) => { machine.EStop = true; "E-Stop triggered by user!".Dump(); };
	
	button.Dump();
	
	Test2(machine);
}

void Test1()
{
	VM vm = new() { DebugMode = true };

	vm.Action("Say a simple hello", () => "Hello".Dump());

	vm.Func<string>("Ask for a user name", () =>
	{
		Console.Write("Please enter your name: ");
		return Console.ReadLine();
	});

	vm.Action("Say hello to the user", () => $"Hello, {vm.Return}".Dump());

	// vm.Action("Throw an exception", () => throw new IOException("Oh gnoes, some kind of IO failure happened!")); 

	vm.Func<bool>("Test if we can perform a boolean operation", () => PerformABooleanOperation());

	vm.Run();
}

void Test2(Machine machine)
{
	VM vm = new() { DebugMode = false, Looping = true };
	vm.Return = 0;
	vm.MonitorEStop = () => machine.EStop;
	
	vm.Func("Increment counter", () => (int)(vm.Return) + 1);
	//vm.Action("Display counter value", () => ((int)vm.Return).Dump());
	vm.Run();
}

bool PerformABooleanOperation()
{
	return true;
}


public class Machine
{
	public bool EStop { get; set; }
}


public class VM
{
	Dictionary<int, VMAction> actions = new();
	int address = 0;
	int programCounter = 0;
	
	public bool DebugMode { get; set; }
	public bool Looping { get; set; }
	
	public Func<bool> MonitorEStop;
	
	public object Return { get; set; }
	
	public void Action(string description, Action act)
	{
		actions.Add(address++, new(description, act));
	}
	
	public void Func<T>(string description, Func<T> func)
	{
		actions.Add(address++, new(description, () => Return = func()));
	}
	
	private void ProcessLoop()
	{
		while (programCounter < actions.Count || Looping)
		{
			if (MonitorEStop())
			{
				"E-Stop detected!".Dump();
				this.Dump();
				break;
			}

			// Attempt to perform the action specified at the current step
			try
			{
				if (DebugMode) "".Dump(actions[programCounter].Description);
				actions[programCounter].Execute();
			}
			catch (VMException ex)
			{
				ex.Dump();
			}

			programCounter++;

			// Reset the program counter if we're looping
			if (Looping && programCounter >= actions.Count)
			{
				programCounter = 0;
			}

			Thread.Sleep(250);
		}
	}

	public void Run()
	{
		Task.Run(ProcessLoop);
	}
}

public class VMException : Exception
{
	public VMException(string description, Exception innerException)
		: base($"Error processing [{description}]", innerException) { }
}

public class IfAction : VMAction
{
	public IfAction(string description, ) : base(description, 
	{
		
	}
}

public class VMAction
{
	public string Description { get; set; }
	public Action Action { get; set; }
	
	public void Execute()
	{
		try
		{
			Action();
		}
		catch (Exception ex)
		{
			throw new VMException(Description, ex);
		}
	}
		
	public VMAction(string description, Action action)
	{
		Description = description;
		Action = action;
	}
}