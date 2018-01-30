using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Energy.Support
{
    public class WinApi
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetProcessName(int pid)
        {
            IntPtr processHandle = OpenProcess(0x0400 | 0x0010, false, pid);

            if (processHandle == IntPtr.Zero)
                return null;

            const int max = 4000;
            StringBuilder s = new StringBuilder(max);
            string result = null;
            if (GetModuleFileNameEx(processHandle, IntPtr.Zero, s, max) > 0)
            {
                result = System.IO.Path.GetFileName(s.ToString());
            }

            CloseHandle(processHandle);

            return result;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
