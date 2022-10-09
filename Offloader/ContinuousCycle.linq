<Query Kind="Program" />

const int MaxTraysPerCart = 24;
const int SeedsPerOPlate = 24;
const int SeedsPerTray = 24;

void Main()
{
	RunTest(false);
}

void RunTest(bool isOrdered = true)
{
	List<int> cart1 = new() { 1, 2, 6, 8 };
	List<int> cart2 = new() { 4, 5, 3, 7 };
	Order order = Order.Create(cart1, cart2, 8, isOrdered);

	// Docking first cart with empty oplates, no seeds assigned
	order.LoadOPlates(new List<int>() { 2, 3, 7 });
	order.DockCart(0);
	order.AssignLoadedSeedsToLoadedOplates();
	order.PickSeeds();
	order.Dump("Docking first cart with empty oplates, no seeds assigned");

	// Docking second cart, some seeds assigned -- oplates not switched
	order.DockCart(1);
	order.AssignLoadedSeedsToLoadedOplates();
	order.PickSeeds();
	order.Dump("Docking second cart, some seeds assigned -- oplates not switched");

	// Leaving second cart in, switching out o-plates
	order.LoadOPlates(new List<int>() { 1, 4, 6 });
	order.AssignLoadedSeedsToLoadedOplates();
	order.PickSeeds();
	order.Dump("Leaving second cart in, switching out o-plates");

	// Docking the first cart
	order.DockCart(0);
	order.AssignLoadedSeedsToLoadedOplates();
	order.PickSeeds();
	order.Dump("Docking the first cart");

	// Loading the last o-plates
	order.LoadOPlates(new List<int>() { 5, 0 });
	order.AssignLoadedSeedsToLoadedOplates();
	order.PickSeeds();
	order.Dump("Loading the last o-plates");

	// Docking the second cart one last time
	order.DockCart(1);
	order.AssignLoadedSeedsToLoadedOplates();
	order.PickSeeds();
	order.Dump("Docking the second cart one last time");
}

public class Seed
{
	public string ID { get; set; }
	public int GroupSequence { get; set; } = -1;
	public string DestinationContainer { get; set; } = null;
	public string DestinationWell { get; set; } = null;
	public bool IsPicked { get; set; } = false;

	public static Seed Create(int trayIndex, int seedIndex)
	{
		Seed result = new()
		{
			ID = $"T{trayIndex:00}-S{seedIndex:000}",
			GroupSequence = Sequence.Next()
		};
		
		return result;
	}

	public override string ToString() => $"{ID} | {GroupSequence:000}";

	object ToDump()
	{
		if (DestinationContainer is not null)
		{
			if (IsPicked)
			{
				return Util.WithStyle(ToString(), "color:orange");
			}
			return Util.WithStyle(ToString(), "color:white");
		}
		return Util.WithStyle(ToString(), "color:lightgreen");
	}
}

public class Tray
{
	public string ID { get; set; }
	public List<Seed> Seeds { get; set; } = new();

	// Adjust this to adjust the output format when dumping objects of this type
	public static int DumpColumns { get; set; } = 8;
	
	public static Tray Create(int index, int numberOfSeeds)
	{
		Tray result = new() { ID = $"T-{index:000}" };
		
		for (int seedIndex = 0; seedIndex < numberOfSeeds; seedIndex++)
		{
			result.Seeds.Add(Seed.Create(index, seedIndex));
		}
		
		return result;
	}
	
	object ToDump()
	{
		int rows = (int)Math.Ceiling(1.0 * Seeds.Count / DumpColumns);
		object[,] seeds = new object[rows, DumpColumns];
		
		int row = 0;
		int column = 0;
		
		foreach (Seed seed in Seeds)
		{
			seeds[row, column++] = seed;
			if (column >= DumpColumns)
			{
				column = 0;
				row++;
			}
		}
		DumpContainer dc = new();
		dc.AppendContent(ID);
		dc.AppendContent(seeds);
		return dc;
	}
}

public class Cart
{
	public int ID { get; set; }
	public List<Tray> Trays { get; set; } = new();
	public bool IsDocked { get; set; } = false;
	
	// Given a collection of tray indices, generates the cart.
	// If we want TRAY-001, TRAY-002, TRAY-004 and TRAY-008 we would pass in { 1, 2, 4, 8 }
	public static Cart Create(int id, List<int> trayIndices)
	{
		Cart result = new() { ID = id };
		foreach (int index in trayIndices)
		{
			result.Trays.Add(Tray.Create(index, SeedsPerTray));
		}
		
		return result;
	}
	

	object ToDump()
	{
		if (IsDocked)
		{
			return Util.WithStyle(this, "color:orange");
		}
		return this;
	}
}

public class Order
{
	public List<OPlate> OPlates { get; set; } = new();
	public List<Cart> Carts { get; set; } = new();
	
	public static Order Create(List<int> cart1, List<int> cart2, int numberOfOPlates, bool areSeedsOrdered = true)
	{
		int numberOfTrays = cart1.Count + cart2.Count;
		Sequence.Create(numberOfTrays * SeedsPerTray, areSeedsOrdered);
		
		
		Order result = new();
		result.Carts.Add(Cart.Create(1, cart1));
		result.Carts.Add(Cart.Create(2, cart2));
		
		for (int i = 0; i < numberOfOPlates; i++)
		{
			result.OPlates.Add(OPlate.Create(i));
		}
		
		return result;
	}

