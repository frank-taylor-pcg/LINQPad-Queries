<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

#load ".\Logger.linq"

void Main()
{
	Test test = new Test();
}

public class Dumpable
{
	protected DumpContainer Container = new DumpContainer();
	public List<Button> Buttons = new List<Button>();
	public Logger Log = new Logger();

	public Dumpable()
	{
		Container.Content = this;
		Container.Dump();
	}

	public void AddButton(string strLabel, Action actClick, bool bToggleOnClick = false, string strLogMessage = null)
	{
		Button b = new Button(strLabel);
		b.Click += (sender, AssemblyLoadEventArgs) => PerformAction(b, actClick, bToggleOnClick, strLogMessage);
		Buttons.Add(b);
		Container.Refresh();
	}

	private void PerformAction(Button b, Action act, bool bToggleOnClick, string strLogMessage)
	{
		if (false == string.IsNullOrWhiteSpace(strLogMessage))
		{
			Log.Add(strLogMessage);
		}
		if (true == bToggleOnClick)
		{
			b.Enabled = !b.Enabled;
		}
		act();
		Container.Refresh();
	}

	// This is subject to change
	private object ToDump() => new { Buttons, Log };
}

public class Test : Dumpable
{
	public Test()
	{
		AddButton("Basic Test", BasicTest);
		AddButton("Toggle Test", ToggleTest, true);
		AddButton("Log Test", LogTest, false, "Log Test clicked");
		AddButton("Enable All Buttons", EnableAllButtons, false, "Enable all buttons clicked");
	}

	void BasicTest() => "BasicTest".Dump();
	void ToggleTest() => "ToggleTest".Dump();
	void LogTest() => "LogTest".Dump();

	void EnableAllButtons()
	{
		foreach (Button b in Buttons)
		{
			b.Enabled = true;
		}
	}
}