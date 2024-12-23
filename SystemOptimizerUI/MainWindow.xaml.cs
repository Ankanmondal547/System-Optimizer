using System;
using System.Collections.Generic;
using System.IO;
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
//using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;
namespace SystemOptimizerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void MonitorCPUUsage_Click(object sender, RoutedEventArgs e)
        {
            // Show the loading page
            LoadingPage loadingPage = new LoadingPage();
            loadingPage.Show();

            try
            {
                // Perform the initialization or delay task asynchronously
                await Task.Run(() =>
                {
                    // Simulate loading or resource-heavy processing
                    Thread.Sleep(5000); // Replace this with actual initialization logic if necessary
                });

                // Launch the CPU Usage Monitor page
                var cpuUsagePage = new CPUUsagePage();
                cpuUsagePage.Show();
            }
            catch (Exception ex)
            {
                // Handle any exceptions if needed
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Close the loading page once the next page is fully loaded
                loadingPage.Close();

                // Close the current page
                this.Close();
            }
        }




        private void CheckDiskSpace_Click(object sender, RoutedEventArgs e)
        {
            // Call the method to check disk space
            // Navigate to another XAML page
            var DiskSpace = new DiskSpace(); // Replace with your next page class
            DiskSpace.Show();
            this.Close();

        }
        public void BackButton_Click(object sender, RoutedEventArgs e) 
        {
            // Navigate to another XAML page
            var Introduction = new Introduction(); // Replace with your next page class
            Introduction.Show();
            this.Close();

        }
        public void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var BoostSystem = new BoostSystem();    
            BoostSystem.Show();
            this.Close();
        }
    }
}
