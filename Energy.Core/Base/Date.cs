using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Date
    {
        public short Year;

        public byte Month;
        
        public byte Day;

        public sbyte Zone;

        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day);
        }

        public static implicit operator Date(DateTime value)
        {
            if (value == DateTime.MinValue)
                return new Date();
            return new Date()
            {
                Year = (short)value.Year,
                Month = (byte)value.Month,
                Day = (byte)value.Day,
            };
        }

        public static explicit operator DateTime(Date value)
        {
            if (value.IsEmpty)
                return DateTime.MinValue;
            return value.ToDateTime();
        }

        public bool IsEmpty
        {
            get
            {
                return Year == 0 && Month == 0 && Day == 0;
            }
        }
    }
}
