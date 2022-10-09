<Query Kind="Program">
  <Namespace>System.Runtime.Serialization</Namespace>
</Query>

void Main()
{
	IPL ipl = new IPL();

	//ipl.Do(() => { "Just testing".Dump(); }, "No parameters");
	//ipl.Do((x) => { $"Parameter Type { x.GetType() }, Parameter Value: { x }".Dump(); }, 10, "Single parameter action");
	//ipl.Do(Add, 2, 3, "Passing action with non-object types");

	//ipl.Do((x, y) => { DivideByZero(x, y); }, 10, 0, "Testing exceptions with DivideByZero");
	
	//ipl.Do(ComplexTest, new IPL_Params(), "Complex Test with IPL_Params class");
	

	//int x = ipl.Do(Add, 2, 3, "Add");
	
	// Why can't this figure out what we're doing?
	double y = ipl.Do(Divide, 10, 1, "Divide");
}

int Add(int x, int y)
{
	return (x + y).Dump();
}

double Divide(int x, int y)
{
	return (x / y).Dump();
}

void ComplexTest(IPL_Params pm)
{
	throw new Exception("An error occurred");
}

public class IPLException : Exception
{
	public IPLException() { }
	
	public IPLException(string description) : base(description) {}
	
	public IPLException(string description, Exception innerException) : base(description, innerException) { }

	public IPLException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

/// <summary> A simple VM for my Interruptable Programming Language </summary>
public class IPL_Params // Requires every Action and Func to be declared using "params object[] args"
{
	// Function signatures supported by the Do methods
	public delegate void ParamsAction(params object[] args);
	public delegate T ParamsFunc<T>(params object[] args);

	// Performs the supplied Action within the scope of the VM
	public void Do(string description, ParamsAction act, params object[] args)
	{
		try
		{
			act(args);
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description} with argument(s): [{ string.Join(',', args.ToList()) }]", ex);
		}
	}

	// Performs the supplied Func within the scope of the VM
	public T Do<T>(string description, ParamsFunc<T> func, params object[] args)
	{
		T result = default(T);
		
		try
		{
			result = func(args);
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description} with argument(s): [{ string.Join(',', args.ToList()) }]", ex);
		}
		
		return result;
	}
}

public class IPL
{
	#region Do: Actions
	private void Execute(Action act, string description, string argValues)
	{
		try
		{
			act();
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description} with argument(s): [{ argValues }]", ex);
		}
	}
	
	public void Do(Action act, string description)
	{
		Execute(act, description, null);
	}
	
	public void Do<T>(Action<T> act, T arg, string description)
	{
		string argValues = $"{arg}";
		
		Execute(() => {
			act(arg);
		}, description, argValues);
	}

	public void Do<T1, T2>(Action<T1, T2> act, T1 arg1, T2 arg2, string description)
	{
		string argValues = $"{arg1}, {arg2}";
		Execute(() =>
		{
			act(arg1, arg2);
		}, description, argValues);
	}

	public void Do<T1, T2, T3>(Action<T1, T2, T3> act, T1 arg1, T2 arg2, T3 arg3, string description)
	{
		string argValues = $"{arg1}, {arg2}, {arg3}";
		Execute(() =>
		{
			act(arg1, arg2, arg3);
		}, description, argValues);
	}

	public void Do<T1, T2, T3, T4>(Action<T1, T2, T3, T4> act, T1 arg1, T2 arg2, T3 arg3, T4 arg4, string description)
	{
		string argValues = $"{arg1}, {arg2}, {arg3}, {arg4}";
		Execute(() =>
		{
			act(arg1, arg2, arg3, arg4);
		}, description, argValues);
	}
	#endregion Do: Actions

	#region Do: Func
	public T Do<T>(Func<T> func, string description)
	{
		T result = default(T);
		try
		{
			result = func();
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description}", ex);
		}
		return result;
	}

	public TResult Do<T, TResult>(Func<T, TResult> func, T arg, string description)
	{
		TResult result = default(TResult);
		try
		{
			result = func(arg);
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description} with argument(s): [{ arg }]", ex);
		}
		return result;
	}

	public TResult Do<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 arg1, T2 arg2, string description)
	{
		TResult result = default(TResult);
		try
		{
			result = func(arg1, arg2);
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description} with argument(s): [{ arg1 }, { arg2}]", ex);
		}
		return result;
	}

	public TResult Do<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2, T3 arg3, string description)
	{
		TResult result = default(TResult);
		try
		{
			result = func(arg1, arg2, arg3);
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description} with argument(s): [{ arg1 }, { arg2 }, { arg3 }]", ex);
		}
		return result;
	}

	public TResult Do<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, string description)
	{
		TResult result = default(TResult);
		try
		{
			result = func(arg1, arg2, arg3, arg4);
		}
		catch (Exception ex)
		{
			throw new IPLException($"IPL Error at : {description} with argument(s): [{ arg1 }, { arg2 }, { arg3 }, { arg4 }]", ex);
		}
		return result;
	}
	#endregion Do: Funcs
}