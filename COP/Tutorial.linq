<Query Kind="Program" />

// http://ad-publications.informatik.uni-freiburg.de/student-projects/entity-component/documentation.html
// Not sure if this is viable as a dev strategy
// Made minor changes to get it to work in C#, mostly adding the 'new' keyword and .NET collections to store data
// Added a simple factory to create new entities
MovementSystem moveSys = new MovementSystem();
RenderSystem renderSys = new RenderSystem();

void Main()
{
	Entity stone = EntityFactory.CreateRacer("Stone");
	Entity hedgehog = EntityFactory.CreateRacer("Hedgehog", new VelocityComponent(1, 1));
	Entity hare = EntityFactory.CreateRacer("Hare", new VelocityComponent(1, 1));

	Console.WriteLine($"The race begins:\n");
	
	RunSystems(3, 1);
	
	Console.WriteLine($"The hedgehog stops moving");
	hedgehog.RemoveComponent(ComponentTypeId.VELOCITY);
	
	Console.WriteLine($"The hedgehog's wife appears and awaits the hare");
	
	// It's possible to get an entity by its ID
	int iWifeId;
	{
		Entity wife = EntityFactory.CreateRacer("Hedgehog's Wife");
		iWifeId = wife.ID;
	}
	
	Entity hedgehogsWife = EntityManager.GetEntity(iWifeId);
	hedgehogsWife.SetComponent(new PositionComponent(12, 12));
	
	Console.WriteLine($"While nobody was watching, someone stole the stone\n");
	EntityManager.DestroyEntity(stone);
	
	RunSystems(3, 1);

	Console.WriteLine($"The hedgehog's wife says hello");
	Console.WriteLine($"The hare gets angry and runs away\n");

	if (hare.HasComponent(ComponentTypeId.NAME))
	{
		NameComponent name = (NameComponent)hare.GetComponent(ComponentTypeId.NAME);
		name.Name = "Angry Hare";
	}
	
	// Alternate method of changing a component's values
	hare.SetComponent(new VelocityComponent(1, 3));
	
	// Remove all components from an entity
	EntityManager.ClearEntity(hedgehog);
	hedgehogsWife.Clear();
	
	RunSystems(4);
	
	Console.WriteLine($"\nThe end");
}

#region Entities
class Entity
{
	public int ID { get; set; }
	private Dictionary<ComponentTypeId, Component> m_Components = new Dictionary<ComponentTypeId, Component>();
	public void SetComponent<T>(T component) where T : Component => m_Components[component.TypeId] = component;
	public bool RemoveComponent(ComponentTypeId typeId) => m_Components.Remove(typeId);
	public bool HasComponent(ComponentTypeId typeId) => m_Components.ContainsKey(typeId);
	public Component GetComponent(ComponentTypeId typeId) => m_Components[typeId];
	public void Clear() => m_Components.Clear();
}

static class EntityManager
{
	public static Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();
	private static int m_currentEntity = 0;
	
	public static Entity CreateEntity()
	{
		Entity result = new Entity() { ID = m_currentEntity };
		Entities[m_currentEntity] = result;
		m_currentEntity++;
		return result;
	}

	public static void DestroyEntity(int iEntityId) => DestroyEntity(Entities[iEntityId]);
	public static void DestroyEntity(Entity e)
	{
		Entities.Remove(e.ID);
		e.Clear();
		e = null;
	}
	
	public static void ClearEntity(int iEntityId) => ClearEntity(Entities[iEntityId]);
	public static void ClearEntity(Entity e) => e.Clear();
	
	public static Entity GetEntity(int iEntityId) => Entities[iEntityId];
}

static class EntityFactory
{
	public static Entity CreateRacer(string name, VelocityComponent v = null)
	{
		Entity result = EntityManager.CreateEntity();
		result.SetComponent(new NameComponent(name));
		result.SetComponent(new PositionComponent(0, 0));
		if (v != null)
		{
			result.SetComponent(v);
		}
		return result;
	}
}
#endregion Entities

#region Components
enum ComponentTypeId
{
	POSITION,
	VELOCITY,
	NAME,
}

class Component
{
	public ComponentTypeId TypeId { get; set; }
}

class PositionComponent : Component
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

class VelocityComponent : Component
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

class NameComponent : Component
{
	public string Name { get; set; }
	public NameComponent(string name)
	{
		TypeId = ComponentTypeId.NAME;
		Name = name;
	}
}
#endregion Components

#region Systems
void RunSystems(int iterations, int timeDelta = 0)
{
	int iDelta = timeDelta;

	for (int i = 0; i < iterations; i++)
	{
		if (timeDelta == 0) iDelta = i;
		moveSys.SetTimeDelta(iDelta);
		moveSys.Run();
		renderSys.Run();
		if (timeDelta != 0) Console.WriteLine();
	}
}

abstract class System
{
	private List<ComponentTypeId> m_Dependencies = new List<ComponentTypeId>();

	public void Run()
	{
		foreach (KeyValuePair<int, Entity> entry in EntityManager.Entities)
		{
			Entity entity = entry.Value;
			if (m_Dependencies.All(x => entity.HasComponent(x)))
			{
				Update(entity);
			}
		}
	}

	protected virtual void SetDependency(ComponentTypeId typeId) => m_Dependencies.Add(typeId);
	protected abstract void Update(Entity e);
}

class MovementSystem : System
{
	private int dt = 0;
	public MovementSystem()
	{
		SetDependency(ComponentTypeId.POSITION);
		SetDependency(ComponentTypeId.VELOCITY);
	}

	public void SetTimeDelta(int time) => dt = time;

	protected override void Update(Entity e)
	{
		VelocityComponent vel = (VelocityComponent)e.GetComponent(ComponentTypeId.VELOCITY);
		PositionComponent pos = (PositionComponent)e.GetComponent(ComponentTypeId.POSITION);
		pos.X += vel.X * dt;
		pos.Y += vel.Y * dt;
	}
}

class RenderSystem : System
{
	public RenderSystem()
	{
		SetDependency(ComponentTypeId.POSITION);
		SetDependency(ComponentTypeId.NAME);
	}

	protected override void Update(Entity e)
	{
		PositionComponent pos = (PositionComponent)e.GetComponent(ComponentTypeId.POSITION);
		NameComponent name = (NameComponent)e.GetComponent(ComponentTypeId.NAME);
		Console.WriteLine($"{name.Name} is at ({pos.X}, {pos.Y})");
	}
}
#endregion Systems

