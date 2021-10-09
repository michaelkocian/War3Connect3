using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using War3Connect3.UI;

namespace War3Connect3.Config
{
    public class W3Config
    {

        // tested for v1.27.1.7085 CZ (byte 0x1b=27 should stand for path version)
        // 0x50, 0x58, 0x33, 0x57 is PX3W which is reversed W3XP(expansion), 
        public readonly int W3_PORT = 6112;
        public readonly byte[] UDP_W3_GAMES_QUERY_1_27 = { 0xf7, 0x2f, 0x10, 0x00, 0x50, 0x58, 0x33, 0x57, 0x1b, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public readonly byte[] UDP_W3_GAMES_QUERY_1_29 = { 0xf7, 0x2f, 0x10, 0x00, 0x50, 0x58, 0x33, 0x57, 0x1d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public readonly double UDP_SEND_INTERVAL = TimeSpan.FromSeconds(3).TotalMilliseconds;

        public byte[] UDP_W3_GAMES_QUERY = null;


        public void SetW3Query(byte verByte) => UDP_W3_GAMES_QUERY = new byte[] { 0xf7, 0x2f, 0x10, 0x00, 0x50, 0x58, 0x33, 0x57, verByte, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };


        // IP of the PC that is running this program
        private IPAddress LocalIP { get; set; }

        // IP of the PC that has a hosted W3 game
        private IPAddress RemoteIP { get; set; }


        private IPEndPoint localEndPoint;
        public IPEndPoint LocalEndPoint => localEndPoint ??= new IPEndPoint(LocalIP, W3_PORT);


        private IPEndPoint remoteEndPoint;
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
            int w3ver = GetW3Version();
            SetW3Query((byte)w3ver);
            try
            {
                input = File.ReadAllText("config.txt");
            }
            catch (FileNotFoundException)
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

        private int GetW3Version()
        {
            int version;
            while (true)
            {
                Console.WriteLine("Enter you warcraft3 version (1.07, 1.08, .., 1.30)");
                string ver = Console.ReadLine().ToLower();

                if (ver.Length == 4 && ver.StartsWith("1."))
                {
                    if (int.TryParse(ver.Substring(2), out version))
                    {
                        return version;
                    }
                }
                using (new SetColor(ConsoleColor.Red))
                    Console.WriteLine("This is not a supported w3 version");
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
