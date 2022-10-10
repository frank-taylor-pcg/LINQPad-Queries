<Query Kind="Program" />

#load "RippleVM\Statements\Statement"
#load "RippleVM\Statements\BlockReference"

void Main()
{
	
}

public class BlockStatement : Statement
{
	protected BlockReference BlockReference { get; set; }
	protected int? NextConditionAddress = null;
	protected int? ExitAddress { get; set; } = null;
}