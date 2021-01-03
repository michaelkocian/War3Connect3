using System;

namespace War3Connect3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Warcraft 3 Connect 3";
            Console.WriteLine("This is an app that can connect warcraft 3 lan over the internet if you have your ports properly forwarded.");

            var config = new Config.W3Config();
        }
    }
}
