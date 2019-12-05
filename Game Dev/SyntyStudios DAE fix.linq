<Query Kind="Program" />

string strBase = @"F:\projects\godot\TheWorld\Village\assets\PolygonPirates";
string strSource = "Models";
string strDest = "Models";

void Main()
{
	string strSrcPath = Path.Combine(strBase, strSource);
	string strDestPath = Path.Combine(strBase, strDest);

	List<string> new_lines = new List<string>();

	foreach (string filename in Directory.GetFiles(strSrcPath, "*.dae"))
	{
		FileInfo fi = new FileInfo(filename);
		
		
		string lines = File.ReadAllText(filename);
		lines = FixHeader(lines);
		lines = FixImageGarbage(lines);
		lines = FixMisc(lines);
		
		string outfile = Path.Combine(strDestPath, fi.Name).Dump();
		File.WriteAllText(outfile, lines);
	}
}

string FixHeader(string line)
{
	string result = line;
	result = Regex.Replace(result, "FBX COLLADA exporter", "LINQPad", RegexOptions.IgnoreCase);
	result = Regex.Replace(result, "meter=\"1.000000\" name=\"meter\"", "meter=\"0.010000\" name=\"meter\"");

	return result;
}

string FixMisc(string line)
{
	string result = line;
	result = Regex.Replace(result, "lambert[0-9]", "blinn");
	result = Regex.Replace(result, "blinn[0-9]", "blinn");
	result = Regex.Replace(result, "Texture_01_A", "blinn");
	result = Regex.Replace(result, "Texture_01", "blinn");
	result = Regex.Replace(result, "aTexture_0[0-9]{1,3}", "blinn");
	return result;
}

string FixImageGarbage(string line)
{
	string result = line;
	// Fix the Image ID garbage
	result = Regex.Replace(result, "file[0-9]{1,4}", "blinn");
	result.Replace("ROCK_lambert8SG1F", "blinn");
	result.Replace("ROCK_blinnSG1", "blinn");

	// Fix the image path garbage (like the following)
	result = Regex.Replace(result, "file://.*</init_from>", "file://blinn.png");
	return result;
}


