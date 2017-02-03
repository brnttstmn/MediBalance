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
        static NamedPipeServerStream kServer = new NamedPipeServerStream("kinect", PipeDirection.InOut);
        static NamedPipeServerStream bServer = new NamedPipeServerStream("board", PipeDirection.InOut);

        static void Main(string[] args)
        {
            runPrograms();
            server(kServer);
        }

        static void runPrograms()
        {
            Process.Start("C:\\Users\\dcnie\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
            Process.Start("C:\\Users\\dcnie\\Source\\Repos\\MediBalance\\WiiBalanceWalker\\bin\\Debug\\WiiBalanceWalker.exe");
        }

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


            while (message != "exit")
            {
                // Poll for response
                string temp = read.ReadLine();
                if (temp != null)
                {
                    message = temp;
                    Console.WriteLine(message); // Once there is a line to read, write it to console
                }
                Thread.Sleep(500);
            }

            // Close Out Program
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            xStream.Close();
        }
    }
}
