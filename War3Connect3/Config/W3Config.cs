using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using War3Connect3.UI;

namespace War3Connect3.Config
{
    public class W3Config
    {

        // tested for v1.27.1.7085 CZ (byte 0x1b=27 should stand for path version)
        // 0x50, 0x58, 0x33, 0x57 is PX3W which is reversed W3XP(expansion), 
        public readonly int W3_PORT = 6112;
        public readonly byte[] W3_UDP_GAMES_QUERY = { 0xf7, 0x2f, 0x10, 0x00, 0x50, 0x58, 0x33, 0x57, 0x1b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public readonly double UDP_HEARTBEAT = TimeSpan.FromSeconds(3).TotalMilliseconds;


        // IP of the PC that is running this program
        public IPAddress LocalIP { get; set; }

        // IP of the PC that has a hosted W3 game
        public IPAddress RemoteIP { get; set; }


        public IPEndPoint localEndPoint;
        public IPEndPoint LocalEndPoint => localEndPoint ??= new IPEndPoint(LocalIP, W3_PORT);
               
               
        public IPEndPoint remoteEndPoint;
        public IPEndPoint RemoteEndPoint => remoteEndPoint ??= new IPEndPoint(RemoteIP, W3_PORT);

        public int PacketSentCount { get; set; } = 0;


        public W3Config()
        {
            ReadConfig();
            GetLocalIP();
        }


        private void ReadConfig()
        {
            IPAddress addr;
            string input = "";
            try
            {
                input = File.ReadAllText("config.txt");
            }
            catch (FileNotFoundException ex)
            {
                using (new SetColor(ConsoleColor.Red))
                    Console.Write("Config file not found,");
                using (new SetColor(ConsoleColor.DarkGray))
                    Console.WriteLine(" add file 'config.txt' with host ip address.");
                GetHostIpFromInput();
                return;
            }
            if (!String.IsNullOrEmpty(input) &&
                IPAddress.TryParse(input, out addr))
            {
                Console.WriteLine("Host ip set to " + addr.ToString());
                RemoteIP = addr;
            }
            else
            {
                using (new SetColor(ConsoleColor.Red))
                    Console.Write("Config file corrupted,");
                using (new SetColor(ConsoleColor.DarkGray))
                    Console.WriteLine(" add the host ip address to file 'config.txt'.");
                GetHostIpFromInput();
            }
        }
    

        private void GetHostIpFromInput()
        {
            bool repeat = true;
            IPAddress addr;
            while (repeat)
            {
                Console.WriteLine("Enter the Host IP Address now:");
                if (IPAddress.TryParse(Console.ReadLine().ToLower(), out addr))
                {
                    RemoteIP = addr;
                    Console.WriteLine("Host ip set to " + addr.ToString());
                    break;
                }
                else
                    using (new SetColor(ConsoleColor.Red))
                        Console.WriteLine("This is not a valid ip address");
            }
        }


        private void GetLocalIP()
        {
            using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            this.LocalIP = endPoint.Address;
        }
    }
}
