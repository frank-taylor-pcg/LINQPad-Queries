<Query Kind="Program" />

#load ".\Components.linq"
#load ".\Entities.linq"

public MovementSystem moveSys = new MovementSystem();
public RenderSystem renderSys = new RenderSystem();

public abstract class COPSystem
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

public void RunSystems(int iterations, int timeDelta = 0)
{
	int iDelta = timeDelta;

	for (int i = 0; i < iterations; i++)
	{
		if (timeDelta == 0) iDelta = i;
		moveSys.SetTimeDelta(iDelta);
		moveSys.Run();
		renderSys.Run();
		if (timeDelta != 0) "".Dump();
	}
}

public class MovementSystem : COPSystem
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

public class RenderSystem : COPSystem
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
		$"{name.Name} is at ({pos.X}, {pos.Y})".Dump();
	}
}
