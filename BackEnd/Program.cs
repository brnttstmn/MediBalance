using System;
using System.Collections.Generic;
using System.IO;
//using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
using System.Threading;
//using System.Threading.Tasks;
using System.Diagnostics;

namespace BackEnd
{
    class Program
    {
        // Instantiate Named Pipes
        static Pipe kinect = new Pipe(new NamedPipeClientStream(".", "kinect", PipeDirection.InOut));
        static Pipe board = new Pipe(new NamedPipeClientStream(".", "board", PipeDirection.InOut));
        static Pipe gui = new Pipe(new NamedPipeClientStream(".", "interface", PipeDirection.InOut));
        static Pipe tunnel = new Pipe(new NamedPipeClientStream(".", "tunnel", PipeDirection.InOut));

        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            // Start Devices
            runPrograms();

            // Connect Pipes
            board.start_client();
            kinect.start_client();
            tunnel.start_client();
            string command = "start";
            kinect.sendcommand(command);

            bool status = true;
            List<string> data_list = new List<String>();

            string data;
            int count = 0;
            int i = 0;
            while (status ==  true)
            {
                //Thread.Sleep(1000);
                if (!kinect.read.EndOfStream && !string.IsNullOrWhiteSpace(kinect.read.Peek().ToString())) { data = kinect.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                if (!board.read.EndOfStream && !string.IsNullOrWhiteSpace(board.read.Peek().ToString())) { data = board.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                if (count > 50)
                {
                    if (!tunnel.read.EndOfStream && !string.IsNullOrWhiteSpace(tunnel.read.Peek().ToString())) { data = tunnel.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                    count = 0 ;
                    if (i > 40) status = false;
                    i++;
                }
                
                Console.WriteLine("Reading {0}",count);
                count++;
            }

            string filePath = @"C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\data.csv";
            //File.OpenWrite(filePath);
            File.AppendAllLines(filePath, data_list);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        /// <summary>
        /// Start all of the programs
        /// </summary>
        static void runPrograms()
        {
            Process.Start("C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
            Process.Start("C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe");
            Process.Start("C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\Tunnel\\bin\\Debug\\Tunnel.exe");
        }
    }
}
  