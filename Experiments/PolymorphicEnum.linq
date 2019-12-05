<Query Kind="Program">
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Runtime.CompilerServices</Namespace>
</Query>

static Default DefaultEnum = new Default();
static Number NumberEnum = new Number();
static Animal AnimalEnum = new Animal();
static Letter LetterEnum = new Letter();

void Main()
{
	// DumpContainer, Dump and the Util. functions are specific to LINQPad.
	DumpContainer dcDefault = new DumpContainer().Dump("Default");
	dcDefault.Content = Util.HorizontalRun(true, DefaultEnum.AllValues, Default.Values, DefaultEnum.SerializeValues());

	DumpContainer dcNumber = new DumpContainer().Dump("Number");
	dcNumber.Content = Util.HorizontalRun(true, NumberEnum.AllValues, Number.Values, NumberEnum.SerializeValues());

	DumpContainer dcAnimal = new DumpContainer().Dump("Animal");
	dcAnimal.Content = Util.HorizontalRun(true, AnimalEnum.AllValues, Animal.Values, AnimalEnum.SerializeValues());

	DumpContainer dcLetter = new DumpContainer().Dump("Letter - Covers the same numeric range as Number");
	dcLetter.Content = Util.HorizontalRun(true, LetterEnum.AllValues, Letter.Values, LetterEnum.SerializeValues());

	DumpContainer dcOne = new DumpContainer().Dump("Test One: switch on just the type");
	dcOne.Content = Util.VerticalRun(
		TestOne(Default.UNKNOWN, "Default.UNKNOWN"),
		TestOne(Number.ONE, "Number.ONE"),
		TestOne(Animal.DOG, "Animal.DOG"),
		TestOne(Letter.X, "Letter.X"),
		TestOne(Number.UNKNOWN, "Number.UNKNOWN")
	);

	DumpContainer dcTwo = new DumpContainer().Dump("Test Two: switch on the full value");
	dcTwo.Content = Util.VerticalRun(TestTwo(Default.UNKNOWN), TestTwo(Number.ONE), TestTwo(Animal.DOG), TestTwo(Letter.X), TestTwo(Letter.UNKNOWN));

	DumpContainer dcThree = new DumpContainer().Dump("Test Three: switch on value using Number values only");
	dcThree.Content = Util.VerticalRun(TestThree(Animal.UNKNOWN), TestThree(Animal.ONE), TestThree(Animal.DOG));

	PolymorphicEnum.Values.Dump("PolymorphicEnum value list should remain empty");
}

#region TEST FUNCTIONS
public string TestOne(EnumValue val, string name)
{
	Dictionary<Type, Func<string>> @switch = new Dictionary<Type, Func<string>>()
	{
		{typeof(Default), () => $"Default Type Found when testing {name}" },
		{typeof(Number), () => $"Number Type Found when testing {name}" },
		{typeof(Animal), () => $"Animal Type Found when testing {name}" },
		{typeof(Letter), () => $"Letter Type Found when testing {name}" }
	};
	
	return @switch[val.TypeRef]();
}

public string TestTwo(EnumValue val)
{
	Dictionary<EnumValue, Func<string>> @switch = new Dictionary<EnumValue, Func<string>>()
	{
		{Default.UNKNOWN, () => "Default.UNKNOWN Found" },
		{Number.ONE, () => "Number.ONE Found" },
		{Animal.DOG, () => "Animal.DOG Found" },
		{Letter.X, () => "Letter.X Found" },
		// Can't do this, it matches Default.UNKNOWN - This one is important. It means that we can't
		// distinguish between the base and Number values as they're actually just a single value.
		// {Letter.UNKNOWN, () => "Letter.UKNOWN found"}
	};

	return @switch[val]();
}

public string TestThree(EnumValue val)
{
	Dictionary<EnumValue, Func<string>> @switch = new Dictionary<EnumValue, Func<string>>()
	{
		{Animal.UNKNOWN, () => "Animal.UNKNOWN Found" },
		{Animal.ONE, () => "Animal.ONE Found" },
		{Animal.DOG, () => "Animal.DOG Found" },
	};

	return @switch[val]();
}
#endregion TEST FUNCTIONS

public class EnumValue
{
	public Type TypeRef;
	public readonly string Name;
	public readonly int Value;

	public EnumValue(Type type, string strName, int iValue)
	{
		TypeRef = type;
		Name = strName;
		Value = iValue;
	}
	
