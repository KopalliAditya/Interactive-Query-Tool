using System.Globalization;
using ConsoleApp;
using CsvHelper;
using Sharprompt;

namespace QueryTool
{
    internal class QueryLogs
    {
        public void QueryLog()
        {
            try
            {
                Prompt.Symbols.Prompt = new Symbol("Query", ">");
                // Prompts for the user
                var path = Prompt.Input<string>(@"Enter folder path to query logs (The path needs to have \ at the end)");
                var name = Prompt.Input<string>("Enter IP Address (xxx.xxx.xxx.xxx)");
                var cpu_id = Prompt.Input<string>("Enter CPU ID (0 or 1)");
                var startTime = Prompt.Input<DateTime>("Enter start time (MM/DD/YYYY HH:MM)");
                var endTime = Prompt.Input<DateTime>("Enter end time (MM/DD/YYYY HH:MM)");

                var inMemoryData = new Dictionary<string, IEnumerable<CPUUsage>>();

                var fileName = DateTime.Parse("2014-10-31 00:00:00");

                // Loading data in memory
                for (int i = 0; i < 24; i++)
                {
                    IEnumerable<CPUUsage> records;
                    using (var reader = new StreamReader(File.Open(path + fileName.ToString(" yyyy-MM-dd HH-mm") + ".csv", FileMode.OpenOrCreate)))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        records = csv.GetRecords<CPUUsage>().ToList();

                        inMemoryData.Add(fileName.ToString("HH-mm"), records);
                    }

                    fileName = fileName.AddHours(1);
                }


                //Input DateTime pre-processing
                var startTimeKey = startTime.AddMinutes(-startTime.Minute);
                var endTimeKey = endTime.AddHours(1).AddMinutes(-endTime.Minute);
                IEnumerable<CPUUsage> resultSelt = new List<CPUUsage>();

                // Adding data from the map as per the user input
                while (startTimeKey <= endTimeKey)
                {
                    var key = startTimeKey.ToString("HH-mm");

                    if (inMemoryData.ContainsKey(key))
                    {
                        var data = inMemoryData[key];
                        resultSelt = resultSelt.Concat(data);
                    }

                    startTimeKey = startTimeKey.AddHours(1);
                }


                double unixstartTime = startTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                double unixendTime = endTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                // Querying the IEnumerable
                var resultSet = resultSelt.Where(x => double.Parse(x.TimeStamp) >= unixstartTime &&
                                                      double.Parse(x.TimeStamp) <= unixendTime &&
                                                      x.CPU_ID == cpu_id &&
                                                      x.IP == name);

                Console.WriteLine("------ Logs ------ ");
                Console.WriteLine($"CPU {cpu_id} usage on " + name);

                foreach (var res in resultSet)
                {
                    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                    dateTime = dateTime.AddSeconds(double.Parse(res.TimeStamp)).ToUniversalTime();
                    Console.WriteLine($"({dateTime}, {res.Usage})");
                }

                Console.WriteLine("------ End ------ ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
