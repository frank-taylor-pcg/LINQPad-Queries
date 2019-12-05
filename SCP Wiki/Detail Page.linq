<Query Kind="Program">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	ParseIndex("http://www.scp-wiki.net/scp-series");
//	ParseDetailPage("http://www.scp-wiki.net/scp-099", "Test Title");
//	ParseDetailPage("http://www.scp-wiki.net/scp-032", "Collapsible blocks");
}

string GetPageSource(string url)
{
	string result = string.Empty;
	WebRequest request = WebRequest.Create(url);
	WebResponse response = request.GetResponse();
	Stream data = response.GetResponseStream();
	using (StreamReader sr = new StreamReader(data))
	{
		result = sr.ReadToEnd();
		result = System.Net.WebUtility.HtmlDecode(result);
	}
	return result;
}

void ParseIndex(string url)
{
	string html = GetPageSource(url);
	List<string> lines = html.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	lines = ExtractLinks(lines);
	foreach (string line in lines)
	{
		int startIndex = line.IndexOf("/");
		int endIndex = line.IndexOf('"', startIndex);
		string relative_url = line.Substring(startIndex, endIndex - startIndex);

		startIndex = line.IndexOf("</a> -") + 6;
		endIndex = line.IndexOf("</li>", startIndex);
		string title = line.Substring(startIndex, endIndex - startIndex).Trim();
		title = Utility.RemoveTags(title);
		
		string scp_number = relative_url.Replace("/", "").ToUpper();
		
		ParseDetailPage($"http://scp-wiki.net{relative_url}", title, scp_number);
	}
}

List<string> ExtractLinks(List<string> lines)
{
	bool bFound = false;
	int iDepth = 0;
	List<string> content = new List<string>();

	foreach (string line in lines)
	{
		if (line.Contains("List of SCPs"))
		{
			bFound = true;
		}
		if (line.Contains("/ul>") && bFound) { iDepth--; }
		if (iDepth > 0) { content.Add(line); }
		if (line.Contains("<ul") && bFound) { iDepth++; }
		if (bFound && line.Contains("/div>")) { bFound = false; }
	}

	return content;
}

void ParseDetailPage(string url, string title, string scp_number)
{
	string html = GetPageSource(url);

	List<string> lines = html.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	lines = ExtractPageContent(lines);

	SCP scp = new SCP();
	scp.Title = title;
	scp.Regions = ExtractRegions(lines);

	scp.Number = scp_number;
	scp.Class = ExtractSpecialRegion("Object Class", scp.Regions);

	scp.References = ExtractReferences(html, scp.Number);
	scp.Image.ExtractURL(lines);
	scp.Image.ExtractCaption(lines);

	scp.Dump(scp_number);
}

string ExtractSpecialRegion(string key, Dictionary<string, string> regions)
{
	regions.TryGetValue(key, out string result);
	regions.Remove(key);
	return result;
}

List<string> ExtractReferences(string html, string itemNumber)
{
	List<string> result = new List<string>();
	MatchCollection matches = Regex.Matches(html, @"SCP\-\d+");
	foreach (Match match in matches)
	{
		foreach (Capture capture in match.Captures)
		{
			result.Add(capture.Value);
		}
	}
	result.RemoveAll(x => x == itemNumber);
	return result.OrderBy(x => x).Distinct().ToList();
}

List<string> ExtractPageContent(List<string> lines)
{
	bool bFound = false;
	int iDepth = 0;
	List<string> content = new List<string>();
	
	foreach (string line in lines)
	{
		if (line.Contains("page-content"))
		{
			bFound = true;
			iDepth++;
		}
		if (line.Contains("<div") && bFound) { iDepth++; }
		if (iDepth > 0) { content.Add(line); }
		if (line.Contains("/div>") && bFound) { iDepth--; }
		if (bFound && iDepth == 0) { bFound = false; }
	}
	
	return content;
}

Dictionary<string, string> ExtractRegions(List<string> lines)
{
	Dictionary<string, string> regions = new Dictionary<string, string>();
	
	string key = null;
	StringBuilder sb = null;
	int iDepth = 0;
	foreach (string line in lines)
	{
		if (line.Contains("<blockquote>")) { iDepth++; }
		if (line.Contains("</blockquote>")) { iDepth--; }
		if (line.Contains("</strong>") && iDepth == 0)
		{
			if (key != null && sb != null)
			{
				regions[key] = sb.ToString().Trim();
			}
			key = ExtractRegionName(line).Trim(new[] {':', ' '});
			sb = new StringBuilder();
		}
		if (line.Contains("footer-wikiwalk-nav"))
		{
			if (key != null && sb != null)
			{
				regions[key] = sb.ToString().Trim();
			}
			key = null;
		}
		if (key != null)
		{
			string clean = Utility.RemoveTags(line);

			if (String.IsNullOrEmpty(key) == false)
			{
				clean = clean?.Replace(key, string.Empty).Trim(new[] { ':', ' ' });
			}

			if (String.IsNullOrWhiteSpace(clean) == false)
			{
				sb.AppendLine(clean);
			}
		}
	}	
	return regions;
}

string ExtractRegionName(string line)
{
	string strResult = null;
	string[] tokens = line.Split(new[] { '<', '>' });
	int index = Array.FindIndex(tokens, token => token == "/strong");
	strResult = tokens[index - 1].Trim();
	return strResult;
}

public static class Utility
{
	public static List<string> RemoveEmptyLines(List<string> lines)
	{
		List<string> lstResult = new List<string>();
		foreach (string line in lines)
		{
			if (string.IsNullOrWhiteSpace(line) == false)
			{
				lstResult.Add(line.Trim());
			}
		}
		return lstResult;
	}
	
	public static string RemoveTags(string line)
	{
		return Regex.Replace(line, "<.*?>", String.Empty);
	}
}

class Image
{
	public string URL { get; set; }
	public string Caption { get; set; }

	public void ExtractURL(List<string> lines)
	{
		string result = string.Empty;
		foreach (string line in lines)
		{
			if (line.Contains("img src"))
			{
				int startIndex = line.IndexOf("img src");
				startIndex = line.IndexOf('"', startIndex) + 1;
				int length = line.IndexOf('"', startIndex) - startIndex;
				result = line.Substring(startIndex, length);
			}
		}
		URL = result;
	}

	public void ExtractCaption(List<string> lines)
	{
		bool bFound = false;
		StringBuilder sb = new StringBuilder();
		foreach (string line in lines)
		{
			if (line.Contains("/div"))
			{
				bFound = false;
			}
			if (bFound)
			{
				sb.AppendLine(line);
			}
			if (line.Contains("scp-image-caption"))
			{
				bFound = true;
			}
		}
		string result = Utility.RemoveTags(sb.ToString());
		Caption = result.Trim();
	}

	object ToDump()
	{
		dynamic result = new ExpandoObject();
		if (String.IsNullOrEmpty(URL) == false)
		{
			result.Image = Util.Image(URL);
			result.URL = URL;
			result.Caption = Caption;
		}
		else
		{
			result = "No Image On File";
		}
		
		return (object)result;
	}
}

class SCP
{
	public string Title { get; set; }
	public string Number { get; set; }
	public string Class { get; set; }
	
	public Image Image { get; set; } = new Image();
	public Dictionary<string, string> Regions { get; set; } = new Dictionary<string, string>();
	public List<string> References { get; set; } = new List<string>();
}