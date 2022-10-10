<Query Kind="Program" />

#load "RippleVM\Statement"

void Main()
{
	
}

public class RippleException : Exception
{
	public RippleException(Statement statement, Exception innerException)
		: base($"Error processing [{statement.Expression} (Line {statement.Address})]", innerException) { }
}