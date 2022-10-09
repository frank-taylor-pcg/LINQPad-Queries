<Query Kind="Program" />

void Main()
{
	Phone("777 55 33 22 2");
}

void Phone(string code)
{
	string[] tokens = code.Split();
	
	foreach (string token in tokens)
	{
		int offset = (int.Parse(token[0].ToString()) - 1) * 3;
		offset += token.Length - 1;
		Console.Write((char)('A' + offset));
	}
	Console.WriteLine();
}

class Time {
	public int Hours { get; set; }
	public int Minutes { get; set; }
}
Time BoothTime()
{
	return new Time() { Hours = 20, Minutes = 19 };
}

/* Clocks:
  int minutes = 4 * paris.Minutes + 2 * newyork.Minutes + 2 * london.Minutes;

  int hours = minutes / 60;
  hours += 4 * paris.Hours + 2 * newyork.Hours + 2 * london.Hours;

  minutes = minutes % 60;

  return new Time() { Hours = hours, Minutes = minutes};
*/

/* Steam pipes - adjust the valves to stop the steam venting */

/*
H and M mean "Hours" and "Minutes" the code for the phone booth is 


*/

/* Buttons

First two, then the last two red buttons

It shows which buttons to press:
- as many rectangular buttons as green buttons

- exactly 3 red buttons

- twice as many 4-sided shapes as 6-sided shapes

- no green buttons

- a total of 17 sides on the shapes
*/


int Vitruvian()
{
	int n1 = 23;
	int n2 = 42;
	int next = 0;

	for (int i = 3; i <= 30; i++)
	{
		next = n1 + n2;
		if (next % 2 == 0) { next -= 1; }
		n1 = n2;
		n2 = next;
	}

	return next;
}

// 115 Dimensions is the answer => O N E H U N D R E D A N D F I F T E E N D I M E N S I O N S
void Cypher(string code)
{
	List<char> lstChars = new List<char>();
	int index = 0;
	foreach (char c in code)
	{
		if (index % 2 == 0)
		{
			lstChars.Add((char)(c - 3));
		}
		index++;
	}
	lstChars.Reverse();
	Console.WriteLine(string.Join(' ', lstChars));
}

string Paintings() => "CENA";

// Go back to the first room, enter 247 for the strings


// The gears - calculate the months:  Gears move to 1833
/*
  return (2150 - 1895) * 12 + 1;
*/

1156 is the number of universes