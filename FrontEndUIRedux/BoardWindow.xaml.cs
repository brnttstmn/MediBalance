using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WiimoteLib;

namespace FrontEndUIRedux
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        public BoardWindow()
        {
            InitializeComponent();
        }

        private void BoardConnect_Click(object sender, RoutedEventArgs e)
        {
            BoardConnect.IsEnabled = false;
            Task.Run(() =>
            {
                connect();
                this.Dispatcher.Invoke(() =>
                {
                    BoardConnect.IsEnabled = true;
                });                
            });         
        }

        private void connect()
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    ConnectionText.Text = "Attempting new Connection\n";
                });
                using (var btClient = new BluetoothClient())
                {
                    var btIgnored = 0;

                    // Find remembered bluetooth devices.
                    this.Dispatcher.Invoke(() =>
                    {
                        ConnectionText.Text += "Removing existing bluetooth devices...\n";
                    });

                    if (true)//checkBox_RemoveExisting.Checked
                    {
                        var btExistingList = btClient.DiscoverDevices(255, false, true, false);

                        foreach (var btItem in btExistingList)
                        {
                            if (!btItem.DeviceName.Contains("Nintendo")) continue;

                            BluetoothSecurity.RemoveDevice(btItem.DeviceAddress);
                            btItem.SetServiceState(BluetoothService.HumanInterfaceDevice, false);
                        }
                    }

                    // Find unknown bluetooth devices.                        
                    this.Dispatcher.Invoke(() =>
                    {
                        ConnectionText.Text += "Searching for bluetooth devices...\nPress the red sync button now to sync!\n";
                    });

                    var btDiscoveredList = btClient.DiscoverDevices(255, false, false, true);

                    foreach (var btItem in btDiscoveredList)
                    {
                        // Just in-case any non Wii devices are waiting to be paired.

                        if (true && !btItem.DeviceName.Contains("Nintendo")) //!checkBox_SkipNameCheck.Checked
                        {
                            btIgnored += 1;
                            continue;
                        }


                        this.Dispatcher.Invoke(() =>
                        {
                            ConnectionText.Text += "Adding: " + btItem.DeviceName + " ( " + btItem.DeviceAddress + " )\n";
                        });


                        // Install as a HID device and allow some time for it to finish.

                        btItem.SetServiceState(BluetoothService.HumanInterfaceDevice, true);
                    }

                    // Allow slow computers to finish installation before connecting.

                    System.Threading.Thread.Sleep(4000);

                    // Connect and send a command, otherwise they sleep and the device disappears.

                    try
                    {
                        if (btDiscoveredList.Length > btIgnored)
                        {
                            var deviceCollection = new WiimoteCollection();
                            deviceCollection.FindAllWiimotes();

                            foreach (var wiiDevice in deviceCollection)
                            {
                                wiiDevice.Connect();
                                wiiDevice.SetLEDs(true, false, false, false);
                                wiiDevice.Disconnect();
                            }
                            this.Dispatcher.Invoke(() =>
                            {
                                ConnectionText.Text += "Finished - You can now close this window.\n";
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                ConnectionText.Text += "Finished - No board(s) found. Please try again.\n";
                            });
                        }
                    }
                    catch (Exception) { }

                }
            }
            catch (Exception ex)
            {

                this.Dispatcher.Invoke(() =>
                {
                    ConnectionText.Text += "Error: " + ex.Message + "\n";
                });
            }
        }
    }
}
