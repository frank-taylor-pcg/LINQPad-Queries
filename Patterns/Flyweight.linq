<Query Kind="Program" />

// Flyweight pattern
// https://www.dofactory.com/net/flyweight-design-pattern
// Most likely I'll never end up using this pattern

void Main()
{
	string document = "AAZZBBZB";
	char[] chars = document.ToCharArray();
	CharacterFactory factory = new CharacterFactory();
	
	// extrinsic state
	int pointSize = 10;
	
	foreach (char c in chars)
	{
		pointSize++;
		Character character = factory.GetCharacter(c);
		character.Display(pointSize);
	}
}

class CharacterFactory
{
	private Dictionary<char, Character> _characters = new Dictionary<char, Character>();
	public Character GetCharacter(char key)
	{
		Character character = null;
		if (_characters.ContainsKey(key))
		{
			character = _characters[key];
		}
		else
		{
			switch (key)
			{
				case 'A': character = new CharacterA(); break;
				case 'B': character = new CharacterB(); break;
				//... blah blah tons more
				case 'Z': character = new CharacterZ(); break;
			}
			_characters.Add(key, character);
		}
		return character;
	}
}

abstract class Character
{
	protected char symbol;
	protected int width;
	protected int height;
	protected int ascent;
	protected int descent;
	protected int pointSize;
	public void Display(int pointSize)
	{
		this.pointSize = pointSize;
		Console.WriteLine($"{symbol} (pointSize {pointSize})");
	}
}

class CharacterA : Character
{
	public CharacterA()
	{
		symbol = 'A';
		height = 100;
		width = 120;
		ascent = 70;
		descent = 0;
	}
}
class CharacterB : Character
{
	public CharacterB()
	{
		symbol = 'B';
		height = 100;
		width = 140;
		ascent = 72;
		descent = 0;
	}
}
class CharacterZ : Character
{
	public CharacterZ()
	{
		symbol = 'Z';
		height = 100;
		width = 100;
		ascent = 68;
		descent = 0;
	}
}