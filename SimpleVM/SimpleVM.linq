<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

// This isn't working
void Main()
{
	Machine machine = new();

	Button button = new Button("Trigger E-Stop");
	button.Click += (sender, args) => { machine.EStop = true; "E-Stop triggered by user!".Dump(); };
	
	button.Dump();
	
	//Test2(machine);
	Test1();
}

void Test1()
{
	VM vm = new() { DebugMode = true };

	vm.Action("Say a simple hello", SayHello);
	//.Func<string>("Ask for a user name", PromptForName)
	// I need a better way to do this
	//.Action("Say hello to the user", () => SayHelloToUser(vm.Return))
	//.Action("Throw an exception", () => throw new IOException("Oh gnoes, some kind of IO failure happened!"))
	//.Func<bool>("Test if we can perform a boolean operation", PerformABooleanOperation)
	vm.Run();
}

void SayHello() => "Hello".Dump();

string PromptForName()
{
	Console.Write("Please enter your name: ");
	return Console.ReadLine();
}

void SayHelloToUser(object user) => $"Hello, {user}".Dump();

bool PerformABooleanOperation() => true;

void Test2(Machine machine)
{
	VM vm = new() { DebugMode = false, Looping = true };
	vm.Return = 0;
	vm.MonitorEStop = () => machine.EStop;
	
	vm.Func("Increment counter", () => (int)(vm.Return) + 1);
	//vm.Action("Display counter value", () => ((int)vm.Return).Dump());
	vm.Run();
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
	
	public VM Action(string description, Action act, params object[] args)
	{
		actions.Add(address++, new(description, act, args));
		return this;
	}
	
	public VM Func<T>(string description, Func<T> func, params object[] args)
	{
		actions.Add(address++, new(description, () => Return = func()));
		return this;
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

	public VM Run()
	{
		Task.Run(ProcessLoop);
		return this;
	}
}

public class VMException : Exception
{
	public VMException(string description, Exception innerException)
		: base($"Error processing [{description}]", innerException) { }
}

//public class IfAction : VMAction
//{
//	public IfAction(string description, ) : base(description, 
//	{
//		
//	}
//}

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
		
	public VMAction(string description, Action action, params object[] args)
	{
		Description = description;
		Action = action;
		if (args.Length > 0)
		{
			
		}
	}
}