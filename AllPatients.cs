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
    public partial class AllPatients : Form
    {
        string query = "";
        public AllPatients(string query,string title)
        {
            InitializeComponent();
            this.query = query;
            this.label2.Text = title;
        }
        public void fillgrid()
        {
            if (gridPatientList.Rows.Count > 0)
                gridPatientList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        int srno = 1;
                        while (reader.Read())
                        {
                            string name, contact, gender, add, age, adhar, date, remark, centertype, centername, attendby;
                            name = reader["Name"].ToString();
                            contact = reader["ContactNo"].ToString();
                            gender = reader["Gender"].ToString();
                            add = reader["Address"].ToString();
                            adhar = reader["AdharCard"].ToString();
                            date = reader["Date"].ToString();
                            remark = reader["Remark"].ToString();
                            centertype = reader["CenterType"].ToString();
                            centername = reader["CenterName"].ToString();
                            attendby = reader["CreatedBy"].ToString();
                            age = reader["Age"].ToString();
                            gridPatientList.Rows.Add(srno, name, contact, gender, add, age, adhar, DateTime.Parse(date), remark, centertype, centername, attendby);
                            srno++;
                        }
                    }
                }
            }
            gridPatientList.ClearSelection();
            lbTotal.Text = gridPatientList.Rows.Count.ToString();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }

        private void AllPatients_Load(object sender, EventArgs e)
        {
            fillgrid();
        }
    }
}
