using System;

using IP_Calculator.DataVisualization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace IP_Calculator.ManualCalculations
{
    class ManualController
    {
        public string ipAddress;

        public int hostBits;//netmask bits length

        public int hostsNumber;

        public bool forcedMinMask;

        public ObservableCollection<ManualDataRow> ManualDataCollection { get; set; }

        public ManualController()
        {
            ManualDataCollection = new ObservableCollection<ManualDataRow>();
        }

        public void CalculateManual(bool isMinNumber, bool isMinSize, int? freeAddr = null, int? minNetworks = null)
        {
            ManualDataCollection.Clear();
            if (isMinNumber)
            {
                InternalAddressRow addrRow = CalculateMinimalNetworksNum(freeAddr);
                IPCalculation ipc = new IPCalculation(addrRow.Ip, addrRow.Netmask);
                AddManualRow(ipc, addrRow.HostsNumber);
            }
            else if (isMinSize)
            {
                var addrRows = CalculateMinimalNetworksSize(freeAddr);
                foreach (var row in addrRows)
                {
                    IPCalculation ipc = new IPCalculation(row.Ip, row.Netmask);
                    AddManualRow(ipc, row.HostsNumber);
                }
            }
            
        }

        private List<InternalAddressRow> CalculateMinimalNetworksSize(int? freeAddr)
        {
            double freePercent = (freeAddr != null) ? ((double)freeAddr/100) : 0.0;
            List<InternalAddressRow> output = new List<InternalAddressRow>();

            var netmasks = GetMinimalSizes(freePercent);

            uint baseIp = ByteArr2Uint(Text2byte(ipAddress));

            foreach(var netmask in netmasks)
            {
                output.Add(new InternalAddressRow()
                {
                    Ip = new InternetProtocolAddress(baseIp),
                    Netmask = Byte.Parse((32 - (int)Math.Log(netmask, 2)).ToString()),
                    HostsNumber = netmask - 3 - (int)(netmask * freePercent)
                });
                baseIp += (uint)netmask;
            }
            return output;
        }

        private InternalAddressRow CalculateMinimalNetworksNum(int? freeAddr)
        {
            int networkSize = 1;
            while (networkSize < hostsNumber)
                networkSize *= 2;

            byte netmask = Byte.Parse((32 - (int)Math.Log(networkSize, 2)).ToString());

            byte[] octets = Text2byte(ipAddress);
            return new InternalAddressRow()
            {
                Ip = new InternetProtocolAddress(octets[0], octets[1], octets[2], octets[3]),
                Netmask = netmask,
                HostsNumber = hostsNumber
            };

        }

        private byte[] Text2byte(string ipAddress)
        {
            String[] ip = (ipAddress).Split('.');
            byte[] octets = new byte[4];

            octets[0] = byte.Parse(ip[0]);
            octets[1] = byte.Parse(ip[1]);
            octets[2] = byte.Parse(ip[2]);
            octets[3] = byte.Parse(ip[3]);

            return octets;
        }

        private uint ByteArr2Uint(byte[] arr)
        {
            if (BitConverter.IsLittleEndian)
                return BitConverter.ToUInt32(arr.Reverse().ToArray(), 0);
            else
                return BitConverter.ToUInt32(arr,0);
        }

        private List<int> GetMinimalSizes(double freeAddr)
        {
            var output = new List<int>();
            int tempHosts = hostsNumber;
            int firstMask = 0;
            if (forcedMinMask)
            {
                firstMask = (int)Math.Pow(2, (32 - hostBits));
                output.Add(firstMask);
                tempHosts -= (firstMask - 3) - (int)(firstMask * freeAddr);
            }
            while (tempHosts > 0)
            {
                int networkSize = 1;
                while (networkSize < tempHosts+3)
                    networkSize *= 2;
                if (forcedMinMask)
                    while (networkSize > firstMask)
                        networkSize /= 2;
                if (networkSize != tempHosts + 3)
                    networkSize /= 2;

                output.Add(networkSize);
                if (networkSize != tempHosts + 3)
                    tempHosts -= (networkSize - 3) - (int)(networkSize * freeAddr);
                else
                    tempHosts -= (networkSize - 3);
            }
            return output;
        }


        private void AddManualRow(IPCalculation ipc, int usedHostsNumber)
        {
            int lp = ManualDataCollection.Count + 1;
            ManualDataCollection.Add(new ManualDataRow()
            {
                Id = lp,
                ShowBinary = false,

                IpAddress = ipc.getNetworkAddress().ToString(),
                BinaryIpAddress = ipc.getNetworkAddress().ToBinaryString(),

                NetMask = ipc.getNetmask().ToString(),
                BinaryNetMask = ipc.getNetmask().ToBinaryString(),

                HostMin = ipc.getfirstAddress().ToString(),
                BinaryHostMin = ipc.getfirstAddress().ToBinaryString(),

                HostMax = ipc.getLastAddress().ToString(),
                BinaryHostMax = ipc.getLastAddress().ToBinaryString(),

                Broadcast = ipc.getBroadcastAddress().ToString(),
                BinaryBroadcast = ipc.getBroadcastAddress().ToBinaryString(),

                Wildcard = ipc.getWildcard().ToString(),
                BinaryWildcard = ipc.getWildcard().ToBinaryString(),

                HostsNum = ipc.getHostnumber().ToString(),

                HostAddressSize = ipc.getHostBits().ToString(),

                NetAddressSize = ipc.getNetworkBits().ToString(),

                FreeHosts = (ipc.getHostnumber() - (uint)usedHostsNumber).ToString()
            }
            );
        }

        private class InternalAddressRow
        {
            public InternetProtocolAddress Ip { get; set; }
            public byte Netmask { get; set; }
            public int HostsNumber { get; set; }
        }
    }
}
