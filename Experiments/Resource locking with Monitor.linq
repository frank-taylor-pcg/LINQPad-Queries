<Query Kind="Program" />

static readonly object objLock = new object();
static int iValue = 0;

void Main()
{
	Stopwatch stopWatch = new Stopwatch();
	stopWatch.Start();
	
	for (int i = 0; i < 100; i++)
	{
		Thread a = new Thread(new ParameterizedThreadStart(AddThread));
		Thread b = new Thread(new ParameterizedThreadStart(SubtractThread));
		a.Start(100 - i);
		b.Start(100 - i);
	}

	stopWatch.Stop();
	// Get the elapsed time as a TimeSpan value.
	TimeSpan ts = stopWatch.Elapsed;

	// Format and display the TimeSpan value.
	string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
			ts.Hours, ts.Minutes, ts.Seconds,
			ts.Milliseconds / 10);
	Console.WriteLine("RunTime " + elapsedTime);
}

private void RunThread(Action action)
{
	bool bLockTaken = false;
	
	try
	{
		while (!bLockTaken)
		{
			Monitor.Enter(objLock, ref bLockTaken);

			if (!bLockTaken)
			{
				Thread.Sleep(1);
			}
		}

		action();
	}
	finally
	{
		if (bLockTaken)
		{
			Monitor.Exit(objLock);
		}
	}
}

void Add() { iValue++; }
void Sub() { iValue--; }

public void AddThread(object objSleepTime)
{
	Thread.Sleep((int)objSleepTime);
	RunThread(Add);	
}

public void SubtractThread(object objSleepTime)
{
	Thread.Sleep((int)objSleepTime);
	for (int i = 0; i < 10_000_000; i++)
	{
		RunThread(Sub);
	}
}