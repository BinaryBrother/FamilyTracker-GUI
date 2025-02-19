using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace NetworkSnitch_GUI
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
        public static string INI_Path = @"D:\Dropbox\Projects\NetworkSnitch\AutoIt\NetworkSnitch.ini";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> trackerSection = LoadTrackerSection(INI_Path);

                foreach (var entry in trackerSection)
                {
                    Console.WriteLine($"Key: {entry.Key}, Value: {entry.Value}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public Dictionary<string, string> LoadTrackerSection(string iniFilePath)
        {
            var result = new Dictionary<string, string>();
            var returnValue = new StringBuilder(255);
            string section = "Tracker";

            // Get all keys in the section
            GetPrivateProfileString(section, null, null, returnValue, returnValue.Capacity, iniFilePath);
            string[] keys = returnValue.ToString().Split('\0', (char)StringSplitOptions.RemoveEmptyEntries);

            foreach (var key in keys)
            {
                GetPrivateProfileString(section, key, null, returnValue, returnValue.Capacity, iniFilePath);
                string value = returnValue.ToString();
                result[key] = value;
            }

            return result;
        }

    }
}
