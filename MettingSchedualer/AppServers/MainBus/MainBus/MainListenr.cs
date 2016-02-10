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
        private class MainBusClient
        {
            public ServerType ServerType { get; set; }

            private byte[] m_Buff = new byte[256];
            private Socket m_Socekt;
            public MainBusClient(Socket client)
            {
                m_Socekt = client;

            }

            private void SetupRecieveCallback(Socket sock)
            {
                try
                {
                    AsyncCallback recieveData = new AsyncCallback(OnRecievedData);
                    sock.BeginReceive(m_Buff, 0, m_Buff.Length, SocketFlags.None, recieveData, sock);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Setup Recieve Callback failed!");
                }
            }

            public byte[] GetRecievedData(IAsyncResult ar)
            {
                int nBytesRec = 0;
                try
                {
                    nBytesRec = m_Socekt.EndReceive(ar);
                }
                catch { }
                byte[] byReturn = new byte[nBytesRec];
                Array.Copy(m_Buff, byReturn, nBytesRec);

                /*
                // Check for any remaining data and display it
                // This will improve performance for large packets 
                // but adds nothing to readability and is not essential
                int nToBeRead = m_sock.Available;
                if( nToBeRead > 0 )
                {
                    byte [] byData = new byte[nToBeRead];
                    m_sock.Receive( byData );
                    // Append byData to byReturn here
                }
                */
                return byReturn;
            }

            private void OnRecievedData(IAsyncResult ar)
            {
                // Socket was the passed in object
                Socket sock = (Socket)ar.AsyncState;

                byte[] aryRet = this.GetRecievedData(ar);

                // If no data was recieved then the connection is probably dead
                if (aryRet.Length < 1)
                {
                    Console.WriteLine("Client {0}, disconnected", m_Socekt.RemoteEndPoint);
                    m_Socekt.Close();

                    return;
                }
            }
        }

        private Dictionary<ServerType, MainBusClient> m_Clients;
        protected void OnConnectRequest(IAsyncResult ar)
        {

            Socket listener = (Socket)ar.AsyncState;
            MainBusClient newConnection = new MainBusClient(listener.EndAccept(ar));
            m_Clients.Add(newConnection.ServerType, newConnection);
            string data = null;
            byte[] bytes = new Byte[1024];

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

        public void StartListening(string ip, int port)
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

                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(new AsyncCallback(OnConnectRequest), listener);

                Console.WriteLine("Press Enter to exit");
                Console.ReadLine();
                Console.WriteLine("OK that does it! Screw you guys I'm going home...");



            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                // Clean up before we go home
                listener.Close();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

        }
    }
}
