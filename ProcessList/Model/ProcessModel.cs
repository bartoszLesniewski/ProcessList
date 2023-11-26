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
        public Process ProcessObject { get; set; }
        public string? Name { get; set; }
        public int? Id { get; set; }
        public string? Priority { get; set; }
        public int? ThreadsNumber { get; set; }
        public string? MainModulePath {  get; set; }
        public long? PhysicalMemoryUsage { get; set; }
        public double? TotalProcessorTimeMinutes { get; set; }
        public string? CpuUsage { get; set; }
        public string? MemoryUsage { get; set; }

        public ProcessModel(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public ProcessModel(Process process)
        {
            ProcessObject = process;
            Name = ProcessUtils.GetProcessParameterAsString(process, "ProcessName");
            Id = ProcessUtils.GetProcessParameterAsInt(process, "Id");
            Priority = ProcessUtils.GetProcessParameterAsString(process, "PriorityClass");
            ThreadsNumber = ProcessUtils.GetProcessParameterAsInt(process, "Threads");
            MainModulePath = ProcessUtils.GetProcessParameterAsString(process, "MainModule");
            PhysicalMemoryUsage = ProcessUtils.GetProcessParameterAsLong(process, "WorkingSet64");
            TotalProcessorTimeMinutes = ProcessUtils.GetProcessParameterAsDouble(process, "TotalProcessorTime");
        }
    }
}