	private static int GetCurrentValue(Type type)
	{
		int result = -1;
		FieldInfo field = type.GetField("CurrentValue", BindingFlags.Public | BindingFlags.Static);
		if (field != null)
		{
			result = (int)field.GetValue(null);
		}

		return result;
	}
	
	private static (Type, int) GetTypeAndCurrentValue(string name, int? inValue = null)
	{
		int value = 0;
		// This is slow, but should only happen once for each EnumValue.
		StackFrame frame = new StackFrame(2);
		Type type = frame.GetMethod().DeclaringType;
		FieldInfo field = type.GetField("CurrentValue", BindingFlags.Public | BindingFlags.Static);
		if (inValue == null)
		{
			value = GetCurrentValue(type);
		}
		else
		{
			value = (int)inValue;
		}
		field.SetValue(null, value + 1);
		
		return (type, value);
	}

	public static implicit operator EnumValue(string name)
	{
		(Type type, int value) = GetTypeAndCurrentValue(name);		
		EnumValue result = new EnumValue(type, name, value++);
		return result;
	}

	public static implicit operator EnumValue((string, int) tuple)
	{
		(Type type, int value) = GetTypeAndCurrentValue(tuple.Item1, tuple.Item2);		
		EnumValue result = new EnumValue(type, tuple.Item1, tuple.Item2);
		return result;
	}
}

class PolymorphicEnum
{
	public static int CurrentValue = 0;

	public static List<EnumValue> Values = new List<EnumValue>();
	public virtual List<EnumValue> AllValues => Values;

	public List<string> SerializeValues()
	{
		List<string> result = new List<string>();
		foreach (EnumValue e in AllValues)
		{
			result.Add(JsonConvert.SerializeObject(e));
		}
		return result;
	}
}

class Default : PolymorphicEnum
{
	// This has to be set ABOVE any EnumValue entries or the value gets screwed up.
	public static new int CurrentValue = PolymorphicEnum.CurrentValue;
	
	public static readonly EnumValue UNKNOWN = "Unknown";
	
	// How can I avoid having to do these steps for every single derivation?
	public override List<EnumValue> AllValues => base.AllValues.Concat(Values).ToList();
	public static new List<EnumValue> Values = new List<EnumValue>() { UNKNOWN };
}

class Number : Default
{
	// This has to be set ABOVE any EnumValue entries or the value gets screwed up.
	public static new int CurrentValue = Default.CurrentValue;
	
	public static readonly EnumValue ONE = "One"; 
	public static readonly EnumValue TWO = "Two";
	public static readonly EnumValue THREE = "Three";

	// How can I avoid having to do these steps for every single derivation?
	public override List<EnumValue> AllValues => base.AllValues.Concat(Values).ToList();
	public static new List<EnumValue> Values = new List<EnumValue>() { ONE, TWO, THREE };
}

class Animal : Number
{
	// This has to be set ABOVE any EnumValue entries or the value gets screwed up.
	public static new int CurrentValue = Number.CurrentValue;
	
	// This enum starts at 100.  This will allow overlap with inherited values.
	public static readonly EnumValue DOG = ("Dog", 100);
	// This will have a backing value one higher than it's predecessor
	public static readonly EnumValue CAT = "Bird";
	// Why would we use the same numeric value for multiple entries? I don't know, but we can.
	// Perhaps we want to have categories within an enum.  All 100s could be "furry animals"
	// then we check the EnumValue.Name property to determine which one.
	public static readonly EnumValue BIRD = ("Cat", 100);

	// How can I avoid having to do these steps for every single derivation?
	public override List<EnumValue> AllValues => base.AllValues.Concat(Values).ToList();
	public static new List<EnumValue> Values = new List<EnumValue>() { DOG, CAT, BIRD };
}

class Letter : Default
{
	// This has to be set ABOVE any EnumValue entries or the value gets screwed up.
	public static new int CurrentValue = Default.CurrentValue;
	
	public static readonly EnumValue X = "X";
	public static readonly EnumValue Y = "Y";
	public static readonly EnumValue Z = "Z";

	// How can I avoid having to do these steps for every single derivation?
	public override List<EnumValue> AllValues => base.AllValues.Concat(Values).ToList();
	public static new List<EnumValue> Values = new List<EnumValue>() { X, Y, Z };
}