using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Weather.Properties;

namespace Weather
{
    internal class WeatherInfo
    {
        public static string cityname = "Нефтегорск";

        public string name, enname;
        public string day, night, sos, date;
        public short num;
        public string web;
        public MaterialDesignThemes.Wpf.PackIconKind iconKind;
        public static List<StackPanel> stacks = new List<StackPanel>();

        private const string nc1 = "<li class='tab-w'><div class='day-week'>",
                             c2 = "</div><div class='numbers-month'>",
                             c1 = "<li class='tab-w current'><div class='day-week'>";

        public static string source = "https://world-weather.ru/pogoda/russia/neftegorsk/";
        private string citynameweb = "},\"name\": \"";
        public static int weekday;
        public static string str;

        private MainWindow _w;
        public WeatherInfo(string name, string enname, short num, MainWindow w)
        {
            _w = w;
            this.name = name;
            this.num = num;
            this.enname = enname;
            GetWeather();
            SetText();
            
        }
        public void SetText()
        {
            if (weekday == num)
            {
                _w.TextDate.Text = date;
                _w.TextDay.Text = "Днём" + day;
                _w.TextNight.Text = "Ночью" + night;
                _w.TextSos.Text = sos;
                _w.SosIcon.Kind = iconKind;
            }
            
                StackPanel panel = new StackPanel();
                _w.Pogodi.Children.Add(panel);
                stacks.Add(panel);
                panel.Style = weekday == num ? (Style)_w.Resources["SelectedDayBG"] : num <= 5 ? (Style)_w.Resources["WorkDayBG"] : (Style)_w.Resources["WeekDayBG"];
                Grid.SetColumn(panel, num - 1);            
                TextBlock dateTextBlock = new TextBlock();
                panel.Children.Add(dateTextBlock);
                dateTextBlock.Text = date;
                dateTextBlock.FontSize = 25;
                dateTextBlock.FontWeight = FontWeights.DemiBold;
                dateTextBlock.VerticalAlignment = VerticalAlignment.Center;
                dateTextBlock.Margin = new Thickness(10);
                dateTextBlock.TextAlignment = TextAlignment.Center;
                //
                TextBlock dayTempTextBlock = new TextBlock();
                panel.Children.Add(dayTempTextBlock);
                dayTempTextBlock.Text = day;
                dayTempTextBlock.FontSize = 20;
                dayTempTextBlock.FontWeight = FontWeights.DemiBold;
                dayTempTextBlock.VerticalAlignment = VerticalAlignment.Center;
                dayTempTextBlock.TextAlignment = TextAlignment.Center;
                //
                MaterialDesignThemes.Wpf.PackIcon icon = new MaterialDesignThemes.Wpf.PackIcon();
                panel.Children.Add(icon);
                icon.Kind = iconKind;
                icon.HorizontalAlignment = HorizontalAlignment.Center;
                icon.Height = 60;
                icon.Width = 60;
        }
        public static void Connect()
        {
            DateTime dateValue = DateTime.Now;
            weekday = Convert.ToInt32(dateValue.DayOfWeek);

            WebRequest request = WebRequest.Create(source);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            str = sr.ReadToEnd();
        }
        public void GetWeather()
        {

            try
            {
                Connect();

                cityname = str.Substring(str.IndexOf(citynameweb) + citynameweb.Length);
                cityname = cityname.Remove(cityname.IndexOf(","));
                _w.CityNameLabel.Content = cityname;

                if (cityname == "Нефтегорск")
                {
                    _w.HomeIcon.Width = 30;
                }
                else
                {
                    _w.HomeIcon.Width = 0;
                }

                //Получение HTML-кода сайта
                string sub;
                /*if (num <= 5)
                   sub = weekday == num ? "<li class='tab-w current'><div class='day-week'>" + name + "</div><div class='numbers-month'>" : "<li class='tab-w'><div class='day-week'>" + name + "</div><div class='numbers-month'>";
                else
                   sub = weekday == num ? "<li class='tab-w weekend current'><div class='day-week'>" + name + "</div><div class='numbers-month'>" : "</li> <li class='tab-w weekend'><div class='day-week'>" + name + "</div><div class='numbers-month'>";*/
                sub = num <= 5 ? "<li class='tab-w'><div class='day-week'>" + name + "</div><div class='numbers-month'>" : "</li> <li class='tab-w weekend'><div class='day-week'>" + name + "</div><div class='numbers-month'>";


                    web = str.Substring(str.IndexOf(sub) + sub.Length); 
                web = web.Remove(web.IndexOf("</li>"));

                //Получение даты
                const string Hmonth = "<div class='month'>";

                date = web.Remove(web.IndexOf("</div>"));
                date = date + " " + web.Substring(web.IndexOf(Hmonth) + Hmonth.Length);
                date = date.Remove(date.IndexOf("</div>"));

                //Получение температуры днем
                const string Hday = "<div class='day-temperature'>";

                day = web.Substring(web.IndexOf(Hday) + Hday.Length);
                day = day.Remove(day.IndexOf("</div>"));
                day = day.IndexOf("</span>") == -1 ? day : day.Substring(day.IndexOf("</span>") + 7);

                //Получение температуры ночью
                const string Hnight = "<div class='night-temperature'>";

                night = web.Substring(web.IndexOf(Hnight) + Hnight.Length);
                night = night.Remove(night.IndexOf("</div>"));
                night = night.IndexOf("</span>") == -1 ? night : night.Substring(night.IndexOf("</span>") + 7);

                //Получение информации об осадках
                const string Hsos = " tooltip' title='";

                sos = web.Substring(web.IndexOf(Hsos) + Hsos.Length);
                sos = sos.Remove(sos.IndexOf("'><"));

                GetWeatherIcon();
            
            }
            catch (System.Net.WebException)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Hide();
                NoConnection noConnection = new NoConnection();
                noConnection.Show();
            }
        }
        private void GetWeatherIcon()
        {
            switch (sos)
                {
                    case "Преимущественно облачно":
                    case "Пасмурно":
                    case "Частично облачно":
                    case "Незначительная облачность":
                        iconKind = MaterialDesignThemes.Wpf.PackIconKind.WeatherCloudy;
                        break;
                    case "Местами сильный дождь":
                    case "Сильный дождь":
                    case "Слабый дождь":
                    case "Дождь":
                        iconKind = MaterialDesignThemes.Wpf.PackIconKind.WeatherHeavyRain;
                        break;
                    case "Кратковременные осадки":
                        iconKind = MaterialDesignThemes.Wpf.PackIconKind.WeatherPouring;
                        break;
                    case "Ясно":
                        iconKind = MaterialDesignThemes.Wpf.PackIconKind.WhiteBalanceSunny;
                        break;
                    case "Дождь с грозой":
                        iconKind = MaterialDesignThemes.Wpf.PackIconKind.WeatherLightningRainy;
                        break;
                    case "Небольшой снег с дождем":
                    case "Снег с дождем":
                        iconKind = MaterialDesignThemes.Wpf.PackIconKind.WeatherSnowyRainy;
                        break;
                case "Снег":
                case "Облачно и слабый снег":
                    iconKind = MaterialDesignThemes.Wpf.PackIconKind.WeatherSnowy;
                    break;
                default:
                        iconKind = MaterialDesignThemes.Wpf.PackIconKind.WeatherCloudyAlert;
                        break;
                }
        }
        
    }
}
