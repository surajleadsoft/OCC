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
    public partial class FullnameForm : Form
    {
        string createdBy = "";
        public FullnameForm(string created)
        {
            InitializeComponent();
            createdBy = created;
        }
        public string getResult()
        {
            return cmbname.Text;
        }
        private void Fullname_Load(object sender, EventArgs e)
        {

        }

        private void cmbname_TextChanged(object sender, EventArgs e)
        {
            cmbname.DroppedDown = true;
            cmbname.DropDownHeight = 200;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Name,ContactNo,AdharCard From Patients WHERE Name LIKE '%" + cmbname.Text + "%' ORDER BY Name";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString("Name");
                            string mob = reader.GetString("ContactNo");
                            string adhar = reader.GetString("AdharCard");
                            string result = name + "-" + mob + "-" + adhar;
                            if (!cmbname.Items.Contains(result))
                                cmbname.Items.Add(result);
                        }
                        cmbname.Update();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cmbname.Text=="")
            {
                MessageBox.Show("Please select patient !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Hide();
        }

        private void cmbname_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
