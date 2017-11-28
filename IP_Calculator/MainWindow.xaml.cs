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

namespace IP_Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            createTableAutomaticly();
            
        }

        private void createTableAutomaticly()
        {            
            var dt = new ObservableCollection<dataRow>
            {
                new dataRow(){Name = "Address",   Data= "192.168.0.1",         Binary ="11000000.10101000.00000000. 00000001"},
                new dataRow(){Name = "Netmask",   Data = "255.255.255.0 = 24", Binary ="11111111.11111111.11111111. 00000000"},
                new dataRow(){Name = "Wildcard",  Data = "0.0.0.255",          Binary ="00000000.00000000.00000000. 11111111"   },
                new dataRow(){Name = "Network" ,  Data = "192.168.0.0/24",     Binary ="11000000.10101000.00000000. 00000000"  },
                new dataRow(){Name = "HostMin" ,  Data = "192.168.0.1",        Binary ="11000000.10101000.00000000. 00000001" },
                new dataRow(){Name = "HostMax" ,  Data = "192.168.0.254",      Binary ="11000000.10101000.00000000. 11111110"  },
                new dataRow(){Name = "Broadcast", Data = "192.168.0.255",      Binary ="11000000.10101000.00000000. 11111111" },
                new dataRow(){Name = "Hosts/Net", Data = "254" ,               Binary ="Class C, Private Internet"},
                new dataRow(){Name = "Host amount", Data = "251" ,             Binary =""},
                new dataRow(){Name = "Amount of free host", Data = "3" ,       Binary =((5.88)).ToString()+"%"},

            };
            table.ItemsSource = dt;

            combo.Items.Add("Wi-Fi");
            combo.Items.Add("Ethernet");

        }

        private void createTableManual()
        {
            var dt = new ObservableCollection<dataRow>
            {
                new dataRow(){Name = "Netmask",   Data = "255.255.255.0 = 24", Binary ="11111111.11111111.11111111. 00000000"},
                new dataRow(){Name = "Wildcard",  Data = "0.0.0.255",          Binary ="00000000.00000000.00000000. 11111111"   },
                new dataRow(){Name = "Network" ,  Data = "192.168.0.0/24",     Binary ="11000000.10101000.00000000. 00000000"  },
                new dataRow(){Name = "HostMin" ,  Data = "192.168.0.1",        Binary ="11000000.10101000.00000000. 00000001" },
                new dataRow(){Name = "HostMax" ,  Data = "192.168.0.254",      Binary ="11000000.10101000.00000000. 11111110"  },
                new dataRow(){Name = "Broadcast", Data = "192.168.0.255",      Binary ="11000000.10101000.00000000. 11111111" },
                new dataRow(){Name = "Hosts/Net", Data = "254" ,               Binary ="Class C, Private Internet"},
                new dataRow(){Name = "Amount of free host", Data = "3" ,       Binary =((5.88)).ToString()+"%"},

            };
            tableManual.ItemsSource = dt;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            createTableManual();
        }
    }
}
