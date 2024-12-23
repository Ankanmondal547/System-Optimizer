using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SystemOptimizerUI
{
    /// <summary>
    /// Interaction logic for BoostSystem.xaml
    /// </summary>
    public partial class BoostSystem : Window
    {
        public BoostSystem()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.AbsoluteUri,
                UseShellExecute = true // Ensures the link opens in the default browser
            });
            e.Handled = true;
        }
        // Method to be executed when the "Back" button is clicked
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

        // Method to be executed when the "Start" button is clicked
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if Terms checkbox is checked
            if (TermsCheckBox.IsChecked != true)
            {
                MessageBox.Show("You must agree to the terms and conditions before proceeding.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                if (CleanTempFilesCheckBox.IsChecked == false && ListStartupProgramsCheckBox.IsChecked == false && PerformDiskCleanupCheckBox.IsChecked == false)
                {
                    MessageBox.Show("Please select at least one option to proceed ..!");
                }
                else
                {


                    // Show loading page while methods are being executed
                    LoadingPage loadingPage = new LoadingPage();
                    loadingPage.Show();

                    try
                    {
                        // Execute methods asynchronously
                        if (CleanTempFilesCheckBox.IsChecked == true)
                        {

                            await Task.Run(() => CleanTemporaryFiles());
                        }

                        if (ListStartupProgramsCheckBox.IsChecked == true)
                        {

                            await Task.Run(() => ListStartupPrograms());
                        }

                        if (PerformDiskCleanupCheckBox.IsChecked == true)
                        {

                            await Task.Run(() => PerformDiskCleanup());
                        }

                        // Close the loading page after all tasks are done
                        loadingPage.Close();

                        // Navigate to another page to show results
                        ResultsPage resultsPage = new ResultsPage();
                        resultsPage.Show();
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        loadingPage.Close();
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        // Method for cleaning temporary files
        private void CleanTemporaryFiles()
        {
            Thread.Sleep(2000);
            string tempPath = Path.GetTempPath();
           // Console.WriteLine($"Cleaning temporary files from: {tempPath}");

            try
            {
                DirectoryInfo dir = new DirectoryInfo(tempPath);
                foreach (FileInfo file in dir.GetFiles())
                {
                    try
                    {
                       // Console.WriteLine($"Attempting to delete file: {file.FullName}");
                        file.Delete();
                        //Console.WriteLine($"Deleted: {file.FullName}");
                    }
                    catch (IOException ioEx)
                    {
                    //    Console.WriteLine($"Error deleting file (locked or in use): {file.FullName} - {ioEx.Message}");
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine($"Error deleting file: {file.FullName} - {ex.Message}");
                    }
                }

                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    try
                    {
                        //Console.WriteLine($"Attempting to delete directory: {subDir.FullName}");
                        subDir.Delete(true);
                        //Console.WriteLine($"Deleted directory: {subDir.FullName}");
                    }
                    catch (IOException ioEx)
                    {
                        //Console.WriteLine($"Error deleting directory (locked or in use): {subDir.FullName} - {ioEx.Message}");
                    }
                    catch (Exception ex)
                    {
                     //   Console.WriteLine($"Error deleting directory: {subDir.FullName} - {ex.Message}");
                    }
                }

                MessageBox.Show("Temporary files cleanup complete.");
            }
            catch (Exception ex)
            {
               // Console.WriteLine($"Error during cleanup: {ex.Message}");
            }
        }

        // Method for listing startup programs
        private void ListStartupPrograms()
        {
            Thread.Sleep(1500);
           // Console.WriteLine("Startup Programs:");
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run"))
            {
                if (key != null)
                {
                    foreach (string name in key.GetValueNames())
                    {
                        MessageBox.Show($"- {name}: {key.GetValue(name)}");
                    }
                }
                else
                {
                    MessageBox.Show("No startup programs found.");
                }
            }
        }

        // Method for performing disk cleanup
        private void PerformDiskCleanup()
        {
            Thread.Sleep(2500);
            //Console.WriteLine("Starting Disk Cleanup...");
            string[] pathsToClean = new[]
            {
                Path.GetTempPath(), // Temporary files
                Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), // Browser cache
                Environment.GetFolderPath(Environment.SpecialFolder.Recent), // Recent files
                Environment.GetFolderPath(Environment.SpecialFolder.History), // History files
            };

            long totalDeletedSize = 0;

            foreach (var path in pathsToClean)
            {
                if (Directory.Exists(path))
                {
                   // Console.WriteLine($"Cleaning: {path}");
                    try
                    {
                        DirectoryInfo dir = new DirectoryInfo(path);
                        foreach (FileInfo file in dir.GetFiles())
                        {
                            totalDeletedSize += file.Length;
                            file.Delete();
                        }

                        foreach (DirectoryInfo subDir in dir.GetDirectories())
                        {
                            subDir.Delete(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine($"Error cleaning {path}: {ex.Message}");
                    }
                }
                else
                {
                    //Console.WriteLine($"Path not found: {path}");
                }
            }

            //MessageBox.Show($"Disk Cleanup completed. Total space freed: {totalDeletedSize / (1024 * 1024)} MB");
            MessageBox.Show($"Disk Cleanup completed.");
        }
    }
}
