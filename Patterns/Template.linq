<Query Kind="Program">
  <Namespace>System.Data.OleDb</Namespace>
</Query>

// Template pattern
// https://www.dofactory.com/net/template-method-design-pattern
// This is a pretty weak example of this
// Note it's modified from the example on the site above to work on my machine

void Main()
{
	DataAccessObject daoCategories = new Categories();
	daoCategories.Run();
	
	DataAccessObject daoProducts = new Products();
	daoProducts.Run();
}

abstract class DataAccessObject
{
	protected string connectionString;
	protected DataSet dataSet;
	public virtual void Connect() => connectionString = "Data Source=.\\SQLExpress;Initial Catalog=TestBed;Integrated Security=SSPI;";
	public abstract void Select();
	public abstract void Process();
	public virtual void Disconnect() => connectionString = string.Empty;
	// The "Template Method"
	public void Run()
	{
		Connect();
		Select();
		Process();
		Disconnect();
	}
}

class Categories : DataAccessObject
{
	public override void Select()
	{
		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			string sql = "select CategoryName from Categories";
			SqlCommand command = new SqlCommand(sql, connection);
			connection.Open();
			
			SqlDataAdapter da = new SqlDataAdapter(command);
			dataSet = new DataSet();
			da.Fill(dataSet);
			connection.Close();
			da.Dispose();
		}
	}
	public override void Process()
	{
		dataSet.Dump("Categories");
	}
}

class Products : DataAccessObject
{
	public override void Select()
	{
		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			string sql = "select ProductName from Products";
			SqlCommand command = new SqlCommand(sql, connection);
			connection.Open();
			
			SqlDataAdapter da = new SqlDataAdapter(command);
			dataSet = new DataSet();
			da.Fill(dataSet);
			connection.Close();
			da.Dispose();
		}
	}
	public override void Process()
	{
		dataSet.Dump("Products");
	}
}