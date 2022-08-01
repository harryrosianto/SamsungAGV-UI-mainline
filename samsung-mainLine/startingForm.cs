using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace samsung_mainLine
{
    public partial class startingForm : Form
    {
        Timer tmr;
        public int loading_counts = 0;
        public startingForm()
        {
            InitializeComponent();
            if (loading_counts == 0)
            {
                loadingStatus.Text = "Load start screen...";
            }
            if (loading_counts == 1)
            {
                loadingStatus.Text = "Load object...";
            }
            if (loading_counts == 2)
            {
                loadingStatus.Text = "Load text...";
            }
            if (loading_counts == 3)
            {
                loadingStatus.Text = "Synchronize Data...";
            }
            if (loading_counts == 4)
            {
                loadingStatus.Text = "Start Application...";
            }
        }

        private void startingForm_Shown(object sender, EventArgs e)
        {
            tmr = new Timer();

            //set time interval 5 sec

            tmr.Interval = 1000;

            //starts the timer

            tmr.Start();

            tmr.Tick += tmr_Tick;


        }

        void tmr_Tick(object sender, EventArgs e)

        {
            loading_counts += 1;

            if (loading_counts == 0)
            {
                loadingStatus.Text = "Loading screen...";
            }
            if (loading_counts == 2)
            {
                loadingStatus.Text = "Load objects...";
            }
            if (loading_counts == 4)
            {
                loadingStatus.Text = "Load texts...";
            }
            if (loading_counts == 6)
            {
                loadingStatus.Text = "Synchronize data...";
            }
            if (loading_counts == 8)
            {
                loadingStatus.Text = "Start application...";
            }
            //after 5 sec stop the timer
            if (loading_counts== 10)
            {
                tmr.Stop();
                //display mainform

                Form1 mf = new Form1();

                mf.Show();

                //hide this form

                this.Hide();
            }
            

            

        }
    }
}
