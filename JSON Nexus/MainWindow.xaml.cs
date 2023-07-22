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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Text.Json;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using JSON_Nexus.Functions;
using JSON_Nexus.Windows;
using System.Runtime.Remoting.Messaging;

namespace JSON_Nexus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\JsonData.json";
        private string json = string.Empty;

        private JObject currentJson;
        private Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        private CookieContainer container;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            container = new CookieContainer();
            dataTypeCBox.Text = "string";
            RequestMethod.Text = "GET";
            BodyType.Text = "JSON";
        }

        #region ReadJSON

        private void OpenBTN(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "JSON Files (*.json)|*.json|Other (*.*)|*.*"
            };

            ofd.ShowDialog();
            
            if (ofd.FileName != string.Empty)
            {
                locationOfJSON.Text = ofd.FileName;

                string json = DisplayJSON.GetJSON(locationOfJSON.Text);

                try
                {
                    currentJson = JObject.Parse(json);
                    JsonData.Text = currentJson.ToString();
                }
                catch 
                {
                    JsonData.Text = json;
                }
            }
        }

        private void ReadBTN(object sender, RoutedEventArgs e)
        {
            string json = DisplayJSON.GetJSON(locationOfJSON.Text);

            try
            {
                currentJson = JObject.Parse(json);
                JsonData.Text = currentJson.ToString();
            }
            catch 
            {
                JsonData.Text = json;
            }
        }
        
        private void EnableTB_Checked(object sender, RoutedEventArgs e)
        {
            JsonData.IsReadOnly = false;
            findBTN.IsEnabled = true;
            findObject.IsEnabled = true;
            readPathBTN.IsEnabled = true;
            readObject.IsEnabled = true;
            ReplaceBTN.IsEnabled = true;
            insteadOfObject.IsEnabled = true;
            withObject.IsEnabled = true;
            withtb.Foreground = Brushes.Black;
        }

        private void EnableTB_Unchecked(object sender, RoutedEventArgs e)
        {
            JsonData.IsReadOnly = true;
            findBTN.IsEnabled = false;
            findObject.IsEnabled = false;
            readPathBTN.IsEnabled = false;
            readObject.IsEnabled = false;
            ReplaceBTN.IsEnabled = false;
            insteadOfObject.IsEnabled = false;
            withObject.IsEnabled = false;
            withtb.Foreground = Brushes.Gray;
        }

        private void JsonData_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (JsonData.Text != string.Empty)
            {
                enableTB.IsEnabled = true;
                saveBTN.IsEnabled = true;
            }
        }

        private void saveBTN_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult save = MessageBox.Show("Save to the same file?", "JSON Nexus", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (save == MessageBoxResult.Yes)
            {
                if (locationOfJSON.Text.StartsWith("http") || locationOfJSON.Text.StartsWith("json:"))
                {
                    File.WriteAllText(defaultPath, JsonData.Text);
                }
                else
                {
                    File.WriteAllText(locationOfJSON.Text, JsonData.Text);
                }

                MessageBox.Show("Successfully saved!", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (save == MessageBoxResult.No)
            {
                SaveFileDialog sfd = new SaveFileDialog 
                { 
                    Filter = "JSON Files (*.json)|*.json|Other (*.*)|*.*" // Text Files (*.txt)|*.txt|
                }; 
                sfd.ShowDialog();

                if (sfd.FileName != string.Empty)
                {
                    File.WriteAllText(sfd.FileName, JsonData.Text);
                    MessageBox.Show("Successfully saved!", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                return;
            }
        }

        private void FindBTN_Click(object sender, RoutedEventArgs e)
        {
            List<string> found = DisplayJSON.Find(currentJson, findObject.Text);

            if(found.Count > 0)
            {
                JsonWindow jsonWindow = new JsonWindow();
                jsonWindow.RetrieveData(found);
                jsonWindow.Show();
            }
            else
            {
                MessageBox.Show("Nothing Found.", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ReadPathBTN_Click(object sender, RoutedEventArgs e)
        {
            JToken token = DisplayJSON.ReadPath(currentJson, readObject.Text);

            if (token != null)
            {
                JsonWindow jsonWindow = new JsonWindow();
                jsonWindow.RetrieveData(token.ToString());
                jsonWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Nothing Found.", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ReplaceBTN_Click(object sender, RoutedEventArgs e)
        {
            if (insteadOfObject.Text.Length > 0)
                JsonData.Text = JsonData.Text.Replace(insteadOfObject.Text, withObject.Text);
        }

        #region PlaceHolders

        private void LocationOfJSON_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "C:/path/to/json or https://example.com/json")
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void LocationOfJSON_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "")
            {
                textBox.Text = "C:/path/to/json or https://example.com/json";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void LocationOfJSON_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(locationOfJSON.Text != "C:/path/to/json or https://example.com/json" && locationOfJSON.Text.Length > 0)
            {
                locationOfJSON.Foreground = Brushes.Black;
            }
            else
            {
                locationOfJSON.Foreground = Brushes.Gray;
            }
        }

        #endregion

        #endregion

        #region CreateJSON

        private void AddItemBTN_Click(object sender, RoutedEventArgs e)
        {
            if(nameOfProperty.Text == string.Empty ||
               valueOfProperty.Text == string.Empty ||
               dataTypeCBox.Text == string.Empty)
            {
                return;
            }

            AddItems();

            itemstb.AppendText($"{nameOfProperty.Text} - {valueOfProperty.Text} - {dataTypeCBox.Text}\n");


            nameOfProperty.Text = string.Empty;
            valueOfProperty.Text = string.Empty;
        }

        private void AddJsonObjectBTN_Click(object sender, RoutedEventArgs e)
        {
            CreateObjectWindow createObject = new CreateObjectWindow();
            createObject.ShowDialog();

            if(createObject.parentName == string.Empty)
            {
                return;
            }

            keyValuePairs[createObject.parentName] = createObject.dataChild;
            itemstb.AppendText($"PName: {createObject.parentName} -");

            for (int i = 0; i < createObject.dataChild.Count; i++)
            {
                itemstb.AppendText($" CKey: {createObject.dataChild.Keys.ElementAt(i)} - CValue: {createObject.dataChild.Values.ElementAt(i)} ");
            }

            itemstb.AppendText(Environment.NewLine);
        }

        private void ConvertBTN_Click(object sender, RoutedEventArgs e)
        {
            json = CreateJSON.Create(keyValuePairs);

            if (keyValuePairs.Count > 0)
                jsontb.Text = $"{JsonConvert.DeserializeObject(json)}\n\nMinified ↓\n{json}";
        }

        private void AddItems()
        {
            List<object> values;
            object value;

            switch (dataTypeCBox.Text)
            {
                case "string":
                    if (valueOfProperty.Text.Contains(','))
                    {
                        values = GetSegments(valueOfProperty.Text, ',');

                        if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(values) ||
                           keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(values))
                        {
                            keyValuePairs[nameOfProperty.Text] = values;
                        }
                    }
                    else
                    {
                        value = valueOfProperty.Text;

                        if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(value) ||
                            keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(value))
                        {
                            keyValuePairs[nameOfProperty.Text] = value;
                        }
                    }
                    break;

                case "int":
                    if (valueOfProperty.Text.Contains(','))
                    {
                        values = GetSegments(valueOfProperty.Text, ',');

                        List<int> ints = new List<int>();

                        foreach (object item in values)
                        {
                            if (int.TryParse(item.ToString(), out int parsed))
                            {
                                ints.Add(parsed);
                            }
                        }

                        if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(ints) ||
                            keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(ints))
                        {
                            keyValuePairs[nameOfProperty.Text] = ints;
                        }
                    }
                    else
                    {
                        value = valueOfProperty.Text;

                        if (int.TryParse(value.ToString(), out int parsed))
                        {
                            if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed) ||
                                 keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed))
                            {
                                keyValuePairs[nameOfProperty.Text] = parsed;
                            }
                        }
                    }
                    break;

                case "boolean":
                    if (valueOfProperty.Text.Contains(','))
                    {
                        values = GetSegments(valueOfProperty.Text, ',');

                        List<bool> booleans = new List<bool>();

                        foreach (object item in values)
                        {
                            if (bool.TryParse(item.ToString(), out bool parsed))
                            {
                                booleans.Add(parsed);
                            }
                        }

                        if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(booleans) ||
                            keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(booleans))
                        {
                            keyValuePairs[nameOfProperty.Text] = booleans;
                        }
                    }
                    else
                    {
                        value = valueOfProperty.Text;

                        if (bool.TryParse(value.ToString(), out bool parsed))
                        {
                            if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed) ||
                                 keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed))
                            {
                                keyValuePairs[nameOfProperty.Text] = parsed;
                            }
                        }
                    }
                    break;

                case "double":
                    if (valueOfProperty.Text.Contains(','))
                    {
                        values = GetSegments(valueOfProperty.Text, ',');

                        List<double> doubles = new List<double>();

                        foreach (object item in values)
                        {
                            if (double.TryParse(item.ToString(), out double parsed))
                            {
                                doubles.Add(parsed);
                            }
                        }

                        if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(doubles) ||
                            keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(doubles))
                        {
                            keyValuePairs[nameOfProperty.Text] = doubles;
                        }
                    }
                    else
                    {
                        value = valueOfProperty.Text;

                        if (double.TryParse(value.ToString(), out double parsed))
                        {
                            if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed) ||
                                 keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed))
                            {
                                keyValuePairs[nameOfProperty.Text] = parsed;
                            }
                        }
                    }
                    break;

                case "decimal":
                    if (valueOfProperty.Text.Contains(','))
                    {
                        values = GetSegments(valueOfProperty.Text, ',');

                        List<decimal> decimals = new List<decimal>();

                        foreach (object item in values)
                        {
                            if (decimal.TryParse(item.ToString(), out decimal parsed))
                            {
                                decimals.Add(parsed);
                            }
                        }

                        if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(decimals) ||
                            keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(decimals))
                        {
                            keyValuePairs[nameOfProperty.Text] = decimals;
                        }
                    }
                    else
                    {
                        value = valueOfProperty.Text;

                        if (decimal.TryParse(value.ToString(), out decimal parsed))
                        {
                            if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed) ||
                                 keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed))
                            {
                                keyValuePairs[nameOfProperty.Text] = parsed;
                            }
                        }
                    }
                    break;

                case "float":
                    if (valueOfProperty.Text.Contains(','))
                    {
                        values = GetSegments(valueOfProperty.Text, ',');

                        List<float> floats = new List<float>();

                        foreach (object item in values)
                        {
                            if (float.TryParse(item.ToString(), out float parsed))
                            {
                                floats.Add(parsed);
                            }
                        }

                        if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(floats) ||
                            keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(floats))
                        {
                            keyValuePairs[nameOfProperty.Text] = floats;
                        }
                    }
                    else
                    {
                        value = valueOfProperty.Text;

                        if (float.TryParse(value.ToString(), out float parsed))
                        {
                            if (!keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed) ||
                                 keyValuePairs.ContainsKey(nameOfProperty.Text) && !keyValuePairs.ContainsValue(parsed))
                            {
                                keyValuePairs[nameOfProperty.Text] = parsed;
                            }
                        }
                    }
                    break;

                default:
                    MessageBox.Show("Invalid datatype");
                    return;
            }
        }

        private List<object> GetSegments(string fullText, char separator)
        {
            string[] splitSegments = fullText.Split(separator);
            List<object> segments = new List<object>();

            segments.AddRange(splitSegments);

            return segments;
        }

        #endregion

        #region Web Requests

        private void SendBTN_Click(object sender, RoutedEventArgs e)
        {
            SendRequest().GetAwaiter();
        }

        private async Task SendRequest()
        {
            responseTXT.Text = string.Empty;
            string url = urlTXT.Text;
            string authorization = authTXT.Text;

            if (url == string.Empty)
                return;

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                if (container.Count > 0) 
                {
                    handler.CookieContainer = container;
                }

                using (HttpClient client = new HttpClient(handler))
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(RequestMethod.Text), url))
                    {
                        if (headersTXT.Text != string.Empty)
                        {
                            string[] lines = headersTXT.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (string line in lines)
                            {
                                int index = line.IndexOf(':');
                                string headerName = line.Substring(0, index);
                                string headerValue = line.Substring(index + 1);
                                headerValue = headerValue.Trim();

                                request.Headers.Add(headerName, headerValue);
                            }
                        }

                        if (authorization != string.Empty && !headersTXT.Text.Contains("Authorization:"))
                        {
                            request.Headers.Add("Authorization", authorization);
                        }

                        if (request.Method != HttpMethod.Get && 
                            request.Method != HttpMethod.Delete && 
                            request.Method != HttpMethod.Options)
                        {
                            StringContent content;

                            switch (BodyType.Text)
                            {
                                case "JSON":
                                    content = new StringContent(bodyTXT.Text, Encoding.UTF8, "application/json");
                                    break;

                                case "Form URL Encoded":
                                    content = new StringContent(bodyTXT.Text, Encoding.UTF8, "application/x-www-form-urlencoded");
                                    break;

                                case "Text / Plain":
                                    content = new StringContent(bodyTXT.Text, Encoding.UTF8, "text/plain");
                                    break;

                                default:
                                    MessageBox.Show("Invalid Body Type", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                            }

                            request.Content = content;
                        }

                        HttpResponseMessage response = await client.SendAsync(request);

                        responseTXT.AppendText("----Response Headers----\n");
                        responseTXT.AppendText($"Status Code: {(int)response.StatusCode} {response.StatusCode}\n");
                        responseTXT.AppendText($"Version: {response.Version}\n");
                        responseTXT.AppendText($"Request Message: {response.RequestMessage}\n");
                        responseTXT.AppendText($"Headers: \n{response.Headers} \n");
                        responseTXT.AppendText("----Response Data----\n");

                        try
                        {
                            responseTXT.AppendText(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()).ToString());
                        }
                        catch
                        {
                            responseTXT.AppendText(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
            }
        }

        private void AddCookies_Click(object sender, RoutedEventArgs e)
        {
            if(urlTXT.Text == string.Empty)
            {
                MessageBox.Show("Cannot add cookies, URL text box is empty.");
                return;
            }

            AddCookies cookieForm = new AddCookies { url = new Uri(urlTXT.Text) };
            cookieForm.ShowDialog();

            container = cookieForm.container;
        }

        private void LoadPayload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.ShowDialog();

            if(opf.FileName != string.Empty)
            {
                bodyTXT.Text = File.ReadAllText(opf.FileName);
            }
        }

        #endregion

        private void AddDefaultHeaders_Click(object sender, RoutedEventArgs e)
        {
            headersTXT.AppendText($"Accept: */* {Environment.NewLine}");
            headersTXT.AppendText($"Accept-Language: en-US {Environment.NewLine}");
            headersTXT.AppendText($"User-Agent: Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox/12.0 {Environment.NewLine}");
            headersTXT.AppendText($"Cache-Control: no-store");
        }

        private void ClearCookies_Click(object sender, RoutedEventArgs e)
        {
            container = new CookieContainer();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            container = new CookieContainer();
            headersTXT.Text = string.Empty;
            urlTXT.Text = string.Empty;
            bodyTXT.Text = string.Empty;
            responseTXT.Text = string.Empty;
        }
    }
}
