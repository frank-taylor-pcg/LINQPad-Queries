<Query Kind="Program">
  <Namespace>System.Net.Sockets</Namespace>
</Query>


void Main()
{
	Console.WriteLine("Starting connection test");
	var _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
	System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse("127.0.0.1");
	System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(ipAddress, 9000);
	_clientSocket.Connect(remoteEndPoint);
	
	string msg = "Hello from LINQPad!";
	byte[] msgBytes = System.Text.Encoding.ASCII.GetBytes(msg);
	_clientSocket.Send(msgBytes);
	
	// Now handle the response - response will not grow to match the size of the incoming data so it must be large enough to contain everything.
	byte[] response = new byte[10];
	_clientSocket.Receive(response);
	Console.WriteLine("Response received:");
	var result = System.Text.Encoding.Default.GetString(response);
	Console.WriteLine(result);
	Console.WriteLine("Connection test complete");
}

// Define other methods and classes here

// This worked! Now why the hell didn't AutoIt work? Probably because it doesn't properly define an endpoint or something.
// This is promising though. If I can get any of the game engines to work like this then I'll be set. This is now the benchmark.
// I've proved that it works, I just need to get it working within 
