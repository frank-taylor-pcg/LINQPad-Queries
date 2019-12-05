<Query Kind="Program" />

void Main()
{
	Util.HorizontalRun(true, System.Drawing.Color.Black, System.Drawing.Color.Blue).Dump();
	Util.HorizontalRun(true, System.Drawing.Color.Black, System.Drawing.Color.Blue).SetSuccess().Dump();
	Util.HorizontalRun(true, System.Drawing.Color.Black.SetWarning(), System.Drawing.Color.Blue.SetDanger()).Dump();
}

// Define other methods and classes here
