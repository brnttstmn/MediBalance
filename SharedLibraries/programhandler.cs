using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SharedLibraries
{
    static class programhandler
    {
        private static List<string> program_List = new List<string>()
        {
            "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\KinectEnvironment\\bin\\Debug\\KinectEnvironment.exe",
            "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\BalanceBoard\\bin\\Debug\\BalanceBoard.exe",
            "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\FrontEndUIRedux\\bin\\Debug\\FrontEndUIRedux.exe",
            "C:\\Users\\" + Environment.UserName + "\\Source\\Repos\\MediBalance\\Bridge\\bin\\Debug\\Bridge.exe"
        };

        static public List<string> programList { get { return program_List; } }

        static void stopPrograms()
        {
            char[] del = { '\\', '.' };
            Parallel.ForEach(programList, program => {

                    string name = program.Split(del)[program.Split(del).Length - 2];
                    foreach (var process in Process.GetProcessesByName(name))
                    {
                        process.Kill();
                    }
            });
        }
    }
}
