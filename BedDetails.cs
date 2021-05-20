using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OCC
{
    public partial class BedDetails : UserControl
    {
        string wardno = "";
        public BedDetails()
        {
            InitializeComponent();
        }
        public void setDetails(string no,string bedno)
        {
            wardno = no;
            lbbedno.Text = bedno;
            
        }
        private string getCount(String q)
        {
            string data = "";
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = q;
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader[0].ToString();
                            data = id;
                        }
                    }
                }
            }
            return data;
        }

        private void lbbedno_TextChanged(object sender, EventArgs e)
        {
            string flag = getCount("Select Flag From Beds WHERE BedName='" + lbbedno.Text + "'");
            if (flag == "0")
            {
                lbstatus.Text = "Available";
                pictureBox1.Image = Properties.Resources.free1;
                bunifuCards1.color = Color.Green;
                lbname.Text = "";
            }
            else
            {
                lbstatus.Text = "Allocated";
                pictureBox1.Image = Properties.Resources.all;
                lbname.Text = getCount("Select Name From Patients WHERE WardCode='" + wardno + "' and BedName='" + lbbedno.Text + "'");
                bunifuCards1.color = Color.Red;
            }
            
        }

        private void lbbedno_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void lbbedno_TextChanged_2(object sender, EventArgs e)
        {
            lbbedno_TextChanged(sender, e);

        }
    }
}
