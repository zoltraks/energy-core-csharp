using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    /// <summary>
    /// Interface for SQL dialect
    /// </summary>
    public interface IDialect
    {
        Energy.Query.Format Format { get; set; }

        string Select(Energy.Query.Select statement);
        string Insert(Energy.Query.Insert statement);
        //string Delete(Energy.Query.Delete statement);
        //string Update(Energy.Query.Update statement);

        string CreateTable(Energy.Source.Structure.Table table);
        string CreateColumn(Energy.Source.Structure.Column column);
        string CreateIndex(Energy.Source.Structure.Index index);

        string DropTable(Energy.Source.Structure.Table table);
        string DropColumn(Energy.Source.Structure.Column column);
        string DropIndex(Energy.Source.Structure.Index index);
    }
}
