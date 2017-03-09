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
using System.Linq;

namespace BackEnd
{
    class Program
    {
        // Instantiate Named Pipes
        static Pipe kinect = new Pipe("kinect", new NamedPipeClientStream(".", "kinect", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
        static Pipe board = new Pipe("board", new NamedPipeClientStream(".", "board", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe");
        static Pipe gui = new Pipe("gui", new NamedPipeServerStream("interface", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\FrontEndUIRedux\\bin\\Debug\\FrontEndUIRedux.exe");
        static Pipe tunnel = new Pipe("tunnel", new NamedPipeClientStream(".", "tunnel", PipeDirection.InOut), "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\Tunnel\\bin\\Debug\\Tunnel.exe");

        // Lists
        static List<Pipe> pipelist = new List<Pipe>() { board, gui }; //kinect, board, tunnel, gui
        static List<Pipe> sensors = pipelist.Except(new List<Pipe>() { gui }).ToList();
        static List<string> data_list = new List<String>();
        

        /// <summary>
        /// Main Program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            run();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void run()
        {
            bool run = true;
            runPrograms();
            while (run)
            {
                try
                {
                    connectPipes();
                    startSensors();
                    readSensors();
                }
                catch (IOException) { Console.WriteLine("Connection Terminated"); }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); run = false; }
                finally { disconnectPipes(); }
            }
        }

        static void tempdemocsv(string data)
        {
            bool status = true;
            int i = 0;
            int count = 0;
            while (status == true)
            {
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

        static void readSensors()
        {
                string line;
                while (true)
                {
                    foreach (Pipe sensor in sensors)
                    {
                        if (!sensor.read.EndOfStream && !string.IsNullOrWhiteSpace(sensor.read.Peek().ToString()))
                        {
                            line = sensor.read.ReadLine();
                            if (pipelist.Contains(gui)) { gui.write.WriteLine(line); }
                            data_list.Add(line);
                            Console.WriteLine(line);
                        }
                    }
                }
        }

        static void connectPipes()
        {
            foreach (Pipe pipe in pipelist)
            {
                pipe.start(pipe.pipe);
            }
        }

        static void disconnectPipes()
        {
            foreach (Pipe pipe in pipelist)
            {
                pipe.stop(pipe.pipe);
            }
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
            foreach (Pipe program in pipelist)
            {
                Process.Start(program.path);
            }
        }
    }
}
  