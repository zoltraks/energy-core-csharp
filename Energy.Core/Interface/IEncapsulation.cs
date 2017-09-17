using System;
using System.Collections.Generic;
using System.Text;

namespace Energy.Interface
{
    /// <summary>
    /// Encapsulation for byte arrays interface.
    /// </summary>
    public interface IEncapsulation
    {
        /// <summary>
        /// Encapsulate data by packing it.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Pack(byte[] data);

        /// <summary>
        /// Decapsulate data by unpacking it.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Unpack(byte[] data);
    }
}
