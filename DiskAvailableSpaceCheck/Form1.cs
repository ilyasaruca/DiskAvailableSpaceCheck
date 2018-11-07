using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms; 

namespace DiskAvailableSpaceCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
         
        private void Form1_Load(object sender, EventArgs e)
        { 
            timer1.Start();
        }

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
            DirectoryInfo di = new DirectoryInfo("replace directory path");

            if (DateTime.Now.Hour>= 6 && DateTime.Now.Hour<=8)   //in the morning is controlled from 6 to 8
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.Name.Contains("E:") && d.IsReady)  //Only E Disk. remove if block if you are going to check all disks
                    { 
                        if (d.AvailableFreeSpace/1024/1024/1024 < 4)  //AvailableFreeSpace convert GB. Less than 4 gigabytes
                        { 
                            foreach (FileInfo file in di.GetFiles())
                            {
                                if (file.Extension==".bak")  //extension control.
                                { 
                                    file.Delete();
                                }
                            }
                        } 
                    }
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {

                Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.Text = "Available Free Space";
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            }
        }
    }
}
