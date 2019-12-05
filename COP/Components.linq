<Query Kind="Program" />

public enum ComponentTypeId
{
	POSITION,
	VELOCITY,
	NAME,
}

public class Component
{
	public ComponentTypeId TypeId { get; set; }
}

public class PositionComponent : Component
{
	public int X { get; set; }
	public int Y { get; set; }
	public PositionComponent(int x, int y)
	{
		TypeId = ComponentTypeId.POSITION;
		X = x;
		Y = y;
	}
}

public class VelocityComponent : Component
{
	public int X { get; set; }
	public int Y { get; set; }
	public VelocityComponent(int x, int y)
	{
		TypeId = ComponentTypeId.VELOCITY;
		X = x;
		Y = y;
	}
}

public class NameComponent : Component
{
	public string Name { get; set; }
	public NameComponent(string name)
	{
		TypeId = ComponentTypeId.NAME;
		Name = name;
	}
}
