using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace samsung_mainLine
{
    public partial class FormTimer : Form
    {
        //private Form1 _formMain;
        public static string secondsValue = "";
        public static string secondsValue2 = "";
        public FormTimer()
        {
            InitializeComponent();
        }
        

        public void bunifuThinButton21_Click(object sender, EventArgs e)
        {

            secondsValue = timerValue.Text;
            secondsValue2 = timerValue2.Text;
            this.Close();
        }
        
        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
