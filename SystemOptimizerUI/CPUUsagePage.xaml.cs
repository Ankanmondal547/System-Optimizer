using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SystemOptimizerUI
{
    /// <summary>
    /// Interaction logic for CPUUsagePage.xaml
    /// </summary>
    public partial class CPUUsagePage : Window
    {
        private PerformanceCounter _cpuCounter; // PerformanceCounter instance
        private CancellationTokenSource _cancellationTokenSource;
        private List<float> _cpuUsageHistory; // List to store CPU usage values

        public CPUUsagePage()
        {
            InitializeComponent();
            // Initialize PerformanceCounter for CPU usage
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _cpuUsageHistory = new List<float>(); // Initialize the usage history list
        }

        // Start Monitoring Button Clicked
        private void StartMonitoring_Click(object sender, RoutedEventArgs e)
        {
            CpuUsageOutput.Text = "Checking CPU Performance...";
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // Start a task to monitor CPU usage asynchronously
            Task.Run(() =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        float cpuUsage = _cpuCounter.NextValue();
                        _cpuUsageHistory.Add(cpuUsage); // Add the usage to history

                        Dispatcher.Invoke(() =>
                        {
                            CpuUsageOutput.Text = $"CPU Usage: {cpuUsage:F2}%";
                        });

                        Thread.Sleep(1000); // Refresh every second
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CpuUsageOutput.Text = "Error fetching CPU data.";
                        MessageBox.Show($"Error: {ex.Message}", "CPU Monitoring", MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            }, token);
        }

        // Stop Monitoring Button Clicked
        private void StopMonitoring_Click(object sender, RoutedEventArgs e)
        {
            // Stop monitoring
            _cancellationTokenSource?.Cancel();

            // Generate and display a performance report
            string report = GeneratePerformanceReport();
            MessageBox.Show(report, "CPU Performance Report", MessageBoxButton.OK, MessageBoxImage.Information);

            CpuUsageOutput.Text = "Monitoring stopped.";
        }

        // Generate a Performance Report Based on CPU Usage History
        private string GeneratePerformanceReport()
        {
            if (_cpuUsageHistory.Count == 0)
                return "No data available to generate a report.";

            // Calculate average CPU usage
            float averageUsage = 0;
            foreach (var usage in _cpuUsageHistory)
            {
                averageUsage += usage;
            }
            averageUsage /= _cpuUsageHistory.Count;

            // Determine performance status
            if (averageUsage <= 30)
                return $"Average CPU Usage: {averageUsage:F2}%\nPerformance: Good";
            else if (averageUsage <= 70)
                return $"Average CPU Usage: {averageUsage:F2}%\nPerformance: Average";
            else
                return $"Average CPU Usage: {averageUsage:F2}%\nPerformance: Need to Upgrade";
        }

        // Back Button Clicked
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to go back?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Navigate to the main window
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close(); // Close the current window
            }
        }

        // Next Button Clicked
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder for Next button logic
            MessageBox.Show("Next button clicked.");
        }
    }
}
