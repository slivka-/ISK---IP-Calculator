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
        public IPAddress NetworkAddress { get; set; }

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

        public IPAddress GetNetworkAddress()
        {
            uint mask = ~(uint.MaxValue >> GetMaskSize());
            byte[] ipBytes = IpAddress.GetAddressBytes();
            byte[] maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();
            byte[] networkIpBytes = new byte[ipBytes.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                networkIpBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }
            return new IPAddress(networkIpBytes);
        }

        public int GetMaskSize()
        {
            int output = 0;
            var bytes = SubnetMask.GetAddressBytes();

            foreach (var b in bytes)
                foreach (char c in Convert.ToString(b, 2).PadLeft(8, '0').ToCharArray())
                    if (c.Equals('1'))
                        output++;
            return output;
        }
    }
}
