using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    [Energy.Attribute.Code.Wrapper]
    public class Assembly
    {
        /// <summary>
        /// Auxiliary module management mechanism in case of application modularization
        /// library modular
        /// </summary>
        [Energy.Attribute.Code.Future(Expected = "2018-05")]
        public class Manager
        {
        }

        /// <summary>
        /// Get list of assemblies of current application domain
        /// </summary>
        /// <returns></returns>
        public static System.Reflection.Assembly[] GetAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Get list of assemblies of current application domain filtered...
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static System.Reflection.Assembly[] GetAssemblies(params string[] filters)
        {
            List<System.Reflection.Assembly> list = new List<System.Reflection.Assembly>();
            bool ignoreCase = true;
            foreach (System.Reflection.Assembly assembly in GetAssemblies())
            {
                string needle = assembly.FullName;
                bool check = Energy.Base.Text.Check(needle
                    , Enumeration.MatchStyle.Any, Energy.Enumeration.MatchMode.Simple, ignoreCase
                    , filters);
                if (!check)
                    continue;
                list.Add(assembly);
            }
            return list.ToArray();
        }
    }
}
