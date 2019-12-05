<Query Kind="Program" />

// Chain of responsibility pattern
// https://www.dofactory.com/net/chain-of-responsibility-design-pattern

void Main()
{
	Approver larry = new Director();
	Approver sam = new VicePresident();
	Approver tammy = new President();
	
	larry.SetSuccessor(sam);
	sam.SetSuccessor(tammy);

	List<Purchase> purchases = new List<Purchase>();
	purchases.Add(new Purchase(2034, 350.00, "Assets"));
	purchases.Add(new Purchase(2035, 32590.10, "Project X"));
	purchases.Add(new Purchase(2036, 122100.00, "Project Y"));
	
	purchases.ForEach(x => larry.ProcessRequest(x));
}

abstract class Approver
{
	protected Approver successor;
	public void SetSuccessor(Approver successor) => this.successor = successor;
	public abstract void ProcessRequest(Purchase purchase);
}

class Director : Approver
{
	public override void ProcessRequest(Purchase purchase)
	{
		if (purchase.Amount < 10_000.0)
		{
			Console.WriteLine($"{ this.GetType().Name } approved request #{ purchase.Number } for [{ purchase.Purpose }]");
		}
		else if (successor != null)
		{
			successor.ProcessRequest(purchase);
		}
	}
}

class VicePresident: Approver
{
	public override void ProcessRequest(Purchase purchase)
	{
		if (purchase.Amount < 25_000.0)
		{
			Console.WriteLine($"{ this.GetType().Name } approved request #{ purchase.Number } for [{ purchase.Purpose }]");
		}
		else if (successor != null)
		{
			successor.ProcessRequest(purchase);
		}
	}
}

class President: Approver
{
	public override void ProcessRequest(Purchase purchase)
	{
		if (purchase.Amount < 100_000.0)
		{
			Console.WriteLine($"{ this.GetType().Name } approved request #{ purchase.Number } for [{ purchase.Purpose }]");
		}
		else
		{
			Console.WriteLine($"Request #{purchase.Number} for [{purchase.Purpose}] requires an executive meeting!");
		}
	}
}

class Purchase
{
	public int Number { get; set; }
	public double Amount { get; set; }
	public string Purpose { get; set; }
	
	public Purchase(int number, double amount, string purpose)
	{
		Number = number;
		Amount = amount;
		Purpose = purpose;
	}
}