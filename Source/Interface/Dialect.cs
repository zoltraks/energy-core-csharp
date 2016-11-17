using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Source.Interface
{
    public interface Dialect
    {
        string Create();
        string Insert();
        string Update();
        string Upsert();
    }
}
