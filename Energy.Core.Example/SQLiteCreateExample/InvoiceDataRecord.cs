using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteCreateExample
{
    public class InvoiceDataRecord: BaseRecord
    {
        public long InvoiceMasterId;

        public string Name;

        public double Quantity;

        public double Tax;

        public double Price;

        public double TotalNet;

        public double TotalGross;
    }
}
