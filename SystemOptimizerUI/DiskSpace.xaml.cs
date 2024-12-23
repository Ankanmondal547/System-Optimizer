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
using System.Windows.Shapes;

namespace SystemOptimizerUI
{
    /// <summary>
    /// Interaction logic for DiskSpace.xaml
    /// </summary>
    public partial class DiskSpace : Window
    {
        public DiskSpace()
        {
            InitializeComponent();
            CheckDiskSpace();
        }
        private void CheckDiskSpace()
        {
            // Get all drives on the computer
            DriveInfo[] drives = DriveInfo.GetDrives();

            // Create a list of objects to display in the DataGrid
            var diskInfoList = new System.Collections.Generic.List<DiskInfo>();

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady) // Check if the drive is ready (i.e., connected and accessible)
                {
                    long totalSpace = drive.TotalSize;
                    long freeSpace = drive.TotalFreeSpace;
                    long usedSpace = totalSpace - freeSpace;
                    double usedPercentage = ((double)usedSpace / totalSpace) * 100;
                    

                    // Create a DiskInfo object to store the drive data
                    diskInfoList.Add(new DiskInfo
                    {
                        DriveName = drive.Name,
                        TotalSize = FormatBytes(totalSpace),
                        FreeSpace = FormatBytes(freeSpace),
                        UsedSpace = FormatBytes(usedSpace),
                        UsedPercentage = $"{usedPercentage:F2}%",
                        UsageCategory = GetUsageCategory(usedPercentage)
                    });
                }
            }

            // Bind the list of disk information to the DataGrid
            DiskSpaceDataGrid.ItemsSource = diskInfoList;
        }

        // Method to format bytes into a more readable format
        private string FormatBytes(long bytes)
        {
            double size = bytes;
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < units.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return $"{size:0.##} {units[unitIndex]}";
        }

        // Event handler for the Back button
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic for the back button (e.g., navigating to another page)
            MessageBoxResult result = MessageBox.Show("Are you sure you want to go back?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // If user clicked Yes, go back to the MainWindow
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close(); // Close the current window
            }
            else
            {
                return;
            }
        }

        // Event handler for the Next button
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement logic to navigate to the next page
            MessageBox.Show("Next button clicked.");
        }
        private string GetUsageCategory(double usedPercentage)
        {
            if (usedPercentage < 50)
                return "Good";
            else if (usedPercentage < 80)
                return "Avg";
            else
                return "Need to Upgrade";
        }
    }

    // Class to hold disk information for display in the DataGrid
    public class DiskInfo
    {
        public string DriveName { get; set; }
        public string TotalSize { get; set; }
        public string FreeSpace { get; set; }
        public string UsedSpace { get; set; }
        public string UsedPercentage { get; set; }

        public string UsageCategory { get; set; }
    }
}

