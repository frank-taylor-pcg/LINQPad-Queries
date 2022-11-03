<Query Kind="Program" />

#load ".\Register"

void Main()
{
	SectorVM svm = new();
	svm.Dump();
}

public interface IConnection { }

public enum SectorMode
{
	IDLE,
	READ,
	WRTE
}



public class SectorVM : IConnection
{
	#region Connections
	public IConnection Up { get; set; }
	public IConnection Down { get; set; }
	public IConnection Left { get; set; }
	public IConnection Right { get; set; }
	#endregion

	public List<IRegister> Registers = new()
	{
		new Register<short>("Acc"),
		new Register<short>("Bak"),
		new Register<short>("Last"),
		new Register<SectorMode>("Mode"),
	};
	
	public Queue<string> Code { get; } = new(20);
	
	object ToDump()
	{
		return Util.HorizontalRun(true, Code, Registers);
	}
}