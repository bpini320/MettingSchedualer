using AppServers.Common;
using Common.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServers.SystemMonitor
{
    internal class SystemMonitor
    {
        private MainBusClient m_Client;

        public SystemMonitor()
        {
            m_Client = new MainBusClient();
            m_Client.ConnectToMainBus("localhost", 1010);

            m_Client.SendPacket(new BaseDataPacket { PType = PacketType.StartDAL });
        }
    }
}
