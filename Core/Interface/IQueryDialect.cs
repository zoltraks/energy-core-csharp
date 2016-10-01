using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface IQueryDialect
    {
        string CreateTable(Energy.Base.Structure.Table table);
        string CreateColumn(Energy.Base.Structure.Column column);
        string CreateIndex(Energy.Base.Structure.Index index);
        string DeleteTable(Energy.Base.Structure.Table table);
        string DeleteColumn(Energy.Base.Structure.Column column);
        string DeleteIndex(Energy.Base.Structure.Index index);

        string Select(Energy.Query.Select select);
    }
}
