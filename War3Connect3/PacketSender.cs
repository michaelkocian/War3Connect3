using System;
using System.Net.Sockets;
using System.Timers;
using War3Connect3.Config;
using War3Connect3.UI;

namespace War3Connect3
{
    public class PacketSender
    {
        private readonly W3Config config;

        public PacketSender(W3Config config)
        {
            this.config = config;
            InitTimer();
        }

        private void InitTimer()
        {
            Timer t = new Timer(config.UDP_SEND_INTERVAL);
            t.Elapsed += T_Elapsed;
            t.Start();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                config.PacketSentCount++;
                using (Socket s = new Socket(SocketType.Dgram, ProtocolType.Udp))
                {
                    s.Bind(config.LocalEndPoint);
                    var GameRequestBytes = config.UDP_W3_GAMES_QUERY;
                    s.SendTo(GameRequestBytes, GameRequestBytes.Length, SocketFlags.None, config.RemoteEndPoint);
                }
                Console.WriteLine($"packet {config.PacketSentCount} sent");
            }
            catch (Exception ex)
            {
                using (new SetColor(ConsoleColor.Red))
                    Console.WriteLine(ex.Message);
            }
        }



    }
}