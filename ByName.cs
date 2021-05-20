using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OCC
{
    public partial class ByName : Form
    {
       
        public ByName()
        {
            InitializeComponent();
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public string getQuery()
        {
            return textBox1.Text;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
