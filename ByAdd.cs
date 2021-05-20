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
    public partial class ByAdd : Form
    {
        string result = "";
        public ByAdd()
        {
            InitializeComponent();
        }
        public string getAddress()
        {
            if (comboBox1.SelectedIndex != 0)
                result = comboBox1.Text;
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
                    var query = "SELECT NameOfArea,NameOfDistrict FROM Areas ORDER BY ID";
                    centerTypes.Add("-- ADDRESS --");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string area = reader.GetString("NameOfArea");
                                string district = reader.GetString("NameOfDistrict");
                                string name = area + "," + district;
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
            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Please enter address properly !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Hide();
        }

        private void ByAdd_Load(object sender, EventArgs e)
        {
            fillCenterType();
        }
    }
}
