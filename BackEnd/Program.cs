using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using SharedLibraries;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace BackEnd
{
    class Program
    {
        // Initialize interprocess communication
        static Pipe kinect = new Pipe("kinect", true);
        static Pipe board = new Pipe("board", true);
        static Pipe gui = new Pipe("interface", false);
        static Pipe fromgui = new Pipe("frominterface", false);
        static TCP band = new TCP("band");

        // Make interporcess communications iterable
        static List<Comm> commList = new List<Comm>() { kinect, board, band, gui, fromgui };
        static List<Comm> sensors = commList.Except(new List<Comm>() { gui }).ToList();

        // Logging and Data Array
        static ConcurrentBag<string> data_list = new ConcurrentBag<string>();
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
            // Connect TCP/IP and Named Pipes
            Comm.connectComm(commList);

            // Start Reading from each sensor (each sensor has a thread allocated to it)
            multiSensorRead();

            // Disconnect TCP/IP and Named Pipes
            Comm.disconnectComm(commList);

            // Log Applicable Data if Logging was Enabled
            if (isLogging)
            {
                data_log = new Log(data_list.ToList(), start_time);
                data_log.logdata();
                data_log.writeCSV(fileName + append);
                isLogging = false;
            }
        }

        private static void guiCommands(string line = null)
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

            // Create Threads per device
            Parallel.ForEach(sensors, sensor => {
                sensor.startThread(() => readSensor(sensor));
                sensor.thread.Start();
            });

            // Run Threads untill exception
            while (!endConnection)
            {
                Thread.Sleep(5);
            }
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
                    if (sensor == fromgui) { guiCommands(((Pipe)sensor).read.ReadLine()); }
                    else if (sensor.commType == typeof(TCP))
                    {
                        var line = ((TCP)sensor).readStream();
                        gui.send(line);
                        if (isLogging) { data_list.Add(line); }
                    }
                    else
                    {
                        var line = ((Pipe)sensor).read.ReadLine();
                        gui.send(line);
                        if (isLogging) { data_list.Add(line); }
                    }
                }
            }
            catch (IOException) { endConnection = true; }
            catch (ObjectDisposedException) { }
            catch (SocketException) { }
            catch (IndexOutOfRangeException) { }
        }
    }
}
