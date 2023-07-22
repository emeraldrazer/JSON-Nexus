using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using JSON_Nexus;

namespace JSON_Nexus.Functions
{
    public static class DisplayJSON
    {
        public static string GetJSON(string path)
        {
            string json = string.Empty;

            if (path.StartsWith("json:"))
            {
                string jsontxt = CreateJSON.Create(path.Substring(5));
                json = JsonConvert.DeserializeObject(jsontxt).ToString();
                return json;
            }

            if (!File.Exists(path) && !path.StartsWith("http"))
            {
                MessageBox.Show("File Doesnt Exist.", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Hand);
                return string.Empty;
            }

            if (path.StartsWith("http") || path.StartsWith("https"))
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        if(path.Contains(" "))
                        {
                            string[] split = path.Split(' ');
                            string authHeader = string.Empty;

                            for (int i = 0; i < split.Length - 1; i++)
                            {
                                if(i < split.Length)
                                {
                                    authHeader = string.Concat($"{authHeader}", " ", $"{split[i + 1]}");
                                }
                            }

                            try
                            {
                                client.DefaultRequestHeaders.Add("Authorization", authHeader);
                            }
                            catch (FormatException)
                            {
                                MessageBox.Show("Please delete any white spaces", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Hand);
                            }

                            HttpResponseMessage response = client.GetAsync(split[0]).Result;

                            if (response == null)
                            {
                                return response.StatusCode.ToString();
                            }

                            json = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            HttpResponseMessage response = client.GetAsync(path).Result;
                            json = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    catch (WebException e)
                    {
                        HttpWebResponse res = (HttpWebResponse)e.Response;

                        MessageBox.Show($"Cannot access URL. Status code: {(int)res.StatusCode} {res.StatusCode}", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Error);
                        return string.Empty;
                    }
                }
            }
            else
            {
                json = File.ReadAllText(path);
            }

            try
            {
                object currentJson = JsonConvert.DeserializeObject<object>(json);
                return currentJson.ToString();
            }
            catch
            {
                return json;
            }
        }

        public static JToken ReadPath(JObject jsonObject, string query)
        {
            JToken currentToken = jsonObject;
            string[] segments = query.Split('.');

            foreach (string segment in segments)
            {
                if (currentToken is JObject)
                {
                    currentToken = ((JObject)currentToken)[segment];
                }
                else if (currentToken is JArray)
                {
                    JArray jsonArray = (JArray)currentToken;

                    if (segment.StartsWith("[") && segment.EndsWith("]"))
                    {
                        int index = int.Parse(segment.Substring(1, segment.Length - 2));
                        currentToken = ((JArray)currentToken)[index];
                    }
                    else
                    {
                        foreach (JObject obj in jsonArray)
                        {
                            JToken nestedToken = obj[segment];
                            if (nestedToken != null)
                            {
                                currentToken = nestedToken;
                                break;
                            }
                        }
                    }
                }
            }

            return currentToken;
        }

        public static List<string> Find(JObject jsonObject, string segment)
        {
            return TraverseAndSearch(jsonObject, segment);
        }

        static List<string> TraverseAndSearch(JToken token, string searchValue)
        {
            List<string> found = new List<string>();

            if (token.Type == JTokenType.Object)
            {
                foreach (JProperty property in token.Children<JProperty>())
                {
                    if (property.Name.Contains(searchValue))
                    {
                        found.Add($"Value: {property.Value}\nPath: {property.Path}\n\n");
                    }
                    else
                    {
                        List<string> childFound = TraverseAndSearch(property.Value, searchValue);
                        if (childFound != null && childFound.Count > 0)
                        {
                            found.AddRange(childFound);
                        }
                    }
                }
            }
            else if (token.Type == JTokenType.Array)
            {
                foreach (JToken childToken in token.Children())
                {
                    List<string> matchingTokens = TraverseAndSearch(childToken, searchValue);
                    if (matchingTokens != null && matchingTokens.Count > 0)
                    {
                        found.AddRange(matchingTokens);
                    }
                }
            }
            else
            {
                if (token.ToString().Contains(searchValue))
                {
                    found.Add($"Value: {token}\nPath: {token.Path}\n\n");
                }
            }

            return found;
        }
    }
}
