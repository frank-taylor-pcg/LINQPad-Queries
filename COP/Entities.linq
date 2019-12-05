<Query Kind="Program" />

#load ".\Components.linq"

public class Entity
{
	public int ID { get; set; }
	private Dictionary<ComponentTypeId, Component> m_Components = new Dictionary<ComponentTypeId, Component>();
	public void SetComponent<T>(T component) where T : Component => m_Components[component.TypeId] = component;
	public bool RemoveComponent(ComponentTypeId typeId) => m_Components.Remove(typeId);
	public bool HasComponent(ComponentTypeId typeId) => m_Components.ContainsKey(typeId);
	public Component GetComponent(ComponentTypeId typeId) => m_Components[typeId];
	public void Clear() => m_Components.Clear();
}

public static class EntityManager
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

public static class EntityFactory
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