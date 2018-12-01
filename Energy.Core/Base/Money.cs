using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Money class
    /// </summary>
    public struct Money
    {
        /// <summary>
        /// Amount
        /// </summary>
        public decimal Amount { get; private set; }

        /// <summary>
        /// Currency
        /// </summary>
        public Monetary Currency { get; private set; }

        public Money(decimal amount, Monetary currency)
            : this()
        {
            if (currency == null)
            {
                //throw new ArgumentNullException(nameof(currency));
                throw new ArgumentNullException();
            }

            Amount = amount;
            Currency = currency;
        }

        public Money(double amount, Monetary currency)
            : this((decimal)amount, currency)
        { }

        public Money Add(Money arg)
        {
            EnsureSameCurrency(this, arg);
            return new Money(Amount + arg.Amount, Currency);
        }

        public static Money operator +(Money m1, Money m2)
        {
            return m1.Add(m2);
        }

        public Money Subtract(Money arg)
        {
            EnsureSameCurrency(this, arg);
            return new Money(Amount - arg.Amount, Currency);
        }

        public static Money operator -(Money m1, Money m2)
        {
            return m1.Subtract(m2);
        }

        private void EnsureSameCurrency(Money arg1, Money arg2)
        {
            if (arg1.Currency != arg2.Currency)
            {
                throw new InvalidOperationException("Can't add amounts with different currencies.");
            }
        }

        public override string ToString()
        {
            //return $"{Amount} {Currency}";
            return String.Concat(Amount, " ", Currency);
        }

        //public static implicit operator Money(string text)
        //{
        //    return new Money();
        //}
    }
}
