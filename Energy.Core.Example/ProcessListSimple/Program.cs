using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace ProcessListSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessInformation.ProcessDictionary d = new ProcessInformation.ProcessDictionary();

            while (true)
            {
                foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcesses())
                {
                    if (d.ContainsKey(process.Id) || d.ContainsProcess(process))
                    {
                        d[process.Id].UpdateProcessorUsage();
                        continue;
                    }
                    d[process.Id] = new ProcessInformation()
                    {
                        Process = process,
                    };
                }

                Energy.Base.Table table = new Energy.Base.Table();
                Energy.Base.Record record;
                //Energy.Base.Table.Row row = table.New();

                foreach (ProcessInformation e in d.Values)
                {
                    record = table.New();
                    System.Diagnostics.Process process = e.Process;
                    //try
                    //{
                    //    record["Process start"] = process.StartTime;
                    //}
                    //catch (Win32Exception)
                    //{
                    //}
                    try
                    {
                        record["Process name"] = Energy.Support.WinApi.GetProcessName(process.Id);
                    }
                    catch
                    {
                    }
                    record["PID"] = process.Id;
                    record["Name"] = process.ProcessName;
                    record["Private bytes"] = process.PrivateMemorySize64;
                    record["CPU"] = e.ProcessorUsage;
                    record["Process Title"] = process.MainWindowTitle;
                }
                DataTable dt = table.ToDataTable();
                Energy.Core.Tilde.WriteLine(Energy.Base.Plain.DataTableToPlainText(dt, new Energy.Base.Plain.TableFormat()
                {
                    Tilde = true,
                    MaximumLength = 25,
                    FormatInteger = "### ### ### ### ###",
                    MinimumLength = 10,
                }
                .SetTildeColorScheme("YELLOW")
                .SetFrameStyle("PADDED4")
                ));
                string input = Console.ReadLine();
                if (input == "." || input == "q")
                    break;
            }
        }
    }
}
