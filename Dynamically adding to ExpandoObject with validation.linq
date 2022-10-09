<Query Kind="Program">
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	PermissionsTest pt = new();
	
	pt.AddFunction("Main", () => MyMainFunction());
	pt.AddFunction("Secondary", () => Secondary());

	pt.AddFunction("Combined", () =>
	{
		"".PadLeft(90, '-').Dump();
		"Running the combined function".Dump();
		"".PadLeft(90, '-').Dump();
		pt.Functions.Main();
		pt.Functions.Secondary();
	});
	
	pt.Functions.Test = 100;
	
	pt.ListFunctions();
	
	pt.Functions.Combined();
}

public void MyMainFunction()
{
	"This does a whole bunch of cool things".Dump();
}

public void Secondary()
{
	"This function does some other stuff".Dump();
}

public class PermissionsTest
{
	public dynamic Functions { get; } = new ExpandoObject();
	private IDictionary<string, object> dict;

	public PermissionsTest()
	{
		dict = Functions as IDictionary<string, object>;
	}
	
	public void AddFunction(string name, Action act)
	{
		dict.Add(name, act);
	}
	
	public void ListFunctions()
	{
		foreach (KeyValuePair<string, object> entry in dict)
		{
			if (entry.Value is Action)
			{
				entry.Key.Dump();
			}
			else
			{
				$"{entry.Key} [ERROR] Not a valid function [[ERROR]]".Dump();
			}
		}
	}
}