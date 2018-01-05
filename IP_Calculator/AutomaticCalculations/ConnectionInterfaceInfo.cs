using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IP_Calculator.AutomaticCalculations
{
    class ConnectionInterfaceInfo
    {
        public string Hostname { get; set; }
        public IPAddress IpAddress { get; set; }
        public IPAddress SubnetMask { get; set; }
        public IPAddress DefGateway { get; set; }

        public string AllInfo
        {
            get
            {
                return string.Format("Name: {0}\r\nIP: {1}\r\nMask: {2}\r\nGateway: {3}\r\n", Hostname, IpAddress, SubnetMask, DefGateway);
            }
        }

        public override string ToString()
        {
            return Hostname;
        }
    }
}
