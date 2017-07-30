using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlServerPlainReport
{
    //[Energy.Attribute.Data.Table("UserTable")]
    [Energy.Attribute.Data.Table(Name = "UserTable", Description = "Users table")]
    [Energy.Attribute.Data.Attribute("Engine", "MyISAM")]
    [Energy.Attribute.Data.Attribute("Charset", "UTF-8")]
    public class UserTableRecord
    {
        [Energy.Attribute.Data.Type("BIGINT")]
        [Energy.Attribute.Data.Column("Id")]
        public long Id;

        [Energy.Attribute.Data.Primary]
        [Energy.Attribute.Data.Type("VARCHAR(50)")]
        [Energy.Attribute.Data.Column("Name")]
        public string Name;

        [Energy.Attribute.Data.Type("VARCHAR(50)")]
        [Energy.Attribute.Data.Column("Phone")]
        public string Phone;
    }
}
