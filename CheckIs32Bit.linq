<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.Windows.Media.Imaging</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.Windows.Media</Namespace>
</Query>

void Main()
{
	
}

bool Is32Bit(string strFilename)
{
	bool bResult = false;
	
	// What's the alternative to this since it's now obsolete?
	Assembly a = Assembly.ReflectionOnlyLoadFrom(strFilename);
	
	a.ManifestModule.GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine);
	
	peKind.Dump();
	return bResult;
}