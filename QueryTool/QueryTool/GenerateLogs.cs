using System.Globalization;
using ConsoleApp;
using CsvHelper;
using Sharprompt;

namespace QueryTool
{
    internal class GenerateLogs
    {
        public void Generate()
        {
            try
            {
                var path = Prompt.Input<string>(@"Enter folder path to generate logs (The path needs to have \ at the end)");
                var startTime = DateTime.Parse("2014-10-31 00:00:00");

                // Split the logs per hour
                for (int i = 0; i < 24; i++)
                {
                    using (var writer = new StreamWriter(File.Open(path + startTime.ToString(" yyyy-MM-dd HH-mm") + ".csv", FileMode.OpenOrCreate)))
                    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                    {
                        csv.WriteRecords(GenerateCSVData(startTime));
                    }

                    startTime = startTime.AddHours(1);
                }

                Console.WriteLine("------ Logs generated successfully ------ ");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

         List<CPUUsage> GenerateCSVData(DateTime startTime)
        {
            var records = new List<CPUUsage>();

            var minsOfDay =
                Enumerable.Range(0, 60).Select(i => startTime.AddMinutes(i).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToList();

            for (int i = 1; i <= 1000; i++)
            {
                var ipAddress = GetRandomIpAddress();
                for (int j = 0; j <= 1; j++)
                {
                    foreach (var timestamp in minsOfDay)
                    {
                        var record = new CPUUsage();
                        record.TimeStamp = timestamp.ToString();
                        record.IP = ipAddress;
                        record.CPU_ID = j.ToString();
                        record.Usage = GetCPUUsage().ToString();

                        records.Add(record);
                    }
                }
            }

            return records;
        }

        string GetRandomIpAddress()
        {
            var random = new Random();
            return $"{random.Next(1, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}.{random.Next(0, 255)}";
        }

        int GetCPUUsage()
        {
            Random random = new Random();
            return random.Next(0, 101);
        }
    }
}
