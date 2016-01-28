using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataModel
{
    public enum ServerType
    {
        SystemMonitor = 1,
        MainBus =2,
        DAL = 3
    }
    public enum PacketType
    {
        StartDAL = 1,
        GetInitData =2
    }

    public class BaseDataPacket 
    {
        public PacketType PType { get; set; }
        public ServerType DestentionType { get; set; }

        public virtual void Serialize(MemoryStream stm)
        {
            stm.WriteByte((byte)PType);
            stm.WriteByte((byte)DestentionType);
        }

        public virtual void DeSerialize(MemoryStream stm)
        {
            PType = (PacketType)stm.ReadByte();
            DestentionType = (ServerType)stm.ReadByte();
        }
    }
}
