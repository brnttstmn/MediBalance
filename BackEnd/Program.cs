using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Globalization;

namespace BackEnd
{
    class Program
    {
        // Instantiate Named Pipes
        static Pipe kinect = new Pipe("kinect", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
        static Pipe board = new Pipe("board", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe");
        static Pipe gui = new Pipe("interface", false, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\FrontEndUIRedux\\bin\\Debug\\FrontEndUIRedux.exe");
        static Pipe tunnel = new Pipe("tunnel", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\Tunnel\\bin\\Debug\\Tunnel.exe");

        
        // Lists
        // You can remove any device/program you do not plan on using from this list... It will take care of the rest.
        static List<Pipe> pipelist = new List<Pipe>() { kinect, board, tunnel, gui }; //kinect, board, tunnel, gui
        static List<Pipe> sensors = pipelist.Except(new List<Pipe>() { gui }).ToList();
        static List<string> data_list = new List<String>();
        
        //Logging and Data Array
        static string timeFormat = "HH:mm:ss:fff";
        static int log_length = 30;
        static int log_interval = 100;
        static string[] data_array = new string[log_length * log_interval];
        static DateTime[] time_array = new DateTime[log_length * log_interval];
        static CultureInfo enUS = new CultureInfo("en-US");



        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            run();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void run()
        {
            bool run = true;
            runPrograms();
            while (run)
            {
                try
                {
                    connectPipes();
                    startSensors();
                    readSensors();
                }
                catch (IOException) { Console.WriteLine("Connection Terminated"); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); run = false; }
                finally { disconnectPipes(); }
            }
        }

        static void tempdemocsv(string data)
        {
            bool status = true;
            int i = 0;
            int count = 0;
            while (status == true)
            {
                if (!kinect.read.EndOfStream && !string.IsNullOrWhiteSpace(kinect.read.Peek().ToString())) { data = kinect.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                if (!board.read.EndOfStream && !string.IsNullOrWhiteSpace(board.read.Peek().ToString())) { data = board.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                if (count > 50)
                {
                    if (!tunnel.read.EndOfStream && !string.IsNullOrWhiteSpace(tunnel.read.Peek().ToString())) { data = tunnel.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                    count = 0;
                    if (i > 40) status = false;
                    i++;
                }

                Console.WriteLine("Reading {0}", count);
                count++;
            }

            string filePath = @"C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\data.csv";
            //File.OpenWrite(filePath);
            File.AppendAllLines(filePath, data_list);
        }

        static void readSensors()
        {
                string line;
                while (true)
                {
                    foreach (Pipe sensor in sensors)
                    {
                        line = sensor.read.ReadLine();
                        if (pipelist.Contains(gui)) { gui.write.WriteLine(line); }
                        data_list.Add(line);
                        Console.WriteLine(line);
                    }
                }
        }

        static void connectPipes()
        {
            Parallel.ForEach(pipelist, pipe => {
                pipe.start();
            });
        }

        static void disconnectPipes()
        {
            Parallel.ForEach(pipelist, pipe => {
                pipe.stop();
            });
        }

        static void startSensors()
        {
            Parallel.ForEach(sensors, sensor => {
                sensor.sendcommand("Start");
            });

            populate_time(DateTime.Now); //creates the time array at the start of data collection
        }

        /// <summary>
        /// Start all of the programs
        /// </summary>
        static void runPrograms()
        {
            Parallel.ForEach(pipelist, program => {
                Process.Start(program.path);
            });
        }

        /// <summary>
        /// Logs a single line of data into the data array
        /// </summary>
        static void log_data(string data)
        {
            Console.WriteLine("Logging");
            int compareresult1;
            int compareresult2;
            string[] split_data = data.Split(',');
            string[] split_time = split_data[0].Split(':');
            //time_compare(, split_data[0]);
            DateTime start_stamp = DateTime.ParseExact(split_data[0], "HH:mm:ss:fff", enUS);
            for (int i = 0; i <= 2998; i++)
            {
                //Console.WriteLine(i);
                compareresult1 = DateTime.Compare(start_stamp, time_array[i]); //compares data to lower cell in time array
                //Console.WriteLine("lower cell is {0}",compareresult1);
                compareresult2 = DateTime.Compare(start_stamp, time_array[i + 1]); //compares data to upper cell in time array
                //Console.WriteLine("upper cell is {0}", compareresult2);

                if (compareresult1 > 0 && compareresult2 < 0) // if data is after lower cell but before the uppercell
                {
                    data_array[i] = data_array[i] + data;
                    Console.WriteLine("Placed {1} in cell {0}! of timestamp {2}", i, data, time_array[i].ToString(timeFormat));
                    return;
                }
            }
        }

        /// <summary>
        /// Makes time array for data comparison
        /// </summary>
        static void populate_time(DateTime start)
        {
            time_array[0] = start;
            string time = time_array[0].ToString(timeFormat);
            Console.WriteLine("{0}", time);
            for (int i = 1; i <= 2999; i++)
            {
                start = start.AddMilliseconds(log_interval);
                time_array[i] = start;
                //time = time_array[i].ToString(timeFormat);
                //Console.WriteLine("{0}",time);
            }
        }
    }
}
  