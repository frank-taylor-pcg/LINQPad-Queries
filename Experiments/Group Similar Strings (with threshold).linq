<Query Kind="Program">
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

// This is still screwed up, but I'm tired of messing with it./
void Main()
{
//	TestEnums(CompareStringsByPairs);
//	TestEnums(CompareStringsByWords);
	TestEnums(CompareStringsByOrderedWords);
}

delegate double ComparisonDelegate(string arg1, string arg2);

void TestEnums(ComparisonDelegate comparison)
{
	double threshold = 0.37;
	List<string> descriptions = new List<string>();
	foreach (Enum value in Enum.GetValues(typeof(ConsoleColor)))
	{
		descriptions.Add(value.GetEnumDescription());
	}
	descriptions.Sort();

	List<(string, string, double)> similarity = new List<(string, string, double)>();

	for (int i = 0; i < descriptions.Count - 1; i++)
	{
		for (int j = i + 1; j < descriptions.Count; j++)
		{
			double dSimilarity = comparison(descriptions[i], descriptions[j]);
			if (dSimilarity > threshold)
			{
				similarity.Add((descriptions[i], descriptions[j], dSimilarity));
			}
		}
	}
	List<(string, string, double)> sorted = similarity.OrderBy(x => x.Item1).ThenByDescending(x => x.Item3).ToList();
	//	sorted.Dump();

	List<List<string>> grouped = new List<List<string>>();

	List<string> unmatched = new List<string>();
	List<string> matched = new List<string>();

	for (int i = 0; i < descriptions.Count - 1; i++)
	{
		if (similarity.Count(x => x.Item1 == descriptions[i]) == 0)
		{
			unmatched.Add(descriptions[i]);
		}
		else
		{
			List<string> current = new List<string>();
			current.Add(descriptions[i]);
			for (int j = i + 1; j < descriptions.Count; j++)
			{
				double dSimilarity = CompareStringsByPairs(descriptions[i], descriptions[j]);
				double iMaxSimilarity = similarity.Where(x => x.Item1 == descriptions[i] && x.Item2 == descriptions[j]).Select(x => x.Item3).DefaultIfEmpty(2.0).Max();
				// For ease of debugging
				string si = descriptions[i];
				string sj = descriptions[j];
				if (dSimilarity >= iMaxSimilarity && matched.Contains(descriptions[j]) == false)
				{
					current.Add(descriptions[j]);
					matched.Add(descriptions[j]);
				}
			}
			if (current.Count > 1)
			{
				grouped.Add(current);
			}
		}
	}
	Util.HorizontalRun(true, unmatched, grouped).Dump();
}

List<string> GetPairs(string strInput) // Do we keep the spaces or discard them?
{
	List<string> result = new List<string>();
	for (int i = 0; i < strInput.Length - 1; i++)
	{
		string strPair = strInput.Substring(i, 2);
		if (strPair.Contains(" ") == false)
		{
			result.Add(strPair);
		}
	}
	return result;
}

double CompareStringsByPairs(string str1, string str2)
{
	List<string> pairs1 = GetPairs(str1);
	List<string> pairs2 = GetPairs(str2);
	
	int iMatchesFound = pairs1.Intersect(pairs2).Count();
	int iTotalSize = pairs1.Count() + pairs2.Count();
	
	return (2.0 * iMatchesFound / iTotalSize);
}

double CompareStringsByWords(string str1, string str2)
{
	List<string> words1 = str1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	List<string> words2 = str2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	
	int iMatchesFound = words1.Intersect(words2).Count();
	int iTotalSize = words1.Count() + words2.Count();
	
	return (2.0 * iMatchesFound / iTotalSize);
}

double CompareStringsByOrderedWords(string str1, string str2)
{
	List<string> words1 = str1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	List<string> words2 = str2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	
	int iMin = Math.Min(words1.Count(), words2.Count());

	double dOrderMatchesFound = 0.0;
	
	double dWeight = 2.0;

	for (int i = 0; i < iMin; i++)
	{
		if (words1[i].Equals(words2[i]))
		{
			dOrderMatchesFound += dWeight;
		}
		dWeight -= 0.5;
	}
	int iMatchesFound = words1.Intersect(words2).Count();
	int iTotalSize = words1.Count() + words2.Count();
	
	return (dOrderMatchesFound + iMatchesFound) / iTotalSize;
}