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
    public partial class Form1 : Form
    {
        public static string username = "", centerType = "", centerName = ""; 
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
            lbloginas.Text = "";
            lblogintime.Text = "";
            lbtype.Text = "";
            panelSearch.Size = panelSearch.MinimumSize;
            panelPatients.Size = panelPatients.MinimumSize;
            panelUser.Size = panelUser.MinimumSize;
            panelReport.Size = panelReport.MinimumSize;
        }

        private void Form1_Load(object sender, EventArgs e)
        {            
            this.Show();
            Login login = new Login();
            login.ShowDialog();
            lbloginas.Text = login.User();
            lblogintime.Text = login.getTime();
            lbtype.Text = login.getUserType();
            if (lbtype.Text.Equals("Staff"))
                panelUser.Visible = false;
            else
                panelUser.Visible = true;
            Form1.username = login.User();
            Form1.centerType = login.getType();
            Form1.centerName = login.getName();
            button5_Click(sender, e);
            label2.Text = "Logout";
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbdate.Text = DateTime.Now.ToShortDateString();
            lbtime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void label2_Enter(object sender, EventArgs e)
        {
           
        }

        private void label2_Leave(object sender, EventArgs e)
        {
           
        }

        private void label2_Click(object sender, EventArgs e)
        {
            DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Logout ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            Application.Exit();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void panelPatients_SizeChanged(object sender, EventArgs e)
        {
            if(panelPatients.Size==panelPatients.MinimumSize)
            {
                PCornor.BackColor = Color.Orange;
                pictureBox1.Image = Properties.Resources.down;
            }
            else if (panelPatients.Size == panelPatients.MaximumSize)
            {
                PCornor.BackColor = SystemColors.Control;
                pictureBox1.Image = Properties.Resources.up;
                panelSearch.Size = panelSearch.MinimumSize;
                panelUser.Size = panelUser.MinimumSize;
                panelUser.Size = panelUser.MinimumSize;
                panelReport.Size = panelReport.MinimumSize;
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (panelPatients.Size == panelPatients.MinimumSize)
                panelPatients.Size = panelPatients.MaximumSize;
            else
                panelPatients.Size = panelPatients.MinimumSize;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (panelSearch.Size == panelSearch.MinimumSize)
                panelSearch.Size = panelSearch.MaximumSize;
            else
                panelSearch.Size = panelSearch.MinimumSize;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            Remark remark = new Remark();
            remark.clearAll();
            remark.unlockAll(false);
            remark.actionButtonNormalStage();
          
            remark.setDetails(username, centerType, centerName);
            remark.fillgrid();
            remark.BringToFront();
            remark.Visible = true;
            panelContainer.Controls.Add(remark);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            Search search = new Search();
            search.setDetails(username, centerType, centerName);
            search.BringToFront();
            search.Visible = true;
            search.fillgrid();
            panelContainer.Controls.Add(search);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            AdvanceSearch advanceSearch = new AdvanceSearch();
            advanceSearch.setDetails(Form1.username, Form1.centerType, Form1.centerName);
            advanceSearch.fillgrid();
            advanceSearch.Visible = true;
            panelContainer.Controls.Add(advanceSearch);
        }

        private void panelSearch_SizeChanged(object sender, EventArgs e)
        {
            if (panelSearch.Size == panelSearch.MinimumSize)
            {
                SCornor.BackColor = Color.Orange;
                pictureBox2.Image = Properties.Resources.down;
               
            }
            else if (panelSearch.Size == panelSearch.MaximumSize)
            {
                SCornor.BackColor = SystemColors.Control;
                pictureBox2.Image = Properties.Resources.up;
                panelPatients.Size = panelPatients.MinimumSize;
                panelUser.Size = panelUser.MinimumSize;
                panelReport.Size = panelReport.MinimumSize;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            AddPatients add = new AddPatients();            
            add.unlockAll(false);
            add.actionButtonNormalStage();
            add.BringToFront();
            add.setDetails(username, centerType, centerName);
            add.Visible = true;
            add.clearAll();
            panelContainer.Controls.Add(add);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Master ob = new Master(Form1.username,Form1.centerType,Form1.centerName);
            ob.ShowDialog(this);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Contact ob = new Contact();
            ob.ShowDialog();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            EmailMaster ob = new EmailMaster(username);
            ob.ShowDialog();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (panelUser.Size == panelUser.MinimumSize)
            {
                panelUser.Size = panelUser.MaximumSize;
                panelSearch.Size = panelSearch.MinimumSize;
                panelPatients.Size = panelPatients.MinimumSize;
            }
            else
                panelUser.Size = panelUser.MinimumSize;
        }

        private void panelUser_SizeChanged(object sender, EventArgs e)
        {
            if (panelUser.Size == panelUser.MinimumSize)
                pictureBox7.Image = Properties.Resources.down;
            else
                pictureBox7.Image = Properties.Resources.up;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            AddUsers user = new AddUsers();
            user.fillgrid();
            user.clearAll();
            user.unlockAll(false);
            user.actionButtonNormalStage();
            user.BringToFront();
            user.Visible = true;
            panelContainer.Controls.Add(user);
        }

        private void panelContainer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            Dashboard dashboard = new Dashboard();
            dashboard.Dock = DockStyle.Fill;
            dashboard.BringToFront();
            dashboard.setCount();
            dashboard.Visible = true;
            panelContainer.Controls.Add(dashboard);
        }

        private void PCornor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelReport_SizeChanged(object sender, EventArgs e)
        {
            if (panelReport.Size == panelReport.MinimumSize)
                pictureBox8.Image = Properties.Resources.down;
            else
                pictureBox8.Image = Properties.Resources.up;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (panelReport.Size == panelReport.MinimumSize)
            {
                RCornor.BackColor = Color.Orange;
                panelReport.Size = panelReport.MaximumSize;
            }
            else if (panelSearch.Size == panelSearch.MaximumSize)
            {
                RCornor.BackColor = SystemColors.Control;
                pictureBox2.Image = Properties.Resources.up;
                panelPatients.Size = panelPatients.MinimumSize;
                panelUser.Size = panelUser.MinimumSize;
                panelSearch.Size = panelSearch.MinimumSize;
                panelReport.Size = panelReport.MinimumSize;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panelContainer.Controls.Clear();
            DailyReports reports = new DailyReports("");
            reports.BringToFront();
            reports.Visible = true;
            reports.setDetails(centerName, centerType);
            reports.loadData();
            panelContainer.Controls.Add(reports);
        }
    }
}
