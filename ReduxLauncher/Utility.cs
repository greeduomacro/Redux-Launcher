using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class LogHandler
    {
        public static string PacketLog
        {
            get
            {
                string path = new FileInfo
                    (System.Reflection.Assembly.GetEntryAssembly().Location).Directory.ToString();
 
                if (!path.EndsWith("\\"))
                    path += "\\";

                path += "packets.log";

                return path;
            }
        }

        public static string ErrorLog
        {
            get
            {
                string path = new FileInfo
                    (System.Reflection.Assembly.GetEntryAssembly().Location).Directory.ToString();

                if (!path.EndsWith("\\"))
                    path += "\\";

                path += "errors.log";

                return path;
            }
        }

        public static string GlobalErrorLog
        {
            get
            {
                string path = new FileInfo
                    (System.Reflection.Assembly.GetEntryAssembly().Location).Directory.ToString();

                if (!path.EndsWith("\\"))
                    path += "\\";

                path += "unhandled.log";

                return path;
            }
        }

        public static void LogPackets(string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(PacketLog, true))
                {
                    writer.WriteLine("[{0:G}] {1}", DateTime.Now, text);
                    writer.WriteLine("/!/\n");
                    writer.Flush(); writer.Close();
                }
            }

            catch { }
        }

        public static void LogErrors(string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ErrorLog, true))
                {
                    writer.WriteLine("[{0:G}] {1}", DateTime.Now, text);
                    writer.WriteLine("/!/\n");
                    writer.Flush(); writer.Close();
                }
            }

            catch { }
        }

        public static void LogGlobalErrors(string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(GlobalErrorLog, true))
                {
                    writer.WriteLine("[{0:G}] {1}", DateTime.Now, text);
                    writer.WriteLine("/!/\n");
                    writer.Flush(); writer.Close();
                }
            }

            catch { }
        }
    }
}
