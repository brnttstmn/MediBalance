using System;
using System.Collections.Generic;
using WiimoteLib;
using System.Text.RegularExpressions;
using System.IO.Pipes;
using System.IO;
using System.Timers;
using System.Threading;

namespace BalanceBoard
{
    class Program
    {
        // Pipe Instantiation
        static NamedPipeServerStream BoardServer;
        static StreamWriter StreamWrite = null;
        static StreamReader StreamRead = null;

        // Wii Board
        static Wiimote wiiDevice = null;
        static string Timingformat = "HH:mm:ss:fff";


        static void Main(string[] args)
        {    
            if (args.Length > 0) { if (args[0] == "Debug") { startDebug(); } }
            else { run(); }
        }

        static void run()
        {
            // Initialize Variables
            var run = true;

            Console.WriteLine("Connecting....");
            while (run)
            {
                try
                {
                    // Connect to Pipe
                    connectPipe();
                    listen();
                    connect();
                    start();
                }
                catch (IOException) { Console.WriteLine("Connection Terminated"); }
                catch(ObjectDisposedException) { Console.WriteLine("Connection Terminated"); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); run = false; }
                finally { BoardServer.Dispose(); }                

            }
        }

        static string listen()
        {
            var message = "";

            while (true)
            {
                message = StreamRead.ReadLine();
                if (message != null)
                {
                    return message;
                }
                Thread.Sleep(500);
            }
        }

        static string start()
        {
            while (true)
            {
                var a = InfoUpdate();
                print(a);
                send(a);
            }
        }

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

        static void print(List<string> data)
        {
            foreach (string line in data)
            {
                Console.WriteLine(line);
            }
        }

        static void send(List<string> data)
        {
            foreach (string line in data)
            {
                StreamWrite.WriteLine(line);
            }
        }

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
                wiiDevice.SetReportType(InputReport.IRAccel, false); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                wiiDevice.SetLEDs(true, false, false, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error... you broke it");
                Console.WriteLine(ex.Message, "Error");
            }

        }

        private static void connectPipe() {
            // Start and Connect to Pipe
            BoardServer = new NamedPipeServerStream("board", PipeDirection.InOut);
            Console.WriteLine("Waiting for Connection...");
            BoardServer.WaitForConnection();
            StreamRead = new StreamReader(BoardServer);
            StreamWrite = new StreamWriter(BoardServer) { AutoFlush = true };
            Console.WriteLine("Pipe Connected.");
        }

        private static List<string> InfoUpdate()
        {
            var result = new List<string>();

            // Get the current raw sensor Lb values.
            var rwWeight = wiiDevice.WiimoteState.BalanceBoardState.WeightLb;
            var rwTopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.TopLeft;
            var rwTopRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.TopRight;
            var rwBottomLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.BottomLeft;
            var rwBottomRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.BottomRight;

            // Format Raw Data
            string Timing = DateTime.Now.ToString(Timingformat);
            var weight = rwWeight.ToString("0.0");
            var topleft = rwTopLeft.ToString("0.0");
            var topright = rwTopRight.ToString("0.0");
            var bottomeleft = rwBottomLeft.ToString("0.0");
            var bottomright = rwBottomRight.ToString("0.0");

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
