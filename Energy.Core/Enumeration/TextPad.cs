using System;

namespace Energy.Enumeration
{
    /// <summary>
    /// Style of representing boolean values
    /// </summary>
    [Flags]
    public enum TextPad
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Pad left
        /// </summary>
        Left = 1,

        /// <summary>
        /// Pad right
        /// </summary>
        Right = 2,

        /// <summary>
        /// Center
        /// </summary>
        /// <remarks>
        /// <p>
        /// Greetings for Johny Lipomal / dedal77 from Zoltraks Xenon :-)
        /// </p>
        /// </remarks>
        Center = Left | Right,

        /// <summary>
        /// Middle
        /// </summary>
        /// <remarks>
        /// <p>
        /// Greetings for Johny Lipomal / dedal77 from Zoltraks Xenon :-)
        /// </p>
        /// </remarks>
        Middle = Center,
    }
}
