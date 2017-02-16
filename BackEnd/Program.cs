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
        //static NamedPipeServerStream kServer = new NamedPipeServerStream("tokinect", PipeDirection.InOut);
        static NamedPipeClientStream kClient = new NamedPipeClientStream(".", "kinect", PipeDirection.InOut);
        //static NamedPipeServerStream bServer = new NamedPipeServerStream("board", PipeDirection.InOut);
        //static NamedPipeClientStream bClient = new NamedPipeClientStream(".", "fromboard", PipeDirection.InOut);

        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Thread.Sleep(1000);

            StreamReader sr;
            StreamWriter sw;

            // Start Devices
            runPrograms();

            // Start Servers
            start_client(kClient);
            sr = start_reader(kClient);
            sw = start_writer(kClient);

            string command = "start";
            sendcommand(sw,command);


            var line = "";
            int i = 0;
            while (i < 1000000)
            {
                //Thread.Sleep(500);
                line = sr.ReadLine();
                if (line != null) { Console.WriteLine(line); }
                //else { Console.WriteLine("Error"); }
                Console.WriteLine("Reading ",i);
                i++;
            }



            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Start all of the programs
        /// </summary>
        static void runPrograms()
        {
            Process.Start("C:\\Users\\dawson\\Source\\Repos\\NewRepo\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
            //Process.Start("C:\\Users\\dcnie\\Source\\Repos\\MediBalance\\WiiBalanceWalker\\bin\\Debug\\WiiBalanceWalker.exe");
        }

        /// <summary>
        /// server: takes a stream and makes it a listening server
        /// </summary>
        /// <param name="xStream"></param>
        static void start_client(NamedPipeClientStream client)
        {
            // Setup Objects
            //var message = "start";
            //var s = new Stopwatch();


            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");
            //server.WaitForConnection();
            client.Connect();
            Console.WriteLine("Conected.");

            // Instantiate Stream reader and Writers
            //var sw = new StreamWriter(client) { AutoFlush = true };
            //var sr = new StreamReader(client);

            //Console.WriteLine("Streams Created");
            /*
            sw.WriteLine(message);

            Console.WriteLine("Start sent");

            var line = "";
            s.Start();
            while (line != null)
            {
                //Thread.Sleep(500);
                line = sr.ReadLine();
                if (line != null) { Console.WriteLine(line); }
                //else { Console.WriteLine("Error"); }
                Console.WriteLine("Reading");         
            }

            //line = sr.ReadLine();
            //if (message != null) { Console.WriteLine(line); }
            
             * //*
            Console.WriteLine("Last Read on key press");
            Console.ReadKey();

            var line1 = " ";
            while (line1 != "")
            {
                line1 = sr.ReadLine();
                if (message != null) { Console.WriteLine(line1); }
            }
            Console.WriteLine("close pipes");
            Console.ReadKey();
            */

            // Close Out Connection
            //client.Close();
            //server.Close();

            
        }

        static void stop_client(NamedPipeClientStream client)
        {
            //Close pipe client        
            client.Close();
        }    


        static StreamReader start_reader(NamedPipeClientStream client)
        {
            // Instantiate Stream reader and Writers
            //var sw = new StreamWriter(client) { AutoFlush = true };
            var sr = new StreamReader(client);
            Console.WriteLine("Reader Stream Created");
            return sr;


        }

        static StreamWriter start_writer(NamedPipeClientStream client)
        {
            // Instantiate Stream reader and Writers
            var sw = new StreamWriter(client) { AutoFlush = true };
            //var sr = new StreamReader(client);
            Console.WriteLine(" Writer Stream Created");
            return sw;
        }

        static void sendcommand(StreamWriter sw , string command)
        {
            sw.WriteLine(command);
            Console.WriteLine("Start sent");
        }


    }
}
