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
    public partial class SelectRemark : Form
    {
        public SelectRemark()
        {
            InitializeComponent();
            this.Size = this.MinimumSize;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SelectRemark_Load(object sender, EventArgs e)
        {
            fillCenterType();
        }
        private void fillCenterType()
        {
            List<string> centerTypes = new List<string>();
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                if (cmbremark.Items.Count > 0)
                    cmbremark.Items.Clear();
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT Name FROM Remarks";
                    centerTypes.Add("--SELECT REMARK--");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("Name");
                                if (!centerTypes.Contains(name))
                                    centerTypes.Add(name);
                            }
                        }
                    }
                }
                centerTypes.Add("OTHER");
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cmbremark.Items.AddRange(centerTypes.ToArray());
                cmbremark.SelectedIndex = 0;
                pb.Close();
                pb.Dispose();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbremark.SelectedIndex == 0 || (cmbremark.Text == "OTHER" && txtother.Text == ""))
            {
                MessageBox.Show("Please select remark properly !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (cmbremark.Text == "OTHER" && txtother.Text.Length > 0)
            {
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO Remarks (Name) values ('" + txtother.Text + "')";
                cmd.ExecuteNonQuery();
                cn.Close();
                this.Hide();
            }
            else
                this.Hide();
        }
        public string getRemark()
        {
            if (cmbremark.Text == "OTHER")
                return txtother.Text;
            else
                return cmbremark.Text;
        }
        private void cmbremark_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbremark.Text == "OTHER")
                this.Size = this.MaximumSize;
            else
                this.Size = this.MinimumSize;
        }
    }
}
