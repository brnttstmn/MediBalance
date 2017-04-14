using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SharedLibraries;
using System.Collections.Concurrent;

namespace BackEnd
{
    class Program
    {
        // Instantiate Named Pipes
        static Pipe kinect = new Pipe("kinect", true);
        static Pipe board = new Pipe("board", true);
        static Pipe gui = new Pipe("interface", false);
        static Pipe fromgui = new Pipe("frominterface", false);
        static TCP band = new TCP("band");

        // Lists
        // You can remove any device/program you do not plan on using from this list... It will take care of the rest.
        static List<Comm> pipelist = new List<Comm>() { kinect, board, band, gui, fromgui }; //kinect, board, band, gui, fromgui
        static List<Comm> sensors = pipelist.Except(new List<Comm>() { gui }).ToList();
        static ConcurrentBag<string> data_list = new ConcurrentBag<string>();

        //Logging and Data Array
        static bool endConnection = false, isLogging = false;
        static Log data_log;
        static DateTime start_time;
        static string fileName = "C:\\Users\\" + Environment.UserName + "\\Desktop\\", append;

        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            run();
        }

        /// <summary>
        /// Runs program
        /// </summary>
        static void run()
        {
            // Start
            Comm.connectComm(pipelist);
            multiSensorRead();
            Comm.disconnectComm(pipelist);

            // Log Applicable Data if Logging was Enabled
            if (isLogging)
            {
                data_log = new Log(data_list.ToList(), start_time);
                data_log.logdata();
                data_log.writeCSV(fileName + append);
                isLogging = false;
            }
        }

        private static void awaitGuiCommands(string line = null)
        {
            if (line != null)
            {
                switch (line)
                {
                    case "SingleLegStanceRadio":
                        start_time = DateTime.Now;
                        append = line + ".csv";
                        isLogging = true;
                        break;
                    case "DoubleLegStanceRadio":
                        start_time = DateTime.Now;
                        append = line + ".csv";
                        isLogging = true;
                        break;
                    case "TandemLegStanceRadio":
                        start_time = DateTime.Now;
                        append = line + ".csv";
                        isLogging = true;
                        break;
                    case "Stop":
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Multithread read
        /// </summary>
        static void multiSensorRead()
        {
            startSensors();

            // Create Threads per device
            Parallel.ForEach(sensors, sensor => {
                sensor.startThread(() => readSensor(sensor));
                sensor.thread.Start();
            });

            // Run Threads untill exception
            try
            {
                while (!endConnection)
                {
                    //foreach (Pipe sensor in sensors)
                    //{
                    //    if (!sensor.thread.IsAlive)
                    //    {
                    //        sensor.startThread(() => readSensor(sensor));
                    //        sensor.thread.Start();
                    //    }
                        
                    //}
                    Thread.Sleep(5);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally { endConnection = false; }
            throw new IOException();
        }

        /// <summary>
        /// Single Pipe read. Exits gracefully on pipe closing.
        /// </summary>
        /// <param name="sensor"></param>
        static void readSensor(Comm sensor)
        {
            try
            {
                while (true)
                {
                    if (sensor == fromgui) { awaitGuiCommands(((Pipe)sensor).read.ReadLine()); }
                    else if (sensor.commType == typeof(TCP))
                    {
                        var line = ((TCP)sensor).readStream();
                        gui.write.WriteLine(line);
                        if (isLogging) { data_list.Add(line); }
                    }
                    else
                    {
                        var line = ((Pipe)sensor).read.ReadLine();
                        gui.write.WriteLine(line);
                        if (isLogging) { data_list.Add(line); }
                    }
                }
            }
            catch (IOException) { endConnection = true; }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Sends start signal to all Sensors
        /// </summary>
        static void startSensors()
        {
            foreach (Comm sensor in sensors)
            {
                if (sensor.commType == typeof(Pipe))
                {
                    Console.WriteLine("starting: " + ((Pipe)sensor).name);
                    ((Pipe)sensor).sendcommand("Start");
                }
            }
        }
    }
}
