using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Energy.Core.Test.Base
{
    /// <summary>
    /// Characterisation tests for Energy.Base.Money.
    /// <br/><br/>
    /// These lock the current behaviour of currency equality and the Money.Value
    /// arithmetic so later refactoring can prove behaviour preservation.
    /// </summary>
    [TestClass]
    public class Money
    {
        private static Energy.Base.Money.Currency Usd()
        {
            return new Energy.Base.Money.Currency("USD", "US Dollar", 840, 2, "$");
        }

        private static Energy.Base.Money.Currency Eur()
        {
            return new Energy.Base.Money.Currency("EUR", "Euro", 978, 2, "€");
        }

        [TestMethod]
        public void Money_Currency_Equality()
        {
            // Currencies compare by their ISO code.
            Assert.IsTrue(Usd() == new Energy.Base.Money.Currency("USD", "Dollar", 840, 2));
            Assert.IsTrue(Usd() != Eur());
            Assert.IsTrue(Usd().Equals(new Energy.Base.Money.Currency("USD", "x", 0, 0)));
            Assert.AreEqual("USD", Usd().ToString());
        }

        [TestMethod]
        public void Money_Value_AddAndSubtract()
        {
            Energy.Base.Money.Currency usd = Usd();
            Energy.Base.Money.Value a = new Energy.Base.Money.Value(10.50m, usd);
            Energy.Base.Money.Value b = new Energy.Base.Money.Value(4.25m, usd);

            Assert.AreEqual(14.75m, a.Add(b).Amount);
            Assert.AreEqual(6.25m, a.Subtract(b).Amount);
            Assert.AreEqual(14.75m, (a + b).Amount);
            Assert.AreEqual(6.25m, (a - b).Amount);
            Assert.AreEqual("USD", a.Add(b).Currency.ToString());
        }

        [TestMethod]
        public void Money_Value_DifferentCurrencyThrows()
        {
            Energy.Base.Money.Value a = new Energy.Base.Money.Value(10m, Usd());
            Energy.Base.Money.Value b = new Energy.Base.Money.Value(10m, Eur());
            Assert.ThrowsExactly<InvalidOperationException>(delegate { a.Add(b); });
        }

        [TestMethod]
        public void Money_Value_NullCurrencyThrows()
        {
            Assert.ThrowsExactly<ArgumentNullException>(delegate
            {
                Energy.Base.Money.Value value = new Energy.Base.Money.Value(1m, null);
            });
        }

        [TestMethod]
        public void Money_Value_ToString()
        {
            Energy.Base.Money.Value value = new Energy.Base.Money.Value(12.5m, Usd());
            Assert.AreEqual("12.5 USD", value.ToString());
        }
    }
}
