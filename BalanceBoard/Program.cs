using System;
using System.Collections.Generic;
using WiimoteLib;
using System.Text.RegularExpressions;
using System.IO.Pipes;
using System.IO;
using System.Timers;

namespace BalanceBoard
{
    class Program
    {

        //Piping Goodness
        static NamedPipeServerStream BoardServer = new NamedPipeServerStream("board", PipeDirection.InOut);
        static System.Timers.Timer infoUpdateTimer = new System.Timers.Timer() { Interval = 50, Enabled = false };
        static StreamWriter StreamWrite = null;
        static StreamReader StreamRead = null;
        static Wiimote wiiDevice = null;
        static string Timing = DateTime.Now.ToString("HH:mm:ss:fff");
        DateTime jumpTime = DateTime.UtcNow;
        bool setCenterOffset = false;

        float naCorners = 0f;
        float oaTopLeft = 0f;
        float oaTopRight = 0f;
        float oaBottomLeft = 0f;
        float oaBottomRight = 0f;

        static void Main(string[] args)
        {
            connect();
        }

        private static void connect() // This is where the initialization exists
        {
            try
            {
                // Pipe initialization
                Console.WriteLine("Connecting....");
                //BoardServer.WaitForConnection();
                //StreamRead = new StreamReader(BoardServer);
                //StreamWrite = new StreamWriter(BoardServer) { AutoFlush = true };
                Console.WriteLine("Connected.");

                //Board Setup
                wiiDevice = new Wiimote();
                var deviceCollection = new WiimoteCollection();


                // Find all connected Wii devices.
                try
                {
                    deviceCollection.FindAllWiimotes();
                }
                catch{
                    BluetoothBoard.add_device();
                    deviceCollection.FindAllWiimotes();
                }
                
                Console.WriteLine(deviceCollection.Count);

                for (int i = 0; i < deviceCollection.Count; i++)
                {
                    wiiDevice = deviceCollection[i];

                    // Device type can only be found after connection, so prompt for multiple devices.

                    if (deviceCollection.Count > 1)
                    {
                        var devicePathId = new Regex("e_pid&.*?&(.*?)&").Match(wiiDevice.HIDDevicePath).Groups[1].Value.ToUpper();
                        continue;
                        //var response = MessageBox.Show("Connect to HID " + devicePathId + " device " + (i + 1) + " of " + deviceCollection.Count + " ?", "Multiple Wii Devices Found", MessageBoxButtons.YesNoCancel);
                        //if (response == DialogResult.Cancel) return;
                        //if (response == DialogResult.No) continue;
                    }

                    // Connect and send a request to verify it worked.

                    wiiDevice.Connect();
                    wiiDevice.SetReportType(InputReport.IRAccel, false); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                    wiiDevice.SetLEDs(true, false, false, false);

                    // Enable processing of updates.

                    infoUpdateTimer.Enabled = true;
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error... Bitch");
                Console.WriteLine(ex.Message, "Error");
            }
            Console.ReadKey();
        }


        private void InfoUpdate()
        {

            if (wiiDevice.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
            {
                Console.WriteLine("DEVICE IS NOT A BALANCE BOARD...");
                return;
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
            string Timing = DateTime.Now.ToString("HH:mm:ss:fff");
            var weight = rwWeight.ToString("0.0");
            var topleft = rwTopLeft.ToString("0.0");
            var topright = rwTopRight.ToString("0.0");
            var bottomeleft = rwBottomLeft.ToString("0.0");
            var bottomright = rwBottomRight.ToString("0.0");
            Logging(Timing, weight, topleft, topright, bottomeleft, bottomright);

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
        }

        public void Logging(string timing, string weight, string TopLeft, string TopRight, string BottomLeft, string BottomRight) //This is where the data is logged
        {
            Console.WriteLine("logging");

            Dictionary<string, string> bdata = new Dictionary<string, string>() {
                { "RWeight", weight}, {"TopLeft",TopLeft}, {"TopRight", TopRight}, {"BottomLeft",BottomLeft}, {"BottomRight",BottomRight },{"Weight Orientation",label_Status.Text}
            };

            foreach (KeyValuePair<string, string> data in bdata)
            {
                var newLine = string.Format("{0},{1},{2};", timing, data.Key, data.Value);
                StreamWrite.WriteLine(newLine);
                Console.WriteLine(newLine);
            }
        }


    }
}
