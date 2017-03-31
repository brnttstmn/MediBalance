﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace SharedLibraries
{
    public class Pipe
    {
        // Private
        private NamedPipeClientStream client = null;
        private NamedPipeServerStream server = null;
        private StreamReader streamRead;
        private StreamWriter streamWrite;
        private Thread readWriteThread;
        private bool active;

        // Public
        public bool isClient { get; }
        public bool isStarted { get { return active; } }
        public string name { get; }
        public string path { get; }

        // Accessors
        public StreamReader read { get { return streamRead; } }
        public StreamWriter write { get { return streamWrite; } }
        public Thread thread { get { return readWriteThread; } }

        // Constructors
        public Pipe(string name, bool isClient, string path)
        {
            this.isClient = isClient;
            this.path = path;
            this.name = name;
            active = false;
        }
        public Pipe(string name, bool isClient)
        {
            this.isClient = isClient;
            this.name = name;
            active = false;
        }

        // Public Methods
        public void start()
        {
            if (!active)
            {
                if (!isClient)
                {
                    startServer();
                }
                else
                {
                    startClient();
                }
                active = true;
            }
        }
        public void stop()
        {
            if (active)
            {
                try
                {
                    if (!isClient) { server.Close(); server = null; }
                    else { client.Close(); client = null; }
                }
                catch (NullReferenceException) { }
                Console.WriteLine("Disconnected: " + name);
                active = false;
            }
        }
        public void sendcommand(string command)
        {
            this.write.WriteLine(command);
            Console.WriteLine("sent: " + command);
        }
        public void startThread(ThreadStart start)
        {
            readWriteThread = new Thread(start);
        }
        public void stopThread(ThreadStart thread)
        {
            readWriteThread.Abort();
        }

        // Public Static Methods
        public static void disconnectPipes(Pipe[] pipelist)
        {
            System.Threading.Tasks.Parallel.ForEach(pipelist, pipe => {
                pipe.stop();
            });
        }
        public static void disconnectPipes(System.Collections.Generic.List<Pipe> pipelist)
        {
            System.Threading.Tasks.Parallel.ForEach(pipelist, pipe => {
                pipe.stop();
            });
        }
        public static void connectPipes(Pipe[] pipelist)
        {
            System.Threading.Tasks.Parallel.ForEach(pipelist, pipe => {
                pipe.start();
            });
        }
        public static void connectPipes(System.Collections.Generic.List<Pipe> pipelist)
        {
            System.Threading.Tasks.Parallel.ForEach(pipelist, pipe => {
                pipe.start();
            });
        }

        //Private Methods
        /// <summary>
        /// Start client
        /// </summary>
        private void startClient()
        {
                Console.WriteLine("Connecting to " + name + "...");
                client = new NamedPipeClientStream(".", name, PipeDirection.InOut);
                client.Connect();
                Console.WriteLine("Connected to " + name + ".");
                streamRead = new StreamReader(client);
                streamWrite = new StreamWriter(client) { AutoFlush = true };
        }
        private void startServer()
        {
            try
            {
                Console.WriteLine("Connecting to " + name + "...");
                server = new NamedPipeServerStream(name, PipeDirection.InOut);
                server.WaitForConnection();
                Console.WriteLine("Connected to " + name + ".");
                streamRead = new StreamReader(server);
                streamWrite = new StreamWriter(server) { AutoFlush = true };
            }
            catch (IOException) { stop(); startServer(); }
        }
    }
}

