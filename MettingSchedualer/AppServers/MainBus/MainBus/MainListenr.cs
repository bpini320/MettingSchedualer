using Common.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AppServers.MainBus
{
    internal class MainListenr
    {
        public void StartListening(string ip,int port)
        {
            string data = null;
            byte[] bytes = new Byte[1024];

            IPHostEntry ipHostInfo = Dns.Resolve(ip);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    data = null;

                    while (true)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        
                        if (bytesRec > 0)
                        {
                            break;
                        }
                    }

                    MemoryStream mem = new MemoryStream(bytes);
                    BaseDataPacket packet = new BaseDataPacket();
                    packet.DeSerialize(mem);

                    
                    switch (packet.PType)
                    {
                        case PacketType.StartDAL:
                            {
                                Console.WriteLine("StartDAL");
                                break;
                            }
                        case PacketType.GetInitData:
                            {
                                Console.WriteLine("GetInitData");
                                break;
                            }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
    }
}
