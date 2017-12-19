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
using IP_Calculator.DataVisualization;
using IP_Calculator.ManualCalculations;

namespace IP_Calculator
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ManualDataRow> manualData = new ObservableCollection<ManualDataRow>();

        private ManualController manualController;

        public MainWindow()
        {          
            InitializeComponent();
            InitManualCalculations();
            CalculateAutomaticly();
        }

        private void createTableAutomaticly(IPCalculation ipc)
        {            
            var dt = new ObservableCollection<DataRow>
            {                
                new DataRow(){Name = "Address",              Data = ipc.getIp()+"",                Binary =ipc.getIp().ToBinaryString() },
                new DataRow(){Name = "Netmask",              Data = ipc.getNetmask()+" = 24",      Binary =ipc.getNetmask().ToBinaryString()},
                new DataRow(){Name = "Wildcard",             Data = ipc.getWildcard()+ "",         Binary =ipc.getWildcard().ToBinaryString()  },
                new DataRow(){Name = "Network" ,             Data = ipc.getNetworkAddress()+"/24", Binary =ipc.getNetworkAddress().ToBinaryString()+""  },
                new DataRow(){Name = "Host Min" ,            Data = ipc.getfirstAddress()+"",      Binary =ipc.getfirstAddress().ToBinaryString()+"" },
                new DataRow(){Name = "Host Max" ,            Data = ipc.getLastAddress()+"",       Binary =ipc.getLastAddress().ToBinaryString()+""  },
                new DataRow(){Name = "Broadcast",            Data = ipc.getBroadcastAddress()+"",  Binary =ipc.getBroadcastAddress().ToBinaryString()+"" },
                //
                new DataRow(){Name = "Hosts/Net",            Data = ipc.getHostnumber()+"" ,       Binary ="Class C, Private Internet"},
                //
                new DataRow(){Name = "Host amount",          Data = "251" ,                        Binary =""},
                //
                new DataRow(){Name = "Amount of free host",  Data = "3" ,                          Binary =((5.88)).ToString()+"%"},
                new DataRow(){Name = "Host Address Size",    Data = ipc.getHostBits() + " Bits" ,  Binary =""},
                new DataRow(){Name = "Network Address Size", Data = ipc.getNetworkBits() + " Bits",Binary =""}
         
            };
            table.ItemsSource = dt;

            combo.Items.Add("Wi-Fi");
            combo.Items.Add("Ethernet");

        }

        private void CalculateAutomaticly()
        {
            InternetProtocolAddress ip = new InternetProtocolAddress(192, 168, 178, 222);
            IPCalculation ipc = new IPCalculation(ip, byte.Parse("24"));
            createTableAutomaticly(ipc);
        }

        private void InitManualCalculations()
        {
            manualController = new ManualController();

            tableManual.ItemsSource = manualController.ManualDataCollection;

            for (int i = 1; i < 32; i++)
            {
                Hostbits.Items.Add(i.ToString());
            }
            Hostbits.SelectedItem = "24";
        }

        #region Controls event handlers

        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            if(NetworkPercent.IsChecked!= null && NetworkPercent.IsChecked == true)
                manualController.CalculateManual((bool)MinNetworkNumRadio.IsChecked, (bool)MinNetworkSizeRadio.IsChecked, NetworkPercentNum.Value);
            else
                manualController.CalculateManual((bool)MinNetworkNumRadio.IsChecked, (bool)MinNetworkSizeRadio.IsChecked);
            tableManual.Items.Refresh();
        }

        private void IpAdressBox_KeyUp(object sender, KeyEventArgs e)
        {
            manualController.ipAddress = IpAdressBox.Text.Replace("_",String.Empty);
        }

        private void Hostbits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            manualController.hostBits = Hostbits.SelectedItem.ToString();
        }

        private void HostsAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(manualController != null && HostsAmount.Value != null)
                manualController.hostsNumber = (int)HostsAmount.Value;
        }

        private void ShowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                int rowToSwitch = (int)((Button)sender).Tag;
                var row = manualController.ManualDataCollection.Where(w => w.Id == rowToSwitch).Single();
                row.ShowBinary = !row.ShowBinary;
                tableManual.Items.Refresh();
                foreach (var col in tableManual.Columns)
                {
                    col.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                }
            }
        }



        #endregion


    }
}
