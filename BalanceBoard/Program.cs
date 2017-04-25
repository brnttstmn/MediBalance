using System;
using System.Collections.Generic;
using WiimoteLib;
using System.IO;
using System.Threading;
using SharedLibraries;

namespace BalanceBoard
{
    class Program
    {
        // Pipe Instantiation
        static Pipe bserver = new Pipe("board", false);

        // Wii Board
        static Wiimote wiiDevice;

        // Weight Offsets
        static float oWeight;
        static float oTopLeft;
        static float oTopRight;
        static float oBottomLeft;
        static float oBottomRight;
        
        static void Main(string[] args)
        {
            try
            {
                // Initialize and start pipe
                bserver.start();

                // Initialize Board variable
                connectBoard();

                // Start reading sensors and send to Named Pipe
                run();
            }

            // Catch IOException when pipe is closed and restart pipe connection
            catch (IOException) { Console.WriteLine("Connection Terminated"); }

            // Catch all other unexpected exceptions, write to terminal and close program
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            // Dispose thread 
            finally { bserver.stop(); }
        }

        /// <summary>
        /// Read Data from sensors and send to Pipe
        /// </summary>
        /// <returns></returns>
        static string run()
        {
            while (true)
            {
                var data = InfoUpdate();
                send(data);
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
            string lines = "";
            foreach (string line in data)
            {
                lines += line;
            }
            Console.WriteLine(lines);
            bserver.send(lines);
        }

        /// <summary>
        /// Verifies that Balance board is connected, then initializes board object
        /// </summary>
        private static void connectBoard()
        {
            try
            {
                // Initialize Board Varibles
                wiiDevice = new Wiimote();
                var deviceCollection = new WiimoteCollection();

                // Verify board is connected. If not, repeat until board is connected.
                do
                {
                    try
                    {
                        Console.WriteLine("Searching for connected board...");
                        deviceCollection.FindAllWiimotes();
                        Console.WriteLine("Board Found!");
                    }
                    catch
                    {
                        Console.WriteLine("No boards are connected...");
                        Thread.Sleep(1000);
                    }
                }
                while (deviceCollection.Count<1);

                // Set wiiDevice variable
                wiiDevice = deviceCollection[0];      
                wiiDevice.Connect();
                wiiDevice.SetReportType(InputReport.IRAccel, false);
                wiiDevice.SetLEDs(true, false, false, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Error");
            }

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
            if (rwWeight < 1)
            {
                // Scale up to Zero
                if (rwWeight + oWeight < 0.0) { oWeight = -rwWeight; }
                if (rwTopLeft + oTopLeft < 0.0) { oTopLeft = -rwTopLeft; }
                if (rwTopRight + oTopRight < 0.0) { oTopRight = -rwTopRight; }
                if (rwBottomLeft + oBottomLeft < 0.0) { oBottomLeft = -rwBottomLeft; }
                if (rwBottomRight + oBottomRight < 0.0) { oBottomRight = -rwBottomRight; }

                // Scale down to Zero
                if (rwWeight + oWeight > 0.0) { oWeight += ( 0 -(rwWeight + oWeight)); }
                if (rwTopLeft + oTopLeft > 0.0) { oTopLeft += (0 - (rwTopLeft + oTopLeft)); }
                if (rwTopRight + oTopRight > 0.0) { oTopRight += (0 - (rwTopRight + oTopRight)); }
                if (rwBottomLeft + oBottomLeft > 0.0) { oBottomLeft += (0 - (rwBottomLeft + oBottomLeft)); }
                if (rwBottomRight + oBottomRight > 0.0) { oBottomRight += (0 - (rwBottomRight + oBottomRight)); }
            }


            // Format Raw Data
            string Timing = DateTime.Now.ToString("HH:mm:ss:fff");
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
