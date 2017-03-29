using System;
using System.Collections.Generic;
using WiimoteLib;
using System.IO.Pipes;
using System.IO;
using System.Threading;

namespace BalanceBoard
{
    class Program
    {
        // Pipe Instantiation
        static NamedPipeServerStream BoardServer;
        static StreamWriter StreamWrite = null;
        static StreamReader StreamRead = null;
        static bool isConnected = false;

        // Wii Board
        static Wiimote wiiDevice = null;
        static string Timingformat = "HH:mm:ss:fff";

        // Offsets
        static float oWeight;
        static float oTopLeft;
        static float oTopRight;
        static float oBottomLeft;
        static float oBottomRight;
        
        static void Main(string[] args)
        {    
            // If called in Debug mode, run DEBUG, else run program
            if (args.Length > 0) { if (args[0] == "Debug") { startDebug(); } }
            else { run(); }
        }

        static void run()
        {
            // Initialize Variables
            var run = true;

            Console.WriteLine("Connecting....");

            // Run untill closed or unexpected Exception
            while (run)
            {
                try
                {
                    // Initialize and start pipe
                    connectPipe(); 

                    // Listen for start command
                    listen(); 

                    // Initialize Board variable (Only do once)
                    if (isConnected==false) { connect(); }                

                    // Start reading from sensors and send to pipe
                    start(); 
                }

                // Catch IOException when pipe is closed and restart pipe connection
                catch (IOException) { Console.WriteLine("Connection Terminated"); }

                // Catch all other unexpected exceptions, write to terminal and close program
                catch (Exception ex) { Console.WriteLine(ex.ToString()); run = false; }

                // Dispose thread 
                finally { BoardServer.Dispose(); }
            }
        }

        /// <summary>
        /// Listen to Pipe Connection and return value
        /// </summary>
        /// <returns></returns>
        static string listen()
        {
            while (true)
            {
                var message = StreamRead.ReadLine();
                if (message != null)
                {
                    return message;
                }
            }
        }

        /// <summary>
        /// Read Data from sensors and send to Pipe
        /// </summary>
        /// <returns></returns>
        static string start()
        {
            while (true)
            {
                var a = InfoUpdate();
                print(a);
                send(a);
            }
        }

        /// <summary>
        /// Debug Mode
        /// </summary>
        /// <param name="mode"></param>
        static void startDebug(int mode=1)
        {
            List<string> a;

            connect();
            switch (mode)
            {
                case 0:
                    start();
                    while (true)
                    {
                        a = InfoUpdate();
                        print(a);
                    }
                default:
                    a = InfoUpdate();
                    print(a);
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;
            }
        }

        /// <summary>
        /// Prints data data list to console
        /// </summary>
        /// <param name="data"></param>
        static void print(List<string> data)
        {
            foreach (string line in data)
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Sends Data List through pipe
        /// </summary>
        /// <param name="data"></param>
        static void send(List<string> data)
        {
            foreach (string line in data)
            {
                StreamWrite.WriteLine(line);
            }
        }

        /// <summary>
        /// Verifies that Balance board is connected, then initializes board object
        /// </summary>
        private static void connect()
        {
            try
            {
                // Initialize Board Varibles
                wiiDevice = new Wiimote();
                var deviceCollection = new WiimoteCollection();

                // Check for Connected Board
                bool again = true;
                while (again)
                {
                    try
                    {
                        Console.WriteLine("Searching for connected board...");
                        deviceCollection.FindAllWiimotes();
                        Console.WriteLine("Board Found!");
                        again = false;
                    }
                    catch
                    {
                        Console.WriteLine("No boards are connected...");
                        Thread.Sleep(1000);
                        //BluetoothBoard.add_device();
                    }
                }

                //Conneect to BalanceBoard
                wiiDevice = deviceCollection[0];      
                wiiDevice.Connect();
                isConnected = true;
                wiiDevice.SetReportType(InputReport.IRAccel, false); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                wiiDevice.SetLEDs(true, false, false, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Error");
            }

        }

        /// <summary>
        /// Initialize and Connect to Pipe
        /// </summary>
        private static void connectPipe() {
            // Start and Connect to Pipe
            BoardServer = new NamedPipeServerStream("board", PipeDirection.InOut);
            Console.WriteLine("Waiting for Connection...");
            BoardServer.WaitForConnection();
            StreamRead = new StreamReader(BoardServer);
            StreamWrite = new StreamWriter(BoardServer) { AutoFlush = true };
            Console.WriteLine("Pipe Connected.");
        }

        /// <summary>
        /// Populates sensor data at a given instance and returns it in a string list
        /// </summary>
        /// <returns></returns>
        private static List<string> InfoUpdate()
        {
            var result = new List<string>();

            // Get the current raw sensor Lb values.
            var rwWeight = wiiDevice.WiimoteState.BalanceBoardState.WeightLb;
            var rwTopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.TopLeft;
            var rwTopRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.TopRight;
            var rwBottomLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.BottomLeft;
            var rwBottomRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.BottomRight;

            // Auto Calibrate
            if (rwWeight + oWeight < 0.0) { oWeight = -rwWeight; }
            if (rwTopLeft + oTopLeft < 0.0) { oTopLeft = -rwTopLeft; }
            if (rwTopRight + oTopRight < 0.0) { oTopRight = -rwTopRight; }
            if (rwBottomLeft + oBottomLeft < 0.0) { oBottomLeft = -rwBottomLeft; }
            if (rwBottomRight + oBottomRight < 0.0) { oBottomRight = -rwBottomRight; }

            // Format Raw Data
            string Timing = DateTime.Now.ToString(Timingformat);
            var weight = (oWeight+rwWeight).ToString("0.0");
            var topleft = (oTopLeft+rwTopLeft).ToString("0.0");
            var topright = (oTopRight+rwTopRight).ToString("0.0");
            var bottomeleft = (oBottomLeft+rwBottomLeft).ToString("0.0");
            var bottomright = (oBottomRight+rwBottomRight).ToString("0.0");

            // Output Raw data
            Dictionary<string, string> bdata = new Dictionary<string, string>()
            {
                { "RWeight", weight}, {"TopLeft",topleft}, {"TopRight", topright}, {"BottomLeft",bottomeleft}, {"BottomRight",bottomright }
            };
            foreach (KeyValuePair<string, string> data in bdata)
            {
                result.Add(string.Format("{0},{1},{2};", Timing, data.Key, data.Value));
            }
            return result;
        }
    }
}
