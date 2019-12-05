<Query Kind="Program" />

// Got the colors from https://bootswatch.com/darkly/

void Main()
{
	
	Util.HorizontalRun(true,
		System.Drawing.Color.Blue,
		System.Drawing.Color.Blue.SetPrimary(),
		System.Drawing.Color.Blue.SetSecondary(),
		System.Drawing.Color.Blue.SetSuccess(),
		System.Drawing.Color.Blue.SetInfo(),
		System.Drawing.Color.Blue.SetWarning(),
		System.Drawing.Color.Blue.SetDanger()
	).Dump();
}


