<Query Kind="Program">
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	EnumOne e = EnumOne.ONE;
	string name = GetFullName(e).Dump();
	
	string[] tokens = name.Split('.');
	string strType = tokens[0].Dump();
	Type t = Type.GetType(strType, false, true).Dump();
	
	var result = Enum.Parse(t, tokens[1]);
	result.Dump();
}

public string GetFullName<T>(T obj)
{
	return $"{obj.GetType().ToString()}.{obj.ToString()}";
}

public enum EnumOne
{
	NONE,
	ONE,
	TWO,
	THREE
}

public enum EnumTwo
{
	UNKNOWN,
	A,
	B,
	C
}