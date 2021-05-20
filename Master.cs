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
    public partial class Master : Form
    {
        string status = "";
        string centerType = "";
        string centerName = "";
        public Master(string user,string type,string name)
        {
            InitializeComponent();
            lbloginas.Text = user;
            centerType = type;
            centerName = name;
            setVisible();
            status = "OPEN";
        }
        public Master(string user,string data)
        {
            InitializeComponent();
            lbloginas.Text = user;
            setVisible();
            setVisible();
            panelTop.Top = symbolBox4.Top;
            panelContainer.Controls.Clear();
            areaMaster1.Dock = DockStyle.Fill;
            areaMaster1.BringToFront();
            areaMaster1.Visible = true;
            areaMaster1.fillgrid();
            areaMaster1.clearAll();
            areaMaster1.actionButtonNormalStage();
            areaMaster1.unlockAll(false);
            panelContainer.Controls.Add(areaMaster1);
            symbolBox4.SymbolColor = SystemColors.HotTrack;
            status = "OK";
        }
        private void setVisible()
        {
            emails1.Visible = false;
            tests1.Visible = false;
            remarks1.Visible = false;
            password1.Visible = false;
            areaMaster1.Visible = false;
            wards1.Visible = false;
            beds1.Visible = false;
        }
        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {           
                button1.ForeColor = SystemColors.HotTrack;
                symbolBox1.SymbolColor = SystemColors.HotTrack;
            
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            if (!emails1.Visible)
            {
                button1.ForeColor = Color.Black;
                symbolBox1.SymbolColor = Color.Black;
            }
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.ForeColor = SystemColors.HotTrack;
            symbolBox2.SymbolColor = SystemColors.HotTrack;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            if (!tests1.Visible)
            {
                button2.ForeColor = Color.Black;
                symbolBox2.SymbolColor = Color.Black;
            }
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.ForeColor = SystemColors.HotTrack;
            symbolBox3.SymbolColor = SystemColors.HotTrack;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            if (!remarks1.Visible)
            {
                button3.ForeColor = Color.Black;
                symbolBox3.SymbolColor = Color.Black;
            }
        }

        

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            if (!password1.Visible)
            {
                button5.ForeColor = Color.Black;
                symbolBox5.SymbolColor = Color.Black;
            }
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.ForeColor = SystemColors.HotTrack;
            symbolBox5.SymbolColor = SystemColors.HotTrack;
        }
        private void setColors(Button btn)
        {
            button1.ForeColor = Color.Black;
            symbolBox1.SymbolColor = Color.Black;
            button2.ForeColor = Color.Black;
            symbolBox2.SymbolColor = Color.Black;
            button3.ForeColor = Color.Black;
            symbolBox3.SymbolColor = Color.Black;
            button5.ForeColor = Color.Black;
            symbolBox5.SymbolColor = Color.Black;
            btn.ForeColor = SystemColors.HotTrack;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            setVisible();
            panelTop.Top = symbolBox1.Top;
            symbolBox1.SymbolColor = SystemColors.HotTrack;
            panelContainer.Controls.Clear();
            emails1.Dock = DockStyle.Fill;
            emails1.BringToFront();
            emails1.Visible = true;
            emails1.fillgrid();
            emails1.clearAll();
            emails1.actionButtonNormalStage();
            emails1.unlockAll(false);
            panelContainer.Controls.Add(emails1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setVisible();
            panelTop.Top = symbolBox2.Top;
            symbolBox2.SymbolColor = SystemColors.HotTrack;
            panelContainer.Controls.Clear();            
            tests1.Dock = DockStyle.Fill;
            tests1.BringToFront();
            tests1.Visible = true;
            tests1.fillgrid();
            tests1.clearAll();
            tests1.actionButtonNormalStage();
            tests1.unlockAll(false);
            panelContainer.Controls.Add(tests1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            setVisible();
            panelTop.Top = symbolBox3.Top;
            panelContainer.Controls.Clear();            
            remarks1.Dock = DockStyle.Fill;
            remarks1.BringToFront();
            remarks1.Visible = true;
            remarks1.fillgrid();
            remarks1.clearAll();
            remarks1.actionButtonNormalStage();
            remarks1.unlockAll(false);
            panelContainer.Controls.Add(remarks1);
            symbolBox3.SymbolColor = SystemColors.HotTrack;
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            setVisible();
            panelTop.Top = symbolBox5.Top;
            panelContainer.Controls.Clear();
            password1.user = lbloginas.Text;
            password1.getUsername();
            password1.Dock = DockStyle.Fill;
            password1.BringToFront();
            password1.Visible = true;
            panelContainer.Controls.Add(password1);
            symbolBox5.SymbolColor = SystemColors.HotTrack;
        }

        private void emails1_VisibleChanged(object sender, EventArgs e)
        {
            if (emails1.Visible)
            {
                button1.ForeColor = SystemColors.HotTrack;
                symbolBox1.SymbolColor = SystemColors.HotTrack;
            }
            else
            {
                button1.ForeColor = Color.Black;
                symbolBox1.SymbolColor = Color.Black;
            }
        }

        private void tests1_VisibleChanged(object sender, EventArgs e)
        {
            if (tests1.Visible)
            {
                button2.ForeColor = SystemColors.HotTrack;
                symbolBox2.SymbolColor = SystemColors.HotTrack;
            }
            else
            {
                button2.ForeColor = Color.Black;
                symbolBox2.SymbolColor = Color.Black;
            }
        }

        private void remarks1_VisibleChanged(object sender, EventArgs e)
        {
            if (remarks1.Visible)
            {
                button3.ForeColor = SystemColors.HotTrack;
                symbolBox3.SymbolColor = SystemColors.HotTrack;
            }
            else
            {
                button3.ForeColor = Color.Black;
                symbolBox3.SymbolColor = Color.Black;
            }
        }

        private void password1_VisibleChanged(object sender, EventArgs e)
        {
            if (password1.Visible)
            {
                button5.ForeColor = SystemColors.HotTrack;
                symbolBox5.SymbolColor = SystemColors.HotTrack;
            }
            else
            {
                button5.ForeColor = Color.Black;
                symbolBox5.SymbolColor = Color.Black;
            }
        }

        private void Master_Load(object sender, EventArgs e)
        {
            if(status.Equals("OPEN"))
                button1_Click(sender, e);
        }

        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.ForeColor = SystemColors.HotTrack;
            symbolBox4.SymbolColor = SystemColors.HotTrack;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            if (!areaMaster1.Visible)
            {
                button4.ForeColor = Color.Black;
                symbolBox4.SymbolColor = Color.Black;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            setVisible();
            panelTop.Top = symbolBox4.Top;
            panelContainer.Controls.Clear();
            areaMaster1.Dock = DockStyle.Fill;
            areaMaster1.BringToFront();
            areaMaster1.Visible = true;
            areaMaster1.fillgrid();
            areaMaster1.clearAll();
            areaMaster1.actionButtonNormalStage();
            areaMaster1.unlockAll(false);
            panelContainer.Controls.Add(areaMaster1);
            symbolBox4.SymbolColor = SystemColors.HotTrack;
        }

        private void areaMaster1_VisibleChanged(object sender, EventArgs e)
        {
            if (areaMaster1.Visible)
            {
                button4.ForeColor = SystemColors.HotTrack;
                symbolBox4.SymbolColor = SystemColors.HotTrack;
            }
            else
            {
                button4.ForeColor = Color.Black;
                symbolBox4.SymbolColor = Color.Black;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            setVisible();
            panelTop.Top = symbolBox6.Top;
            panelContainer.Controls.Clear();
            wards1.Dock = DockStyle.Fill;
            wards1.BringToFront();
            wards1.Visible = true;
            wards1.fillgrid();
            wards1.clearAll();
            wards1.actionButtonNormalStage();
            wards1.unlockAll(false);
            panelContainer.Controls.Add(wards1);
            symbolBox6.SymbolColor = SystemColors.HotTrack;
        }

        private void wards1_VisibleChanged(object sender, EventArgs e)
        {
            if (wards1.Visible)
            {
                button6.ForeColor = SystemColors.HotTrack;
                symbolBox6.SymbolColor = SystemColors.HotTrack;
            }
            else
            {
                button6.ForeColor = Color.Black;
                symbolBox6.SymbolColor = Color.Black;
            }
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            button6.ForeColor = SystemColors.HotTrack;
            symbolBox6.SymbolColor = SystemColors.HotTrack;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            if (!wards1.Visible)
            {
                button6.ForeColor = Color.Black;
                symbolBox6.SymbolColor = Color.Black;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void beds1_Load(object sender, EventArgs e)
        {

        }

        private void beds1_VisibleChanged(object sender, EventArgs e)
        {
            if (beds1.Visible)
            {
                button7.ForeColor = SystemColors.HotTrack;
                symbolBox7.SymbolColor = SystemColors.HotTrack;
            }
            else
            {
                button7.ForeColor = Color.Black;
                symbolBox7.SymbolColor = Color.Black;
            }
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            button7.ForeColor = SystemColors.HotTrack;
            symbolBox7.SymbolColor = SystemColors.HotTrack;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            if (!beds1.Visible)
            {
                button7.ForeColor = Color.Black;
                symbolBox7.SymbolColor = Color.Black;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            setVisible();
            panelTop.Top = symbolBox7.Top;
            panelContainer.Controls.Clear();
            beds1.Dock = DockStyle.Fill;
            beds1.BringToFront();
            beds1.Visible = true;
            beds1.clearAll();
            beds1.fillgrid();            
            beds1.actionButtonNormalStage();
            beds1.unlockAll(false);
            panelContainer.Controls.Add(beds1);
            beds1.setDetails(centerType, centerName);
            symbolBox7.SymbolColor = SystemColors.HotTrack;
        }
    }
}
