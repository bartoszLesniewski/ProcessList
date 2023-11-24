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
        public static object? GetProcessParameter(Process process, string parameterName)
        {
            try
            {
                return process.GetType().GetProperty(parameterName)!.GetValue(process);
            }
            catch (Exception)
            {
                return null;
            }
        }

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
    }
}
