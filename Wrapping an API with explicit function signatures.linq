<Query Kind="Program" />

void Main()
{
	DoTest<bool>();
	DoTest<int>();
	DoTest<uint>();
	DoTest<short>();
	DoTest<ushort>();
	DoTest<long>();
	DoTest<ulong>();
	DoTest<float>();
	DoTest<double>();
	DoTest<string>();
	DoTest<ThirdPartyAPI.SapFeature>();
	DoTest<ThirdPartyAPI.SapLut>();
	DoTest<ThirdPartyAPI.SapXfer>();
	DoTest<ThirdPartyAPI.Unsupported>();
	DoTest<ThirdPartyAPI.ExceptionThrower>();
}

public void DoTest<T>()
{
	OperationResult result = APIWrapper.GetValue(out T value);
	Util.HorizontalRun(true, value, result).Dump($"Testing with type {typeof(T)}");
}

public class OperationResult
{
	public bool Success { get; set; } = false;
	public string ErrorMessage { get; set; } = "Unspecified";
	public Exception FullException { get; set; } = null;
}

public static class APIWrapper
{
	public static readonly List<Type> SupportedTypes;
	
	static APIWrapper()
	{
		SupportedTypes = new()
		{
			typeof(bool),
			typeof(int),
			typeof(uint),
			typeof(short),
			typeof(ushort),
			typeof(long),
			typeof(ulong),
			typeof(float),
			typeof(double),
			typeof(string),
			typeof(ThirdPartyAPI.SapFeature),
			typeof(ThirdPartyAPI.SapLut),
			typeof(ThirdPartyAPI.SapXfer),
			typeof(ThirdPartyAPI.ExceptionThrower),
		};
	}

	/// <summary> Gets the value from the 3rd party API as the implicitly identified type. </summary>
	/// <param name="value">An out parameter of type <see cref="T">.  Note this will automatically be cast to the correct type.</param>
	/// <returns>An OperationResult object containing the success or failure of the call and any relevant messages and exceptions that occurred.</returns>
	public static OperationResult GetValue<T>(out T value)
	{
		OperationResult result = GetValue(out object objValue, typeof(T));

		value = default;

		if (result.Success)
		{
			value = (T)objValue;
		}

		return result;
	}

	/// <summary> Gets the value from the 3rd party API as an object when we explicitly set the type to retrieve. </summary>
	/// <param name="value">An out parameter of type <see cref="object">.  Note this will need to be cast to the correct type.</param>
	/// <param name="type">The type of data the object represents</param>
	/// <returns>An OperationResult object containing the success or failure of the call and any relevant messages and exceptions that occurred.</returns>
	public static OperationResult GetValue(out object value, Type type)
	{
		OperationResult result = new();
		value = default;

		if (!SupportedTypes.Contains(type))
		{
			result.ErrorMessage = $"[{type}] is not a type supported by {nameof(APIWrapper)}";
			return result;
		}
		
		try
		{
			if (type == typeof(bool)) { value = ThirdPartyAPI.GetBool(); }
			if (type == typeof(int)) { value = ThirdPartyAPI.GetInt(); }
			if (type == typeof(uint)) { value = ThirdPartyAPI.GetUInt(); }
			if (type == typeof(short)) { value = ThirdPartyAPI.GetShort(); }
			if (type == typeof(ushort)) { value = ThirdPartyAPI.GetUShort(); }
			if (type == typeof(long)) { value = ThirdPartyAPI.GetLong(); }
			if (type == typeof(ulong)) { value = ThirdPartyAPI.GetULong(); }
			if (type == typeof(float)) { value = ThirdPartyAPI.GetFloat(); }
			if (type == typeof(double)) { value = ThirdPartyAPI.GetDouble(); }
			if (type == typeof(string)) { value = ThirdPartyAPI.GetString(); }
			if (type == typeof(ThirdPartyAPI.SapFeature)) { value = ThirdPartyAPI.GetSapFeature(); }
			if (type == typeof(ThirdPartyAPI.SapLut)) { value = ThirdPartyAPI.GetSapLut(); }
			if (type == typeof(ThirdPartyAPI.SapXfer)) { value = ThirdPartyAPI.GetSapXfer(); }
			if (type == typeof(ThirdPartyAPI.ExceptionThrower)) { value = ThirdPartyAPI.GetExceptionThrower(); }
			
			result.Success = true;
		}
		catch (Exception ex)
		{
			result.ErrorMessage = ex.Message;
			result.FullException = ex;
		}

		return result;
	}
}

//--------------------------------------------------------------------------------------------------------------
#region Interface provided by 3rd party API

public static class ThirdPartyAPI
{
	public static bool GetBool() => true;

	public static int GetInt() => -1;
	public static uint GetUInt() => 1;

	public static short GetShort() => -2;
	public static ushort GetUShort() => 2;

	public static long GetLong() => -3;
	public static ulong GetULong() => 3;

	public static float GetFloat() => -4.0f;
	public static double GetDouble() => 4.0;

	public static string GetString() => "API Value";

	public static SapFeature GetSapFeature() => new SapFeature();
	public static SapLut GetSapLut() => new SapLut();
	public static SapXfer GetSapXfer() => new SapXfer();
	public static ExceptionThrower GetExceptionThrower() => new ExceptionThrower();

	public class SapFeature { /* ... */ }
	public class SapLut { /* ... */ }
	public class SapXfer { /* ... */ }
	public class Unsupported { /* ... */ }

	public class ExceptionThrower
	{
		public ExceptionThrower()
		{
			throw new NotImplementedException("Show that we can properly report errors thrown by the 3rd party API");
		}
	}
}

#endregion