<Query Kind="Program" />

public interface IRegister { }
public class Register<T> : IRegister
{
	public string Name { get; }
	
	public T Value { get; private set; }
	
	public void Set(T value) => Value = value;

	public Register(string name = "Undefined")
	{
		Name = name;
	}
}