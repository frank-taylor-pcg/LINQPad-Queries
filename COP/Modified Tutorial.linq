<Query Kind="Program" />

// http://ad-publications.informatik.uni-freiburg.de/student-projects/entity-component/documentation.html
// Not sure if this is viable as a dev strategy
// Made minor changes to get it to work in C#, mostly adding the 'new' keyword and .NET collections to store data
// Added a simple factory to create new entities

#load ".\Components.linq"
#load ".\Entities.linq"
#load ".\Systems.linq"

void Main()
{
	Entity stone = EntityFactory.CreateRacer("Stone");
	Entity hedgehog = EntityFactory.CreateRacer("Hedgehog", new VelocityComponent(1, 1));
	Entity hare = EntityFactory.CreateRacer("Hare", new VelocityComponent(1, 1));

	$"The race begins:\n".Dump();
	
	RunSystems(3, 1);
	
	$"The hedgehog stops moving".Dump();
	hedgehog.RemoveComponent(ComponentTypeId.VELOCITY);
	
	$"The hedgehog's wife appears and awaits the hare".Dump();
	
	// It's possible to get an entity by its ID
	int iWifeId;
	{
		Entity wife = EntityFactory.CreateRacer("Hedgehog's Wife");
		iWifeId = wife.ID;
	}
	
	Entity hedgehogsWife = EntityManager.GetEntity(iWifeId);
	hedgehogsWife.SetComponent(new PositionComponent(12, 12));
	
	$"While nobody was watching, someone stole the stone\n".Dump();
	EntityManager.DestroyEntity(stone);
	
	RunSystems(3, 1);

	$"The hedgehog's wife says hello".Dump();
	$"The hare gets angry and runs away\n".Dump();

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
	
	$"\nThe end".Dump();
}

