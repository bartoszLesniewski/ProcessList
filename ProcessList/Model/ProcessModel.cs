using ProcessList.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessList.Model
{
    public class ProcessModel
    {
        public string? Name { get; set; }
        public int? Id { get; set; }
        public string? Priority { get; set; }
        public int? ThreadsNumber { get; set; }
        public string? MainModulePath {  get; set; }
        public int? ParentID { get; set; }
        public string? CpuUsage { get; set; }
        public string? MemoryUsage { get; set; }

        public ProcessModel(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public ProcessModel(Process process)
        {
            Name = ProcessUtils.GetProcessParameterAsString(process, "ProcessName");
            Id = process.Id;
            Priority = ProcessUtils.GetProcessParameterAsString(process, "PriorityClass");
            ThreadsNumber = process.Threads.Count;
            MainModulePath = ProcessUtils.GetProcessParameterAsString(process, "MainModule");

            //Name = ProcessUtils.GetProcessParameter(process, "ProcessName")?.ToString() ?? "Unknown";
            //Id = Convert.ToInt32(ProcessUtils.GetProcessParameter(process, "Id"));
            //Priority = ProcessUtils.GetProcessParameter(process, "PriorityClass")?.ToString() ?? "Unknown";
            //ThreadsNumber = (ProcessUtils.GetProcessParameter(process, "Threads") as IList)?.Count;
            //MainModulePath = (ProcessUtils.GetProcessParameter(process, "MainModule") as ProcessModule)?.FileName;
        }
    }
}
