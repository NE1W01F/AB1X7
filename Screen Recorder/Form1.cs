using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using Accord.Video.FFMPEG;
using System.Drawing;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace Screen_Recorder
{
    public partial class Form1 : Form
    {
        public static byte[] file = File.ReadAllBytes(Application.ExecutablePath);

        public Form1()
        {
            InitializeComponent();
        }

        public static void StartUp()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Application.ExecutablePath,
                Arguments = string.Empty, // if you need to pass any command line arguments to your stub, enter them here
                UseShellExecute = true,
                Verb = "runas"
            };
            Process.Start(startInfo);
        }

        public bool IsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public void SomeMethod()
        {
            if (this.IsElevated)
            {

            }
            else
            {
                StartUp();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            vf.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        public static Bitmap bp;
        public static Graphics gr;
        private void timer1_Tick(object sender, EventArgs e)
        {
            bp = new Bitmap(800, 600);
            gr = Graphics.FromImage(bp);
            gr.CopyFromScreen(0, 0, 0, 0, new Size(bp.Width, bp.Height));
            pictureBox1.Image = bp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            vf.WriteVideoFrame(bp);
        }

        public static VideoFileWriter vf;

        private void Form1_Load(object sender, EventArgs e)
        {
            SomeMethod();
            timer1.Interval = 20;
            timer1.Tick += timer1_Tick;
            vf = new VideoFileWriter();
            vf.Open(Path.GetRandomFileName() + ".avi", 800, 600, 25, VideoCodec.MPEG4, 1000000);
        }
    }
}