	public void AssignLoadedSeedsToLoadedOplates()
	{
		List<OPlate> loadedOPlates = OPlates.Where(x => x.IsLoaded).ToList();
		
		List<Seed> seedList = GetSeeds();
		
		// Grab the currently docked cart
		Cart cart = Carts.Where(x => x.IsDocked).FirstOrDefault();
		if (cart is null)
		{
			"You have to dock a cart first!".Dump();
			return;
		}
		
		// Keep assigning seeds until we can't anymore
		while (true)
		{
			// Grab the first tray that has seeds that haven't been assigned to an o-plate
			Tray tray = cart.Trays.Where(x => x.Seeds.Any(x => x.DestinationContainer is null)).FirstOrDefault();
			if (tray is null)
			{
				"No seeds left to pick".Dump();
				return;
			}
			
			bool CanContinueMapping = true;
			while (CanContinueMapping)
			{
				// Grab the first unassigned seed on the current tray
				Seed seed = tray.Seeds.Where(x => x.DestinationContainer is null).FirstOrDefault();
				if (seed is null)
				{
					"All seeds assigned for current tray".Dump();
					break;
				}

				// Grab the first available o-plate
				OPlate oplate = loadedOPlates.Where(x => !x.HasSeedAssigned()).FirstOrDefault();
				if (oplate is null)
				{
					"No available o-plates left".Dump();
					return;
				}

				// Assign the entire group sequence range they belong in to the available o-plate
				Range range = Sequence.GetGroupRange(seed.GroupSequence);
				
				List<Seed> seedsToAssign = seedList
					.Where(x => x.GroupSequence >= range.Start.Value)
					.Where(x => x.GroupSequence <= range.End.Value)
					.OrderBy(x => x.GroupSequence)
					.ToList();
				
				foreach (Seed s in seedsToAssign)
				{
					// Determine if the seed is on the currently loaded cart (for debugging display purposes -- not necessary in the final product)
					oplate.AssignSeed(s);
				}
			}
		}
	}

	public List<Seed> GetSeeds()
	{
		List<Seed> result = new();
		foreach (Cart cart in Carts)
		{
			foreach (Tray tray in cart.Trays)
			{
				foreach (Seed seed in tray.Seeds)
				{
					result.Add(seed);
				}
			}
		}
		return result;
	}
	
	public void DockCart(int index)
	{
		Carts[index].IsDocked = true;
		Carts[1 - index].IsDocked = false;
	}
	
	// Unloads all of the o-plates and loads the specified indexes
	public void LoadOPlates(List<int> indices)
	{
		foreach (OPlate plate in OPlates)
		{
			plate.IsLoaded = false;
		}
		
		foreach (int index in indices)
		{
			OPlates[index].IsLoaded = true;
		}
	}
	
	public void PickSeeds()
	{
		// Get the currently docked cart
		Cart cart = Carts.Where(x => x.IsDocked).FirstOrDefault();
		if (cart is null) return;
		
		// Get the list of loaded oplates
		List<OPlate> loadedOPlates = OPlates.Where(x => x.IsLoaded).ToList();
		if (loadedOPlates.Count == 0) return;
		
		// Get the list of unpicked seeds for the currently loaded o-plates
		List<Seed> unpickedSeeds = new();
		foreach (OPlate plate in loadedOPlates)
		{
			unpickedSeeds.AddRange(plate.GetUnpickedSeeds());
		}
		
		foreach (Seed s in unpickedSeeds)
		{
			bool isOnCurrentCart = cart.Trays.Where(x => x.Seeds.Any(x => x.GroupSequence == s.GroupSequence)).Any();

			if (isOnCurrentCart)
			{
				s.IsPicked = true;
			}
		}
	}

	object ToDump() => Util.HorizontalRun(true, OPlates, Carts);
}

public class OPlate
{
	private int row = 0;
	private int col = 0;
	
	public string ID { get; set; }
	// For simplicity of representation and viewing, just store the IDs for the seeds
	public Seed[,] Seeds { get; set; } = new Seed[4,6];
	
	public bool IsLoaded { get; set; } = false;
	
	public bool HasSeedAssigned()
	{
		for (int row = 0; row < 4; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				if (Seeds[row, col] is not null) return true;
			}
		}
		
		return false;
	}
	
	public static OPlate Create(int index) => new() { ID = $"OP-{index:000}" };
	
	public static List<OPlate> GenerateOPlates(int count)
	{
		List<OPlate> result = new();
		
		for (int i = 0; i < count; i++)
		{
			result.Add(Create(i));
		}
		
		return result;
	}
	
	public void AssignSeed(Seed s)
	{
		Seeds[row, col] = s;
		s.DestinationContainer = ID;
		s.DestinationWell = $"{row},{col}";
						
		col++;
		if (col >= 6)
		{
			col = 0;
			row++;
		}
	}
	
	public List<Seed> GetUnpickedSeeds()
	{
		List<Seed> result = new();
		for (int row = 0; row < 4; row++)
		{
			for (int col = 0; col < 6; col++)
			{
				if (Seeds[row, col] is not null && !Seeds[row, col].IsPicked)
					result.Add(Seeds[row, col]);
			}
		}
		return result;
	}
}

public static class Sequence
{
	private static int currentIndex = 0;
	private static List<int> sequence = new();
	
	public static void Create(int count, bool isOrdered = true)
	{
		currentIndex = 0;
		
		sequence.Clear();
		for (int i = 0; i < count; i++)
		{
			sequence.Add(i);
		}
		
		if (!isOrdered)
		{
			Random rnd = new Random();
			sequence = sequence.OrderBy(x => rnd.Next()).ToList();
		}
	}
	
	public static int Next() => sequence[currentIndex++];

	public static Range GetGroupRange(int index)
	{
		int startRange = (index / 24) * 24;
		int endRange = startRange + 23;
		return new Range(startRange, endRange);
	}
}