using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Base
{
    /// <summary>
    /// Hashing functions for sequences of characters or bytes
    /// </summary>
    public class Hash
    {
        /// <summary>
        /// For each characters do a 5-bit left circular shift 
        /// and XOR in character numeric value (CRC variant)
        /// </summary>
        /// <param name="value"></param>
        /// <returns>32-bit hash value for a string (uint)</returns>
        public static uint CRC(string value)
        {
            uint h = 0;
            for (int i = 0; i < value.Length; i++)
            {
                h = (h << 5) ^ ((h & 0xf8000000) >> 27) ^ value[i];
            }
            return h;
        }

        /// <summary>
        /// For each characters do a 5-bit left circular shift 
        /// and XOR in character numeric value (CRC variant)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint CRC2(string value)
        {
            uint k;
            uint h = 0;
            for (int i = 0; i < value.Length; i++)
            {
                k = value[i];
                // extract high-order 5 bits from h
                // 0xf8000000 is the hexadecimal representation 
                // for the 32-bit number with the first five 
                // bits = 1 and the other bits = 0 
                var s = h & 0xf8000000;
                // shift h left by 5 bits
                h = h << 5;
                // move the highorder 5 bits to the low-order end and XOR into h
                h = h ^ (s >> 27);
                //h = h ^ s;
                // end and XOR into h
                h = h ^ k;
            }
            return h;
        }

        /// <summary>
        /// For each characters add character numeric value and left shift by 4 bits (PJW hash)
        /// </summary>
        /// <remarks>
        /// Aho, Sethi, and Ullman pp. 434-438
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>24-bit hash value for a string (uint)</returns>
        public static uint PJW(string value)
        {
            uint h = 0, g;
            for (int i = 0; i < value.Length; i++)
            {
                h = (h << 4) + value[i];
                // check if top 4 bits of h aren't zero
                if ((g = h & 0xf0000000) != 0)
                {
                    // move them to the low end of h
                    h = h ^ (g >> 24);            
                    // zero top 4 bits of h
                    h = h ^ g;     
                }
            }
            return h;
        }
    }
}
