<Query Kind="Statements">
  <Namespace>System.IO.MemoryMappedFiles</Namespace>
</Query>

using (var mmf = MemoryMappedFile.OpenExisting("Shared Memory Demo"))
using (var accessor = mmf.CreateViewAccessor())
{
	accessor.ReadInt32(500).Dump();
}