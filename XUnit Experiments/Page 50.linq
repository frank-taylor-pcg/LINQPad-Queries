<Query Kind="Program">
  <NuGetReference>xunit</NuGetReference>
  <NuGetReference>Xunit.Runner.LinqPad</NuGetReference>
  <Namespace>Xunit</Namespace>
  <Namespace>Xunit.Runner.LinqPad</Namespace>
  <CopyLocal>true</CopyLocal>
</Query>

void Main()
{
	XunitRunner.Run(Assembly.GetExecutingAssembly());
}

public static class XUnitHelpers
{
	public static void Fail(string strMessage) => Assert.True(false, strMessage);
	public static void Pass(string strMessage) => Assert.True(true, strMessage);
}

public class Address
{
	public string Street { get; set; }
	public string City { get; set; }
	public string State { get; set; }
	public string License { get; set; }
	public string Country { get; set; }
	
	public Address(string street, string city, string state, string license, string country)
	{
		Street = street;
		City = city;
		State = state;
		License = license;
		Country = country;
	}
}

public class Customer
{
	public int ID { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public decimal Age { get; set; }
	public Address BillingAddress { get; set; }
	public Address ShippingAddress { get; set; }
	
	public Customer(int id, string first, string last, decimal age, Address billing, Address shipping)
	{
		ID = id;
		FirstName = first;
		LastName = last;
		Age = age;
		BillingAddress = billing;
		ShippingAddress = shipping;
	}
}

public class Product
{
	public int ID { get; set; }
	public string Name { get; set; }
	public decimal Price { get; set; }
	
	public Product(int id, string name, decimal price)
	{
		ID = id;
		Name = name;
		Price = price;
	}
}

public class LineItem
{
	public Invoice Invoice { get; set; }
	public Product Product { get; set; }
	public decimal Quantity { get; set; }
	public decimal Total { get; set; }
	
	public LineItem(Invoice i, Product p, decimal qty, decimal total)
	{
		Invoice = i;
		Product = p;
		Quantity = qty;
		Total = total;
	}
}

public class Invoice
{
	public Customer Customer { get; set; }
	
	public List<LineItem> LineItems { get; set; }
	
	public Invoice(Customer customer)
	{
		Customer = customer;
	}
	
	public void AddItemQuantity(Product p, int qty)
	{
		LineItems.Add(new LineItem(this, p, qty, p.Price * qty));
	}
}

public class Tests
{
	public void testAddItemQuantity_severalQuantity_v1()
	{
		Address billingAddress = null;
		Address shippingAddress = null;
		Customer customer = null;
		Product product = null;
		Invoice invoice = null;
		try
		{
			// Set up the fixture
			billingAddress = new Address("1222 1st St SW", "Calgary", "Alberta", "T2N 2V2", "Canada");
			shippingAddress = new Address("1333 1st St SW", "Calgary", "Alberta", "T2N 2V2", "Canada");
			customer = new Customer(99, "John", "Doe", 30, billingAddress, shippingAddress);
			product = new Product(88, "SomeWidget", 19.99M);
			invoice = new Invoice(customer);
			
			// Exercise SUT
			invoice.AddItemQuantity(product, 5);
			// Verify outcome
			List<LineItem> lineItems = invoice.LineItems;
			if (lineItems.Count() == 1)
			{
				LineItem expected = new LineItem(invoice, product, 5, 30, 69.96M);
				
				LineItem actItem = lineItems.First();
				Assert.Equal(expected, actItem);
			}
			else
			{
				XUnitHelpers.Fail("Invoice should have exactly 1 line item");
			}
		}
		finally
		{
			// Teardown
			invoice = null;
			product = null;
			customer = null;
			billingAddress = null;
			shippingAddress = null;
		}
	}
}