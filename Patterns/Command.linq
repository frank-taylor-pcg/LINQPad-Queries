<Query Kind="Program" />

// Command pattern
// https://www.dofactory.com/net/command-design-pattern

void Main()
{
	User user = new User();

	user.Compute('+', 100);
	user.Compute('-', 50);
	user.Compute('*', 10);
	user.Compute('/', 2);
	
	user.Undo(4);
	
	user.Redo(3);
}

abstract class Command
{
	public abstract void Execute();
	public abstract void UnExecute();
}

class CalculatorCommand : Command
{
	public char Operator { get; set; }
	public int Operand { get; set; }
	public Calculator Calculator { get; set; }

	public CalculatorCommand(Calculator calculator, char @operator, int operand)
	{
		Calculator = calculator;
		Operator = @operator;
		Operand = operand;
	}

	public override void Execute()
	{
		Calculator.Operation(Operator, Operand);
	}

	public override void UnExecute()
	{
		Calculator.Operation(Undo(Operator), Operand);
	}
	
	private char Undo(char @operator)
	{
		switch (@operator)
		{
			case '+': return '-';
			case '-': return '+';
			case '*': return '/';
			case '/': return '*';
			default: throw new ArgumentException("@operator");
		}
	}
}

class Calculator
{
	private int _curr = 0;
	public void Operation(char @operator, int operand)
	{
		switch (@operator)
		{
			case '+': _curr += operand; break;
			case '-': _curr -= operand; break;
			case '*': _curr *= operand; break;
			case '/': _curr /= operand; break;
		}
		Console.WriteLine($"Current value = { _curr,3} (following { @operator } { operand })");
	}
}

class User
{
	private Calculator _calculator = new Calculator();
	private List<Command> _commands = new List<Command>();
	private int _current = 0;
	public void Redo(int levels)
	{
		Console.WriteLine($"\n---- Redo {levels} levels");
		for (int i = 0; i < levels; i++)
		{
			if (_current < _commands.Count - 1)
			{
				Command command = _commands[_current++];
				command.Execute();
			}
		}
	}
	public void Undo(int levels)
	{
		Console.WriteLine($"\n---- Undo {levels} levels");
		for (int i = 0; i < levels; i++)
		{
			if (_current > 0)
			{
				Command command = _commands[--_current];
				command.UnExecute();
			}
		}
	}
	public void Compute(char @operator, int operand)
	{
		Command command = new CalculatorCommand(_calculator, @operator, operand);
		command.Execute();
		
		// Add the command to the Undo list
		_commands.Add(command);
		_current++;
	}
}