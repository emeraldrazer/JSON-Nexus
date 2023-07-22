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
using System.IO;
using Microsoft.Win32;

namespace JSON_Nexus.Windows
{
    /// <summary>
    /// Interaction logic for JsonWindow.xaml
    /// </summary>
    public partial class JsonWindow : Window
    {
        public TextBox currentTextBox;

        public JsonWindow()
        {
            InitializeComponent();
            currentTextBox = JsonData;
        }

        public void RetrieveData(string data)
        {
            currentTextBox.Text = data.ToString();
        }

        public void RetrieveData(IEnumerable<string> data)
        {
            foreach (string item in data)
            {
                currentTextBox.AppendText(item);
            }
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JSON Files (*.json)|*.json|Other (*.*)|*.*";
            sfd.ShowDialog();

            if (sfd.FileName != String.Empty)
            {
                File.WriteAllText(sfd.FileName, currentTextBox.Text);
                MessageBox.Show("File saved successfully!", "JSON Nexus", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (closeOnSave.IsChecked == true)
            {
                this.Close();
            }
        }

        private void ReplaceBTN_Click(object sender, RoutedEventArgs e)
        {
            currentTextBox.Text = currentTextBox.Text.Replace(insteadOfObject.Text, withObject.Text);
        }
    }
}
