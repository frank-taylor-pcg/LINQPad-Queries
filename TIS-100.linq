<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

void Main()
{
//	DrawCore(10, 12, "N/A", "IDLE", 0);
	DumpContainer dc = new DumpContainer();
	dc.AppendContent(new ACC());
	dc.AppendContent(new BAK());
	dc.AppendContent(new LAST());
	dc.AppendContent(new MODE());
	dc.AppendContent(new IDLE());
	dc.Dump();
}

private TextBox CreateBox<T>(T value)
{
	TextBox result = new TextBox();
	result.Width = "4em";
	result.Text = $"{value}";
	return result;
}

private void DrawCore(int acc, int bak, string last, string mode, int idle)
{
	Label accLabel = new Label("ACC");
	Label bakLabel = new Label("BAK");
	Label lastLabel = new Label("LAST");
	Label modeLabel = new Label("MODE");
	Label idleLabel = new Label("IDLE");

	TextBox accValue = CreateBox(acc);
	TextBox bakValue = CreateBox(bak);
	TextBox lastValue = CreateBox(last);
	TextBox modeValue = CreateBox(mode);
	TextBox idleValue = CreateBox(idle);

	LINQPad.Controls.
	
	Div accDiv = new Div(accLabel, accValue);
	Div bakDiv = new Div(bakLabel, bakValue);
	Div lastDiv = new Div(lastLabel, lastValue);
	Div modeDiv = new Div(modeLabel, modeValue);
	Div idleDiv = new Div(idleLabel, idleValue);
	
	Div registers = new Div(accDiv, bakDiv, lastDiv, modeDiv, idleDiv);
	
	TextArea txtCode = new TextArea("", 20);
	txtCode.AutoHeight = false;
	txtCode.Cols = 20;
	txtCode.Rows = 15;
	
	WrapPanel panel = new WrapPanel(txtCode, registers);
	
	panel.Dump();
}

public class Register<T>
{
	public T Value { get; set; }
}

public class ACC : Register<int> { }
public class BAK : Register<int> { }
public class LAST : Register<string> { }
public class MODE : Register<string> { }
public class IDLE : Register<int> { }
