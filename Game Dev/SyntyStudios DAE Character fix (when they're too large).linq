<Query Kind="Program" />

string strPath = @"F:\projects\godot\TheWorld\Village\assets\PolygonPirates\Models\Characters.dae";

void Main()
{
	string text = File.ReadAllText(strPath);

	text = text.Replace("<", "\n<");
	text = text.Replace("\n</", "</");

	string[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

	List<string> lstLines = new List<string>();
	
	foreach (string line in lines)
	{
		if (String.IsNullOrWhiteSpace(line) == false)
		{
			lstLines.Add(line.Trim());
		}
	}

	DAE dae = new DAE() { Lines = lstLines };
	dae.Process();
	dae.WriteUniqueCharacters(@"F:\projects\godot\TheWorld\Village\assets\PolygonPirates\Characters");
	dae.Dump();
}

class DAE
{
	private enum ProcessStageId
	{
		HEADER,
		GEOMETRY_LIBRARY,
		END_GEOMETRY_LIBRARY,
		CONTROLLER_LIBRARY,
		END_CONTROLLER_LIBRARY,
		FOOTER
	}
	private Library current;

	public List<string> Lines { get; set; } = new List<string>();
	public StringBuilder Header { get; set; } = new StringBuilder();
	public List<Library> GeometryLibrary { get; set; } = new List<Library>();
	public List<Library> ControllerLibrary { get; set; } = new List<Library>();
	public StringBuilder Footer { get; set; } = new StringBuilder();

	public void Process()
	{
		// Start by populating the "header"
		ProcessStageId stage = ProcessStageId.HEADER;

		foreach (string line in Lines)
		{
			if (stage == ProcessStageId.END_CONTROLLER_LIBRARY)
			{
				stage = ProcessStageId.FOOTER;
			}
			if (line.Contains("<library_geometries"))
			{
				stage = ProcessStageId.GEOMETRY_LIBRARY;
			}
			if (line.Contains("</library_geometries>"))
			{
				stage = ProcessStageId.END_GEOMETRY_LIBRARY;
			}
			if (line.Contains("<library_controllers"))
			{
				stage = ProcessStageId.CONTROLLER_LIBRARY;
			}
			if (line.Contains("</library_controllers>"))
			{
				stage = ProcessStageId.END_CONTROLLER_LIBRARY;
			}
			switch (stage)
			{
				case ProcessStageId.HEADER: ProcessHeader(line); break;
				case ProcessStageId.GEOMETRY_LIBRARY: ProcessGeometryLibrary(line); break;
				case ProcessStageId.CONTROLLER_LIBRARY: ProcessControllerLibrary(line); break;
				case ProcessStageId.FOOTER: ProcessFooter(line); break;
				case ProcessStageId.END_GEOMETRY_LIBRARY:
				case ProcessStageId.END_CONTROLLER_LIBRARY:
					break;
				default: throw new NotImplementedException("What the fuck?");
			}
		}
	}
	
	public void WriteUniqueCharacters(string strPath)
	{
		foreach (Library character in GeometryLibrary.Where(x => x.Name.StartsWith("Character") == true))
		{
			StringBuilder result = new StringBuilder();
			result.AppendLine(Header.ToString());
			result.AppendLine("<library_geometries>");
			foreach (Library accessory in GeometryLibrary.Where(x => x.Name.StartsWith("Character") == false))
			{
				//result.AppendLine(accessory.Body.ToString());
			}
			result.AppendLine(character.Body.ToString());
			result.AppendLine("</library_geometries>");
			result.AppendLine("<library_controllers>");
			result.AppendLine(ControllerLibrary.Where(x => x.Name.StartsWith(character.Name)).Select(x => x.Body.ToString()).First());
			result.AppendLine("</library_controllers>");
			result.AppendLine(Footer.ToString());
			string filename = Path.Combine(strPath, $"{character.Name}.dae");
			File.WriteAllText(filename, result.ToString());
			break;
		}
	}
	
	private void ProcessHeader(string line)
	{
		Header.AppendLine(line);
	}

	private void ProcessGeometryLibrary(string line)
	{
		if (line.Contains("<geometry id"))
		{
			current = new Library();
			current.Name = line.Split(new char[] { '"', '-' })[1];
		}
		else if (line.Contains("</geometry>"))
		{
			GeometryLibrary.Add(current);
			current = null;
		}
		else if (current != null)
		{
			current.Body.AppendLine(line);
		}
	}
	
	private void ProcessControllerLibrary(string line)
	{
		if (line.Contains("<controller id"))
		{
			current = new Library();
			current.Name = line.Split('"')[1];
		}
		else if (line.Contains("</controller>"))
		{
			ControllerLibrary.Add(current);
			current = null;
		}
		else if (current != null)
		{
			current.Body.AppendLine(line);
		}
	}

	private void ProcessFooter(string line)
	{
		Footer.AppendLine(line);
	}
}

class Library
{
	public string Name { get; set; }
	public StringBuilder Body { get; set; } = new StringBuilder();
	
	private object ToDump() => Name;
}

