using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    class Log
    {
        //Logging and Data Array
        private bool debug = false;
        private List<string> data_list;
        private static string timeFormat = "HH:mm:ss:fff";
        private static int loglength = 15;
        private static int loginterval = 100;
        private string[] data_array = new string[loglength * loginterval];
        private DateTime[] time_array = new DateTime[loglength * loginterval];
        private CultureInfo enUS = new CultureInfo("en-US");
        private DateTime startTime;
        private const string defaultLocation = "C:\\test.csv";

        // Constructors
        public Log(List<string> data, DateTime start_time, bool debugMode = false)
        {
            data_list = data;
            startTime = start_time;
            debug = debugMode;
        }

        // Public Methods
        /// <summary>
        /// Logs a single line of data into the data array
        /// </summary>
        public void logdata()
        {
            // Variables
            bool miss;
            int compareresult1, compareresult2, misstracker = 0;
            string[] splitdata;
            DateTime start_time;
            char[] delim = { ',', ';' };

            // Populate Time array
            populatetime();

            // Place data in time slots
            if (debug) { Console.WriteLine("Placing Data in Time Slots"); }
            foreach (string datat in data_list)
            {
                var data = datat.Replace(';','\n');
                miss = true;
                splitdata = data.Split(delim);
                try
                {
                    start_time = DateTime.ParseExact(data.Split(',')[0], timeFormat, enUS);
                    for (int i = 0; i <= ((loglength * loginterval) - 2); i++)
                    {
                        compareresult1 = DateTime.Compare(start_time, time_array[i]); //compares data to lower cell in time array
                        compareresult2 = DateTime.Compare(start_time, time_array[i + 1]); //compares data to upper cell in time array

                        if (compareresult1 > 0 && compareresult2 < 0) // if data is after lower cell but before the uppercell
                        {
                            data_array[i] = data_array[i] + data;
                            if (debug) { Console.WriteLine("Placed {1} in cell {0}! of timestamp {2}.", i, data, time_array[i].ToString(timeFormat)); }
                            miss = false;
                        }
                        if (start_time == time_array[i])
                        {
                            data_array[i] = data_array[i] + data;
                            if (debug) { Console.WriteLine("Placed {1} in cell {0}! of timestamp {2}.", i, data, time_array[i].ToString(timeFormat)); }
                            miss = false;
                        }
                    }
                    if (miss)
                    {
                        if (debug) { Console.WriteLine("Data missed,{0}.", data); }
                        misstracker++;
                    }
                }
                catch (Exception) { }
            }
            if (debug) { Console.WriteLine("Logging Finished with {0} errors.", misstracker); }
        }

        /// <summary>
        /// prints current data log stored in the data array
        /// </summary>
        public void printlog()
        {
            foreach (string cell in data_array) Console.WriteLine(cell);
        }

        public void writeCSV(string filePath = defaultLocation)
        {
            var csv = new StringBuilder();

            foreach (string cell in data_array) { if (cell != null && cell.Length>1) { csv.AppendLine(cell); } }

            File.WriteAllText(filePath, csv.ToString());
        }

        // Private Methods
        /// <summary>
        /// Makes time array for data comparison
        /// </summary>
        private void populatetime()
        {
            if (debug) { Console.WriteLine("Populating Time Data Array."); }
            time_array[0] = startTime;
            string time = time_array[0].ToString(timeFormat);
            Console.WriteLine("{0}", time);
            for (int i = 1; i <= ((loglength * loginterval) - 1); i++)
            {
                startTime = startTime.AddMilliseconds(loginterval);
                time_array[i] = startTime;
                //time = time_array[i].ToString(timeFormat);
                //Console.WriteLine("{0}",time);
            }
        }
    }
}
