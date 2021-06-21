using System;
using System.Diagnostics;
using System.IO;

namespace VeryStupidLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Release", "Infart.WindowsDesktop.exe"));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
