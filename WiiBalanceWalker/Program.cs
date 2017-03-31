using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
//using System.Text.RegularExpressions;
//using System.Timers;
// using VJoyLibrary;
//using WiimoteLib;
namespace WiiBalanceWalker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        //    Prompt();
       
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
            
        }
        static void Prompt()
        {
            Console.WriteLine("This is the MediBalance Wii Board Beginning Sequence.");
            Console.WriteLine("Press Enter to run connection GUI.");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Application.Run(new FormMain());
            }
            else
            {
                Console.WriteLine("Try Again.");
                Console.ReadKey();
            }
            // Keep the console window open in debug mode.
          //  Console.WriteLine("Press any key to exit.");
          //  Console.ReadKey();
        }

    }
}
//hit start, runs gui for bluetooth set up and connection,
//figure out how to run the code for a set number of time
