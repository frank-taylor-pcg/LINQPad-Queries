<Query Kind="Program" />

// Works great for the first model, but the vertices are additive, so I need to keep a running count of the max vertex value used in previous files and subtract that value from subsequent files
void Main()
{
	string base_path = @"F:\projects\godot\TheWorld\Village\assets\PolygonPirates\Characters";
	string source_name = "Characters.obj.joined";
	
	string[] lines = File.ReadAllLines(Path.Combine(base_path, source_name));

	string name = string.Empty;
	StringBuilder sb = new StringBuilder();
	
	int maxVertex = 0;
	int currentMax = 0;
	
	int i = 0;

	foreach (string line in lines)
	{
		if (line.StartsWith("g "))
		{
			if (name != string.Empty)
			{
				maxVertex = currentMax;
				i++;
				File.WriteAllText(Path.Combine(base_path, name), sb.ToString());
				if (i > 1) break;
			}
			name = line.Replace("g ", "") + ".obj";
			InitializeStringBuilder(sb, line);
		}
		else if (line.StartsWith("#") == false)
		{
			sb.AppendLine(line);
		}
	}
}

void InitializeStringBuilder(StringBuilder sb, string line)
{
	sb.Clear();
	sb.AppendLine(line);
}