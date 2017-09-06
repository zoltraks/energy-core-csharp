using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface IQueryDialect
    {
        string CreateTable(Energy.Source.Structure.Table table);
        string CreateColumn(Energy.Source.Structure.Column column);
        string CreateIndex(Energy.Source.Structure.Index index);
        string DeleteTable(Energy.Source.Structure.Table table);
        string DeleteColumn(Energy.Source.Structure.Column column);
        string DeleteIndex(Energy.Source.Structure.Index index);

        string Select(Energy.Query.Script script);
    }
}
