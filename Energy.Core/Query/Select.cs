using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Query
{
    public class Select
    {
        public List<string> Columns = new List<string>();

        public List<string> Tables = new List<string>();

        public List<string> Join = new List<string>();
    }
}
