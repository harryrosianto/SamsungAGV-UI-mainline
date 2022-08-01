using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace samsung_mainLine
{
    public partial class Form1 : Form
    {

        public List<AGVCallingModel> AGVData = new();
        public List<AGVErrorModel> AGVError = new();
        public int counts;
        DateTime dt = new DateTime();
        public string missionTime;
        //public static string secondsValue = "";
        public int flags = 0;

        public Form1()
        {   
            InitializeComponent();
            callAPI();
            modeButton.MouseHover += ModeButton_MouseHover;
            modeButton.MouseLeave += ModeButton_MouseLeave;
            logsButton.MouseHover += LogsButton_MouseHover;
            logsButton.MouseLeave += LogsButton_MouseLeave;
            timerButton.MouseHover += TimerButton_MouseHover;
            timerButton.MouseLeave += TimerButton_MouseLeave;
            homeButton.MouseHover += HomeButton_MouseHover;
            homeButton.MouseLeave += HomeButton_MouseLeave;

            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            timerData.Text = Convert.ToString(counts);
            Console.WriteLine(flags);

        }
        private async void callAPI()
        {
            List<AGVCallingModel> showData = new();
            List<AGVErrorModel> errorData = new();
            missionTime = DateTime.Now.ToString("HH:mm:ss");

            AGVCallingModel temp = new(missionTime, "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp2 = new(missionTime, "AGV-2", "Station 7", "Running");
            AGVCallingModel temp3 = new(missionTime, "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp4 = new(missionTime, "AGV-2", "Station 7", "Running");
            AGVCallingModel temp5 = new(missionTime, "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp6 = new(missionTime, "AGV-2", "Station 7", "Running");
            AGVCallingModel temp7 = new(missionTime, "AGV-1", "Station 3", "Finish");
            AGVCallingModel temp8 = new(missionTime, "AGV-2", "Station 7", "Running");
            AGVCallingModel temp9 = new(missionTime, "AGV-2", "Station 9", "Running");
            AGVCallingModel temp10 = new(missionTime, "AGV -2", "Station 9", "Running");
            showData.Add(temp);
            showData.Add(temp2);
            showData.Add(temp3);
            showData.Add(temp4);
            showData.Add(temp5);
            showData.Add(temp6);
            showData.Add(temp7);
            showData.Add(temp8);
            showData.Add(temp9);
            showData.Add(temp10);
            showData.Add(temp);
            showData.Add(temp2);
            showData.Add(temp3);
            showData.Add(temp4);
            showData.Add(temp5);
            showData.Add(temp6);

            AGVErrorModel err = new("1", "AGV-1", "EMC STOP", "OBS STOP");
            AGVErrorModel err2 = new("2", "AGV-2", "EMC STOP", "OBS STOP");
            errorData.Add(err);
            errorData.Add(err2);

            gridViewDS.DataSource = showData;
            //gridViewError.DataSource = errorData;
            Console.WriteLine(counts);
            //timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            
        }

        public class AGVCallingModel
        {
            public string Time { get; set; }
            public string Name { get; set; }
            public string Station { get; set; }
            public string Status { get; set; }
            public AGVCallingModel(string time, string agvname, string deliv, string status)
            {
                this.Time = time;
                this.Name = agvname;
                this.Station = deliv;
                this.Status = status;
            }
        }

        public class AGVErrorModel
        {
            public string Time { get; set; }
            public string Name { get; set; }
            public string Error { get; set; }
            public string Obstacle { get; set; }
            public AGVErrorModel(string dateTimeNow, string agvName, string errorCode, string obscode)
            {
                this.Time = dateTimeNow;
                this.Name = agvName;
                this.Error = errorCode;
                this.Obstacle = obscode;
            }
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void timerButton_Click(object sender, EventArgs e)
        {
            FormTimer f = new();
            f.ShowDialog();
            //flags = 1;
            counts = Convert.ToInt32(FormTimer.secondsValue);
            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            timerData.Text = FormTimer.secondsValue;
            //if (flags == 1)
            //{
            //    bunifuPanel3.BackColor = Color.Yellow;
            //}
        }
        public void homeButton_Click(object sender, EventArgs e)
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counts--;
            if (counts == 0)
            {
                timer1.Stop();
            }
            Console.WriteLine(flags);
            timerLabel.Text = dt.AddSeconds(counts).ToString("mm:ss");
        }

        private void modeButton_Click(object sender, EventArgs e)
        {
            lightMode lmForm = new lightMode();
            this.Hide();
            lmForm.Show();
        }

        

        private void ModeButton_MouseHover(object? sender, EventArgs e)
        {
            modeIndicator.Visible = true;
        }
        private void ModeButton_MouseLeave(object? sender, EventArgs e)
        {
            modeIndicator.Visible = false;
        }
        private void LogsButton_MouseHover(object? sender, EventArgs e)
        {
            logsIndicator.Visible = true;
        }
        private void LogsButton_MouseLeave(object? sender, EventArgs e)
        {
            logsIndicator.Visible = false;
        }
        private void TimerButton_MouseHover(object? sender, EventArgs e)
        {
            timerIndicator.Visible = true;
        }
        private void TimerButton_MouseLeave(object? sender, EventArgs e)
        {
            timerIndicator.Visible = false;
        }
        private void HomeButton_MouseHover(object? sender, EventArgs e)
        {
            homeIndicator.Visible = true;
        }
        private void HomeButton_MouseLeave(object? sender, EventArgs e)
        {
            homeIndicator.Visible = false;
        }

        private void bunifuLabel14_Click(object sender, EventArgs e)
        {
            
        }

        private void logsButton_Click(object sender, EventArgs e)
        {
            if (gridViewDS.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                string outputName = DateTime.Today.ToString("dd-mm-yyyy");
                sfd.FileName = "Logs Exported - " + outputName + ".csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("failed to export csv file" + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = gridViewDS.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[gridViewDS.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += gridViewDS.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < gridViewDS.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += gridViewDS.Rows[i - 1].Cells[j].Value.ToString() + ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("csv file exported successfully", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("no delivery status to export!", "Info");
            }
        }
    }
}