<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
  <UseNoncollectibleLoadContext>true</UseNoncollectibleLoadContext>
</Query>

void Main()
{
	string strTestObject = "Test Object";
	
	Test test = new()
	{
		MyDouble = 2.0,
		MyFloat = 3f,
		MyInt = 4,
		MyObject = strTestObject,
		MyString = "Test String"
	};
	
	JsonConvert.SerializeObject(test, Newtonsoft.Json.Formatting.Indented).Dump();
	
	Console.WriteLine();
	
	XmlSerializer xml = new XmlSerializer(typeof(Test));
	
	StringWriter sw = new();
	xml.Serialize(sw, test);
	sw.Dump();
	sw.ToString().Dump("StringWriter");

	StringReader sr = new(sw.ToString());
	StringBuilder sb = new();

	int index = 0;
	while (sr.Peek() > -1)
	{
		string line = sr.ReadLine();
		if (index != 3)
		{
			sb.AppendLine(line);
		}
		index++;
	}

	sb.Dump("StringBuilder");

	StringReader testSR = new(sb.ToString());
	xml.Deserialize(testSR).Dump("Deserialized object");
}

[Serializable]
public class Test
{
	public double MyDouble { get; set; }
	public float MyFloat { get; set; }
	public int MyInt { get; set; }
	public object MyObject { get; set; }
	public string MyString { get; set; }
}