﻿using System;
using War3Connect3.Config;
using War3Connect3.UI;

namespace War3Connect3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "War3Connect3 - LAN utility";
            Console.WriteLine("This is an app that can connect warcraft 3 lan over the internet if you have your ports properly forwarded.");

            var p = new Program();

           p.PreventExit();
        }

        public void PreventExit()
        {
            using (new SetColor(ConsoleColor.DarkGray))
                Console.WriteLine("Type 'exit' to terminate this app.");
            while (true)
                if (Console.ReadLine().ToLower() == "exit")
                    break;
        }


        W3Config config;

        public Program()
        {
            config = new Config.W3Config();

            new PacketSender();


            using (new SetColor(ConsoleColor.Green))
                Console.WriteLine("Start Warcraft 3 -> lan");
        }


    }
}
