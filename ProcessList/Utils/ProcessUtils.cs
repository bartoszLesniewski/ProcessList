using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ProcessList.Utils
{
    public class ProcessUtils
    {
        public static string GetProcessParameterAsString(Process process, string parameterName)
        {
            string val = "Unknown";
            try
            {
                switch (parameterName)
                {
                    case "ProcessName":
                        val = process.ProcessName;
                        break;
                    case "PriorityClass":
                        val = process.PriorityClass.ToString();
                        break;
                    case "MainModule":
                        val = process.MainModule != null ? process.MainModule.FileName! : "Unknown";
                        break;
                }

                return val;
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }

        public static int? GetProcessParameterAsInt(Process process, string parameterName)
        {
            int? val = null;

            try
            {
                switch(parameterName)
                {
                    case "Id":
                        val = process.Id;
                        break;
                    case "Threads":
                        val = process.Threads.Count;
                        break;
                    default:
                        val = null;
                        break;
                }

                return val;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static long? GetProcessParameterAsLong(Process process, string parameterName)
        {
            long? val = null;
            try
            {
                switch (parameterName)
                {
                    case "WorkingSet64":
                        val = process.WorkingSet64;
                        break;
                    default:
                        val = null;
                        break;
                }

                return val;
            }
            catch
            {
                return null;
            }
        }

        public static double? GetProcessParameterAsDouble(Process process, string parameterName)
        {
            double? val = null;
            try
            {
                switch (parameterName)
                {
                    case "TotalProcessorTime":
                        val = Math.Round(process.TotalProcessorTime.TotalMinutes, 3);
                        break;
                    default:
                        val = null;
                        break;
                }

                return val;
            }
            catch
            {
                return null;
            }
        }

        public static List<string>? GetProcessModules(Process process)
        {
            List<string> modules = new List<string>();
            try
            {
                var modulesCollection = process.Modules;
                foreach (ProcessModule module in modulesCollection)
                {
                    modules.Add(module.ModuleName!);
                }
                return modules;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
