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
        static Pipe tunnel = new Pipe("tunnel", true);

        static Dictionary<Pipe, string> programList = new Dictionary<Pipe, string>()
        {
            {kinect, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe"},
            {board, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe"},
            {gui, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\FrontEndUIRedux\\bin\\Debug\\FrontEndUIRedux.exe"},
            {tunnel, "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\Bridge\\bin\\Debug\\Bridge.exe"},
        };
        // Lists
        // You can remove any device/program you do not plan on using from this list... It will take care of the rest.
        static List<Pipe> pipelist = new List<Pipe>() { kinect, board, tunnel, gui, fromgui }; //kinect, board, tunnel, gui, fromgui
        static List<Pipe> sensors = pipelist.Except(new List<Pipe>() { gui }).ToList();
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
            // Restart Programs
            //stopPrograms();
            //runPrograms();

            // Start
            while (true)
            {
                try
                {
                    Pipe.connectPipes(pipelist);
                    guiCommandsSetup();
                    multiSensorRead();
                }
                catch (IOException) { Console.WriteLine("Connection Terminated\n\n"); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                finally
                {
                    Pipe.disconnectPipes(pipelist);
                }

                // Log Applicable Data if Logging was Enabled
                if (isLogging)
                {
                    data_log = new Log(data_list.ToList(), start_time);
                    data_log.logdata();
                    data_log.writeCSV(fileName + append);
                    isLogging = false;
                }
            }
        }

        private static void guiCommandsSetup()
        {
            Thread listen = new Thread(() => awaitGuiCommands());
            if (!listen.IsAlive) { listen.Start(); }
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

        /// <summary>
        /// Single Pipe read. Exits gracefully on pipe closing.
        /// </summary>
        /// <param name="sensor"></param>
        static void readSensor(Pipe sensor)
        {
            try
            {
                if (sensor == fromgui) { awaitGuiCommands(sensor.read.ReadLine()); }
                else
                {
                    var line = sensor.read.ReadLine();
                    if (line != "/n")
                    {
                        gui.write.WriteLine(line);
                        if (isLogging) { data_list.Add(line); }
                        //Console.WriteLine(line);
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
            foreach (Pipe sensor in sensors)
            {
                Console.WriteLine("starting: " + sensor.name);
                sensor.sendcommand("Start");
            }
        }

        /// <summary>
        /// Start all of the programs
        /// </summary>
        static void runPrograms()
        {
            Parallel.ForEach(pipelist, program => {
                if (programList.ContainsKey(program))
                {
                    Process.Start(programList[program]);
                }
            });
        }

        /// <summary>
        /// Kills all processes
        /// </summary>
        static void stopPrograms()
        {
            char[] del = { '\\', '.' };
            Parallel.ForEach(pipelist, program => {
                if (programList.ContainsKey(program))
                {
                    string name = programList[program].Split(del)[programList[program].Split(del).Length - 2];
                    foreach (var process in Process.GetProcessesByName(name))
                    {
                        process.Kill();
                    }
                }
            });
        }

    }
}
