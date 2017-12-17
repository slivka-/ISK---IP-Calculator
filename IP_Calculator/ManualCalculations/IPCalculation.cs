﻿using System;

namespace IP_Calculator.ManualCalculations
{
    class IPCalculation
    {
        byte cidr = 0;
        UInt32 hostnumber = 0;
        InternetProtocolAddress ip;
        InternetProtocolAddress networkaddress;
        InternetProtocolAddress netmask;
        InternetProtocolAddress rnetmask;
        InternetProtocolAddress firstAddress;
        InternetProtocolAddress lastAddress;
        InternetProtocolAddress broadcastAddress;
        InternetProtocolAddress wildcard;

        public IPCalculation(InternetProtocolAddress ip, byte cidr)
        {
            this.ip = ip;
            this.cidr = cidr;
            setnetmask();
            setHostnumber();
        }

        private void setnetmask()
        {
            byte octet = 0;
            byte value = 128;
            byte[] netmask = new byte[4];
            for (int i = 0; i <= 32; i++)
            {
                if (i % 8 == 0 && i > 0)
                {
                    netmask[(i / 8) - 1] = octet;
                    octet = 0;
                    value = 128;
                }
                if (i < cidr)
                {
                    octet += value;
                }

                value /= 2;
            }

            this.netmask = new InternetProtocolAddress(netmask[0], netmask[1], netmask[2], netmask[3]);
            rnetmask = new InternetProtocolAddress(this.netmask.IPAddress ^ UInt32.MaxValue);
            setWildcard(netmask);
        }

        private void setWildcard(byte[] wildcard)
        {
            for (int i = 0; i < wildcard.Length; i++)
            {
                wildcard[i] = (byte)(255 - wildcard[i]);
            }

            this.wildcard = new InternetProtocolAddress(wildcard[0], wildcard[1], wildcard[2], wildcard[3]);
        }

        private void setHostnumber()
        {
            byte hostbytes = (byte)(32 - cidr);
            UInt32 hosts = 1;
            for (int i = 0; i < hostbytes; i++)
            {
                hosts *= 2;
            }
            hostnumber = hosts - 2;
        }

        public InternetProtocolAddress getIp()
        {
            return this.ip;
        }

        public InternetProtocolAddress getNetmask()
        {
            return netmask;
        }

        public InternetProtocolAddress getWildcard()
        {
            return this.wildcard;
        }

        public UInt32 getHostnumber()
        {
            return hostnumber;
        }

        public byte getHostBits()
        {
            return (byte)(32 - cidr);
        }

        public byte getNetworkBits()
        {
            return cidr;
        }

        public InternetProtocolAddress getfirstAddress()
        {
            firstAddress = new InternetProtocolAddress((ip.IPAddress & netmask.IPAddress) + 1);
            networkaddress = new InternetProtocolAddress(ip.IPAddress & netmask.IPAddress);
            return firstAddress;
        }

        public InternetProtocolAddress getNetworkAddress()
        {
            networkaddress = new InternetProtocolAddress(ip.IPAddress & netmask.IPAddress);
            return networkaddress;
        }

        public InternetProtocolAddress getLastAddress()
        {
            if (firstAddress == null)
                getfirstAddress();
            if (networkaddress == null)
                getNetworkAddress();
            lastAddress = new InternetProtocolAddress((networkaddress.IPAddress ^ rnetmask.IPAddress) - 1);
            return lastAddress;
        }

        public InternetProtocolAddress getBroadcastAddress()
        {
            if (firstAddress == null)
                getfirstAddress();
            if (networkaddress == null)
                getNetworkAddress();
            broadcastAddress = new InternetProtocolAddress((networkaddress.IPAddress ^ rnetmask.IPAddress));
            return broadcastAddress;
        }

    }
}
