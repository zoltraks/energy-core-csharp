using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteCreateExample
{
    public class InvoiceMasterRecord: BaseRecord
    {
        public string Index;

        public string Description;

        public AddressRecord AddressId;

        public InvoiceDataRecord[] InvoiceData;       
    }
}
