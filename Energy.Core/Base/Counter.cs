using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Counter class
    /// </summary>
    public class Counter
    {
        private readonly object _Lock = new object();

        private long _Value;
        /// <summary>Value</summary>
        public long Value { get { return GetValue(); } set { SetValue(value); } }

        private long _Minimum;
        /// <summary>Minimum</summary>
        public long Minimum { get { return GetMinimum(); } set { SetMinimum(value); } }

        private long _Maximum;
        /// <summary>Maximum</summary>
        public long Maximum { get { return GetMaximum(); }  set { SetMaximum(value); } }

        private bool _Overflow;
        /// <summary>Overflow</summary>
        public bool Overflow { get { return GetOverflow(); } set { SetOverflow(value); } }

        /// <summary>Value</summary>
        public ulong Absolute { get { return GetAbsolute(); } set { SetAbsolute(value); } }

        public Counter()
        {
            _Minimum = 0;
        }

        public Counter(long minimum)
        {
            _Minimum = minimum;
            _Value = minimum;
        }

        public Counter(long minimum, long maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _Value = minimum;
        }

        public Counter(long minimum, long maximum, long value)
        {
            _Minimum = minimum;
            _Maximum = maximum;
            _Value = value;
        }

        public long GetValue()
        {
            lock (_Lock)
            {
                return _Value;
            }
        }

        public void SetValue(long value)
        {
            lock (_Lock)
            {
                _Value = value;
            }
        }

        public ulong GetAbsolute()
        {
            lock (_Lock)
            {
                return (ulong)_Value;
            }
        }

        public void SetAbsolute(ulong value)
        {
            lock (_Lock)
            {
                _Value = (long)value;
            }
        }

        public long GetMinimum()
        {
            lock (_Lock)
            {
                return _Minimum;
            }
        }

        public void SetMinimum(long value)
        {
            lock (_Lock)
            {
                _Minimum = value;
            }
        }

        public long GetMaximum()
        {
            lock (_Lock)
            {
                return _Maximum;
            }
        }

        public void SetMaximum(long value)
        {
            lock (_Lock)
            {
                _Maximum = value;
            }
        }

        public bool GetOverflow()
        {
            lock (_Lock)
            {
                return _Overflow;
            }
        }

        public void SetOverflow(bool value)
        {
            lock (_Lock)
            {
                _Overflow = value;
            }
        }

        /// <summary>
        /// Reset counter with specified value.
        /// </summary>
        /// <remarks>
        /// This is a copy of Reset() to avoid locking object twice.
        /// </remarks>
        public void Reset(long value)
        {
            lock (_Lock)
            {
                _Overflow = false;
                _Value = value;
            }
        }

        /// <summary>
        /// Reset counter with initial (Minimum) value.
        /// </summary>
        /// <remarks>
        /// This is a copy of Reset(T value) to avoid locking object twice.
        /// </remarks>
        public void Reset()
        {
            lock (_Lock)
            {
                _Overflow = false;
                if (_Minimum == default(long))
                    _Value = 0;
                else
                    _Value = (long)_Minimum;
            }
        }

        public override string ToString()
        {
            lock (_Lock)
            {
                return _Value.ToString();
            }
        }

        public void Decrement()
        {
            lock (_Lock)
            {
                try
                {
                    if (_Minimum == default(long))
                    {
                        if (_Value == 0)
                        {
                            _Overflow = true;
                            return;
                        }
                        if (_Value < 0)
                        {
                            _Overflow = true;
                            _Value = 0;
                            return;
                        }
                    }
                    else
                    {
                        if (_Value == _Minimum)
                        {
                            _Overflow = true;
                            return;
                        }
                        if (_Value < _Minimum)
                        {
                            _Overflow = true;
                            _Value = _Minimum;
                            return;
                        }
                    }
                    _Value--;
                }
                catch (ArgumentOutOfRangeException)
                {
                    _Overflow = true;
                    _Value = _Minimum;
                }
            }
        }

        public void Increment()
        {
            lock (_Lock)
            {
                try
                {
                    if (_Maximum == 0)
                    {
                        if (_Value == long.MaxValue)
                        {
                            _Overflow = true;
                            _Value = _Minimum;
                            return;
                        }
                    }
                    else
                    {
                        if (_Value == _Maximum)
                        {
                            _Overflow = true;
                            _Value = _Minimum;
                            return;
                        }
                    }
                    _Value++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    _Value = _Minimum;
                }
            }
        }

        public static long Increment(ref long value)
        {
            return value = value == long.MaxValue ? 0 : value + 1;
        }

        public static ulong Increment(ref ulong value)
        {
            return value = value == ulong.MaxValue ? 0 : value + 1;
        }

        public static int Increment(ref int value)
        {
            return value = value == int.MaxValue ? 0 : value + 1;
        }

        public static uint Increment(ref uint value)
        {
            return value = value == uint.MaxValue ? 0 : value + 1;
        }

        public static void Increment(ref byte value)
        {
            value = (byte)(value == byte.MaxValue ? 0 : value + 1);
        }
        public static void Increment(ref sbyte value)
        {
            value = (sbyte)(value == sbyte.MaxValue ? 0 : value + 1);
        }

        public static float Increment(ref float value)
        {
            return value = value > float.MaxValue - 1 ? 0 : value + 1.0F;
        }

        public static double Increment(ref double value)
        {
            return value = value > double.MaxValue - 1.0 ? 0 : value + 1.0;
        }

        public static decimal Increment(ref decimal value)
        {
            return value = value > decimal.MaxValue - 1.0M ? 0 : value + 1.0M;
        }
    }
}