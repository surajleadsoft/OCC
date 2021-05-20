using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace OCC
{
    public partial class EmailFiles : Form
    {
        string main_query = "";
        public EmailFiles(string query)
        {
            InitializeComponent();
            main_query = query;
        }

        private void EmailFiles_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
        
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                AllEmails all = new AllEmails();
                all.Dock = DockStyle.Fill;
                all.fillCombo();
                all.setQuery(main_query,"All");
                panel5.Controls.Add(all);
                all.BringToFront();
                
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                
                
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panelSpecific_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                SpecificEmail spc = new SpecificEmail();
                spc.Dock = DockStyle.Fill;
                spc.setQuery(main_query, "Spc");               
                spc.Visible = true;
               
                panel5.Controls.Add(spc);
                spc.fillCombo();
                spc.BringToFront();
            }
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }
    }
}
