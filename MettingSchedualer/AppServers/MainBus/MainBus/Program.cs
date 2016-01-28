using AppServers.MainBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainBus
{
    class Program
    {
        static void Main(string[] args)
        {
            MainListenr mainListenr = new MainListenr();
            string ip = "localhost";
            int port = 1010;
            if(args.Length>1)
            {
                ip = args[0];
                int.TryParse(args[1], out port);
            }
            mainListenr.StartListening(ip, port);
        }
    }
}
