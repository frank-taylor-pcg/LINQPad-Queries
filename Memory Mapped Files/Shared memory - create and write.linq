<Query Kind="Statements">
  <Namespace>System.IO.MemoryMappedFiles</Namespace>
</Query>

using (var mmf = MemoryMappedFile.CreateNew("Shared Memory Demo", 1000))
using (var accessor = mmf.CreateViewAccessor())
{
	accessor.Write(500, 123);
}