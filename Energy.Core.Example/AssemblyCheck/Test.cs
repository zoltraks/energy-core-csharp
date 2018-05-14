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
            Energy.Base.Collection.StringDictionary<Assembly> dictionary = Energy.Base.Class.CreateAssemblyDictionaryByShortName(assemblies);
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
