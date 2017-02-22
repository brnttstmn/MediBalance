﻿using System;
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
            tunnel.start_client();
            //kinect.start_client();
            //board.start_client();
            string command = "start";
            //kinect.sendcommand(command);


            var line = "";
            var line2 = "";
            var line3 = "";
            int i = 0;
            while (i < 1000000)
            {
                //Thread.Sleep(500);
                //line2 = kinect.read.ReadLine();
                if (line2 != null) { Console.WriteLine(line2); }
                //line = board.read.ReadLine();
                if (line != null) { Console.WriteLine(line); }
                //else { Console.WriteLine("Error"); }
                line3 = tunnel.read.ReadLine();
                if (line3 != null) { Console.WriteLine(line3); }

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
            //Process.Start("C:\\Users\\dawson\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe");
            //Process.Start("C:\\Users\\dawson\\Source\\Repos\\MediBalance\\WiiBalanceWalker\\bin\\Debug\\WiiBalanceWalker.exe");
            Process.Start("C:\\Users\\dawson\\Source\\Repos\\MediBalance\\Tunnel\\bin\\Debug\\Tunnel.exe");
        }
    }
}
