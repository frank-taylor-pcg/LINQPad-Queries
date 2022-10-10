<Query Kind="Program" />

#load "RippleVM\RippleVM"
#load "RippleVM\RippleException"

void Main()
{
	Statement s = new();
	s.ToString().Dump();	
}

public class Statement
{
	protected RippleVM VM;
	
	public int Address { get; set; }

	public string Expression { get; set; }

	public int LineNumber { get; set; }

	public Action Action { get; set; }

	public void Execute()
	{
		try
		{
			Action?.Invoke();
		}
		catch (Exception ex)
		{
			throw new RippleException(this, ex);
		}
	}

	// Simple statements should always validate if they compile.  Complex or multi-part statements require a bit more to successfully validate.
	public virtual bool IsValid()
	{
		return true;
	}

	public virtual void Reset() { }

	public override string ToString()
	{
		return $"[VM/C# {Address,3}/{LineNumber}] : {GetType().Name} ({Expression})";
	}
}