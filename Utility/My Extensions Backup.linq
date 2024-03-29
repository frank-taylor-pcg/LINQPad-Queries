<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Dynamic</Namespace>
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

public static string Theme = "darkly";

void Main()
{
	// Write code to test your extensions here. Press F5 to compile and run.
}

public static class MyExtensions
{
	#region BOOTSTRAP THEMES ENABLED!
	// TODO: Has a bug the prevents the HorizontalRun command from working properly once SetClass has been called
	public static object SetClass(this object input, CSSClassId classId)
	{
		switch (classId)
		{
			case CSSClassId.PRIMARY: return input.SetClass("primary");
			case CSSClassId.SECONDARY: return input.SetClass("secondary");
			case CSSClassId.SUCCESS: return input.SetClass("success");
			case CSSClassId.INFO: return input.SetClass("info");
			case CSSClassId.WARNING: return input.SetClass("warning");
			case CSSClassId.DANGER: return input.SetClass("danger");
			default: return input.SetClass("spacer");
		}
	}

	// ToHtmlString doesn't give a partial, but the full HTML page, including the CSS. We don't need all of that so strip it out
	private static string ExtractContent(string strHtml)
	{
		int start = strHtml.IndexOf("<body>") + 6;
		int end = strHtml.IndexOf("</body>");
		return strHtml.Substring(start, end - start);
	}

	private static string AddClassToDiv(string strHtml, string strClass)
	{
		strHtml = strHtml.Replace("div class=\"spacer\"", $"div class=\"{Theme} {strClass}\"");
		strHtml = strHtml.Replace("div style", $"div class=\"{Theme} {strClass}\" style");
		strHtml = strHtml.Replace("<div>", $"<div class=\"{Theme} {strClass}\">");
		return strHtml;
	}

	public static object SetClass(this object input, string strClass)
	{
		string strHtml = Util.ToHtmlString(input);
		strHtml = ExtractContent(strHtml);
		strHtml = AddClassToDiv(strHtml, strClass);
		return Util.RawHtml(strHtml);
	}
	public static object SetPrimary(this object input) => input.SetClass("primary");
	public static object SetSecondary(this object input) => input.SetClass("secondary");
	public static object SetSuccess(this object input) => input.SetClass("success");
	public static object SetInfo(this object input) => input.SetClass("info");
	public static object SetWarning(this object input) => input.SetClass("warning");
	public static object SetDanger(this object input) => input.SetClass("danger");

	public static void DumpClass(this object input, string strClass) => input.SetClass(strClass).Dump();
	public static void DumpPrimary(this object input) => input.DumpClass("primary");
	public static void DumpSecondary(this object input) => input.DumpClass("secondary");
	public static void DumpSuccess(this object input) => input.DumpClass("success");
	public static void DumpInfo(this object input) => input.DumpClass("info");
	public static void DumpWarning(this object input) => input.DumpClass("warning");
	public static void DumpDanger(this object input) => input.DumpClass("danger");
	#endregion BOOTSTRAP THEMES ENABLED!

	public static void SetTheme(ColorThemeId themeId)
	{
		switch (themeId)
		{
			case ColorThemeId.CERULEAN: Theme = "cerulean"; break;
			case ColorThemeId.COSMO: Theme = "cosmo"; break;
			case ColorThemeId.CYBORG: Theme = "cyborg"; break;
			case ColorThemeId.DARKLY: Theme = "darkly"; break;
			case ColorThemeId.FLATLY: Theme = "flatly"; break;
			case ColorThemeId.JOURNAL: Theme = "journal"; break;
			case ColorThemeId.LITERA: Theme = "litera"; break;
			default: Theme = "default"; break;
		}
	}
}

public enum CSSClassId
{
	DEFAULT,
	PRIMARY,
	SECONDARY,
	SUCCESS,
	INFO,
	WARNING,
	DANGER
}

static object ToDump(object input)
{
	switch (input)
	{
		case Color color: return FormatColor(color);
		default: return input;
	}
}

static object FormatColor(Color color)
{
	dynamic result = new ExpandoObject();
	result.Name = color.Name;
	result.RGB = $"({color.R}, {color.G}, {color.B})";
	result.Hex = ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb()));
	result.Swatch = Util.WithStyle("Swatch", $"color:{result.Hex}; background-color:{result.Hex}");
	return result;
}

public enum ColorThemeId
{
	DEFAULT,
	CERULEAN,
	COSMO,
	CYBORG,
	DARKLY,
	FLATLY,
	JOURNAL,
	LITERA,
}
