using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Core.Test.Query
{
    [TestClass]
    public class Parameter
    {
        [TestMethod]
        public void Parse()
        {
            string expect;

            string result;
            string query;

            Energy.Query.Parameter.Bag bag = new Energy.Query.Parameter.Bag();
            bag.Set("param1", "value1");
            bag.Set("param2", "value2");
            query = "INSERT INTO table1 ( column1 , column2 ) VALUES ( @param1 , @param2 )";
            expect = "INSERT INTO table1 ( column1 , column2 ) VALUES ( 'value1' , 'value2' )";
            result = bag.Parse(query);
            Assert.AreEqual(expect, result);

            bag.Clear();
            bag.Set("param1", 1.23);
            bag.Set("param2", "'AB'");
            query = "INSERT INTO table1 ( column1 , column2 ) VALUES ( @param1 , @param2 )";
            expect = "INSERT INTO table1 ( column1 , column2 ) VALUES ( '1.23' , '''AB''' )";
            result = bag.Parse(query);
            Assert.AreEqual(expect, result);

            bag.Clear();
            bag.Set("b", new byte[] { 1, 2, 3, 4 }, "binary");
            query = "@b";
            expect = "0x01020304";
            result = bag.Parse(query);
            Assert.AreEqual(expect, result);

            query = @"INSERT INTO [dic_place] (
  [location]
 ,[code]
 ,[count]
)
 VALUES 
 ( 'loc'
  , @EditCode
  ,@EditCount
)
 SELECT SCOPE_IDENTITY()";
            bag.Clear();
            bag.Set("@EditCode", "ww", Enumeration.FormatType.Text);
            bag.Set("@EditCount", "12", Enumeration.FormatType.Number);
            result = bag.Parse(query);
            Assert.IsNotNull(result);

            bag.Clear();
            bag.Set("where", "1=1", Enumeration.FormatType.Plain);
            result = bag.Parse("@WHERE");
            Assert.AreEqual("1=1", result);

            bag.Clear();
            bag.Set("where", "1=1", "PlAiN");
            result = bag.Parse("@WHERE");
            Assert.AreEqual("1=1", result);

            bag.Clear();
            bag.Set("a", "1");
            bag.Set("@a", "2");
            bag.Set("a", "3");
            Assert.AreEqual(1, bag.Count);
            result = bag.Parse("SELECT '@a' + @a");
            Assert.AreEqual("SELECT '@a' + '3'", result);
        }

        [TestMethod]
        public void Template()
        {
            string expect;
            string result;
            string template;

            template = @"
INSERT INTO [dbo].[Orders] 
    ([order number]
    ,[order_date]
    )
VALUES
    (<order number, nvarchar(35),>
    ,<order_date , date,>
    )
GO
";
            expect = @"
INSERT INTO [dbo].[Orders] 
    ([order number]
    ,[order_date]
    )
VALUES
    (@order_number
    ,@order_date
    )
GO
";
            result = Energy.Query.Parameter.Template.ConvertToParameterizedQuery(template);
            Assert.AreEqual(expect, result);

            template = "<a>";
            expect = "@a";
            result = Energy.Query.Parameter.Template.ConvertToParameterizedQuery(template);
            Assert.AreEqual(expect, result);

            template = "<a>1";
            expect = "@a1";
            result = Energy.Query.Parameter.Template.ConvertToParameterizedQuery(template);
            Assert.AreEqual(expect, result);

            template = "1<a>";
            expect = "1@a";
            result = Energy.Query.Parameter.Template.ConvertToParameterizedQuery(template);
            Assert.AreEqual(expect, result);
        }

        [TestMethod]
        public void Option()
        {
            string text;
            string have;
            string must;

            Energy.Query.Parameter.Bag bag;
            
            bag = new Energy.Query.Parameter.Bag();
            bag.Set("a", "'X'");
            bag.Set("b", true);
            bag.Set("c", 3.1415);
            bag.Set("@zero", 0);

            text = "@not_present @@xx";
            must = "@not_present @@xx";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            text = "@not_present @@xx";
            must = "'' @@xx";
            bag.UnknownAsEmpty = true;
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            text = "@a @b @c";
            must = "'''X''' '1' '3.1415'";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            bag.SetType("b", Energy.Enumeration.FormatType.Number);
            bag.SetType("c", Energy.Enumeration.FormatType.Number);

            text = "@a @b @c";
            must = "'''X''' 1 3.1415";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            bag.Unicode = true;
            must = "N'''X''' 1 3.1415";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            text = "@unknown";
            bag.Unicode = true;
            bag.UnknownAsEmpty = true;
            must = "N''";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            text = "@unknown";
            bag.Unicode = true;
            bag.UnknownAsNull = true;
            must = "NULL";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            bag.SetType("zero", Energy.Enumeration.FormatType.Number);

            text = "@zero @a";
            bag.Unicode = false;
            must = "'0' '''X'''";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);

            text = "@zero @a";
            bag.UnknownAsEmpty = false;
            bag.UnknownAsNull = false;
            must = "'0' @a";
            have = bag.Parse(text);
            Assert.AreEqual(must, have);
        }

        public class Class
        {
            public class C1
            {
                public Energy.Query.Parameter.Bag Bag { get; set; }
            }
        }

        [TestMethod]
        public void Serialize()
        {
            var o1 = new Class.C1();
            o1.Bag = new Energy.Query.Parameter.Bag();
            o1.Bag.Set("a", "b");
            o1.Bag.Set("c", 1);
            string j;
            j = System.Text.Json.JsonSerializer.Serialize(o1);
            Assert.IsNotNull(j);
            var opt = new System.Text.Json.JsonSerializerOptions()
            {
            };
            var o2 = System.Text.Json.JsonSerializer.Deserialize<Class.C1>(j, opt);
            Assert.IsNotNull(o2);
            string s;
            s = o2.Bag.Get("a").ToString();
            Assert.IsNotNull(s);
            string xml;
            xml = Energy.Base.Xml.Serialize(o1);
            Assert.IsNotNull(xml);
            xml = Energy.Base.Xml.Serialize(o2);
            Assert.IsNotNull(xml);
        }
    }
}
