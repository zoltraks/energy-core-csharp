using System;

namespace Energy.Base
{
    /// <summary>
    /// Counter class
    /// </summary>
    public class Counter
    {
        #region Lock

        private readonly object _Lock = new object();

        #endregion

        #region Property

        private long _Value = 0;
        /// <summary>Current value</summary>
        public long Value { get { return GetValue(); } set { SetValue(value); } }

        private long _Minimum = 0;
        /// <summary>Minimum value</summary>
        public long Minimum { get { return GetMinimum(); } set { SetMinimum(value); } }

        private long _Maximum = long.MaxValue;
        /// <summary>Maximum value</summary>
        public long Maximum { get { return GetMaximum(); } set { SetMaximum(value); } }

        private bool _Overflow = false;
        /// <summary>Overflow flag which will be set if last operation caused overflow</summary>
        public bool Overflow { get { return GetOverflow(); } set { SetOverflow(value); } }

        private bool _Loop = false;
        /// <summary>
        /// Loop mode. When this option is set to true, 
        /// value will be reset on overflow to minimum on increment or maximum on decrement.
        /// If this option is set to false which is the default, value will stay on maximum or minimum value.
        /// </summary>
        public bool Loop { get { return GetLoop(); } set { SetLoop(value); } }

        /// <summary>Absolute value</summary>
        public ulong Absolute { get { return GetAbsolute(); } set { SetAbsolute(value); } }

        #endregion

        #region Constructor

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

        #endregion

        #region Private

        private long GetValue()
        {
            lock (_Lock)
            {
                return _Value;
            }
        }

        private void SetValue(long value)
        {
            lock (_Lock)
            {
                _Value = value;
            }
        }

        private ulong GetAbsolute()
        {
            lock (_Lock)
            {
                return (ulong)_Value;
            }
        }

        private void SetAbsolute(ulong value)
        {
            lock (_Lock)
            {
                _Value = (long)value;
            }
        }

        private long GetMinimum()
        {
            lock (_Lock)
            {
                return _Minimum;
            }
        }

        private void SetMinimum(long value)
        {
            lock (_Lock)
            {
                _Minimum = value;
            }
        }

        private long GetMaximum()
        {
            lock (_Lock)
            {
                return _Maximum;
            }
        }

        private void SetMaximum(long value)
        {
            lock (_Lock)
            {
                _Maximum = value;
            }
        }

        private bool GetOverflow()
        {
            lock (_Lock)
            {
                return _Overflow;
            }
        }

        private void SetOverflow(bool value)
        {
            lock (_Lock)
            {
                _Overflow = value;
            }
        }

        private bool GetLoop()
        {
            lock (_Lock)
            {
                return _Loop;
            }
        }

        private void SetLoop(bool value)
        {
            lock (_Lock)
            {
                _Loop = value;
            }
        }

        #endregion

        #region Reset

        /// <summary>
        /// Reset counter with specified value.
        /// </summary>
        /// <remarks>
        /// This is a copy of Reset() to avoid locking object twice.
        /// </remarks>
        public long Reset(long value)
        {
            lock (_Lock)
            {
                _Overflow = false;
                _Value = value;
                return _Value;
            }
        }

        /// <summary>
        /// Reset counter with initial (Minimum) value.
        /// </summary>
        /// <remarks>
        /// This is a copy of Reset(T value) to avoid locking object twice.
        /// </remarks>
        public long Reset()
        {
            lock (_Lock)
            {
                _Overflow = false;
                _Value = (long)_Minimum;
                return _Value;
            }
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            lock (_Lock)
            {
                return _Value.ToString();
            }
        }

        #endregion

        #region Increment

        /// <summary>
        /// Increment value.
        /// </summary>
        /// <returns></returns>
        public long Increment()
        {
            lock (_Lock)
            {
                try
                {
                    if (_Value >= _Maximum)
                    {
                        _Overflow = true;
                        if (_Loop)
                        {
                            _Value = _Minimum;
                        }
                        else
                        {
                            _Value = _Maximum;
                        }
                        return _Value;
                    }
                    if (_Overflow)
                    {
                        _Overflow = false;
                    }
                    return ++_Value;
                }
                catch (ArgumentOutOfRangeException)
                {
                    _Value = _Minimum;
                    return _Value;
                }
            }
        }

        /// <summary>
        /// Increment by specified value.
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public long Increment(long by)
        {
            lock (_Lock)
            {
                try
                {
                    if (_Maximum - by < _Value)
                    {
                        _Overflow = true;
                        if (_Loop)
                        {
                            _Value = _Value- _Maximum + by;
                        }
                        else
                        {
                            _Value = _Maximum;
                        }
                        return _Value;
                    }
                    if (_Overflow)
                    {
                        _Overflow = false;
                    }
                    _Value += by;
                    return _Value;
                }
                catch (ArgumentOutOfRangeException)
                {
                    _Value = _Minimum;
                    return _Value;
                }
            }
        }

        #endregion

        #region Decrement

        /// <summary>
        /// Decrement value.
        /// </summary>
        /// <returns></returns>
        public long Decrement()
        {
            lock (_Lock)
            {
                try
                {

                    if (_Value <= _Minimum)
                    {
                        _Overflow = true;
                        if (_Loop)
                        {
                            _Value = _Maximum;
                        }
                        else
                        {
                            _Value = _Minimum;
                        }
                        return _Value;
                    }
                    if (_Overflow)
                    {
                        _Overflow = false;
                    }
                    return --_Value;
                }
                catch (ArgumentOutOfRangeException)
                {
                    _Overflow = true;
                    _Value = _Minimum;
                    return _Value;
                }
            }
        }

        #endregion

        #region Static

        #region Increment

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

        #endregion

        #region Decrement

        public static long Decrement(ref long value)
        {
            return value = value == long.MinValue ? 0 : value - 1;
        }

        public static ulong Decrement(ref ulong value)
        {
            return value = value == ulong.MinValue ? 0 : value - 1;
        }

        public static int Decrement(ref int value)
        {
            return value = value == int.MinValue ? 0 : value - 1;
        }

        public static uint Decrement(ref uint value)
        {
            return value = value == uint.MinValue ? 0 : value - 1;
        }

        public static void Decrement(ref byte value)
        {
            value = (byte)(value == byte.MinValue ? 0 : value - 1);
        }

        public static void Decrement(ref sbyte value)
        {
            value = (sbyte)(value == sbyte.MinValue ? 0 : value - 1);
        }

        public static float Decrement(ref float value)
        {
            return value = value > float.MinValue + 1 ? 0 : value - 1.0F;
        }

        public static double Decrement(ref double value)
        {
            return value = value > double.MinValue + 1.0 ? 0 : value - 1.0;
        }

        public static decimal Decrement(ref decimal value)
        {
            return value = value > decimal.MinValue + 1.0M ? 0 : value - 1.0M;
        }

        #endregion

        #endregion
    }
}