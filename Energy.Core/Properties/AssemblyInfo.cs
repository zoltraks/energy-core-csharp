#if NETCF
    //
#elif WindowsCE || PocketPC || WINDOWS_PHONE
    //
#define NETCF
#elif COMPACT_FRAMEWORK
//
#define NETCF
#else
    //
#endif

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Energy.Core")]
[assembly: AssemblyDescription("How much reality can you take?")]
[assembly: AssemblyConfiguration("Company")]
[assembly: AssemblyCompany("Filip Golewski <f.golewski@gmail.com>")]
[assembly: AssemblyProduct("Energy.Core")]
[assembly: AssemblyCopyright("Filip Golewski <f.golewski@gmail.com>")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("88888888-dead-beef-cafe-64198cd937df")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("20.12.16")]
#if !NETCF
[assembly: AssemblyFileVersion("20.12.11")]
#endif
