<Query Kind="Program" />

void Main()
{
	Employees e = new Employees();
	e.Attach(new Clerk());
	e.Attach(new Director());
	e.Attach(new President());
	
	e.Accept(new IncomeVisitor());
	e.Accept(new VacationVisitor());
}

interface IVisitor
{
	void Visit(Element element);
}
class IncomeVisitor : IVisitor
{
	public void Visit(Element element)
	{
		Employee employee = element as Employee;
		employee.Income *= 1.10;
		Console.WriteLine($"{employee.GetType().Name} {employee.Name}'s new income: {employee.Income:C}\n");
	}
}
class VacationVisitor : IVisitor
{
	public void Visit(Element element)
	{
		Employee employee = element as Employee;
		employee.VacationDays += 3;
		Console.WriteLine($"{employee.GetType().Name} {employee.Name}'s new vacation days: {employee.VacationDays}\n");
	}
}

abstract class Element
{
	public abstract void Accept(IVisitor visitor);
}
class Employee : Element
{
	public string Name { get; set; }
	public double Income { get; set; }
	public int VacationDays { get; set; }

	public Employee(string name, double income, int vacationDays)
	{
		Name = name;
		Income = income;
		VacationDays = vacationDays;
	}
	public override void Accept(IVisitor visitor)
	{
		visitor.Visit(this);
	}
}

class Employees
{
	private List<Employee> _employees = new List<Employee>();
	public void Attach(Employee employee) => _employees.Add(employee);
	public void Detach(Employee employee) => _employees.Remove(employee);
	public void Accept(IVisitor visitor) => _employees.ForEach(p => p.Accept(visitor));
}

class Clerk : Employee
{
	public Clerk() : base("Hank", 25000.0, 14) {}
}
class Director : Employee
{
	public Director() : base("Elly", 35000.0, 16) {}
}
class President : Employee
{
	public President() : base("Dick", 45000.0, 21) {}
}