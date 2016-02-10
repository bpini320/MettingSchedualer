using Common.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AppServers.Common
{
    public class MainBusClient
    {
        private Socket m_Sender;
        public void ConnectToMainBus(string ip, int port)
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try
            {

                IPHostEntry ipHostInfo = Dns.Resolve(ip);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                m_Sender = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    m_Sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        m_Sender.RemoteEndPoint.ToString());

                    


                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void SendPacket(BaseDataPacket pct)
        {
            MemoryStream mem = new MemoryStream();
            pct.Serialize(mem);
            m_Sender.Send(mem.ToArray());
        }

        public void Shutdown()
        {
            m_Sender.Shutdown(SocketShutdown.Both);
            m_Sender.Close();
        }
    }
}
