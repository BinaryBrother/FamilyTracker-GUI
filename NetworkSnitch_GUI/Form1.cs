using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        private Label[] LabelArray;
        private PictureBox[] PictureBoxArray;

        public Form1()
        {
            InitializeComponent();
            LabelArray = new Label[] { label1, label2, label3 };
            PictureBoxArray = new PictureBox[] { pictureBox1, pictureBox2, pictureBox3 };

            Timer timer = new Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _UpdateUI();
        }
        private void _UpdateUI()
        {
            Dictionary<string, string> TrackerData = ReadIniSection(INI_Path, "Tracker");
            Dictionary<string, string> MonitorData = ReadIniSection(INI_Path, "Monitor");
            //LabelArray[0].Text = "Test";
            string MonitorName = "";
            string TrackerStatus = "";

            int Count = -1;
            foreach (var entry in TrackerData)
            {
                Count++;
                foreach (var entry2 in MonitorData)
                {
                    if (entry.Key == entry2.Key)
                    {
                        TrackerStatus = entry.Value;
                        MonitorName = entry2.Value;
                        LabelArray[Count].Text = MonitorName;
                        if (TrackerStatus == "DOWN")
                        {
                            PictureBoxArray[Count].Image = Properties.Resources.images;
                        }
                        else
                        {
                            PictureBoxArray[Count].Image = Properties.Resources.online;
                        }
                    }
                }

            }

        }
        static Dictionary<string, string> ReadIniSection(string filePath, string section)
        {
            var result = new Dictionary<string, string>();
            if (!File.Exists(filePath))
                return result;
            string[] lines = Array.Empty<string>();
            try { lines = File.ReadAllLines(filePath); }
            catch (Exception ex) { /* Disregard File Locks */ }
            bool inSection = false;

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    inSection = trimmedLine.Equals($"[{section}]", StringComparison.OrdinalIgnoreCase);
                    continue;
                }

                if (inSection && trimmedLine.Contains("="))
                {
                    string[] keyValue = trimmedLine.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        result[keyValue[0].Trim()] = keyValue[1].Trim();
                    }
                }
            }
            return result;
        }
    }
}
