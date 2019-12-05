<Query Kind="Program" />

// Facade pattern
// https://www.dofactory.com/net/facade-design-pattern

void Main()
{
	Mortgage mortgage = new Mortgage();
	
	Customer customer = new Customer("Frank Taylor");
	bool eligible = mortgage.IsEligible(customer, 250_000);
	Console.WriteLine($"\n{customer.Name} has been { (eligible ? "Approved" : "Rejected") }");
}

class Bank
{
	public bool HasSufficientSavings(Customer c, int amount)
	{
		Console.WriteLine($"Check bank account for {c.Name}");
		return true;
	}
}

class Credit
{
	public bool HasGoodCredit(Customer c)
	{
		Console.WriteLine($"Check credit score for {c.Name}");
		return true;
	}
}

class Loan
{
	public bool HasNoBadLoans(Customer c)
	{
		Console.WriteLine($"Check for existing bad loans for {c.Name}");
		return true;
	}
}

class Customer
{
	public string Name { get; set; }
	public Customer(string name) { Name = name; }
}

class Mortgage
{
	private Bank _bank = new Bank();
	private Loan _loan = new Loan();
	private Credit _credit = new Credit();
	
	public bool IsEligible(Customer cust, int amount)
	{
		Console.WriteLine($"{cust.Name} applies for {amount:C} loan\n");
		bool eligible = true;
		
		eligible &= _bank.HasSufficientSavings(cust, amount);
		eligible &= _loan.HasNoBadLoans(cust);
		eligible &= _credit.HasGoodCredit(cust);
		
		return eligible;
	}
}