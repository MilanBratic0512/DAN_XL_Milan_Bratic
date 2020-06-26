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

        static List<string> compNames = new List<string>();
        static void ReadColorFromTheFile()
        {
            string[] lines = File.ReadAllLines(path);
            colors = lines.ToList();
        }
        static void Request(string format, string color, string orientation)
        {
            while (compNames.Count < 10)
            {
                if (compNames.Count == 10)
                {
                    return;
                }
                Thread t = new Thread(() => Printer(format, color, orientation));
                t.Name = Thread.CurrentThread.Name;
                Thread.Sleep(100);
                Console.WriteLine("Computer {0} has sent request to print an {1} document format." +
                " Color: {2}, orientation: {3}", Thread.CurrentThread.Name, format, color, orientation);

                t.Start();
            }
        }
        static void Printer(string format, string color, string orientation)
        {
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
