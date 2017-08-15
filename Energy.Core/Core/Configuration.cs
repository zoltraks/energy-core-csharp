using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Core
{
    /// <summary>
    /// Configuration
    /// </summary>
    public class Configuration : Energy.Interface.ILoadFromFile
    {        
        public abstract class File
        {
            private string _FileName = Energy.Core.Information.GetCurrentNamespace() + ".conf";
            /// <summary>File</summary>
            [System.Xml.Serialization.XmlIgnore]
            public string FileName { get { return _FileName; } set { _FileName = value; } }

            private DateTime _FileStamp;
            /// <summary>Stamp</summary>
            [System.Xml.Serialization.XmlIgnore]
            public DateTime FileStamp { get { return _FileStamp; } set { _FileStamp = value; } }

            private string _FilePath = "";
            /// <summary>File</summary>
            [System.Xml.Serialization.XmlIgnore]
            public string FilePath { get { return _FilePath; } set { _FilePath = value; } }

            private int _FileSize;
            /// <summary>Size</summary>
            [System.Xml.Serialization.XmlIgnore]
            public int FileSize { get { return _FileSize; } set { _FileSize = value; } }
        }

        public class Option
        {
            public string Name;
            public bool Switch;
            public string Value;

            public class List : List<Option>
            {
                /// <summary>
                /// Add an argument to a list or return existing one if found.
                /// </summary>
                /// <param name="name">Argument name</param>
                /// <returns>Argument object</returns>
                public Option Add(string name)
                {
                    Option item = Find(x => x.Name == name);
                    if (item == null)
                    {                        
                        item = new Core.Configuration.Option() { Name = name };
                        base.Add(item);
                    }
                    return item;
                }
            }
        }
       
        private Option.List _Options = new Option.List();
        /// <summary>Arguments</summary>
        public Option.List Options { get { return _Options; } set { _Options = value; } }

        public class Element
        {
            public string Name;
            public object Value;
            //public string this() { get; set; }
        }

        public bool Load(string file)
        {
            //Configuration configuration = new Configuration();
            return true;
        }

        public bool Load(Energy.Source.Configuration connection)
        {
            //connection.Fetch(connection.Query.Create(table));
            //Configuration configuration = new Configuration();
            return true;            
        }

        public void Parse(string[] args)
        {
            throw new NotImplementedException();
        }

        public Element this[string key]
        {
            get
            {
                return new Element();
            }
        }

        public Element Select(params string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Element item = this[array[0]];
                if (item != null)
                    return item;
            }
            return null;
        }


        #region Static

        /// <summary>
        /// Create configuration object from command line arguments
        /// using Energy.Attribute.Command attributes.
        /// </summary>
        /// <param name="args">Command line options</param>
        /// <param name="type">Configuration class type</param>
        /// <param name="style">Command line option style settings</param>
        /// <returns>Configuration object</returns>
        public static object Create(string[] args, System.Type type, Energy.Core.Shell.OptionStyle style)
        {
            System.Collections.Generic.List<string> optList = new System.Collections.Generic.List<string>();
            System.Collections.Generic.List<string> argList = new System.Collections.Generic.List<string>();

            foreach (string arg in args)
            {
                if (style.IsOption(arg))
                    optList.Add(arg);
                else
                    argList.Add(arg);
            }

            System.Collections.Generic.Dictionary<string, List<string>> optionDictionary
                = new System.Collections.Generic.Dictionary<string, List<string>>();
            System.Collections.Generic.Dictionary<string, Energy.Attribute.Command.OptionAttribute> attributeDictionary
                = new System.Collections.Generic.Dictionary<string, Energy.Attribute.Command.OptionAttribute>();

            string[] a = Energy.Base.Class.GetFieldsAndProperties(type, true);
            foreach (string f in a)
            {
                Energy.Attribute.Command.OptionAttribute[] optionAttributes = (Energy.Attribute.Command.OptionAttribute[])
                    Energy.Base.Class.GetFieldOrPropertyAttributes<Energy.Attribute.Command.OptionAttribute>(type, f, true, false);
                if (optionAttributes != null && optionAttributes.Length > 0)
                {
                    foreach (Energy.Attribute.Command.OptionAttribute optionAttribute in optionAttributes)
                    {
                        System.Collections.Generic.List<string> synonym = new System.Collections.Generic.List<string>();
                        synonym.Add(optionAttribute.Name);
                        synonym.Add(optionAttribute.Short);
                        synonym.Add(optionAttribute.Long);
                        if (optionAttribute.Alternatives != null)
                            synonym.AddRange(optionAttribute.Alternatives);
                        foreach (string name in synonym)
                        {
                            if (string.IsNullOrEmpty(name))
                                continue;
                            if (!optionDictionary.ContainsKey(name))
                                optionDictionary[name] = new List<string>();
                            optionDictionary[name].Add(f);
                        }

                    }
                }
                Energy.Attribute.Command.ParameterAttribute[] parameterAttributes = (Energy.Attribute.Command.ParameterAttribute[])
                    Energy.Base.Class.GetFieldOrPropertyAttributes(type, f, typeof(Energy.Attribute.Command.ParameterAttribute), true, false);
            }
            return null;
        }

        #endregion
    }
}
