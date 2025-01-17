﻿using ProcessList.Utils;
using System.Diagnostics;

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
        public double? TotalProcessorTimeMinutes { get; set; }
        public string? CpuUsage { get; set; }
        public double? PhysicalMemoryUsage { get; set; }

        public ProcessModel(Process process)
        {
            ProcessObject = process;
            Name = ProcessUtils.GetProcessParameterAsString(process, "ProcessName");
            Id = ProcessUtils.GetProcessParameterAsInt(process, "Id");
            Priority = ProcessUtils.GetProcessParameterAsString(process, "PriorityClass");
            ThreadsNumber = ProcessUtils.GetProcessParameterAsInt(process, "Threads");
            MainModulePath = ProcessUtils.GetProcessParameterAsString(process, "MainModule");
            PhysicalMemoryUsage = ProcessUtils.GetProcessParameterAsDouble(process, "WorkingSet64");
            TotalProcessorTimeMinutes = ProcessUtils.GetProcessParameterAsDouble(process, "TotalProcessorTime");
            CpuUsage = ProcessUtils.GetProcessCpuUsage(process);
        }
    }
}
