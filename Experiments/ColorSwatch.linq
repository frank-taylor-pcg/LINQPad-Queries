<Query Kind="Program">
  <Namespace>System.Drawing</Namespace>
</Query>

void Main()
{
	int iNumColors = GetNonSystemColors().Count();
	
	int iNumCols = 4;
	int iNumRows = iNumColors / iNumCols;
	if (iNumColors % iNumRows > 0) iNumRows++;

	List<Color>[] Colors = new List<Color>[iNumCols];
	
	int iRow = 0;
	int iCol = 0;
	
	for (int i = 0; i < iNumCols; i++)
	{
		Colors[i] = new List<Color>();
	}
	
	foreach (Color color in GetNonSystemColors().OrderBy(x => x.R).ThenBy(x => x.G).ThenBy(x => x.B))
	{
		Colors[iCol].Add(color);
		iRow++;
		if (iRow >= iNumRows)
		{
			iCol++;
			iRow = 0;
		}
	}
	
	Util.HorizontalRun(true, Colors.ToArray()).Dump();
	Util.HorizontalRun(true, Colors[0].SetSuccess(), Colors[1].SetWarning(), Colors[2].SetDanger(), Colors[3].SetSecondary()).Dump();
}

List<Color> GetNonSystemColors()
{
	List<Color> result = new List<Color>();
	
	foreach (KnownColor knownColor in Enum.GetValues(typeof(KnownColor)))
	{
		Color color = Color.FromKnownColor(knownColor);
		if (false == color.IsSystemColor)
		{
			result.Add(color);
		}
	}
	
	return result;
}