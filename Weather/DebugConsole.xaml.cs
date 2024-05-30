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
    /// Логика взаимодействия для DebugConsole.xaml
    /// </summary>
    public partial class DebugConsole : Window
    {
        public DebugConsole()
        {
            InitializeComponent();
        }
        public void Write(string text)
        {
            ct.Text = text;
        }
        public void WriteLine(string text)
        {
            ct.Text = ct.Text + System.Environment.NewLine + text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ct.FontSize = (ct.FontSize + 5) < 50 ? ct.FontSize + 5 : ct.FontSize;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ct.FontSize = (ct.FontSize - 10) > 5 ? ct.FontSize - 5 : ct.FontSize;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ct.IsReadOnly = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ct.IsReadOnly = true;
        }

        private void ct_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox s = (TextBox)sender;
            if (s.Text.Contains("command ; clear"))
            {
                s.Text = "cоmmand ; help - вызов всех доступных команд";
            }
            if (s.Text.Contains("command ; help"))
            {
                s.Text = s.Text.Remove(s.Text.IndexOf("command ; help"));
                s.Text = s.Text + System.Environment.NewLine + "command ; clear - очистить косноль";
            }
        }
    }
}
