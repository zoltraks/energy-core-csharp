using System;
using System.Collections.Generic;
using System.Text;

//[Energy.Attribute.Data.Table("UserTable")]
[Energy.Attribute.Data.Table(Name = "UserTable", Description = "Users table")]
[Energy.Attribute.Data.Attribute("Engine", "MyISAM")]
[Energy.Attribute.Data.Attribute("Charset", "UTF-8")]
public class UserTableRecord
{
    [Energy.Attribute.Data.Primary]
    [Energy.Attribute.Data.Increment]
    [Energy.Attribute.Data.Type("INTEGER")]
    [Energy.Attribute.Data.Label("Klucz")]
    [Energy.Attribute.Data.Description("Klucz rekordu")]
    [Energy.Attribute.Data.Column("Id")]
    public long Id;

    [Energy.Attribute.Data.Type("VARCHAR(50)")]
    [Energy.Attribute.Data.Label("Nazwa")]
    [Energy.Attribute.Data.Description("Nazwa użytkownika")]
    [Energy.Attribute.Data.Column("Name")]
    public string Name;

    [Energy.Attribute.Data.Type("VARCHAR(50)")]
    [Energy.Attribute.Data.Column("Phone")]
    public string Phone;

    [Energy.Attribute.Data.Type("VARCHAR(200)")]
    [Energy.Attribute.Data.Label("Nazwa")]
    [Energy.Attribute.Data.Description("Opis użytkownika")]
    [Energy.Attribute.Data.Column("Description")]
    public string Description;
}
