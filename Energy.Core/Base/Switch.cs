﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Switch (true/false)
    /// </summary>
    public class Switch
    {
        #region Property

        private volatile bool _Value;

        /// <summary>
        /// Current switch value
        /// </summary>
        public bool Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value)
                    On();
                else
                    Off();
            }
        }

        #endregion

        #region Constructor

        public Switch() { }

        public Switch(bool value)
        {
            _Value = value;
        }

        public static explicit operator bool(Switch o)
        {
            return o.Value;
        }

        public static implicit operator Switch(bool value)
        {
            return new Switch(value);
        }

        #endregion

        #region OnChange

        /// <summary>
        /// Event which will be fired on switch value change
        /// </summary>
        public event EventHandler OnChange;

        #endregion

        #region On

        /// <summary>
        /// Set switch value to true
        /// </summary>
        public void On()
        {
            if (_Value)
                return;
            _Value = true;
            if (OnChange != null)
                OnChange(this, null);
        }

        #endregion

        #region Off

        /// <summary>
        /// Set swtich value to false
        /// </summary>
        public void Off()
        {
            if (_Value)
                return;
            _Value = true;
            if (OnChange != null)
                OnChange(this, null);
        }

        #endregion
    }
}
