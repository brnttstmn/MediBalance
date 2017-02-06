using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BackEnd
{
    class Program
    {
        // Instantiate Named Pipes
        static NamedPipeServerStream kServer = new NamedPipeServerStream("tokinect", PipeDirection.InOut);
        static NamedPipeClientStream kClient = new NamedPipeClientStream(".", "fromkinect", PipeDirection.InOut);
        static NamedPipeServerStream bServer = new NamedPipeServerStream("board", PipeDirection.InOut);
        static NamedPipeClientStream bClient = new NamedPipeClientStream(".", "fromboard", PipeDirection.InOut);

        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Start Devices
            runPrograms();

            // Start Servers
            server(kServer, kClient);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Start all of the programs
        /// </summary>
        static void runPrograms()
        {
            Process.Start("C:\\Users\\dcnie\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
            //Process.Start("C:\\Users\\dcnie\\Source\\Repos\\MediBalance\\WiiBalanceWalker\\bin\\Debug\\WiiBalanceWalker.exe");
        }

        /// <summary>
        /// server: takes a stream and makes it a listening server
        /// </summary>
        /// <param name="xStream"></param>
        static void server(NamedPipeServerStream server, NamedPipeClientStream client)
        {
            // Setup Objects
            var message = "start";
            var s = new Stopwatch();

            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");
            server.WaitForConnection();
            client.Connect();
            Console.WriteLine("Conected.");

            // Instantiate Stream reader and Writers
            var sw = new StreamWriter(server) { AutoFlush = true };
            var sr = new StreamReader(client);

            sw.WriteLine(message);

            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(15))
            {
                var line = sr.ReadLine();
                if (message != null) { Console.WriteLine(line); }         
            }

            // Close Out Connection
            client.Close();
            server.Close();
        }
        
    }
}
