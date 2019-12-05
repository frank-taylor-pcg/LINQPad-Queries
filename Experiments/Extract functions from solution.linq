<Query Kind="Program" />

#region Static Values
static string strSearchValue = string.Empty;
static string strSourcePath = @"F:\projects\elixir\ExMWS";
static string[] ignoredFolders =
{
	// Hidden folders
	@"\.git", @"\.vs", @"\.nuget",
	// Project folders
	@"\Debug", @"\obj", @"\Properties", @"\packages", @"\bin",
	// Miscellaneous
	@"\TBD"
};
static Dictionary<string, string> formatting = new Dictionary<string, string>()
{
	{ "//", "color:gray;" },
	{ "public", "color:lightgreen" },
	{ "abstract", "color:orange;" },
	{ "virtual", "color:violet;" }
};
#endregion Static Values

void Main()
{
//	strSearchValue = "Queue";
	ProcessDirectory(strSourcePath);
}

// Process all files in the directory passed in, recurse on any directories 
// that are found, and process the files they contain.
public static void ProcessDirectory(string targetDirectory)
{
	// Process the list of files found in the directory.
	string[] fileEntries = Directory.GetFiles(targetDirectory);
	foreach (string fileName in fileEntries)
		ProcessFile(fileName);

	// Recurse into subdirectories of this directory.
	string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
	foreach (string subdirectory in subdirectoryEntries)
	{
		if (!ignoredFolders.Any(f => subdirectory.Contains(f)))
		{
			ProcessDirectory(subdirectory);
		}
	}
}

// Insert logic for processing found files here.
public static void ProcessFile(string path)
{
	FileInfo fi = new FileInfo(path);
	CodeFile cf = new CodeFile()
	{
		Filename = fi.Name,
		Fullname = path
	};
	
	if (fi.Extension == ".cs")
	{
		IEnumerable<string> lines = File.ReadAllLines(path);

		try
		{
			cf.Namespace = lines.Where(s => s.Contains("namespace")).FirstOrDefault().Replace("namespace", "").Trim();
		}
		catch (Exception)
		{
			cf.Dump();
			lines.Dump();
		}

		// This is crude, but functional. There are numerous cases where functions will be removed or field/property declarations will be kept.
		// Also, if the item doesn't specify its accessibility (which it should) it will be missed.
		IEnumerable<string> candidates = lines.Where(a => IsFunctionDeclaration(a));

		if (candidates.Count() > 0)
		{
			bool bDump = false;
			foreach (string str in candidates)
			{
				if (strSearchValue != string.Empty)
				{
					if (str.ToUpper().Contains(strSearchValue.ToUpper()))  // Do specific search here
					{
						cf.FunctionNames.Add(str.Replace("{", "").Replace("}", "").Trim());
						bDump = true;
					}
				}
				else
				{
					cf.FunctionNames.Add(str.Replace("{", "").Replace("}", "").Trim());
					bDump = true;
				}
			}
			if (bDump)
			{
				cf.Dump();
			}
		}
	}
}

// Test if the current line is a function declaration.
public static bool IsFunctionDeclaration(string strLine)
{
	bool bResult = false;
	// Function declarations `should` have an accessibility qualifier
	bResult |= strLine.Contains("protected");
	bResult |= strLine.Contains("private");
	bResult |= strLine.Contains("public");
	bResult |= strLine.Contains("internal");
	// Function declarations require parentheses
	bResult &= strLine.Contains("(");
	// If the line contains accessors, then it's most likely a property not a function declaration
	bResult &= !strLine.Contains("get;");
	bResult &= !strLine.Contains("set;");
	// Removes fields/properties instantiated with the "new" keyword.  The space should avoid problems where something is named with "new" in it
	bResult &= !strLine.Contains("new ");
	// Removes fields/properties that utilize cast without removing functions with default parameters
	bResult &= (strLine.IndexOf("=") == -1 || strLine.IndexOf("=") > strLine.IndexOf("("));
	return bResult;
}

// Used to format the display of a code file.
class CodeFile
{
	public string Filename { get; set; }
	public string Fullname { get; set; }
	public string Namespace { get; set; }
	public List<string> FunctionNames { get; set; } = new List<string>();

	object ToDump()
	{
		IDictionary<string, object> custom = new System.Dynamic.ExpandoObject();
		custom["Filename"] = Util.OnDemand(Filename, () => Fullname);
		custom["Namespace"] = Namespace;
		
		List<object> lstFunctions = new List<object>();
		
		foreach (string strName in FunctionNames)
		{
			string sKeyResult = formatting.Keys.FirstOrDefault<string>(s => strName.Contains(s));
			if (String.IsNullOrEmpty(sKeyResult))
			{
				lstFunctions.Add(strName);
			}
			else
			{
				lstFunctions.Add(Util.WithStyle(strName, formatting[sKeyResult]));
			}
		}

		custom["FunctionNames"] = lstFunctions;

		return custom;
	}
}