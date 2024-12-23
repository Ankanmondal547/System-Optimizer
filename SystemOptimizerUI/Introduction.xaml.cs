using System;
using System.Collections.Generic;
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
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : Window
    {
        public Introduction()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Exit the application
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to another XAML page
            var MainWindow = new MainWindow(); // Replace with your next page class
            MainWindow.Show();
            this.Close(); // Close the current window
        }
    }
}
