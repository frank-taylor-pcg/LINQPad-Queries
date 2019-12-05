<Query Kind="Program" />

// Got the colors from https://bootswatch.com/darkly/

void Main()
{
	foreach (ColorThemeId theme in Enum.GetValues(typeof(ColorThemeId)))
	{
		DoColorTestDump(theme);
	}
}

class Test
{
	public string Value = "Test";
}

void DoColorTestDump(ColorThemeId themeId)
{
	Test test = new Test();
	
	MyExtensions.SetTheme(themeId);
	Util.HorizontalRun(true,
		themeId,
		test.SetPrimary(),
		test.SetSecondary(),
		test.SetSuccess(),
		test.SetInfo(),
		test.SetWarning(),
		test.SetDanger()
	).Dump();
}