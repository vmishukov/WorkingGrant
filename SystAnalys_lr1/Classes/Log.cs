using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SystAnalys_lr1
{
    public class Log
    {
        private static object sync = new object();
        public static string filename;
    

        public Log()
        {
            filename = string.Format("Log/{0:yyyy-MM-dd_HH.mm.ss}_tournament.log", DateTime.Now);
        }

        public Log(string postfix) 
        {
            filename = string.Format("Log/{0:yyyy-MM-dd_HH.mm.ss}_{1}.log", DateTime.Now, postfix);
        }

        public static void WriteLog(String str)
        {
            try
            {
                //string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                //if (!Directory.Exists(pathToLog))  Directory.CreateDirectory(pathToLog);
                    string fullText = string.Format("[{0:HH:mm:ss}] [{1}]\r\n", DateTime.Now, str);
                       File.AppendAllText(filename, fullText);
         

            }
            catch (Exception e)
            {
               Console.WriteLine("Log error" + e.Message);
            }
            
        }
    }
}
