using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using System.Windows.Markup;

namespace JSON_Nexus.Windows
{
    /// <summary>
    /// Interaction logic for CreateObjectWindow.xaml
    /// </summary>
    public partial class CreateObjectWindow : Window
    {
        private List<StackPanel> dynamicSPs;
        public string parentName = string.Empty;

        public Dictionary<string, object> data;
        public Dictionary<object, object> dataChild;

        public CreateObjectWindow()
        {
            InitializeComponent();

            dynamicSPs = new List<StackPanel>();
            data = new Dictionary<string, object>();
            dataChild = new Dictionary<object, object>();

            CreateProperty();
        }
        private void AddPropertyBTN_Click(object sender, RoutedEventArgs e)
        {
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
                Text = "Name Of Child Property",
                Margin = new Thickness(0, 10, 0, 0)
            };

            TextBox textBox = new TextBox
            {
                Name = "nameOfChildProperty",
                Margin = new Thickness(13, 10, 0, 0),
                Width = 120,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            TextBlock textBlock2 = new TextBlock
            {
                Text = "Value Of Child Property",
                Margin = new Thickness(9, 10, 0, 0)
            };

            TextBox textBox2 = new TextBox
            {
                Name = "valueOfChildProperty",
                Margin = new Thickness(10, 10, 0, 0),
                Width = 120,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            TextBlock textblock3 = new TextBlock
            {
                Text = "Datetype",
                Margin = new Thickness(10, 10, 0, 0)
            };

            ComboBox comboBox = new ComboBox
            {
                Margin = new Thickness(10, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 20,
                Width = 80,
                FontSize = 11,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                ItemsSource = new List<string> { "string", "int", "boolean", "double", "decimal", "float" }       
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
            sp.Children.Add(textblock3);
            sp.Children.Add(comboBox);
            sp.Children.Add(button);
            dynamicSPs.Add(sp);

            childPropertiesSP.Children.Add(sp);
        }

        private void RemoveObjectBTN(object sender, RoutedEventArgs e)
        {
            StackPanel parentStackPanel = (StackPanel)((Button)sender).Parent;
            // StackPanel parentStackPanel = FindParentStackPanel((Button)sender);

            dynamicSPs.Remove(parentStackPanel);
            childPropertiesSP.Children.Remove(parentStackPanel);
        }

        //private StackPanel FindParentStackPanel(DependencyObject child)
        //{
        //    DependencyObject parent = VisualTreeHelper.GetParent(child);

        //    while (parent != null && !(parent is StackPanel))
        //    {
        //        parent = VisualTreeHelper.GetParent(parent);
        //    }

        //    return parent as StackPanel;
        //}

        private List<string> RetrieveValues()
        {
            List<string> content = new List<string>();
            
            foreach (StackPanel stackPanel in dynamicSPs)
            {
                string nameOfChild = string.Empty;
                string valueOfChild = string.Empty;
                string datatype = string.Empty;

                foreach (TextBox textBox in stackPanel.Children.OfType<TextBox>())
                {
                    switch (textBox.Name)
                    {
                        case "nameOfChildProperty":
                            nameOfChild = textBox.Text;
                            break;

                        case "valueOfChildProperty":
                            valueOfChild = $"'{textBox.Text}'";
                            break;

                        default:
                            break;
                    }
                }

                foreach (ComboBox combo in stackPanel.Children.OfType<ComboBox>())
                {
                    datatype = combo.Text;
                }

                content.Add($"{nameOfChild} {valueOfChild} {datatype}");

            }

            return content;
        }

        private void AddObjectBTN_Click(object sender, RoutedEventArgs e)
        {
            //Dictionary<string, object> data = new Dictionary<string, object>();
            
            List<string> content = RetrieveValues();

            foreach (string item in content)
            {
                string[] split = item.Split(' ');

                List<string> formattedSplit = new List<string>();
                formattedSplit.Add(split[0]);

                if (split.Length > 2)
                {
                    string secondSplit = string.Join(" ", split.Skip(1).Take(split.Length - 2));
                    if (secondSplit.StartsWith("'") && secondSplit.EndsWith("'"))
                    {
                        formattedSplit.Add(secondSplit.Trim('\''));
                    }
                    else
                    {
                        formattedSplit.AddRange(secondSplit.Split(' '));
                    }
                }

                formattedSplit.Add(split[split.Length - 1]);

                List<object> values;
                object value;

                switch (formattedSplit[2].ToLower())
                {
                    case "string":
                        if (formattedSplit[1].Contains(','))
                        {
                            values = GetSegments(formattedSplit[1], ',');

                            if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(values) ||
                                dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(values))
                            {
                                dataChild[formattedSplit[0]] = values;
                            }
                        }
                        else
                        {
                            value = formattedSplit[1];

                            if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(value) ||
                                dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(value))
                            {
                                dataChild[formattedSplit[0]] = value;
                            }
                        }
                        break;
                    case "int":
                        if (formattedSplit[1].Contains(','))
                        {
                            values = GetSegments(formattedSplit[1], ',');

                            List<int> ints = new List<int>();

                            foreach (object _item in values)
                            {
                                if(int.TryParse(_item.ToString(), out int parsed))
                                {
                                    ints.Add(parsed);
                                }
                            }

                            if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(ints) ||
                                dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(ints))
                            {
                                dataChild[formattedSplit[0]] = ints;
                            }
                        }
                        else
                        {
                            value = formattedSplit[1];

                            if(int.TryParse(value.ToString(), out int parsed))
                            {
                                if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed) ||
                                    dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed))
                                {
                                    dataChild[formattedSplit[0]] = parsed;
                                }
                            }
                        }
                        break;
                    case "boolean":
                        if (formattedSplit[1].Contains(','))
                        {
                            values = GetSegments(formattedSplit[1], ',');

                            List<bool> booleans = new List<bool>();

                            foreach (object _item in values)
                            {
                                if (bool.TryParse(_item.ToString(), out bool parsed))
                                {
                                    booleans.Add(parsed);
                                }
                            }

                            if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(booleans) ||
                                dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(booleans))
                            {
                                dataChild[formattedSplit[0]] = booleans;
                            }
                        }
                        else
                        {
                            value = formattedSplit[1];

                            if (bool.TryParse(value.ToString(), out bool parsed))
                            {
                                if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed) ||
                                    dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed))
                                {
                                    dataChild[formattedSplit[0]] = parsed;
                                }
                            }
                        }
                        break;
                    case "double":
                        if (formattedSplit[1].Contains(','))
                        {
                            values = GetSegments(formattedSplit[1], ',');

                            List<double> doubles = new List<double>();

                            foreach (object _item in values)
                            {
                                if (double.TryParse(_item.ToString(), out double parsed))
                                {
                                    doubles.Add(parsed);
                                }
                            }

                            if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(doubles) ||
                                dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(doubles))
                            {
                                dataChild[formattedSplit[0]] = doubles;
                            }
                        }
                        else
                        {
                            value = formattedSplit[1];

                            if (double.TryParse(value.ToString(), out double parsed))
                            {
                                if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed) ||
                                    dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed))
                                {
                                    dataChild[formattedSplit[0]] = parsed;
                                }
                            }
                        }
                        break;
                    case "decimal":
                        if (formattedSplit[1].Contains(','))
                        {
                            values = GetSegments(formattedSplit[1], ',');

                            List<decimal> decimals = new List<decimal>();

                            foreach (object _item in values)
                            {
                                if (decimal.TryParse(_item.ToString(), out decimal parsed))
                                {
                                    decimals.Add(parsed);
                                }
                            }

                            if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(decimals) ||
                                dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(decimals))
                            {
                                dataChild[formattedSplit[0]] = decimals;
                            }
                        }
                        else
                        {
                            value = formattedSplit[1];

                            if (decimal.TryParse(value.ToString(), out decimal parsed))
                            {
                                if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed) ||
                                    dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed))
                                {
                                    dataChild[formattedSplit[0]] = parsed;
                                }
                            }
                        }
                        break;
                    case "float":
                        if (formattedSplit[1].Contains(','))
                        {
                            values = GetSegments(formattedSplit[1], ',');

                            List<float> floats = new List<float>();

                            foreach (object _item in values)
                            {
                                if (float.TryParse(_item.ToString(), out float parsed))
                                {
                                    floats.Add(parsed);
                                }
                            }

                            if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(floats) ||
                                dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(floats))
                            {
                                dataChild[formattedSplit[0]] = floats;
                            }
                        }
                        else
                        {
                            value = formattedSplit[1];

                            if (float.TryParse(value.ToString(), out float parsed))
                            {
                                if (!dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed) ||
                                    dataChild.ContainsKey(formattedSplit[0]) && !dataChild.ContainsValue(parsed))
                                {
                                    dataChild[formattedSplit[0]] = parsed;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            data[nameOfParentProperty.Text] = dataChild;
            parentName = nameOfParentProperty.Text;

            Close();
        }

        private List<object> GetSegments(string fullText, char separator)
        {
            string[] splitSegments = fullText.Split(separator);
            List<object> segments = new List<object>();

            segments.AddRange(splitSegments);

            return segments;
        }
    }
}
