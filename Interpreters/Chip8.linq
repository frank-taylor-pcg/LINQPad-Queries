<Query Kind="Program">
  <Namespace>System.Timers</Namespace>
</Query>

#region CONSTANTS

// Registers
const byte REGISTER_COUNT = 0xF;
const byte FLAG_REGISTER = 0xF;

// Address ranges


// 

#endregion CONSTANTS

ushort IndexRegister;
ushort ProgramCounter;

bool bRunning = true;

int index = 0;

void Main()
{
	ChipTimer DelayTimer = new ChipTimer();
	ChipTimer Soundtimer = new ChipTimer();
	
	while (bRunning)
	{
		// Check the timer
		DelayTimer.Check();
		Soundtimer.Check();
		$"Echo {index++}".Dump();
		
		if (index == 100)
		{
			bRunning = false;
		}
	}
}

public class Register
{
	const ushort REGISTER_COUNT = 0xF;
	const ushort FLAG_REGISTER = 0xF;
	
	byte[] Data = new byte[REGISTER_COUNT];

	public Register() { Data = Enumerable.Repeat((byte)0, REGISTER_COUNT).ToArray(); }
}

public class Stack
{
	const ushort STACK_SIZE = 0xF;
	byte StackPointer = 0;
	ushort[] Data = new ushort[STACK_SIZE];

	public Stack() { Data = Enumerable.Repeat((ushort)0, STACK_SIZE).ToArray(); }
	
	public void Push(byte value)
	{
		if (StackPointer < STACK_SIZE)
		{
			Data[StackPointer++] = value;
		}
	}
	
	public ushort Pop()
	{
		ushort uResult = 0;
		if (StackPointer > 0)
		{
			uResult = Data[StackPointer--];
		}
		return uResult;
	}
}

public class Memory
{
	const ushort MEMORY_SIZE = 0xFFF;

	const ushort RESERVED_START = 0x000;
	const ushort RESERVED_END = 0x1FF;

	const ushort CHARACTER_MAP_START = 0x050;
	const ushort CHARACTER_MAP_END = 0x0A0;

	const ushort PROGRAM_SPACE_START = 0x200;
	const ushort PROGRAM_SPACE_END = 0xFFF;
	
	byte[] Data = new byte[MEMORY_SIZE];

	public Memory() { Data = Enumerable.Repeat((byte)0, MEMORY_SIZE).ToArray(); }
}

public class ChipTimer
{
	private System.Timers.Timer m_Timer;
	ManualResetEvent mreEvent = new ManualResetEvent(false);
	int m_value = 0;
	
	public ChipTimer()
	{
		m_Timer = new System.Timers.Timer(16); // 16.666 is what it should be, but context-switching results in a slightly slower response
		m_Timer.Elapsed += Pulse;
		m_Timer.AutoReset = true;
		m_Timer.Enabled = true;
	}
	
	private void Pulse(object objNull, ElapsedEventArgs e)
	{
		mreEvent.Set();
		
		if (m_value > 0)
		{
			m_value--;
		}
	}
	
	public void Check()
	{
		mreEvent.WaitOne(-1);
		mreEvent.Reset();
	}
	
	public void Prime(int iVal)
	{
		m_value = iVal;
	}
}