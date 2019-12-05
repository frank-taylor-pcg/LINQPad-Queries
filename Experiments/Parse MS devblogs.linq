<Query Kind="Program">
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	string url = @"https://devblogs.microsoft.com/dotnet/introducing-the-new-microsoftdatasqlclient/";

	WebClient wc = new WebClient();
	string result = wc.DownloadString(url);
	List<string> lines = result.Replace("><", ">\n<").Split('\n').ToList();
	
	List<string> content = Block.Extract(lines, "<div class=\"entry-content", "</div>", "<div").Dump();
	
	// Works great if there is only one table, needs to be tested with multiple
	// have an overload that takes an offset maybe and have this return the offset
	List<string> table_lines = Block.Extract(lines, "<table>", "</table>").Dump();
	
	Table.BuildTable(table_lines).Dump();
}

class Block
{
	public static List<string> Extract(List<string> page, string startingTag, string endingTag, string nestedBlockBegin = null)
	{
		List<string> result = new List<string>();
		bool bBlockFound = false;
		int iDepth = 0;
		
		foreach (string line in page)
		{
			if (line.Contains(startingTag))
			{
				bBlockFound = true;
				iDepth = 1;
			}
			if (bBlockFound && nestedBlockBegin != null && line.Contains(nestedBlockBegin)) { iDepth++; }
			if (bBlockFound) { result.Add(line); }
			if (bBlockFound && line.Contains(endingTag)) { iDepth--; }
			if (bBlockFound && iDepth == 0) { bBlockFound = false; }
		}
		return result;
	}
}

class Table
{
	private List<List<string>> _data = new List<List<string>>();
	
	#region Overloaded [] operator
	public string this[int x, int y]
	{
		get
		{
			return _data[x][y];
		}
		set
		{
			int numRows = _data.Count();
			if (numRows == y)
			{
				_data.Add(new List<string>());
			}
			_data[y].Add(value);
		}
	}
	#endregion Overloaded [] operator

	public static Table BuildTable(List<string> lines)
	{
		Table table = null;
		int iRow = -1;
		int iCol = -1;

		foreach (string line in lines)
		{
			if (line.Contains("<table>")) { table = new Table(); }
			if (line.Contains("<tr>")) { iRow++; }
			if (line.Contains("<td>") || line.Contains("<th>")) { iCol++; table[iCol, iRow] = CellCleanup(line); }
			if (line.Contains("</tr>")) { iCol = -1; }
			if (line.Contains("</table>")) { iRow = -1; iCol = -1; }
		}
		return table;
	}

	private static string CellCleanup(string line)
	{
		return line
			.Replace("<th>", "")
			.Replace("</th>", "")
			.Replace("<td>", "")
			.Replace("</td>", "")
			.Replace("&#8211;", "-");
	}

	private object ToDump()
	{
		string[,] result = new string[_data.Count(), _data[0].Count()];
		
		for (int y = 0; y < _data[0].Count(); y++)
		{
			for (int x = 0; x < _data.Count(); x++)
			{
				result[x, y] = _data[x][y];
			}
		}
		
		return result;
	}
}
