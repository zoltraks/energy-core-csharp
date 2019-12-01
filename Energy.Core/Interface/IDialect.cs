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

        string Select(Energy.Query.Statement.Select statement);
        string Insert(Energy.Query.Statement.Insert statement);
        //string Delete(Energy.Query.Statement.Delete statement);
        //string Update(Energy.Query.Statement.Update statement);

        string CreateTable(Energy.Source.Structure.Table table);
        string CreateColumn(Energy.Source.Structure.Column column);
        string CreateIndex(Energy.Source.Structure.Index index);
        string CreateDescription(Energy.Source.Structure.Table table);

        string DropTable(Energy.Source.Structure.Table table);
        string DropTable(string tableName);
        string DropColumn(Energy.Source.Structure.Column column);
        string DropIndex(Energy.Source.Structure.Index index);
    }
}
