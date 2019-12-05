<Query Kind="Program">
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
</Query>

// Start by getting the frequencies of each number
static Dictionary<string, int> getFrequencies(string [] tokens)
{
	Dictionary<string, int> result = new Dictionary<string, int>();
	
	foreach(var i in tokens) {
		if (result.ContainsKey(i)) {
			result[i] += 1;
		}
		else {
			result[i] = 1;
		}
	}
	
	return result;
}

// Get the overall array frequency
static int getArrayFrequency(Dictionary<string, int> freqs)
{
	int result = 0;
	// For some reason I can't iterate over the Dictionary with LINQ
	foreach(KeyValuePair<string, int> entry in freqs) {
		if (entry.Value > result) {
			result = entry.Value;
		}
	}
	return result;
}

// Get the elements that have frequencies matching the overall array frequency
static List<string> getMatchingFrequencies(Dictionary<string, int> freqs, int arr_freq) {
	List<string> result = new List<string>();
	
	foreach(KeyValuePair<string, int> entry in freqs) {
		if (entry.Value == arr_freq) {
			result.Add(entry.Key);
		}
	}
	
	return result;
}

// Get the length of the array containing all instances of the given element
static int getRunLength(string [] tokens, string s) {
	int start = Array.IndexOf(tokens, s);
	int end = Array.LastIndexOf(tokens, s);
	
	return end - start + 1;
}

static void Main(string[] args) {
	int size = Convert.ToInt32(Console.ReadLine());
	string elements = Console.ReadLine();
	
	string [] tokens = elements.Split(' ');
	
	Dictionary <string, int> frequencies = getFrequencies(tokens);
	int arrayFrequency = getArrayFrequency(frequencies);
	List<string> matches = getMatchingFrequencies(frequencies, arrayFrequency);
	
	// This assumes that the size entered is correct. Feels wrong, should I ignore the entered size and use the array size instead?
	int min_length = size;
	
	foreach (var m in matches) {
		int length = getRunLength(tokens, m);
		if (length < min_length) {
			min_length = length;
		}
	}
	
	// Display the result
	Console.WriteLine(min_length);
}