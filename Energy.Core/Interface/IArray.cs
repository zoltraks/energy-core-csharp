using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    public interface IArray<T>
    {
        void Clear();

        int Count { get; set; }

        T First { get; set; }

        T Last { get; set; }
    }
}
