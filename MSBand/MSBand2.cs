using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Band;
using System.Threading.Tasks;
using System.Collections;

namespace MediBalance
{
    class MSBand2
    {
        string timeFormat = "HH:mm:ss:fff";
        private IBandClient bandClient = null;

        public async Task<int> everything(int RunTime, TextBlock OutputText, Tcp_Client tcpClient)
        {
            bool connected = true;
            OutputText.Visibility = Visibility.Visible;
            try
            {
                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (pairedBands.Length < 1)
                {
                    return -1;
                }
                // Connect to Microsoft Band.
                using (bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {

                        bool heartRateConsentGranted;
                        // Check whether the user has granted access to the HeartRate sensor.
                        if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() == UserConsent.Granted)
                        { heartRateConsentGranted = true; }
                        else { heartRateConsentGranted = await bandClient.SensorManager.HeartRate.RequestUserConsentAsync(); }
                        if (!heartRateConsentGranted) { return -1; }

                        bool gsrConsentGranted;
                        // Check whether the user has granted access to the HeartRate sensor.
                        if (bandClient.SensorManager.Gsr.GetCurrentUserConsent() == UserConsent.Granted)
                        { gsrConsentGranted = true; }
                        else { gsrConsentGranted = await bandClient.SensorManager.Gsr.RequestUserConsentAsync(); }
                        if (!gsrConsentGranted) { return -1; }

                        // Subscribe to HeartRate data.
                        bandClient.SensorManager.HeartRate.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                var data_string = string.Format("{0},Heartrate,{1,0:D3};", DateTime.Now.ToString(timeFormat), args.SensorReading.HeartRate);

                                OutputText.Text = data_string;
                                try { await tcpClient.send(string.Format(data_string)); }
                                catch (Exception) { connected = false; OutputText.Text = "TCP Disconnected"; return; }
                            });
                        };
                        await bandClient.SensorManager.HeartRate.StartReadingsAsync();

                        // Subscribe to GSR data.
                        bandClient.SensorManager.Gsr.ReadingChanged += async (s, args) =>
                        {
                            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                var data_string = string.Format("{1},gsr,{0,0:D9};", args.SensorReading.Resistance, DateTime.Now.ToString(timeFormat));
                                OutputText.Text = data_string;
                                try { await tcpClient.send(string.Format(data_string)); }
                                catch (Exception) { connected = false; OutputText.Text = "TCP Disconnected"; return; }
                            });
                        };
                        await bandClient.SensorManager.Gsr.StartReadingsAsync();



                    // Run time to collect samples
                    //await Task.Delay(TimeSpan.FromSeconds(RunTime));
                    while (connected) { await Task.Delay(5); }
                    OutputText.Text = "Band Time Complete";


                    // Shut off Sensors
                    await bandClient.SensorManager.HeartRate.StopReadingsAsync();
                    await bandClient.SensorManager.Gsr.StopReadingsAsync();
                    return 0;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }

}