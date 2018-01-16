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
            autoController = new AutomaticController(sweepProgressBar);

            interfacesBox.ItemsSource = autoController.connectionsCollection;

            autoTable.ItemsSource = autoController.discoveredHosts;

            autoResultTable.ItemsSource = autoController.resultCollection;

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

        private void DrawNetBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<ManualDataRow> a = manualController.ManualDataCollection;

            string text2 = "";

            if (a.Count <= 4)
            {
                text2 = ManualCalculations.Draw.createFile(a);
            }
            else
            {
                text2 = ManualCalculations.Draw.createFile2(a);
            }
            String text = text2 + "end network file.";
            System.IO.File.WriteAllText(@"WriteText.txt", text);

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.EnableRaisingEvents = false;
            proc.StartInfo.FileName = @"Graphic\Graphic\bin\Debug\Graphic.exe";
            proc.StartInfo.Arguments = @"WriteText.txt";
            proc.Start();

        }

        #endregion

        #region Automatic controls event handlers

        private void interfacesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            autoController.selectedConnection = (ConnectionInterfaceInfo)interfacesBox.SelectedItem;
        }

        private void autoCalcBtn_Click(object sender, RoutedEventArgs e)
        {
            
            autoController.CalculateOptimal((int)pingDuration.Value,autoTable,autoResultTable,(bool)MinSizes.IsChecked);
        }

        private void AutoShowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                string rowToSwitch = (string)((Button)sender).Tag;
                var row = autoController.discoveredHosts.Where(w => w.Hostname == rowToSwitch).Single();
                row.ShowBinary = !row.ShowBinary;
                autoTable.Items.Refresh();                
            }
        }

        private void AutoResultShowBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag != null)
            {
                int rowToSwitch = (int)((Button)sender).Tag;
                var row = autoController.resultCollection.Where(w => w.Id == rowToSwitch).Single();
                row.ShowBinary = !row.ShowBinary;
                autoResultTable.Items.Refresh();
            }
        }

        #endregion
    }
}
