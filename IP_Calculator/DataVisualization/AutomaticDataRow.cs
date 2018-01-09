using System;

namespace IP_Calculator.DataVisualization
{
    class AutomaticDataRow
    {

        public bool ShowBinary { get; set; }
        public int Id { get; set; }

        public string IpAddress
        {
            get
            {
                if (!ShowBinary)
                    return ipAddress;
                else
                    return BinaryIpAddress;
            }
            set
            {
                ipAddress = value;
            }
        }
        public string NetMask
        {
            get
            {
                if (!ShowBinary)
                    return netMask;
                else
                    return BinaryNetMask;
            }
            set
            {
                netMask = value;
            }
        }
        public string HostMin
        {
            get
            {
                if (!ShowBinary)
                    return hostMin;
                else
                    return BinaryHostMin;
            }
            set
            {
                hostMin = value;
            }
        }
        public string HostMax
        {
            get
            {
                if (!ShowBinary)
                    return hostMax;
                else
                    return BinaryHostMax;
            }
            set
            {
                hostMax = value;
            }
        }
        public string Broadcast
        {
            get
            {
                if (!ShowBinary)
                    return broadcast;
                else
                    return BinaryBroadcast;
            }
            set
            {
                broadcast = value;
            }
        }
        public string Wildcard
        {
            get
            {
                if (!ShowBinary)
                    return wildcard;
                else
                    return BinaryWildcard;
            }
            set
            {
                wildcard = value;
            }
        }
        public string HostsNum { get; set; }
        public string FreeHosts { get; set; }
        public string HostAddressSize { get; set; }
        public string NetAddressSize { get; set; }

        public string BinaryIpAddress { get; set; }
        public string BinaryNetMask { get; set; }
        public string BinaryHostMin { get; set; }
        public string BinaryHostMax { get; set; }
        public string BinaryBroadcast { get; set; }
        public string BinaryWildcard { get; set; }

        public string OccHosts
        {
            get
            {
                return (int.Parse(FreeHosts) - int.Parse(HostsNum)).ToString();
            }
        }

        private string ipAddress;
        private string netMask;
        private string hostMin;
        private string hostMax;
        private string broadcast;
        private string wildcard;

    }
}
