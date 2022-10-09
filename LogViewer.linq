<Query Kind="Program" />

string log = @"
00:08:32.6789 : 1023[Warning]-event (MRE) Timeout  Process:Chipping/Wait MRE {Looking for next seed}  PROCESS:Chipping/Wait MRE
                1006[Warning]-Thread information.  Thread:Singulation Handler/EventTimeout  THREAD:Singulation Handler/EventTimeout
                2100[Warning]-Machine information  Machine:1/Production  MACHINE:1/Production  Last Reboot:11/19/2021 1:24:06 AM
01:23:45.6789 : 1813[Error]-The movement is not within coarse tolerance  Axis:End Mill Channel 2/Move to center {Positional Tolerance/Error:0.1/-2.42391706385408}  AXIS:End Mill Channel 2/Move to center
                1052[Error]-Axis error.  Process:Chipping/RSI Axis Wait For {Jumping to step 33}  PROCESS:Chipping/RSI Axis Wait For
                1006[Error]-Thread information.  Thread:Chipping Platform 2/ProcessErrorRSIAxis  THREAD:Chipping Platform 2/ProcessErrorRSIAxis
                2100[Error]-Machine information  Machine:1/Production  MACHINE:1/Production  Last Reboot:11/19/2021 1:24:06 AM
01:23:46.6789 : 1813[Error]-The movement is not within coarse tolerance  Axis:End Mill Channel 2/Move to center {Positional Tolerance/Error:0.1/-2.42391706385408}  AXIS:End Mill Channel 2/Move to center
                1052[Error]-Axis error.  Process:Chipping/RSI Axis Wait For {Jumping to step 33}  PROCESS:Chipping/RSI Axis Wait For
                1006[Error]-Thread information.  Thread:Chipping Platform 2/ProcessErrorRSIAxis  THREAD:Chipping Platform 2/ProcessErrorRSIAxis
                2100[Error]-Machine information  Machine:1/Production  MACHINE:1/Production  Last Reboot:11/19/2021 1:24:06 AM
01:23:47.6789 : 1813[Error]-The movement is not within coarse tolerance  Axis:End Mill Channel 2/Move to center {Positional Tolerance/Error:0.1/-2.42391706385408}  AXIS:End Mill Channel 2/Move to center
                1052[Error]-Axis error.  Process:Chipping/RSI Axis Wait For {Jumping to step 33}  PROCESS:Chipping/RSI Axis Wait For
                1006[Error]-Thread information.  Thread:Chipping Platform 2/ProcessErrorRSIAxis  THREAD:Chipping Platform 2/ProcessErrorRSIAxis
                2100[Error]-Machine information  Machine:1/Production  MACHINE:1/Production  Last Reboot:11/19/2021 1:24:06 AM
01:23:48.6789 : 1813[Error]-The movement is not within coarse tolerance  Axis:End Mill Channel 2/Move to center {Positional Tolerance/Error:0.1/-2.42391706385408}  AXIS:End Mill Channel 2/Move to center
                1052[Error]-Axis error.  Process:Chipping/RSI Axis Wait For {Jumping to step 33}  PROCESS:Chipping/RSI Axis Wait For
                1006[Error]-Thread information.  Thread:Chipping Platform 2/ProcessErrorRSIAxis  THREAD:Chipping Platform 2/ProcessErrorRSIAxis
                2100[Error]-Machine information  Machine:1/Production  MACHINE:1/Production  Last Reboot:11/19/2021 1:24:06 AM
";

List<LogEntry> LogEntries = new();

void Main()
{
	string[] lines = log.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

	LogEntry logEntry = null;

	foreach (string line in lines)
	{
		LineEntry lineEntry = new(line);

		if (lineEntry.Timestamp != null)
		{
			AddLogEntry(logEntry);
			logEntry = new()
			{
				Primary = lineEntry
			};
		}
		else
		{
			logEntry?.Extra.Add(lineEntry);
		}
	}

	AddLogEntry(logEntry);
	LogEntries.Dump();
}

public void AddLogEntry(LogEntry entry)
{
	int index = LogEntries.Count() - 1;
	
	// Add the previous log entry if it exists
	if (entry != null)
	{
		// Would probably need to do a bit deeper check to avoid hiding some non-duplicates
		if (index >= 0 && entry.Primary.ErrorCode.Equals(LogEntries[index].Primary.ErrorCode))
		{
			LogEntries[index].Duplicates.Add(entry);
		}
		else
		{
			LogEntries.Add(entry);
			index++;
		}
	}
}

public enum SeverityLevel
{
	UNKNOWN,
	INFO,
	WARNING,
	ERROR
}

public class LogEntry
{
	public LineEntry Primary { get; set; }
	public List<LineEntry> Extra { get; set; } = new List<LineEntry>();
	public List<LogEntry> Duplicates = new List<LogEntry>();
	public int Count => Duplicates.Count();
	
	object ToDump() => new
	{
		Primary,
		Extra,
		Count,
		Duplicates = new Lazy<List<LogEntry>>(() => Duplicates)
	};
}

public class LineEntry
{
	private string line;

	public TimeOnly? Timestamp { get; set; } = null;
	public int ErrorCode { get; set; } = -1;
	public SeverityLevel Severity { get; set; }
	public string Description { get; set; }
	public List<string> Details { get; set; } = new();

	private string[] separators = new string[] { " : ", "[", "]-", "  " };

	public LineEntry(string line)
	{
		this.line = line;
		string[] tokens = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
		Parse(tokens);
	}

	private void Parse(string[] tokens)
	{
		int index = 0;
		
		// Attempts to extract a timestamp, only increments the token index if one is found
		if (ExtractTimeStamp(tokens[index]))
		{
			index++;
		}

		GetErrorCode(tokens[index++]);
		GetSeverity(tokens[index++]);
		Description = tokens[index++];

		for (int i = index; i < tokens.Length; i++)
		{
			Details.Add(tokens[i]);
		}
	}

	// Pull the timestamp from the line if it exists
	private bool ExtractTimeStamp(string token)
	{
		bool success = TimeOnly.TryParse(token, out TimeOnly value);
		if (success)
		{
			Timestamp = value;
		}
		return success;
	}

	private void GetErrorCode(string token)
	{
		bool success = int.TryParse(token, out int value);
		if (success)
		{
			ErrorCode = value;
		}
	}

	private void GetSeverity(string token)
	{
		switch (token)
		{
			case "Info": Severity = SeverityLevel.INFO; break;
			case "Warning": Severity = SeverityLevel.WARNING; break;
			case "Error": Severity = SeverityLevel.ERROR; break;
			default: Severity = SeverityLevel.UNKNOWN; break;
		}
	}
}