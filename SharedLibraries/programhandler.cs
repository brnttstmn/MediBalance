using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SharedLibraries
{
    public static class programhandler
    {
        private static char[] del = { '\\', '.' };
        private static List<string> program_List = new List<string>()
        {
            "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe",
            "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe",
            //"C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\FrontEndUIRedux\\bin\\Debug\\FrontEndUIRedux.exe",
            "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BackEnd\\bin\\Debug\\BackEnd.exe"
        };

        static public List<string> programList { get { return program_List; } }

        public static void stopPrograms()
        {
            Parallel.ForEach(programList, program => {

                    string name = program.Split(del)[program.Split(del).Length - 2];
                    foreach (var process in Process.GetProcessesByName(name))
                    {
                        process.Kill();
                    }
            });
        }
        public static void stopProgramsExcept(string except)
        {
            
            Parallel.ForEach(programList, program => {
                if (program.Contains(except))
                {
                    string name = program.Split(del)[program.Split(del).Length - 2];
                    foreach (var process in Process.GetProcessesByName(name))
                    {
                        process.Kill();
                    }
                }
            });
        }

        public static void runPrograms()
        {
            Parallel.ForEach(program_List, program => {
                Process.Start(program);
            });
        }
    }
}
