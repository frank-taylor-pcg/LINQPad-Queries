<Query Kind="Program" />

// Singleton pattern (and variants)
// https://www.dofactory.com/net/singleton-design-pattern
#region Theory
public class Singleton
{
	private static Singleton _instance = new Singleton();
	private Singleton() { }
	public static Singleton GetInstance
	{
		get { return _instance; }
	}
}

public class LazySingleton
{
	private static LazySingleton _instance = null;
	private LazySingleton() { }
	public static LazySingleton GetInstance
	{
		get
		{
			if (_instance == null)
				_instance = new LazySingleton();
			return _instance;
		}
	}
}

public class ThreadSafeSingleton
{
	private static ThreadSafeSingleton _instance = null;
	private ThreadSafeSingleton() { }
	private static object objLock = new object();
	public static ThreadSafeSingleton GetInstance
	{
		get
		{
			lock (objLock)
			{
				if (_instance == null)
					_instance = new ThreadSafeSingleton();
				return _instance;
			}
		}
	}
}
#endregion Theory

void Main()
{
	LoadBalancer b1 = LoadBalancer.GetLoadBalancer();
	LoadBalancer b2 = LoadBalancer.GetLoadBalancer();
	LoadBalancer b3 = LoadBalancer.GetLoadBalancer();
	LoadBalancer b4 = LoadBalancer.GetLoadBalancer();

	if (b1 == b2 && b2 == b3 && b3 == b4)
	{
		Console.WriteLine("Same instance");
	}
	
	LoadBalancer balancer = LoadBalancer.GetLoadBalancer();
	for (int i = 0; i < 15; i++)
	{
		Console.WriteLine($"Dispatch request to {balancer.Server}");
	}
}

class LoadBalancer
{
	private static LoadBalancer _instance;
	private static object objLock = new object();

	private List<string> _servers = new List<string>();
	private Random _random = new Random();
	
	protected LoadBalancer()
	{
		_servers.Add("Server 1");
		_servers.Add("Server 2");
		_servers.Add("Server 3");
		_servers.Add("Server 4");
		_servers.Add("Server 5");
	}
	
	// Double-check locking (and this variation of it as well) are not guaranteed to be thread-safe. I need to do more research.
	// Allegedly doing static initialization instead of lazy initialization could avoid the problem (the Singleton class above).
	public static LoadBalancer GetLoadBalancer()
	{
		if (_instance != null) return _instance;
		
		lock(objLock)
		{
			if (_instance == null) _instance = new LoadBalancer();
		}
		
		return _instance;
	}
	
	public string Server
	{
		get
		{
			int r = _random.Next(_servers.Count);
			return _servers[r].ToString();
		}
	}
}