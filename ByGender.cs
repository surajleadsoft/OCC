using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OCC
{
    public partial class ByGender : Form
    {
        string gender = "";
        public ByGender()
        {
            InitializeComponent();
        }
        public string getQuery()
        {
            return gender;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                gender = "Male";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                gender = "Female";
        }
    }
}
