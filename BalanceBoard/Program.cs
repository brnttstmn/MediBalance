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
                    if (isConnected==false) { connect(); }                    
                    start();
                }
                catch (IOException) { Console.WriteLine("Connection Terminated"); }
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
                isConnected = true;
                wiiDevice.SetReportType(InputReport.IRAccel, false); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                wiiDevice.SetLEDs(true, false, false, false);
            }
            catch (Exception ex)
            {
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
