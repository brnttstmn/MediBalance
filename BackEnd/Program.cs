using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BackEnd
{
    class Program
    {
        // Instantiate Named Pipes
        static Pipe kinect = new Pipe("kinect", new NamedPipeClientStream(".", "kinect", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
        static Pipe board = new Pipe("board",new NamedPipeClientStream(".", "board", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe");
        static Pipe gui = new Pipe("gui", new NamedPipeServerStream("interface", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\FrontEndUIRedux\\bin\\Debug\\FrontEndUIRedux.exe");
        static Pipe tunnel = new Pipe("tunnel",new NamedPipeClientStream(".", "tunnel", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\Tunnel\\bin\\Debug\\Tunnel.exe");
        static List<Pipe> sensors = new List<Pipe>() { board }; //kinect, board, tunnel
        static List<Pipe> everything = new List<Pipe>() { board, gui }; //kinect, board, tunnel, gui

        static List<string> data_list = new List<String>();
        static string data;

        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            // Start Devices
            runPrograms();

            // Connect Pipes
            connectPipes();
            startSensors();
            readSensor();         

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        static void tempdemocsv()
        {
            bool status = true;
            int i = 0;
            int count = 0;
            while (status == true)
            {
                //Thread.Sleep(1000);
                if (!kinect.read.EndOfStream && !string.IsNullOrWhiteSpace(kinect.read.Peek().ToString())) { data = kinect.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                if (!board.read.EndOfStream && !string.IsNullOrWhiteSpace(board.read.Peek().ToString())) { data = board.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                if (count > 50)
                {
                    if (!tunnel.read.EndOfStream && !string.IsNullOrWhiteSpace(tunnel.read.Peek().ToString())) { data = tunnel.read.ReadLine(); data_list.Add(data); Console.WriteLine(data); }
                    count = 0;
                    if (i > 40) status = false;
                    i++;
                }

                Console.WriteLine("Reading {0}", count);
                count++;
            }

            string filePath = @"C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\data.csv";
            //File.OpenWrite(filePath);
            File.AppendAllLines(filePath, data_list);
        }

        static void readSensor()
        {
            foreach (Pipe sensor in sensors)
            {
                if (!sensor.read.EndOfStream && !string.IsNullOrWhiteSpace(sensor.read.Peek().ToString()))
                {
                    data = sensor.read.ReadLine();
                    gui.write.Write(data);
                    data_list.Add(data);
                    Console.WriteLine(data);
                }
            }
        }

        static void connectPipes()
        {
            foreach (Pipe sensor in sensors)
            {
                sensor.start_client();
            }
            gui.start_server();
        }

        static void startSensors()
        {
            foreach (Pipe sensor in sensors)
            {
                sensor.sendcommand("Start");
            }
        }

        /// <summary>
        /// Start all of the programs
        /// </summary>
        static void runPrograms()
        {
            foreach (Pipe program in everything)
            {
                Process.Start(program.path);
            }
        }
    }
}
  