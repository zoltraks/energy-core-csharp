using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    public class Trap : IDisposable
    {
        public DateTime Start { get; private set; }

        public DateTime Stop { get; private set; }

        public TimeSpan Span { get { return GetSpan(); } }

        public double Time { get { return GetTime(); } }

        public double Limit { get; set; }

        public Trap()
        {
            Start = DateTime.Now;
        }

        public Trap(double timeLimit)
            : this()
        {
            Limit = timeLimit;
        }

        public Trap(double timeLimit, Energy.Base.Anonymous.Action action)
            : this(timeLimit)
        {
            _ActionFunction = action;
        }

        public Trap(double timeLimit, Energy.Base.Anonymous.Action<TimeSpan> action)
          : this(timeLimit)
        {
            _ActionFunctionTimeSpan = action;
        }

        private double GetTime()
        {
            return GetSpan().TotalSeconds;
        }

        private Energy.Base.Anonymous.Action _ActionFunction;

        private Energy.Base.Anonymous.Action<TimeSpan> _ActionFunctionTimeSpan;

        private TimeSpan GetSpan()
        {
            if (Stop == DateTime.MinValue)
                return TimeSpan.MinValue;
            else
                return Stop - Start;
        }

        public void Dispose()
        {
            Finish();
        }

        private void Finish()
        {
            Stop = DateTime.Now;
            if (Limit == 0 || Time < Limit)
            {
                return;
            }
            if (_ActionFunction != null)
                _ActionFunction();
            if (_ActionFunctionTimeSpan != null)
                _ActionFunctionTimeSpan(Span);
        }
    }
}
