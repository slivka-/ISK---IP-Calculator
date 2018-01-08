using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using IP_Calculator.DataVisualization;
using IP_Calculator.ManualCalculations;
using System.Windows;

namespace IP_Calculator.AutomaticCalculations
{
    class AutomaticController
    {
        private object lockObj = new object();

        public ObservableCollection<ConnectionInterfaceInfo> discoveredHosts;

        public ObservableCollection<ConnectionInterfaceInfo> connectionsCollection;

        public ObservableCollection<AutomaticDataRow> resultCollection;

        public ConnectionInterfaceInfo selectedConnection;

        private List<int> completedPings;

        private ProgressBar sweepProgressBar;

        public AutomaticController(ProgressBar progressBar)
        {
            connectionsCollection = new ObservableCollection<ConnectionInterfaceInfo>();
            discoveredHosts = new ObservableCollection<ConnectionInterfaceInfo>();
            resultCollection = new ObservableCollection<AutomaticDataRow>();
            sweepProgressBar = progressBar;
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

        private void ScanNetworkForHosts(int pingDuration, DataGrid dataGrid, DataGrid resultGrid)
        {
            

            var toScanPerOctet = selectedConnection.SubnetMask
                                                 .GetAddressBytes()
                                                 .Select(s => -1 + (int)Math.Pow(2, Convert.ToString(s, 2)
                                                                                 .PadLeft(8, '0')
                                                                                 .ToCharArray()
                                                                                 .Where(w => w.Equals('0')).Count())).ToArray();

            var octets = selectedConnection.GetNetworkAddress().GetAddressBytes().Select(s => (int)s).ToArray();

            completedPings = new List<int>();
            for (int x = 0; x < toScanPerOctet.Sum(); x++)
                completedPings.Add(x);

            Task.Factory.StartNew(() => WaitForSweepComplete(toScanPerOctet.Sum(), dataGrid, resultGrid));

            int counter = 0;
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
                            p.PingCompleted += new PingCompletedEventHandler((s, e) => p_PingCompleted(s,e,counter++));
                            p.SendAsync(ip, pingDuration, ip);
                        }
                    }
                }
            }
        }

        private void p_PingCompleted(object sender, PingCompletedEventArgs e, int threadNum)
        {
            lock (lockObj)
            {
                string ip = (string)e.UserState;
                
                if (e.Reply != null && e.Reply.Status == IPStatus.Success)
                {
                    //TODO: write successfull pings to array, calculate network size
                    //?? real hosts number prediction with found hosts ??
                    Console.WriteLine("Nr:{0}, {1} is up: ({2} ms)", completedPings, ip, e.Reply.RoundtripTime);
                    byte[] ipBytes = ip.Split('.').Select(s => byte.Parse(s)).ToArray();
                    discoveredHosts.Add(new ConnectionInterfaceInfo()
                    {
                        Hostname = "Discovered host " + threadNum,
                        IpAddress = new System.Net.IPAddress(ipBytes),
                        SubnetMask = selectedConnection.SubnetMask,
                        DefGateway = selectedConnection.DefGateway,
                        ShowBinary = false
                    });
                }
                completedPings.Remove(threadNum);
            }
        }

        private void WaitForSweepComplete(int pingsNum, DataGrid dataGrid, DataGrid resultGrid)
        {
            sweepProgressBar.Dispatcher.Invoke(delegate {
                sweepProgressBar.Minimum = 0;
                sweepProgressBar.Maximum = completedPings.Count;
            });
            while (completedPings.Count > 0)
            {
                sweepProgressBar.Dispatcher.Invoke(delegate {
                    sweepProgressBar.Value = pingsNum - completedPings.Count();
                });
                Thread.Sleep(5);
            }
            sweepProgressBar.Dispatcher.Invoke(delegate {
                sweepProgressBar.Value = pingsNum - completedPings.Count();
            });
            sweepProgressBar.Dispatcher.Invoke(delegate
            {
                dataGrid.Items.Refresh();
            });
            CalculateNetworks(resultGrid);
            Console.WriteLine("Sweep complete");
        }

        private void CalculateNetworks(DataGrid resultGrid)
        {
            
            if (true)
            {
                InternalAddressRow addrRow = CalculateMinimalNetworksNum();
                IPCalculation ipc = new IPCalculation(addrRow.Ip, addrRow.Netmask);
                Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => AddAutomaticRow(ipc, addrRow.HostsNumber)));
            }
            else if (false)
            {
                var addrRows = CalculateMinimalNetworksSize();
                foreach (var row in addrRows)
                {
                    IPCalculation ipc = new IPCalculation(row.Ip, row.Netmask);
                    AddAutomaticRow(ipc, row.HostsNumber);
                }
            }
        }


        private List<InternalAddressRow> CalculateMinimalNetworksSize()
        {
            
            List<InternalAddressRow> output = new List<InternalAddressRow>();

            var netmasks = GetMinimalSizes();

            uint baseIp = ByteArr2Uint(Text2byte(selectedConnection.DefGateway.ToString()));

            foreach (var netmask in netmasks)
            {
                output.Add(new InternalAddressRow()
                {
                    Ip = new InternetProtocolAddress(baseIp),
                    Netmask = Byte.Parse((32 - (int)Math.Log(netmask, 2)).ToString()),
                    HostsNumber = netmask - 3 - (int)(netmask)
                });
                baseIp += (uint)netmask;
            }
            return output;
        }

        private InternalAddressRow CalculateMinimalNetworksNum()
        {
            int networkSize = 1;
            while (networkSize < discoveredHosts.Count)
                networkSize *= 2;

            byte netmask = Byte.Parse((32 - (int)Math.Log(networkSize, 2)).ToString());

            byte[] octets = Text2byte(selectedConnection.DefGateway.ToString());
            return new InternalAddressRow()
            {
                Ip = new InternetProtocolAddress(octets[0], octets[1], octets[2], octets[3]),
                Netmask = netmask,
                HostsNumber = discoveredHosts.Count
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
                return BitConverter.ToUInt32(arr, 0);
        }

        private List<int> GetMinimalSizes()
        {
            var output = new List<int>();
            int tempHosts = discoveredHosts.Count;

            while (tempHosts > 0)
            {
                int networkSize = 1;
                while (networkSize < tempHosts + 3)
                    networkSize *= 2;
                if (networkSize != tempHosts + 3)
                    networkSize /= 2;

                output.Add(networkSize);
                if (networkSize != tempHosts + 3)
                    tempHosts -= (networkSize - 3) - (int)(networkSize);
                else
                    tempHosts -= (networkSize - 3);
            }
            return output;
        }


        private void AddAutomaticRow(IPCalculation ipc, int usedHostsNumber)
        {
            int lp = resultCollection.Count + 1;
            resultCollection.Add(new AutomaticDataRow()
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

        public void CalculateOptimal(int pingDuration, DataGrid dataGrid, DataGrid resultGrid)
        {
            discoveredHosts.Clear();
            resultCollection.Clear();
            if (selectedConnection != null)
            {
                ScanNetworkForHosts(pingDuration, dataGrid, resultGrid);
            }
        }

        private class InternalAddressRow
        {
            public InternetProtocolAddress Ip { get; set; }
            public byte Netmask { get; set; }
            public int HostsNumber { get; set; }
        }

    }
}
