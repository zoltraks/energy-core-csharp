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
        public decimal Amount { get; }

        /// <summary>
        /// Currency
        /// </summary>
        public Currency Currency { get; }

        public Money(decimal amount, Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            Amount = amount;
            Currency = currency;
        }

        public Money(double amount, Currency currency)
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
            return $"{Amount} {Currency}";
        }
    }
}
