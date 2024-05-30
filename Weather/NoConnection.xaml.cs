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

namespace Weather
{
    /// <summary>
    /// Логика взаимодействия для NoConnection.xaml
    /// </summary>
    public partial class NoConnection : Window
    {
        
        public NoConnection()
        {
            InitializeComponent();
            
        }

        public static void ChangeMayReason(string text)
        {
           
        }
        private void Shutdown(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Tryagain(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow;
            try
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
                WeatherInfo.Connect();
                Hide();
            } catch (System.Net.WebException)
            {

            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
