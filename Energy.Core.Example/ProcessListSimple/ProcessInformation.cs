using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ProcessListSimple
{
    public class ProcessInformation
    {
        public class ProcessDictionary : Dictionary<long, ProcessInformation>
        {
            public bool ContainsProcess(Process process)
            {
                foreach (KeyValuePair<long, ProcessInformation> _ in this)
                    if (_.Value.Process == process)
                        return true;
                return false;
            }
        }

        public Process Process;

        public double ProcessorUsage;

        public DateTime StartTime;

        DateTime LastCheckTime;

        TimeSpan LastTotalProcessorTime;

        public void UpdateProcessorUsage()
        {
            if (Process == null)
                return;

            try
            {
                if (Process.HasExited)
                    return;
            }
            catch
            { }

            try
            {
                if (LastCheckTime == DateTime.MinValue)
                {
                    LastCheckTime = DateTime.Now;
                    //LastTotalProcessorTime = Process.TotalProcessorTime;
                }
                else
                {
                    DateTime currentTime = DateTime.Now;
                    TimeSpan currentTotalProcessorTime = Process.TotalProcessorTime;

                    double processorUsage = (currentTotalProcessorTime.TotalMilliseconds - LastTotalProcessorTime.TotalMilliseconds)
                        / currentTime.Subtract(LastCheckTime).TotalMilliseconds
                        / Environment.ProcessorCount;

                    ProcessorUsage = processorUsage;
                    LastCheckTime = currentTime;
                    LastTotalProcessorTime = currentTotalProcessorTime;
                }
            }
            catch (Win32Exception)
            { }
        }

        public void UpdateProcessorStartTime()
        {
            if (Process == null || Process.HasExited)
                return;

            try
            {
                StartTime = Process.StartTime;
            }
            catch (Win32Exception)
            { }
            catch
            { }
        }
    }
}
