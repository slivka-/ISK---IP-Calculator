using System;

using IP_Calculator.DataVisualization;
using System.Collections.ObjectModel;

namespace IP_Calculator.ManualCalculations
{
    class ManualController
    {
        public string ipAddress;

        public string hostBits;

        public ObservableCollection<ManualDataRow> ManualDataCollection { get; set; }

        public ManualController()
        {
            ManualDataCollection = new ObservableCollection<ManualDataRow>();
        }

        public void CalculateManual()
        {
            ManualDataCollection.Clear();
            byte[] octets = Text2byte(ipAddress);
            InternetProtocolAddress ip = new InternetProtocolAddress(octets[0], octets[1], octets[2], octets[3]);
            IPCalculation ipc = new IPCalculation(ip, byte.Parse(hostBits));
            AddManualRow(ipc);
        }

        private byte[] Text2byte(string ipAddress)
        {
            String[] ip = (ipAddress).Split(new Char[] { ',', '.' });
            byte[] octets = new byte[4];

            octets[0] = byte.Parse(ip[0]);
            octets[1] = byte.Parse(ip[1]);
            octets[2] = byte.Parse(ip[2]);
            octets[3] = byte.Parse(ip[3]);

            return octets;
        }

        private void AddManualRow(IPCalculation ipc)
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

                    NetAddressSize = ipc.getNetworkBits().ToString()

                }
            );
        }
    }
}
