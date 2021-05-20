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
    public partial class ByCenter : Form
    {
        
        public ByCenter()
        {
            InitializeComponent();
        }
        private void fillCenterType()
        {
            if (cmbtype.Items.Count > 0)
                cmbtype.Items.Clear();
            if (cmbname.Items.Count > 0)
                cmbname.Items.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT CenterType FROM centerType";
                cmbtype.Items.Add("--CENTER TYPE--");
                cmbname.Items.Add("--CENTER NAME--");
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string name = reader.GetString("CenterType");
                            if (!cmbtype.Items.Contains(name))
                                cmbtype.Items.Add(name);
                        }
                    }
                }
                cmbtype.SelectedIndex = 0;
                cmbname.SelectedIndex = 0;
            }
        }
        private void fillCenterName()
        {
            if (cmbname.Items.Count > 0)
                cmbname.Items.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT CenterName FROM Centers WHERE CenterType='" + cmbtype.Text + "'";
                cmbname.Items.Add("--CENTER NAME--");
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string name = reader.GetString("CenterName");
                            if (!cmbname.Items.Contains(name))
                                cmbname.Items.Add(name);
                        }
                    }
                }
                cmbname.SelectedIndex = 0;
            }
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtype.SelectedIndex != 0)
                fillCenterName();
        }

        private void ByCenter_Load(object sender, EventArgs e)
        {
            fillCenterType();
        }
        
        
        private void cmbname_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }
        public string getQuery()
        {
            return cmbtype.Text + ":" + cmbname.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
