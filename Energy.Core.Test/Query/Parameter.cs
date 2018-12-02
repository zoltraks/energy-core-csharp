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
            result = Energy.Query.Parameter.Template.ConvertToParametrizedQuery(template);
            Assert.AreEqual(expect, result);

            template = "<a>";
            expect = "@a";
            result = Energy.Query.Parameter.Template.ConvertToParametrizedQuery(template);
            Assert.AreEqual(expect, result);

            template = "<a>1";
            expect = "@a1";
            result = Energy.Query.Parameter.Template.ConvertToParametrizedQuery(template);
            Assert.AreEqual(expect, result);

            template = "1<a>";
            expect = "1@a";
            result = Energy.Query.Parameter.Template.ConvertToParametrizedQuery(template);
            Assert.AreEqual(expect, result);
        }
    }
}
