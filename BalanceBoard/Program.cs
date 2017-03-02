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
        static NamedPipeServerStream BoardServer = new NamedPipeServerStream("board", PipeDirection.InOut);
        static StreamWriter StreamWrite = null;
        static StreamReader StreamRead = null;

        // Wii Board
        static Wiimote wiiDevice = null;
        static string Timingformat = "HH:mm:ss:fff";


        static void Main(string[] args)
        {    
            Console.WriteLine("Connecting....");
            connect(); // Check connection to board... Connect board if not already connected
            run(); // Start listening and performing tasks as assigned from the backend
            //startDebug(); // Run Local instances
        }

        static void run()
        {
            // Initialize Variables
            var command = "";
            var run = true;

            // Connect to Pipe
            connectPipe();

            // Start Listening to commands
            while (run)
            {
                // Listen for command
                Console.WriteLine("Listening");
                command = listen();
                Console.WriteLine(command);

                // Perform Command
                switch (command)
                {
                    case "start":
                        start();
                        break;
                    //case "test":
                    //    test();
                    //    break;
                    //case "stop":
                    //    run = false;
                    //    break;
                    default:
                        run = true;
                        break;
                }
                //temporary. needs debugging
                run = false;
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
                try
                {
                    Console.WriteLine("Searching for connected board...");
                    deviceCollection.FindAllWiimotes();
                    Console.WriteLine("Board Found!");
                }
                catch
                {
                    Console.WriteLine("No boards are connected...");
                    BluetoothBoard.add_device();
                    deviceCollection.FindAllWiimotes();
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
            Console.WriteLine("Starting Pipe");
            BoardServer.WaitForConnection();
            StreamRead = new StreamReader(BoardServer);
            StreamWrite = new StreamWriter(BoardServer) { AutoFlush = true };
            Console.WriteLine("Pipe Connected.");
        }

        private static List<string> InfoUpdate()
        {
            DateTime jumpTime = DateTime.UtcNow;
            bool setCenterOffset = false;
            float naCorners = 0f;
            float oaTopLeft = 0f;
            float oaTopRight = 0f;
            float oaBottomLeft = 0f;
            float oaBottomRight = 0f;
            string status = "";
            var result = new List<string>();

            if (wiiDevice.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
            {
                Console.WriteLine("DEVICE IS NOT A BALANCE BOARD...");
                return result;
            }

            // Get the current raw sensor Lb values.
            var rwWeight = wiiDevice.WiimoteState.BalanceBoardState.WeightLb;
            var rwTopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.TopLeft;
            var rwTopRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.TopRight;
            var rwBottomLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.BottomLeft;
            var rwBottomRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesLb.BottomRight;

            // Prevent negative values by tracking lowest possible value and making it a zero based offset.
            if (rwTopLeft < naCorners) naCorners = rwTopLeft;
            if (rwTopRight < naCorners) naCorners = rwTopRight;
            if (rwBottomLeft < naCorners) naCorners = rwBottomLeft;
            if (rwBottomRight < naCorners) naCorners = rwBottomRight;

            // Negative total weight is reset to zero as jumping or lifting the board causes negative spikes, which would break 'in use' checks.
            var owWeight = rwWeight < 0f ? 0f : rwWeight;
            var owTopLeft = rwTopLeft -= naCorners;
            var owTopRight = rwTopRight -= naCorners;
            var owBottomLeft = rwBottomLeft -= naCorners;
            var owBottomRight = rwBottomRight -= naCorners;

            // Get offset that would make current values the center of mass.
            if (setCenterOffset)
            {
                setCenterOffset = false;

                var rwHighest = Math.Max(Math.Max(rwTopLeft, rwTopRight), Math.Max(rwBottomLeft, rwBottomRight));

                oaTopLeft = rwHighest - rwTopLeft;
                oaTopRight = rwHighest - rwTopRight;
                oaBottomLeft = rwHighest - rwBottomLeft;
                oaBottomRight = rwHighest - rwBottomRight;
            }

            // Keep values only when board is being used, otherwise offsets and small value jitters can trigger unwanted actions.
            if (owWeight > 0f)
            {
                owTopLeft += oaTopLeft;
                owTopRight += oaTopRight;
                owBottomLeft += oaBottomLeft;
                owBottomRight += oaBottomRight;
            }
            else
            {
                owTopLeft = 0;
                owTopRight = 0;
                owBottomLeft = 0;
                owBottomRight = 0;
            }


            // Show the raw sensor values.
            string Timing = DateTime.Now.ToString(Timingformat);
            var weight = rwWeight.ToString("0.0");
            var topleft = rwTopLeft.ToString("0.0");
            var topright = rwTopRight.ToString("0.0");
            var bottomeleft = rwBottomLeft.ToString("0.0");
            var bottomright = rwBottomRight.ToString("0.0");

            // Calculate each weight ratio.
            var owrPercentage = 100 / (owTopLeft + owTopRight + owBottomLeft + owBottomRight);
            var owrTopLeft = owrPercentage * owTopLeft;
            var owrTopRight = owrPercentage * owTopRight;
            var owrBottomLeft = owrPercentage * owBottomLeft;
            var owrBottomRight = owrPercentage * owBottomRight;

            // Calculate balance ratio.
            var brX = owrBottomRight + owrTopRight;
            var brY = owrBottomRight + owrBottomLeft;

            // Diagonal ratio used for turning on the spot.
            var brDL = owrPercentage * (owBottomLeft + owTopRight);
            var brDR = owrPercentage * (owBottomRight + owTopLeft);
            var brDF = Math.Abs(brDL - brDR);

            // Convert sensor values into actions.
            var sendLeft = false;
            var sendRight = false;
            var sendForward = false;
            var sendBackward = false;
            var sendModifier = false;
            var sendJump = false;
            var sendDiagonalLeft = false;
            var sendDiagonalRight = false;

            // Display actions.
            status = "Result: ";

            // Detect jump but use a time limit to stop it being active while off the board.
            if (owWeight < 1f)
            {
                if (DateTime.UtcNow.Subtract(jumpTime).Seconds < 2) sendJump = true;
            }
            else
            {
                jumpTime = DateTime.UtcNow;
            }

            // Check for diagonal pressure only when no other movement actions are active.
            if (!sendLeft && !sendRight && !sendForward && !sendBackward && brDF > 15)
            {
                if (brDL > brDR) sendDiagonalLeft = true;
                else sendDiagonalRight = true;
            }

            if (sendForward) status += "Forward";
            if (sendLeft) status += "Left";
            if (sendBackward) status += "Backward";
            if (sendRight) status += "Right";
            if (sendModifier) status += " + Modifier";
            if (sendJump) status += "Jump";
            if (sendDiagonalLeft) status += "Diagonal Left";
            if (sendDiagonalRight) status += "Diagonal Right";

            Dictionary<string, string> bdata = new Dictionary<string, string>() {
                { "RWeight", weight}, {"TopLeft",topleft}, {"TopRight", topright}, {"BottomLeft",bottomeleft}, {"BottomRight",bottomright },{"Weight Orientation",status}
            };

            foreach (KeyValuePair<string, string> data in bdata)
            {
                result.Add(string.Format("{0},{1},{2};", Timing, data.Key, data.Value));
            }
            return result;
        }
    }
}
