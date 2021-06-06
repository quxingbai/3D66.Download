using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _3D66.Download.Tools
{
    public class SocketTool
    {
        public static Socket STCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public static String ServerTarget = "192.168.0.255:5556";
        static SocketTool()
        {
            //IPAddress ip = Dns.GetHostAddresses(Dns.GetHostName()).Where(w => w.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();
            //STCP.Bind(IPEndPoint.Parse(ip.ToString()));
            STCP.Connect(IPEndPoint.Parse(ServerTarget));
        }
        public static void SendImageByte(byte[] bs)
        {
            var j = JsonConvert.SerializeObject(new
            {
                Header = PCTool.GetPCUserName(),
                Image = bs,
            });
            //STCP.SendTo(Encoding.UTF8.GetBytes(j), IPEndPoint.Parse(ServerTarget));
            STCP.Send(Encoding.UTF8.GetBytes(j));
        }
    }
}
