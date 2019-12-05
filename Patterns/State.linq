<Query Kind="Program" />

// State pattern
// https://www.dofactory.com/net/state-design-pattern

void Main()
{
	Account account = new Account("Frank Taylor");
	account.Deposit(500.0);
	account.Deposit(300.0);
	account.Deposit(550.0);
	account.PayInterest();
	account.Withdraw(2000.00);
	account.Withdraw(1100.00);
}

abstract class State
{
	public Account Account { get; set; }
	public double Balance { get; set; }
	protected double interest;
	protected double lowerLimit;
	protected double upperLimit;
	
	public abstract void Deposit(double amount);
	public abstract void Withdraw(double amount);
	public abstract void PayInterest();
	public abstract void StateChangeCheck(); // How can I make this both protected, and required to override?
}

class RedState : State
{
	private double _serviceFee;
	public RedState(State state)
	{
		Balance = state.Balance;
		Account = state.Account;
		Initialize();
	}
	private void Initialize()
	{
		interest = 0.0;
		lowerLimit = -100.0;
		upperLimit = 0.0;
		_serviceFee = 15.00;
	}
	public override void Deposit(double amount)
	{
		Balance += amount;
		StateChangeCheck();
	}
	public override void Withdraw(double amount)
	{
		amount = amount - _serviceFee;
		Console.WriteLine("No funds available for withdrawal!");
	}
	public override void PayInterest() { } // No interest is paid
	public override void StateChangeCheck()
	{
		if (Balance > upperLimit) Account.State = new SilverState(this);
	}
}

class SilverState : State
{
	public SilverState(State state) : this(state.Balance, state.Account) { }
	public SilverState(double balance, Account account)
	{
		Balance = balance;
		Account = account;
		Initialize();
	}
	public void Initialize()
	{
		interest = 0.0;
		lowerLimit = 0.0;
		upperLimit = 1000.0;
	}
	public override void Deposit(double amount)
	{
		Balance += amount;
		StateChangeCheck();
	}
	public override void Withdraw(double amount)
	{
		Balance -= amount;
		StateChangeCheck();
	}
	public override void PayInterest()
	{
		Balance += interest * Balance;
		StateChangeCheck();
	}
	public override void StateChangeCheck()
	{
		if (Balance < lowerLimit)
		{
			Account.State = new RedState(this);
		}
		else if (Balance > upperLimit)
		{
			Account.State = new GoldState(this);
		}
	}
}

class GoldState : State
{
	public GoldState(State state) : this(state.Balance, state.Account) { }
	public GoldState(double balance, Account account)
	{
		Balance = balance;
		Account = account;
		Initialize();
	}
	private void Initialize()
	{
		interest = 0.05;
		lowerLimit = 1000.0;
		upperLimit = 10_000_000.0;
	}
	public override void Deposit(double amount)
	{
		Balance += amount;
		StateChangeCheck();
	}
	public override void Withdraw(double amount)
	{
		Balance -= amount;
		StateChangeCheck();
	}
	public override void PayInterest()
	{
		Balance += interest * Balance;
		StateChangeCheck();
	}
	public override void StateChangeCheck()
	{
		if (Balance < 0.0)
		{
			Account.State = new RedState(this);
		}
		else if (Balance < lowerLimit)
		{
			Account.State = new SilverState(this);
		}
	}
}

class Account
{
	private string _owner;
	public State State { get; set; }
	public double Balance => State.Balance;
	public Account(string owner)
	{
		_owner = owner;
		State = new SilverState(0.0, this);
	}
	private void ActionReport(string msg, double amount)
	{
		Console.WriteLine($"{msg} {amount:C} --- ");
		if (Balance < 0)
		{
			Console.WriteLine($" Balance = -{Balance:C}");
		}
		else
		{
			Console.WriteLine($" Balance =  {Balance:C}");
		}
		Console.WriteLine($" Status = {State.GetType().Name}\n");
	}
	public void Deposit(double amount)
	{
		State.Deposit(amount);
		ActionReport("Deposited", amount);
	}
	public void Withdraw(double amount)
	{
		State.Withdraw(amount);
		ActionReport("Withdrew", amount);
	}
	public void PayInterest()
	{
		State.PayInterest();
		ActionReport("Interest Paid", Balance);
	}
}