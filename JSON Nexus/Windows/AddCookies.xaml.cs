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
using System.Net.Http;
using System.Net;

namespace JSON_Nexus.Windows
{
    /// <summary>
    /// Interaction logic for AddCookies.xaml
    /// </summary>
    public partial class AddCookies : Window
    {
        public Uri url;
        private List<StackPanel> dynamicSPs;
        public CookieContainer container;

        public AddCookies()
        {
            dynamicSPs = new List<StackPanel>();
            container = new CookieContainer();

            InitializeComponent();
            CreateProperty();
        }

        private void CreateProperty()
        {
            StackPanel sp = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            TextBlock textBlock = new TextBlock
            {
                Text = "Cookie Name",
                Margin = new Thickness(0, 10, 0, 0)
            };

            TextBox textBox = new TextBox
            {
                Name = "cookieName",
                Margin = new Thickness(13, 10, 0, 0),
                Width = 120,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            TextBlock textBlock2 = new TextBlock
            {
                Text = "Cookie Value",
                Margin = new Thickness(9, 10, 0, 0)
            };

            TextBox textBox2 = new TextBox
            {
                Name = "cookieValue",
                Margin = new Thickness(10, 10, 0, 0),
                Width = 120,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            Button button = new Button
            {
                Content = "Remove",
                Name = "RemoveObjectBTN",
                Width = 70,
                Height = 20,
                Margin = new Thickness(20, 10, 0, 0),
            };

            button.Click += RemoveObjectBTN;

            sp.Children.Add(textBlock);
            sp.Children.Add(textBox);
            sp.Children.Add(textBlock2);
            sp.Children.Add(textBox2);
            sp.Children.Add(button);
            dynamicSPs.Add(sp);

            cookieContainer.Children.Add(sp);
        }

        private CookieContainer RetrieveValues()
        {
            CookieContainer cookie = new CookieContainer();

            foreach (StackPanel stackPanel in dynamicSPs)
            {
                string cookieName = string.Empty;
                string cookieValue = string.Empty;

                foreach (TextBox textBox in stackPanel.Children.OfType<TextBox>())
                {
                    switch (textBox.Name)
                    {
                        case "cookieName":
                            cookieName = textBox.Text;
                            break;

                        case "cookieValue":
                            cookieValue = textBox.Text;
                            break;

                        default:
                            break;
                    }
                }

                cookie.Add(url, new Cookie(cookieName, cookieValue));
            }

            return cookie;
        }

        private void RemoveObjectBTN(object sender, RoutedEventArgs e)
        {
            StackPanel parentStackPanel = (StackPanel)((Button)sender).Parent;

            dynamicSPs.Remove(parentStackPanel);
            cookieContainer.Children.Remove(parentStackPanel);
        }

        private void AddCookieProperty_Click(object sender, RoutedEventArgs e)
        {
            CreateProperty();
        }

        private void AddCookieToRequest_Click(object sender, RoutedEventArgs e)
        {
            container = RetrieveValues();
            Close();
        }
    }
}
