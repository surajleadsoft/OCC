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
    public partial class Address : Form
    {
        string result = "";
        public static string username = "";
        public Address(string user)
        {
            InitializeComponent();
            Address.username = user;
        }
        public string getAddress()
        {
            if (textBox1.Text != "" && comboBox1.SelectedIndex != 0)
                result = textBox1.Text + "," + comboBox1.Text;
            return result;
        }
        private void fillCenterType()
        {
            List<string> centerTypes = new List<string>();
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                if (comboBox1.Items.Count > 0)
                    comboBox1.Items.Clear();
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT NameOfArea,NameOfDistrict FROM Areas ORDER BY NameOfArea";
                    centerTypes.Add("-- ADDRESS --");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                               
                                string district = reader.GetString("NameOfDistrict");
                                string name = district;
                                if (!centerTypes.Contains(name))
                                    centerTypes.Add(name);
                            }
                        }
                    }
                }
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                comboBox1.Items.AddRange(centerTypes.ToArray());
                comboBox1.SelectedIndex = 0;
                pb.Close();
                pb.Dispose();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please enter address properly !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (getCount() == 0)
            {
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "INSERT INTO Areas (NameOfArea,NameOfDistrict) VALUES ('"+textBox1.Text+"','"+comboBox1.Text+"')";
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            this.Hide();
        }
        private int getCount()
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(ID) FROM Areas WHERE NameOfArea='"+textBox1.Text+"' and NameOfDistrict='"+comboBox1.Text+"'";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string data = reader[0].ToString();
                            if (data != "")
                                srno = Int32.Parse(data);
                        }
                    }
                }
            }
            return srno;
        }
        private void Address_Load(object sender, EventArgs e)
        {
            fillCenterType();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Master ob = new Master(Address.username, "Address");
            ob.ShowDialog();
            fillCenterType();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.DroppedDown = true;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT NameOfArea FROM Areas WHERE NameOfArea LIKE '%"+textBox1.Text+"%' LIMIT 10";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string area = reader.GetString("NameOfArea");
                            if (!textBox1.Items.Contains(area))
                                textBox1.Items.Add(area);
                        }
                    }
                }
            }
        }

        private void textBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT NameOfDistrict FROM Areas WHERE NameOfArea = '" + textBox1.Text + "'";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string area = reader.GetString("NameOfDistrict");
                            comboBox1.Text = area;
                        }
                    }
                }
            }
        }
    }
}
