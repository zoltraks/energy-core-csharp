using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyCheck
{
    public class Test
    {
        public static void GetAllAssemblies()
        {
            Assembly[] assemblies = Energy.Base.Class.GetAssemblies();
            if (assemblies == null)
                return;
            WriteAssemblies(assemblies);
            Energy.Core.Tilde.WriteLine("~c~Filtering out only ~w~System.~c~ elements");
            Energy.Base.Collection.StringDictionary<Assembly> dictionary = Energy.Base.Class.CreateAssemblyDictionaryByShortName(assemblies);
            dictionary = dictionary.Filter(Energy.Enumeration.MatchMode.Simple, true, new string[] { "System." });
            WriteAssemblies(dictionary.GetValueArray());
        }

        private static void WriteAssemblies(Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                Console.WriteLine(assembly.CodeBase);
                Console.WriteLine(assembly.FullName);
                Console.WriteLine(assembly.Location);
                Module manifestModule = assembly.ManifestModule;
                if (manifestModule != null)
                {
                    Console.WriteLine(manifestModule.Name);
                }
                Console.WriteLine("---");
            }
        }
    }
}
