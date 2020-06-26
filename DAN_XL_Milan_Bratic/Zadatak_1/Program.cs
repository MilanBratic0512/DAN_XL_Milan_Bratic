using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        private static object locker = new object();
        private static object locker2 = new object();
        static string[] format = new string[] { "A3", "A4" };
        static string[] orientation = new string[] { "portrait", "landscape" };
        static List<string> colors = new List<string>();
        static Random rnd = new Random();
        static string path = "../../Paleta.txt";
        static EventWaitHandle auto = new AutoResetEvent(true);
        static EventWaitHandle auto2 = new AutoResetEvent(true);

        //computer names list
        static List<string> compNames = new List<string>();
        /// <summary>
        /// method for read color from the file
        /// </summary>
        static void ReadColorFromTheFile()
        {
            string[] lines = File.ReadAllLines(path);
            colors = lines.ToList();
        }
        /// <summary>
        /// method for print request
        /// </summary>
        static void Request(string format, string color, string orientation)
        {
            //rotates loop until each computer is on the list
            while (compNames.Count < 10)
            {
                
                Thread t = new Thread(() => Printer(format, color, orientation));
                t.Name = Thread.CurrentThread.Name;
                Thread.Sleep(100);
                Console.WriteLine("Computer {0} has sent request to print an {1} document format." +
                " Color: {2}, orientation: {3}", Thread.CurrentThread.Name, format, color, orientation);

                t.Start();
            }
        }
        /// <summary>
        /// method for printing
        /// </summary>
        static void Printer(string format, string color, string orientation)
        {
            //depending on the format, thread entry
            if (format == "A3")
            {
                auto.WaitOne();
                if (compNames.Count == 10)
                {
                    return;
                }
                Thread.Sleep(1000);
                Console.WriteLine("The computer {0} user can pick up the document A3 format.", Thread.CurrentThread.Name);
                auto.Set();
            }
            else
            {
                auto2.WaitOne();
                if (compNames.Count == 10)
                {
                    return;
                }
                Thread.Sleep(1000);
                Console.WriteLine("The computer {0} user can pick up the document A4 format.", Thread.CurrentThread.Name);
                auto2.Set();
            }
            //if theres no computer in the list, add it
            if (!compNames.Contains(Thread.CurrentThread.Name))
            {
                compNames.Add(Thread.CurrentThread.Name);
            }

        }
        static void Main(string[] args)
        {
            ReadColorFromTheFile();
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(() => Request(format[rnd.Next(0, 2)], colors[rnd.Next(0, colors.Count)], orientation[rnd.Next(0, 2)]));
                t.Name = "" + (i + 1);
                t.Start();
            }
            Console.ReadLine();
        }
    }
}
