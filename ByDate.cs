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
    public partial class ByDate : Form
    {
        public ByDate()
        {
            InitializeComponent();
        }
        public string getQuery()
        {
            return dateTimePicker1.Text + ":" + dateTimePicker2.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
