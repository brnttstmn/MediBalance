using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    class Pipe
    {
        public NamedPipeClientStream client;
        public string name;
        public string path;
        public StreamReader read;
        public StreamWriter write;

        public Pipe(string name, NamedPipeClientStream pipe, string path)
        {
            this.client = pipe;
            this.path = path;
            this.name = name;
        }

        public void start_client()
        {
            // Waiting for Connection
            Console.WriteLine("Waiting for connection...");

            //server.WaitForConnection();
            this.client.Connect();
            Console.WriteLine("Conected.");

            // Start Streams
            start_reader();
            start_writer();

        }

        public void stop_client(NamedPipeClientStream client)
        {
            //Close pipe client        
            client.Close();
        }

        public void start_reader()
        {
            // Instantiate Stream reader and Writers
            //var sw = new StreamWriter(client) { AutoFlush = true };
            this.read = new StreamReader(this.client);
        }

        public void start_writer()
        {
            // Instantiate Stream reader and Writers
            this.write = new StreamWriter(this.client) { AutoFlush = true };
        }

        public void sendcommand(string command)
        {
            this.write.WriteLine(command);
            Console.WriteLine("Start sent");
        }

    }
}
