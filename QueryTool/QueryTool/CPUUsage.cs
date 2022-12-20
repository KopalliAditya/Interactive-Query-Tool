using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace ConsoleApp
{
    internal class CPUUsage
    {
        public string? TimeStamp { get; set; }

        public string? IP { get; set; }

        public string? CPU_ID { get; set; }

        public string? Usage { get; set; }
    }
}
