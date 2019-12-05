<Query Kind="Program" />

void Main()
{
}

class Element
{
	
}

class Contributor
{
	public string Author { get; set; }
	public string AuthoringTool { get; set; }
	public string Comments { get; set; }
	public string Copyright { get; set; }
	public string SourceData { get; set; }
}

class Asset
{
	public Contributor Contributer = new Contributor();
	public DateTime Created { get; set; }
	public Element Keywords { get; set; }
	public DateTime Modified { get; set; }
	public string Revision { get; set; }
	public string Subject { get; set; }
	public string Title { get; set; }
	public Element Unit { get; set; }
	public Element UpAxis { get; set; }
}

enum LibraryType
{
	ANIMATION,
	CAMERA,
	CONTROLLER,
	EFFECT,
	GEOMETRY,
	IMAGE,
	LIGHT,
	MATERIAL,
	NODE
}

class Library<TLibraryType>
{
	public string ID { get; set; }
	public string Name { get; set; }
	public Asset Asset { get; set; }
//	public Extra Extra { get; set; }
	public TLibraryType Type { get; set; }
}


class Collada
{
	public string Version { get; set; }
	
}

class Unit
{
	public string Name { get; set; }
	public float Meter { get; set; }
}