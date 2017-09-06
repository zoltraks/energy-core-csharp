﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Anonymous
    {
        public delegate TOut Function<TIn, TOut>(TIn input);

        public delegate void Function<TIn>(TIn input);
    }
}