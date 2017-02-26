using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace MediBalance
{
    public class Tcp_Client
    {

        StreamSocket s = null;

        public void close()
        {
            s.Dispose();
        }

        public void create_socket()
        {
            if (s == null)
            {
                s = new StreamSocket();
            }
            else
            {
                close();
                s = new StreamSocket();
            }
        }

        public async void connect(string ip)
        {
            HostName hostName = new HostName(ip);
            string port = "8001";

            try
            {
                // Connect to the server
                await s.ConnectAsync(hostName, port);
            }
            catch (Exception exception)
            {
                switch (SocketError.GetStatus(exception.HResult))
                {     
                    case SocketErrorStatus.HostNotFound:
                        return;
                        throw;
                    default:
                        return;

                }

            }
        }

        //public async Task writer(DataWriter outstream)
        //{

        //} 


        public async Task<string> sendit(string host, string port, string message)
        {


            HostName hostName;
            string response_from_server;

            using (s = new StreamSocket())
            {
                hostName = new HostName(host);

                // Set NoDelay to false so that the Nagle algorithm is not disabled
                s.Control.NoDelay = false;

                try
                {
                    // Connect to the server
                    await s.ConnectAsync(hostName, port);

                    // Send the message
                    await this.send(message);

                    // Read response
                    response_from_server = await this.read();
                    //s.Close();
                    return response_from_server;

                }
                catch (Exception exception)
                {
                    switch (SocketError.GetStatus(exception.HResult))
                    {
                        case SocketErrorStatus.HostNotFound:
                            return "error Host not Found";
                            throw;
                        default:
                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
                            throw;
                    }
                }
            }

        }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
        public async Task send(string message)
        {
            DataWriter writer;

            // Create the data writer object backed by the in-memory stream. 
            using (writer = new DataWriter(s.OutputStream))
            {
                // Set the Unicode character encoding for the output stream
                writer.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                // Specify the byte order of a stream.
                writer.ByteOrder = ByteOrder.LittleEndian;

                // Gets the size of UTF-8 string.
                writer.MeasureString(message);
                // Write a string value to the output stream.
                writer.WriteString(message);

                // Send the contents of the writer to the backing stream.
                try
                {
                    await writer.StoreAsync();
                }
                catch (Exception exception)
                {
                    switch (SocketError.GetStatus(exception.HResult))
                    {
                        case SocketErrorStatus.HostNotFound:
                            // Handle HostNotFound Error
                            throw;
                        default:
                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
                            throw;
                    }
                }

                await writer.FlushAsync();
                // In order to prolong the lifetime of the stream, detach it from the DataWriter
                writer.DetachStream();
            }
        }


        public async Task<String> read()
        {
            DataReader reader;
            StringBuilder strBuilder;

            using (reader = new DataReader(s.InputStream))
            {
                strBuilder = new StringBuilder();

                // Set the DataReader to only wait for available data (so that we don't have to know the data size)
                reader.InputStreamOptions = Windows.Storage.Streams.InputStreamOptions.Partial;
                // The encoding and byte order need to match the settings of the writer we previously used.
                reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                reader.ByteOrder = Windows.Storage.Streams.ByteOrder.LittleEndian;

                // Send the contents of the writer to the backing stream. 
                // Get the size of the buffer that has not been read.
                await reader.LoadAsync(256);

                // Keep reading until we consume the complete stream.
                while (reader.UnconsumedBufferLength > 0)
                {
                    strBuilder.Append(reader.ReadString(reader.UnconsumedBufferLength));
                    await reader.LoadAsync(256);
                }

                reader.DetachStream();
                return strBuilder.ToString();
            }
        }





    }



}
