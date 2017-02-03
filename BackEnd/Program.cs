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
        static NamedPipeServerStream kServer = new NamedPipeServerStream("kinect", PipeDirection.InOut);
        static NamedPipeServerStream bServer = new NamedPipeServerStream("board", PipeDirection.InOut);

        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            runPrograms();
            server(kServer);
        }

        /// <summary>
        /// Start all of the programs
        /// </summary>
        static void runPrograms()
        {
            Process.Start("C:\\Users\\dcnie\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
            Process.Start("C:\\Users\\dcnie\\Source\\Repos\\MediBalance\\WiiBalanceWalker\\bin\\Debug\\WiiBalanceWalker.exe");
        }

        /// <summary>
        /// server: takes a stream and makes it a listening server
        /// </summary>
        /// <param name="xStream"></param>
        static void server(NamedPipeServerStream xStream)
        {
            // Setup Objects
            var message = "";

            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");
            xStream.WaitForConnection();
            Console.WriteLine("Conected!");

            // Instantiate Stream reader and Writers
            var write = new StreamWriter(xStream) { AutoFlush = true };
            var read = new StreamReader(xStream);

            message = Console.ReadLine();
            write.WriteLine(message);

            // Close Out Program
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            xStream.Close();
        }
    }
}
