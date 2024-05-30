using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignColors;

namespace Weather
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DebugConsole debugConsole = new DebugConsole();

        WeatherInfo mon;
        WeatherInfo tue;
        WeatherInfo wed;
        WeatherInfo thu;
        WeatherInfo fri;
        WeatherInfo sat;
        WeatherInfo sun;
        WeatherInfo[] array;
        StackPanel soopp;
        public string[] Themes { get; set; }
        public MainWindow()
        {
            WeatherInfo.stacks.Clear();
            WeatherInfo.source = "https://world-weather.ru/pogoda/russia/neftegorsk/";
            bool catched = false;
            InitializeComponent();
            try
            {
                WeatherInfo.Connect();
            }
            catch 
            {
                catched = true;
                Hide();
                NoConnection noConnection = new NoConnection();
                noConnection.Show();
            }
            if (!catched)
            {
                mon = new WeatherInfo("Понедельник", "mon", 1, this);
                tue = new WeatherInfo("Вторник", "tue", 2, this);
                wed = new WeatherInfo("Среда", "wed", 3, this);
                thu = new WeatherInfo("Четверг", "thu", 4, this);
                fri = new WeatherInfo("Пятница", "fri", 5, this);
                sat = new WeatherInfo("Суббота", "sat", 6, this);
                sun = new WeatherInfo("Воскресенье", "sun", 7, this);
                WeatherInfo[] array = { mon, tue, wed, thu, fri, sat, sun };
                this.array = array;
                Themes = new string[] { "Небесная тема                                                                          ", "Светлая тема", "Темная тема" };
                DataContext = this;
                soopp = WeatherInfo.stacks[0];
            }
        }

        //Цветовая палитра
        public Brush color1 = (Brush)new BrushConverter().ConvertFromString("#CAF0F8");
        public Brush color2 = (Brush)new BrushConverter().ConvertFromString("#ade8f4");
        public Brush color3 = (Brush)new BrushConverter().ConvertFromString("#90e0ef");
        public Brush color4 = (Brush)new BrushConverter().ConvertFromString("#48cae4");
        public Brush color5 = (Brush)new BrushConverter().ConvertFromString("#00b4d8");
        private enum SelectedTheme {Black, White, Blue}
        private void ToGithub(object sender, RoutedEventArgs e)
        {
            var sInfo = new System.Diagnostics.ProcessStartInfo("https://github.com/Hazzz895?tab=repositories")
            {
                UseShellExecute = true,
            };
            System.Diagnostics.Process.Start(sInfo);
        }
        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            Popup_ChangeTown.Visibility = Visibility.Hidden;
            Popup_ChangeTheme.Visibility = Visibility.Visible;
            Popup.Visibility = Visibility.Visible;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
            doubleAnimation.From = Popup.Opacity;
            doubleAnimation.To = 1;
            Popup.BeginAnimation(OpacityProperty, doubleAnimation);
            Blur.Radius = 10;
        }
        
        private void ChangeTown(object sender, RoutedEventArgs e)
        {
            Popup_ChangeTown.Visibility = Visibility.Visible;
            Popup_ChangeTheme.Visibility = Visibility.Hidden;
            Popup.Visibility = Visibility.Visible;
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
            doubleAnimation.From = Popup.Opacity;
            doubleAnimation.To = 1;
            Popup.BeginAnimation(OpacityProperty, doubleAnimation);
            Blur.Radius = 10;
        }

        private void ChangeTownComfirm(object sender, RoutedEventArgs e)
        {
            if ((bool)ChangeTownCheckBox.IsChecked ? ChangeTownTextBox.Text.Length < 3 || ChangeCountry.Text.Length < 3 : ChangeTownTextBox.Text.Length < 3)
            {
                PopupWarnText.Text = "Поле не должно быть пустым";
                ShowPopupWarn(true);
            }
            else
            {
                if (Regex.IsMatch(ChangeTownTextBox.Text, "^[a-zA-Z0-9]*$") && Regex.IsMatch(ChangeCountry.Text, "^[a-zA-Z0-9]*$") || ChangeTownTextBox.Text.Contains('-'))
                { 
                    bool catched = false;
                    if ((bool)ChangeTownCheckBox.IsChecked)
                    {
                        try
                        {
                            WeatherInfo.source = "https://world-weather.ru/pogoda/" + ChangeCountry.Text + "/" + ChangeTownTextBox.Text + "/";
                            WeatherInfo.stacks.Clear();
                            WeatherInfo.Connect();
                        }
                        catch (System.Net.WebException)
                        {
                            NoConnection.ChangeMayReason("Название города написано некоректно");
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Hide();
                            NoConnection noConnection = new NoConnection();
                            noConnection.Show();
                            catched = true;
                        }
                        if (!catched)
                        {
                            debugConsole.WriteLine(WeatherInfo.source);
                            foreach (WeatherInfo x in array)
                            {
                                x.GetWeather();
                                x.SetText();
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            WeatherInfo.source = "https://world-weather.ru/pogoda/" + "russia/" + ChangeTownTextBox.Text + "/";
                            WeatherInfo.stacks.Clear();
                            WeatherInfo.Connect();
                        }
                        catch (System.Net.WebException)
                        {
                            NoConnection.ChangeMayReason("Название города написано некоректно");
                            Hide();
                            NoConnection noConnection = new NoConnection();
                            noConnection.Show();
                            catched = true;
                        }

                        if (!catched)
                        {
                            debugConsole.WriteLine(WeatherInfo.source);
                            foreach (WeatherInfo x in array)
                            {
                                x.GetWeather();
                                x.SetText();
                            }
                        }
                    }
                    Popup.Visibility = Visibility.Hidden;
                    Blur.Radius = 0;
                }
                else
                {
                    PopupWarnText.Text = "Города должны быть на латинице";
                    ShowPopupWarn(true);
                }
            }
        }
        
        private void ShowPopupWarn(bool show)
        {
            if (show)
            {
                PopupWarn.Visibility = Visibility.Visible;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
                doubleAnimation.From = 0;
                doubleAnimation.To = 1;
                PopupWarn.BeginAnimation(OpacityProperty, doubleAnimation);
            }
            else
            {
                //PopupWarn.Opacity = 0;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
                doubleAnimation.From = PopupWarn.Opacity;
                doubleAnimation.To = 0;
                PopupWarn.BeginAnimation(OpacityProperty, doubleAnimation);
            }
        }

        private void ShowPopupWarn(bool show, string text)
        {
            if (show)
            {
                //PopupWarn.Opacity = 1;
                PopupWarnText.Text = text;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
                doubleAnimation.From = 0;
                doubleAnimation.To = 1;
                PopupWarn.BeginAnimation(OpacityProperty, doubleAnimation);
            }
            else
            {
                //PopupWarn.Opacity = 0;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
                doubleAnimation.From = PopupWarn.Opacity;
                doubleAnimation.To = 0;
                PopupWarn.BeginAnimation(OpacityProperty, doubleAnimation);
            }
        }
        private void HidePopup(object sender, RoutedEventArgs e)
        {
            Popup.Visibility = Visibility.Hidden;
            Blur.Radius = 0;
        }

        private bool IsConsoleShowed = false;
        private void ShowOrHideConsole(object sender, RoutedEventArgs e)
        {
            if (!IsConsoleShowed)
            {
                debugConsole.Show();
                ShowConsoleText.Content = "Скрыть консоль";
            }
            else
            {
                debugConsole.Hide();
                ShowConsoleText.Content = "Показать консоль";
            }
            IsConsoleShowed = !IsConsoleShowed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ChangeDayButton(object sender, RoutedEventArgs e)
        {
            for (short i = 0; i < 7; i++)
            {

                WeatherInfo.stacks[i].Style = i <= 4 ? (Style)Resources["WorkDayBG"] : (Style)Resources["WeekDayBG"];
            }
            WeatherInfo.stacks[Grid.GetColumn((Button)sender)].Style = (Style)Resources["SelectedDayBG"];
            debugConsole.WriteLine(Grid.GetColumn((Button)sender).ToString() + WeatherInfo.stacks.Count);
            foreach (WeatherInfo x in array)
            {
                if (x.num - 1 == (Grid.GetColumn((Button)sender)))
                {
                    TextDate.Text = x.date;
                    TextDay.Text = "Днём " + x.day;
                    TextNight.Text = "Ночью " + x.night;
                    TextSos.Text = x.sos;
                    SosIcon.Kind = x.iconKind;
                }
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
            doubleAnimation.From = ChangeCountry.Opacity;
            doubleAnimation.To = 1;
            ChangeCountry.BeginAnimation(OpacityProperty, doubleAnimation);
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(100);
            doubleAnimation.From = ChangeCountry.Opacity;
            doubleAnimation.To = 0;
            ChangeCountry.BeginAnimation(OpacityProperty, doubleAnimation);
        }
        private void ChangeCountry_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowPopupWarn(false);
        }

        private void ExecutionRestart(object sender, RoutedEventArgs e)
        {
            Process.Start(Assembly.GetEntryAssembly().Location);
            Process.GetCurrentProcess().Kill();
            Application.Current.Shutdown();
        }
    }
}
