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
using IP_Calculator.AutomaticCalculations;
using System.Net;

namespace IP_Calculator
{
    public partial class MainWindow : Window
    {
        private ManualController manualController;

        private AutomaticController autoController;

        public MainWindow()
        {          
            InitializeComponent();
            InitAutomaticCalculations();
            InitManualCalculations();
        }

        /*
        private void createTableAutomaticly(IPCalculation ipc)
        {            
            var dt = new ObservableCollection<DataRow>
            {                
                new DataRow(){Name = "Address",              Data = ipc.getIp().ToString(),                Binary =ipc.getIp().ToBinaryString() },
                new DataRow(){Name = "Netmask",              Data = ipc.getNetmask()+" = 24",      Binary =ipc.getNetmask().ToBinaryString()},
                new DataRow(){Name = "Wildcard",             Data = ipc.getWildcard().ToString(),         Binary =ipc.getWildcard().ToBinaryString()  },
                new DataRow(){Name = "Network" ,             Data = ipc.getNetworkAddress()+"/24", Binary =ipc.getNetworkAddress().ToBinaryString().ToString()  },
                new DataRow(){Name = "Host Min" ,            Data = ipc.getfirstAddress().ToString(),      Binary =ipc.getfirstAddress().ToBinaryString().ToString() },
                new DataRow(){Name = "Host Max" ,            Data = ipc.getLastAddress().ToString(),       Binary =ipc.getLastAddress().ToBinaryString().ToString()  },
                new DataRow(){Name = "Broadcast",            Data = ipc.getBroadcastAddress().ToString(),  Binary =ipc.getBroadcastAddress().ToBinaryString().ToString() },
                //
                new DataRow(){Name = "Hosts/Net",            Data = ipc.getHostnumber().ToString() ,       Binary ="Class B"},
                //
                new DataRow(){Name = "Host amount",          Data = "251" ,                        Binary =""},
                //
                new DataRow(){Name = "Amount of free host",  Data = "3" ,                          Binary =((5.88)).ToString()+"%"},
                new DataRow(){Name = "Host Address Size",    Data = ipc.getHostBits() + " Bits" ,  Binary =""},
                new DataRow(){Name = "Network Address Size", Data = ipc.getNetworkBits() + " Bits",Binary =""}
         
            };
            table.ItemsSource = dt;
                        
        }
        */

        private string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        private void InitManualCalculations()
        {
            manualController = new ManualController();

            tableManual.ItemsSource = manualController.ManualDataCollection;

            for (int i = 1; i < 29; i++)
            {
                Hostbits.Items.Add(i.ToString());
            }
            Hostbits.SelectedItem = "24";
        }

        private void InitAutomaticCalculations()
        {
            autoController = new AutomaticController();

            interfacesBox.ItemsSource = autoController.connectionsCollection;

        }

        #region Manual controls event handlers

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
            manualController.hostBits = int.Parse(Hostbits.SelectedItem.ToString());
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

        private void ForceMask_Checked(object sender, RoutedEventArgs e)
        {
            if (ForceMask.IsChecked != null)
                manualController.forcedMinMask = (bool)ForceMask.IsChecked;
        }

        #endregion

        #region Automatic controls event handlers

        private void interfacesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #endregion
    }
}
