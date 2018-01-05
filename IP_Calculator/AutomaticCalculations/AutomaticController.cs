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
        public ObservableCollection<ConnectionInterfaceInfo> connectionsCollection;

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
    }
}
