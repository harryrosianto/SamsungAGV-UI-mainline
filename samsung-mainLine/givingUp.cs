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
    public partial class givingUp : Form
    {
        public static string jobIDValue = "";
        public givingUp()
        {
            InitializeComponent();
        }
            

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
                jobIDValue = jobIDText.Text;
                this.Close();
       
        }
    }
}
