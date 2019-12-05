<Query Kind="Program" />

// Abstract Factory Pattern
// https://www.dofactory.com/net/abstract-factory-design-pattern
// There is a lot of discussion about this online, nobody can seem to agree what it is, let alone why it's useful...
// I'll keep coming back to it until I understand it

void Main()
{
	List<Ecosystem> world = new List<Ecosystem>();
	world.Add(new Ecosystem(new AfricaFactory()));
	world.Add(new Ecosystem(new AmericaFactory()));
	
	foreach (Ecosystem local in world)
	{
		local.RunFoodChain();
	}
}

public abstract class ContinentFactory
{
	public abstract Carnivore CreateCarnivore();
	public abstract Herbivore CreateHerbivore();
}

public class AfricaFactory : ContinentFactory
{
	public override Carnivore CreateCarnivore()
	{
		return new Lion();
	}

	public override Herbivore CreateHerbivore()
	{
		return new Wildebeest();
	}
}

public class AmericaFactory : ContinentFactory
{
	public override Carnivore CreateCarnivore()
	{
		return new Wolf();
	}

	public override Herbivore CreateHerbivore()
	{
		return new Bison();
	}
}

public abstract class Herbivore { }
public abstract class Carnivore
{
	public abstract void Eat(Herbivore h);
}

public class Wildebeest : Herbivore { }
public class Bison : Herbivore { }
public class Lion : Carnivore
{
	public override void Eat(Herbivore h)
	{
		Console.WriteLine(this.GetType().Name + " eats " + h.GetType().Name);
	}
}
public class Wolf : Carnivore
{
	public override void Eat(Herbivore h)
	{
		Console.WriteLine(this.GetType().Name + " eats " + h.GetType().Name);
	}
}

public class Ecosystem
{
	private Herbivore _herbivore;
	private Carnivore _carnivore;
	
	public Ecosystem(ContinentFactory factory)
	{
		_carnivore = factory.CreateCarnivore();
		_herbivore = factory.CreateHerbivore();
	}
	
	public void RunFoodChain()
	{
		_carnivore.Eat(_herbivore);
	}
}