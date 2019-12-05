<Query Kind="Program" />

string basePath = @"F:\projects\godot\TheWorld\Village\assets\PolygonPirates\Characters";

void Main()
{
	string filename = Path.Combine(basePath, "Characters.dae");
	
	string text = File.ReadAllText(filename);

	text = text.Replace("<", "\n<").Replace(">", ">\n");

	string[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

	List<string> lstLines = new List<string>();

	foreach (string line in lines)
	{
		if (String.IsNullOrWhiteSpace(line) == false)
		{
			lstLines.Add(line.Trim());
		}
	}

//	lstLines.Dump();
	Element root = null;
	Element current = root;
	
	foreach (string line in lstLines)
	{
		if (line.StartsWith("</"))
		{
			if (current != null && line.Contains(current.Type))
			{
				current.IsProper = true;
				current = current.Parent;
			}
		}
		else if (line.StartsWith("<"))
		{
//			line.Dump();
			Element parent;
			if (current == null)
			{
				parent = new Element(null);
				root = parent;
				root.Type = "root";
			}
			else
			{
				parent = current;
			}
			current = new Element(parent);
			string[] tokens = line.Split(new char[] { '<', ' ', '=', '>', '/', '?' }, StringSplitOptions.RemoveEmptyEntries);
			current.Type = tokens[0];
			current.OpeningTag = line;
			parent.Children.Add(current);
			
			// Self-terminated tag
			if (line.Contains("/>"))
			{
				current.IsProper = true;
				current = current.Parent;
			}
		}
		else
		{
			if (current != null)
			{
				current.Contents.AppendLine(line);
			}
		}
	}
	Element.FilterByCharacter = false;
	//Element.CharacterName = "Character_English_Soldier_01";
	
	root.ToString().Dump();
//	root.Dump();
}

class Element
{
	public Element(Element parent)
	{
		Parent = parent;
		if (Parent != null)
		{
			Indent = $"{Parent.Indent}  ";
		}
	}
	
	public string Indent { get; set; } = string.Empty;
	public string Type { get; set; }
	public string OpeningTag { get; set; }
	public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
	public bool IsProper { get; set; } = false;
	public Element Parent { get; set; } = null;
	public List<Element> Children { get; set; } = new List<Element>();
	public StringBuilder Contents { get; set; } = new StringBuilder();
	
	// Use these values in the ToString() and ToDump() methods to select only those
	// items in the libraries that match the CharacterName specifed.  By making these
	// static I only have to set them once and they're shared by all copies.
	public static bool FilterByCharacter { get; set; } = false;
	public static string CharacterName { get; set; } = string.Empty;

	public override string ToString()
	{
		if (FilterByCharacter)
		{
			if (Parent != null && OpeningTag.Contains("Character_") == true && OpeningTag.Contains(CharacterName) == false)
			{
				return null;
			}
		}
//		string isProper = IsProper ? "Proper" : "Improper";
		StringBuilder result = new StringBuilder();
		result.Append($"{Indent}{OpeningTag}\n");
		foreach (Element child in Children.Distinct())
		{
			result.Append(child.ToString());
		}
		return result.ToString();
	}

	private object ToDump()
	{
		if (FilterByCharacter)
		{
			if (Parent != null && OpeningTag.Contains("Character_") == true && OpeningTag.Contains(CharacterName) == false)
			{
				return null;
			}
		}
		return new { OpeningTag, Children };
	}
	//	{
	//		string[] types = new string[] { "geometry", "controller", "animation", "visual_scene", "node"};
	//		string type = (types.Contains(Type)) ? OpeningTag : Type;
	//		return new { IsProper, type, Children };
//	}
}