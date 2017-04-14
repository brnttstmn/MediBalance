using System.Windows;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.Threading.Tasks;

namespace FrontEndUIRedux
{
    /// <summary>
    /// Interaction logic for BandWindow.xaml
    /// </summary>
    public partial class BandWindow : Window
    {
        public BandWindow()
        {
            InitializeComponent();
        }

        private void BandConnect_Click(object sender, RoutedEventArgs e)
        {
            BandConnect.IsEnabled = false;
            Task.Run(() =>
            {
                connect();
                this.Dispatcher.Invoke(() =>
                {
                    BandConnect.IsEnabled = true;
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
                            if (!btItem.DeviceName.Contains("MSFT Band")) continue;

                            BluetoothSecurity.RemoveDevice(btItem.DeviceAddress);
                            btItem.SetServiceState(BluetoothService.HumanInterfaceDevice, false);
                        }
                    }

                    // Find unknown bluetooth devices.                        
                    this.Dispatcher.Invoke(() =>
                    {
                        ConnectionText.Text += "Searching for bluetooth devices...\nMake sure band is in pair mode!\n";
                    });

                    var btDiscoveredList = btClient.DiscoverDevices(255, false, false, true);

                    foreach (var btItem in btDiscoveredList)
                    {
                        // Just in-case any non Wii devices are waiting to be paired.

                        if (true && !btItem.DeviceName.Contains("MSFT Band")) //!checkBox_SkipNameCheck.Checked
                        {
                            btIgnored += 1;
                            continue;
                        }


                        this.Dispatcher.Invoke(() =>
                        {
                            ConnectionText.Text += "Adding: " + btItem.DeviceName + " ( " + btItem.DeviceAddress + " )\n";
                        });


                        // Install as a HID device and allow some time for it to finish.
                        //BluetoothSecurity.PairRequest(btItem.DeviceAddress, DEVICE_PIN);
                        btItem.SetServiceState(BluetoothService.HumanInterfaceDevice, true);
                    }

                    // Allow slow computers to finish installation before connecting.

                    System.Threading.Thread.Sleep(4000);

                    // Connect and send a command, otherwise they sleep and the device disappears.

                    try
                    {
                        if (btDiscoveredList.Length > btIgnored)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                ConnectionText.Text += "Finished - You can now close this window.\n";
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                ConnectionText.Text += "Finished - No band(s) found. Please try again.\n";
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
