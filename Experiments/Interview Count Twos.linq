<Query Kind="Program">
  <Namespace>System</Namespace>
  <Namespace>System.IO</Namespace>
</Query>

static int countThem(int num) {
	int result = 0;
	
	string work = num.ToString();
	
	result = work.Split('2').Length - 1;
	
	return result;
}

void Main()
{
	/*int n;
	Int32.TryParse(Console.ReadLine(), out n);
	int count = 0;
	
	if (n > 0) {
		for (int i = 2; i <= n; i++) {
			count += countThem(i);
		}
	}
	Console.WriteLine(count);
	*/
	
	for (int i = 1; i <= 10; i++) {
		Console.WriteLine("{0} has {1} twos in it", i, countThem(3));
	}
}

// Define other methods and classes here
