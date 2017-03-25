using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace BackEnd
{
    class Program
    {
        // Instantiate Named Pipes
        static Pipe kinect = new Pipe("kinect", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
        static Pipe board = new Pipe("board", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe");
        static Pipe gui = new Pipe("interface", false, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\FrontEndUIRedux\\bin\\Debug\\FrontEndUIRedux.exe");
        static Pipe tunnel = new Pipe("tunnel", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\Tunnel\\bin\\Debug\\Tunnel.exe");
        static Pipe testapp = new Pipe("testapp", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\testapp\\bin\\Debug\\testapp.exe");
        static Pipe testapp2 = new Pipe("testapp2", true, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\testapp2\\bin\\Debug\\testapp2.exe");

        // Lists
        // You can remove any device/program you do not plan on using from this list... It will take care of the rest.
        static List<Pipe> pipelist = new List<Pipe>() { testapp,testapp2, gui }; //kinect, board, tunnel, gui
        static List<Pipe> sensors = pipelist.Except(new List<Pipe>() { gui }).ToList();
        static List<string> data_list = new List<String>();

        //Logging and Data Array
        static bool endConnection = false;
        static string timeFormat = "HH:mm:ss:fff";
        static int loglength = 15;
        static int loginterval = 100;
        static string[] data_array = new string[loglength * loginterval];
        static DateTime[] time_array = new DateTime[loglength * loginterval];
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
                    multiSensorRead();
                }
                catch (IOException) { Console.WriteLine("Connection Terminated"); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); run = false; }
                finally { disconnectPipes(); }
    
                //logdata();
                printlog();
               
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

        static void multiSensorRead()
        {
            // Create Thread Array and Device Count
            Thread[] threads = new Thread[sensors.Count];
            var sensorAndThreads = sensors.Zip(threads, (s, t) => new { Sensor = s, sThread = t });

            // Create Threads per device
            Parallel.ForEach(sensors, sensor => {
                sensor.startThread(() => readSensor(sensor));
            });

            // Run Threads untill exception
            try
            {
                while (!endConnection)
                {
                    Parallel.ForEach(sensors, sensor =>
                    {
                        if (!sensor.thread.IsAlive)
                        {
                            sensor.startThread(() => readSensor(sensor));
                            sensor.thread.Start();
                        }
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally { endConnection = false; }
            throw new IOException();
        }
        static void readSensor(Pipe sensor)
        {
            try
            {
                var line = sensor.read.ReadLine();
                if (line != "/n")
                {
                    gui.write.WriteLine(line);
                    data_list.Add(line);
                    Console.WriteLine(line);
                }
            }
            catch (IOException) { endConnection = true; }
            catch (ObjectDisposedException) { }
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

            populatetime(DateTime.Now); //creates the time array at the start of data collection
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
        static void logdata()
        {
            Console.WriteLine("Logging"); //Debugging
            int compareresult1;
            int compareresult2;
            int misstracker = 0;
            bool miss;
            foreach (string data in data_list)
            {
                miss = true;
                string[] splitdata = data.Split(',');
                string[] splittime = splitdata[0].Split(':');
                //time_compare(, split_data[0]);
                DateTime start_time = DateTime.ParseExact(splitdata[0], "HH:mm:ss:fff", enUS);
                for (int i = 0; i <= ((loglength * loginterval) - 2); i++)
                {
                    //Console.WriteLine(i);
                    compareresult1 = DateTime.Compare(start_time, time_array[i]); //compares data to lower cell in time array
                    //Console.WriteLine("lower cell is {0}",compareresult1); //Debugging
                    compareresult2 = DateTime.Compare(start_time, time_array[i + 1]); //compares data to upper cell in time array
                    //Console.WriteLine("upper cell is {0}", compareresult2); //Debugging 

                    if (compareresult1 > 0 && compareresult2 < 0) // if data is after lower cell but before the uppercell
                    {
                        data_array[i] = data_array[i] + data;
                        //data_array[i] = string.Format("{0},{1},{2},{3},{4}", data_array[i] , splitdata[1] , splitdata[2] , splitdata[3] , splitdata[4] );

                        Console.WriteLine("Placed {1} in cell {0}! of timestamp {2}", i, data, time_array[i].ToString(timeFormat));
                        miss = false;
                    }
                }
                if (miss == true)
                {
                    Console.WriteLine("Data missed,{0}" , data);
                    misstracker++;
                }
            }
            Console.WriteLine("Logging Finished with {0} errors", misstracker);
        }

        /// <summary>
        /// prints current data log stored in the data array
        /// </summary>
        static void printlog()
        {
            foreach (string cell in data_array) Console.WriteLine(cell);
        }



            /// <summary>
            /// Makes time array for data comparison
            /// </summary>
            static void populatetime(DateTime start)
        {
            time_array[0] = start;
            string time = time_array[0].ToString(timeFormat);
            Console.WriteLine("{0}", time);
            for (int i = 1; i <= ((loglength * loginterval) - 1); i++)
            {
                start = start.AddMilliseconds(loginterval);
                time_array[i] = start;
                //time = time_array[i].ToString(timeFormat);
                //Console.WriteLine("{0}",time);
            }
        }
    }
}
  