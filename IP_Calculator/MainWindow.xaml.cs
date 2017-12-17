using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IP_Calculator.dataVisualization;
using IPCalc;

namespace IP_Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {          
            InitializeComponent();
            calculateAutomaticly();
            init_hostbits();
        }

        private void createTableAutomaticly(IPCalculation ipc)
        {            
            var dt = new ObservableCollection<dataRow>
            {                
                new dataRow(){Name = "Address",              Data = ipc.getIp()+"",                Binary =ipc.getIp().ToBinaryString() },
                new dataRow(){Name = "Netmask",              Data = ipc.getNetmask()+" = 24",      Binary =ipc.getNetmask().ToBinaryString()},
                new dataRow(){Name = "Wildcard",             Data = ipc.getWildcard()+ "",         Binary =ipc.getWildcard().ToBinaryString()  },
                new dataRow(){Name = "Network" ,             Data = ipc.getNetworkAddress()+"/24", Binary =ipc.getNetworkAddress().ToBinaryString()+""  },
                new dataRow(){Name = "Host Min" ,            Data = ipc.getfirstAddress()+"",      Binary =ipc.getfirstAddress().ToBinaryString()+"" },
                new dataRow(){Name = "Host Max" ,            Data = ipc.getLastAddress()+"",       Binary =ipc.getLastAddress().ToBinaryString()+""  },
                new dataRow(){Name = "Broadcast",            Data = ipc.getBroadcastAddress()+"",  Binary =ipc.getBroadcastAddress().ToBinaryString()+"" },
                //
                new dataRow(){Name = "Hosts/Net",            Data = ipc.getHostnumber()+"" ,       Binary ="Class C, Private Internet"},
                //
                new dataRow(){Name = "Host amount",          Data = "251" ,                        Binary =""},
                //
                new dataRow(){Name = "Amount of free host",  Data = "3" ,                          Binary =((5.88)).ToString()+"%"},
                new dataRow(){Name = "Host Address Size",    Data = ipc.getHostBits() + " Bits" ,  Binary =""},
                new dataRow(){Name = "Network Address Size", Data = ipc.getNetworkBits() + " Bits",Binary =""}
         
            };
            table.ItemsSource = dt;

            combo.Items.Add("Wi-Fi");
            combo.Items.Add("Ethernet");

        }
 

        private void createTableManual(IPCalculation ipc)
        {
            var dt = new ObservableCollection<dataRow>
            {
                new dataRow(){Name = "Netmask",     Data = ipc.getNetmask()+" = "+Hostbits.SelectedItem,       Binary =ipc.getNetmask().ToBinaryString()},
                new dataRow(){Name = "Wildcard",    Data = ipc.getWildcard()+"",                               Binary =ipc.getWildcard().ToBinaryString() },
                new dataRow(){Name = "Network" ,    Data = ipc.getNetworkAddress()+"/"+Hostbits.SelectedItem,  Binary =ipc.getNetworkAddress().ToBinaryString() },
                new dataRow(){Name = "Host Min" ,   Data = ipc.getfirstAddress()+"",                           Binary =ipc.getfirstAddress().ToBinaryString() },
                new dataRow(){Name = "Host Max" ,   Data = ipc.getLastAddress()+"",                            Binary =ipc.getLastAddress().ToBinaryString() },
                new dataRow(){Name = "Broadcast",   Data = ipc.getBroadcastAddress()+"",                       Binary =ipc.getBroadcastAddress().ToBinaryString() },
                //
                new dataRow(){Name = "Hosts/Net",   Data = ipc.getHostnumber()+"" ,                            Binary ="Class C, Private Internet"},
                //
                new dataRow(){Name = "Amount of free host", Data = "3" ,                                       Binary =((5.88)).ToString()+"%"},
                new dataRow(){Name = "Host Address Size",   Data = ipc.getHostBits() + " Bits" ,               Binary =""},
                new dataRow(){Name = "Network Address Size",Data = ipc.getNetworkBits() + " Bits" ,            Binary =""}

            };
            tableManual.ItemsSource = dt;

        }

        private void init_hostbits()
        {
            for (int i = 1; i < 32; i++)
            {
                Hostbits.Items.Add(i.ToString());
            }
            Hostbits.SelectedItem = "24";
        }

        private void calculateManual()
        {
            byte[] octets = text2byte();
            InternetProtocolAddress ip = new InternetProtocolAddress(octets[0], octets[1], octets[2], octets[3]);            
            IPCalculation ipc = new IPCalculation(ip, byte.Parse(Hostbits.SelectedItem.ToString()));
            createTableManual(ipc);
        }

        private void calculateAutomaticly()
        {
            InternetProtocolAddress ip = new InternetProtocolAddress(192,168,178,222);
            IPCalculation ipc = new IPCalculation(ip, byte.Parse("24"));
            createTableAutomaticly(ipc);
        }

        private byte[] text2byte()
        {            
            String[] ip = (adresIp.Text).Split(new Char[] { ',', '.' });
            byte[] octets = new byte[4];

            octets[0] = byte.Parse(ip[0]);
            octets[1] = byte.Parse(ip[1]);
            octets[2] = byte.Parse(ip[2]);
            octets[3] = byte.Parse(ip[3]);

            return octets;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            calculateManual();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void tableManual_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }        

        private void table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Hostbits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
