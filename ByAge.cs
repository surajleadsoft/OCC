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
    public partial class ByAge : Form
    {
        public ByAge()
        {
            InitializeComponent();
        }
        public string getQuery()
        {
            if (textBox1.Text.All(char.IsDigit) && textBox2.Text.All(char.IsDigit))
                return textBox1.Text + ":" + textBox2.Text;
            else
                return "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ByAge_Load(object sender, EventArgs e)
        {

        }
    }
}
