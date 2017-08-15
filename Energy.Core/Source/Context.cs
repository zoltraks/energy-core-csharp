using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Source
{
    public class Context
    {
        public Energy.Source.Connection Connection { get; set; }

        public Context() { }

        public Context(Energy.Source.Connection connection)
        {
            this.Connection = connection;
        }

        public Context Select(System.Type type)
        {
            return this;
        }

        public Context From(string table)
        {
            return this;
        }

        public Context Where(string condition)
        {
            return this;
        }

        public Context Join(string table)
        {
            return this;
        }

        public Context Group(params string[] columns)
        {
            return this;
        }

        public Context Having(string condition)
        {
            return this;
        }
    }
}
