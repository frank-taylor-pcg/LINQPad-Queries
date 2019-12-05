<Query Kind="Program" />

public string strTemplate = @"
.{0}.{1} table {{ border: 2px solid {2}; }}
.{0}.{1} td {{ border: 1px solid {2}; }}
.{0}.{1} th {{ border: 1px solid {2}; }}
.{0}.{1} td.typeheader {{
	background-color: {2};
	border: 1px solid {2};
}}
";

void Main()
{
	using (StreamWriter sr = new StreamWriter(@"C:\Users\Frank\Documents\LINQPad Queries\Utility\Custom.css"))
	{
		sr.Write(CreateTheme("Default", GetCeruleanColors()));
		sr.Write(CreateTheme("Cerulean", GetCeruleanColors()));
		sr.Write(CreateTheme("Cosmo", GetCosmoColors()));
		sr.Write(CreateTheme("Cyborg", GetCyborgColors()));
		sr.Write(CreateTheme("Darkly", GetDarklyColors()));
		sr.Write(CreateTheme("Flatly", GetFlatlyColors()));
		sr.Write(CreateTheme("Journal", GetJournalColors()));
		sr.Write(CreateTheme("Litera", GetLiteraColors()));
	}
}

Dictionary<string, string> GetDefaultColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#0062cc");
	ColorDictionary.Add("secondary", "#6c757d");
	ColorDictionary.Add("success", "#28a745");
	ColorDictionary.Add("info", "#17a2b8");
	ColorDictionary.Add("warning", "#ffc107");
	ColorDictionary.Add("danger", "#dc3545");
	return ColorDictionary;
}
/*
Dictionary<string, string> GetDefaultColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "");
	ColorDictionary.Add("secondary", "");
	ColorDictionary.Add("success", "");
	ColorDictionary.Add("info", "");
	ColorDictionary.Add("warning", "");
	ColorDictionary.Add("danger", "");
	return ColorDictionary;
}
*/
Dictionary<string, string> GetCeruleanColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#2FA4E7");
	ColorDictionary.Add("secondary", "#E9ECEF");
	ColorDictionary.Add("success", "#73A839");
	ColorDictionary.Add("info", "#033C73");
	ColorDictionary.Add("warning", "#F39C12");
	ColorDictionary.Add("danger", "#C71C22");
	return ColorDictionary;
}

Dictionary<string, string> GetCosmoColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#2780E3");
	ColorDictionary.Add("secondary", "#373a3c");
	ColorDictionary.Add("success", "#3FB618");
	ColorDictionary.Add("info", "#9954BB");
	ColorDictionary.Add("warning", "#FF7518");
	ColorDictionary.Add("danger", "#FF0039");
	return ColorDictionary;
}

Dictionary<string, string> GetCyborgColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#2A9FD6");
	ColorDictionary.Add("secondary", "#555");
	ColorDictionary.Add("success", "#77B300");
	ColorDictionary.Add("info", "#9933CC");
	ColorDictionary.Add("warning", "#FF8800");
	ColorDictionary.Add("danger", "#CC0000");
	return ColorDictionary;
}

Dictionary<string, string> GetDarklyColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#375A7F");
	ColorDictionary.Add("secondary", "#444444");
	ColorDictionary.Add("success", "#00BC8C");
	ColorDictionary.Add("info", "#3498DB");
	ColorDictionary.Add("warning", "#DD5600");
	ColorDictionary.Add("danger", "#E74C3C");
	return ColorDictionary;
}

Dictionary<string, string> GetFlatlyColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#2C3E50");
	ColorDictionary.Add("secondary", "#95a5a6");
	ColorDictionary.Add("success", "#18BC9C");
	ColorDictionary.Add("info", "#3498DB");
	ColorDictionary.Add("warning", "#F39C12");
	ColorDictionary.Add("danger", "#E74C3C");
	return ColorDictionary;
}

Dictionary<string, string> GetJournalColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#EB6864");
	ColorDictionary.Add("secondary", "#aaa");
	ColorDictionary.Add("success", "#22B24C");
	ColorDictionary.Add("info", "#369");
	ColorDictionary.Add("warning", "#F5E625");
	ColorDictionary.Add("danger", "#F57A00");
	return ColorDictionary;
}

Dictionary<string, string> GetLiteraColors()
{
	Dictionary<string, string> ColorDictionary = new Dictionary<string, string>();
	ColorDictionary.Add("primary", "#4582EC");
	ColorDictionary.Add("secondary", "#adb5bd");
	ColorDictionary.Add("success", "#02B875");
	ColorDictionary.Add("info", "#17a2b8");
	ColorDictionary.Add("warning", "#f0ad4e");
	ColorDictionary.Add("danger", "#d9534f");
	return ColorDictionary;
}

string CreateTheme(string strThemeName, Dictionary<string, string> dictColorValues)
{
	StringBuilder sb = new StringBuilder();
	sb.AppendLine($"/*{ "".PadLeft(40, '-') }");
	sb.AppendLine($"- { strThemeName } Theme");
	sb.AppendLine($"{ "".PadLeft(40, '-') }*/");
	foreach (KeyValuePair<string, string> entry in dictColorValues)
	{
		sb.AppendLine(String.Format(strTemplate.Trim(), strThemeName.ToLower(), entry.Key, entry.Value));
	}
	return sb.ToString().Dump();
}