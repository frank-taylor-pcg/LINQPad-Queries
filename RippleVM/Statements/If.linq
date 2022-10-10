<Query Kind="Program" />

#load "RippleVM\RippleVM"
#load "RippleVM\Statements\Statement"
#load "RippleVM\RippleException"
#load "RippleVM\Statements\BlockStatement"

void Main()
{
	If i = new(null, () => false, "blah", 1);
	i.Action();
}

public class If : BlockStatement
{
	public Func<bool> Condition { get; set; }
		
	public If(RippleVM vm, Func<bool> condition, string expression, int lineNumber)
	{
		VM = vm;
		Condition = condition;
		Expression = expression;
		LineNumber = lineNumber;
		Action = Perform;
	}
	
	private void Perform()
	{
		if (Condition())
		{
			BlockReference.Resolved = true;
		}
		if (!Condition())
		{
			if (NextConditionAddress is null && ExitAddress is null)
			{
				throw new RippleException(this, new ArgumentNullException(nameof(ExitAddress)));
			}
			
			int jumpAddress = (int)(NextConditionAddress ?? ExitAddress);
			
			VM.JumpTo(jumpAddress);
		}
		// Otherwise, if we've resolved the block
		else
		{
		}
	}

	public override bool IsValid()
	{
		return ExitAddress is not null;
	}
}