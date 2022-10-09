<Query Kind="Program" />

const int NUM_ITERATIONS = 1_000_000_000;

void Main()
{
	Console.WriteLine($"Comparison over {NUM_ITERATIONS:N0} iterations:");
	Console.WriteLine($"  Using Actions  : {PerformTest(new FirstRunIsSpecialCaseAction())}");
	Console.WriteLine($"  Using a boolean: {PerformTest(new FirstRunIsSpecialCaseBoolean())}");
}

interface ISpecialCase
{
	public void DoRun();
}

double PerformTest(ISpecialCase specialCase)
{
	DateTime testStart = DateTime.Now;
	for (int i = 0; i < NUM_ITERATIONS; i++)
	{
		specialCase.DoRun();
	}
	DateTime testEnd = DateTime.Now;
	return (testEnd - testStart).TotalMilliseconds;
}

class FirstRunIsSpecialCaseAction : ISpecialCase
{
	private Action actDoRun;
	private int numTimes = 0;

	public FirstRunIsSpecialCaseAction() => actDoRun = DoFirstRun;
	
	private void DoFirstRun()
	{
		actDoRun = DoAllOtherRuns;
		numTimes++;
	}
	
	private void DoAllOtherRuns() => numTimes++;
	public void DoRun() => actDoRun(); // If I wasn't implementing the ISpecialCase interface for this comparison the action itself could be made public and called directly.
}

class FirstRunIsSpecialCaseBoolean : ISpecialCase
{
	private bool bIsFirstRun = true;
	private int numTimes = 0;
	
	public void DoFirstRun()
	{
		numTimes++;
		bIsFirstRun = false;
	}

	private void DoAllOtherRuns() => numTimes++;

	public void DoRun()
	{
		if (bIsFirstRun)
		{
			DoFirstRun();
		}
		else
		{
			DoAllOtherRuns();
		}
	}
}