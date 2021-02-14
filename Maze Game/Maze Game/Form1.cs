using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze_Game
{
    public partial class frmMain : Form
    {
        int counter = 0;
        Stopwatch ss;
        TimeSpan bestRecord;
        TimeSpan elapsed;

        System.Media.SoundPlayer hitSound;
        System.Media.SoundPlayer startSound;
        System.Media.SoundPlayer finishSound;

        string hitSoundURL = @"C:\Windows\Media\ding.wav";
        string startSoundURL = @"C:\Windows\Media\Windows Unlock.wav";
        string finishSoundURL = @"C:\Windows\Media\Windows Notify Email.wav";

        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            ss = new Stopwatch();

            SendPointerToStart();
            cbDarkMode.Items.Add("Active");
            cbDarkMode.Items.Add("InActive");
            if (!File.Exists(hitSoundURL) || !File.Exists(startSoundURL) || !File.Exists(finishSoundURL))
            {
                OpenFileDialog OPF = new OpenFileDialog();
                string path;
                OPF.Filter = "Audio (*wma)|.wma|";
                OPF.Multiselect = false;
                OPF.Title = "Select One wma file for hit sound";
                ///////////
                if (!File.Exists(hitSoundURL))
                {
                    if (OPF.ShowDialog() == DialogResult.OK)
                    {
                        path = OPF.FileName;
                        hitSound = new System.Media.SoundPlayer(path);
                    }
                }
                ///////////
                if (!File.Exists(startSoundURL))
                {
                    if (OPF.ShowDialog() == DialogResult.OK)
                    {
                        path = OPF.FileName;
                        startSound = new System.Media.SoundPlayer(path);
                    }
                }
                //////////
                if (!File.Exists(finishSoundURL))
                {
                    if (OPF.ShowDialog() == DialogResult.OK)
                    {
                        path = OPF.FileName;
                        finishSound = new System.Media.SoundPlayer(path);
                    }
                }
            }
            else
            {
                hitSound = new System.Media.SoundPlayer(hitSoundURL);
                startSound = new System.Media.SoundPlayer(startSoundURL);
                finishSound = new System.Media.SoundPlayer(finishSoundURL);
            }
        }

        private void MazeHit(object sender, EventArgs e)
        {
            hitSound.Play();
            timerRecord.Enabled = false;
            ss.Stop();
            counter++;

            if (MessageBox.Show("YOU LOSE!!!!!") == DialogResult.OK)
                SendPointerToStart();
        }

        public void SendPointerToStart()
        {
            Point startingPoint = panelMain.Location;
            startingPoint.Offset(95, 74);
            Cursor.Position = PointToScreen(startingPoint);
            lblLose.Text = $"You losed {counter} Times";
        }

        private void lblFinish_MouseEnter(object sender, EventArgs e)
        {
            new Thread(() => 
            {
                if (TimeSpan.Compare(bestRecord, elapsed) == -1)
                {
                    bestRecord = elapsed;
                    lblBestRecord.Text = bestRecord.Minutes.ToString() + ":" + bestRecord.Seconds.ToString();
                }
            }).Start();

            finishSound.Play();
            timerRecord.Enabled = false;
            ss.Stop();
            counter++;
            if (MessageBox.Show($"You win \nYou try {counter.ToString()} times 😉\nYour recored time is : {lblTimer.Text}", counter.ToString()) == DialogResult.OK)
                SendPointerToStart();
            counter = 0;
        }

        private void lblPlay_MouseEnter(object sender, EventArgs e)
        {
            startSound.Play();
            ss.Start();
            if (timerRecord.Enabled == false)
            {
                timerRecord.Enabled = true;
            }
        }

        private void cbDarkMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Label> labels = panelMain.Controls.OfType<Label>().ToList();
            labels.Remove(lblStart);
            labels.Remove(lblFinish);
            labels.Remove(lblShyestehhs);
            Color lblColor;
            Color panelMainColor;
            Color toolStripColor;
            if (cbDarkMode.SelectedItem == "Active")
            {
                lblColor = Color.DeepSkyBlue;
                panelMainColor = Color.Gray;
                toolStripColor = Color.Silver;
            }
            else
            {
                lblColor = Color.Gold;
                panelMainColor = Color.LemonChiffon;
                toolStripColor = Color.White;
            }
            panelMain.BackColor = panelMainColor;
            toolStrip.BackColor = toolStripColor;
            cbDarkMode.BackColor = lblDarkMode.BackColor = toolStripColor;
            foreach (Label lbl in labels)
            {
                lbl.BackColor = lblColor;
            }
        }

        private void timerRecord_Tick(object sender, EventArgs e)
        {
            elapsed = this.ss.Elapsed;
            lblTimer.Text = elapsed.Minutes.ToString("00") + ":" + elapsed.Seconds.ToString("00");
        }
    }
}
