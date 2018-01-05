using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IP_Calculator.AutomaticCalculations
{
    class AutomaticController
    {
        private object lockObj = new object();

        private List<ConnectionInterfaceInfo> discoveredHosts;

        public ObservableCollection<ConnectionInterfaceInfo> connectionsCollection;

        public ConnectionInterfaceInfo selectedConnection;

        private int completedPings;

        public AutomaticController()
        {
            connectionsCollection = new ObservableCollection<ConnectionInterfaceInfo>();
            ScanForConnections();
        }

        private void ScanForConnections()
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback && adapter.OperationalStatus == OperationalStatus.Up)
                {
                    var gatewayList = adapter.GetIPProperties().GatewayAddresses.ToList();
                    if (gatewayList.Count > 0)
                    {
                        foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                        {
                            if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                connectionsCollection.Add(new ConnectionInterfaceInfo()
                                {
                                    Hostname = adapter.Name,
                                    IpAddress = unicastIPAddressInformation.Address,
                                    SubnetMask = unicastIPAddressInformation.IPv4Mask,
                                    DefGateway = gatewayList.First().Address
                                });
                            }
                        }
                    }
                }
            }
        }

        private void ScanNetworkForHosts()
        {
            completedPings = 0;

            var toScanPerOctet = selectedConnection.SubnetMask
                                                 .GetAddressBytes()
                                                 .Select(s => -1 + (int)Math.Pow(2, Convert.ToString(s, 2)
                                                                                 .PadLeft(8, '0')
                                                                                 .ToCharArray()
                                                                                 .Where(w => w.Equals('0')).Count())).ToArray();

            var octets = selectedConnection.GetNetworkAddress().GetAddressBytes().Select(s => (int)s).ToArray();

            for (int i = octets[0]; i <= octets[0] + toScanPerOctet[0]; i++)
            {
                for (int j = octets[1]; j <= octets[1] + toScanPerOctet[1]; j++)
                {
                    for (int k = octets[2]; k <= octets[2] + toScanPerOctet[2]; k++)
                    {
                        for (int l = octets[3]; l <= octets[3] + toScanPerOctet[3]; l++)
                        {
                            string ip = string.Format("{0}.{1}.{2}.{3}", i, j, k, l);
                            Ping p = new Ping();
                            p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
                            p.SendAsync(ip, 100, ip);
                        }
                    }
                }
            }
        }

        private void p_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            lock (lockObj)
            {
                completedPings++;
                string ip = (string)e.UserState;
                if (e.Reply != null && e.Reply.Status == IPStatus.Success)
                {
                    //TODO: write successfull pings to array, calculate network size
                    //?? real hosts number prediction with found hosts ??
                    //wait for closing of all threads
                    Console.WriteLine("Nr:{0}, {1} is up: ({2} ms)", completedPings, ip, e.Reply.RoundtripTime);
                    byte[] ipBytes = ip.Split('.').Select(s => byte.Parse(s)).ToArray();
                    discoveredHosts.Add(new ConnectionInterfaceInfo()
                    {
                        Hostname = "Host nr "+completedPings,
                        IpAddress = new System.Net.IPAddress(ipBytes),
                        SubnetMask = selectedConnection.SubnetMask,
                        DefGateway = selectedConnection.DefGateway
                    });
                }
            }
        }
    
        public void CalculateOptimal()
        {
            discoveredHosts = new List<ConnectionInterfaceInfo>();
            ScanNetworkForHosts();

            if (selectedConnection != null)
            {
                Console.WriteLine(selectedConnection.ToString());
                Console.WriteLine(selectedConnection.GetMaskSize());
            }
        }

    }
}
