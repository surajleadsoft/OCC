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
    public partial class TotalBeds : Form
    {
        List<BedDetails> listControl = new List<BedDetails>();
        public TotalBeds(string type,string name)
        {
            InitializeComponent();
            lbtype.Text = type;
            lbname.Text = name;
            fillWardCode();
        }
        private void fillCombo()
        {
            if (item_panel.Controls.Count > 0)
                item_panel.Controls.Clear();
            TableLayoutPanel.CheckForIllegalCrossThreadCalls = false;
            BedDetails.CheckForIllegalCrossThreadCalls = false;
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            BackgroundWorker bwConn = new BackgroundWorker();
            PleaseWait pb = new PleaseWait();
            bwConn.DoWork += (sender, e) =>
            {
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT * From Beds WHERE (CenterType='" + lbtype.Text + "' and CenterName='" + lbname.Text + "') and (WardCode='" + cmbcode.Text + "') Order By ID";
                    using (var command = new MySqlCommand(query, connection))
                    {

                        using (var reader = command.ExecuteReader())
                        {
                            int rowIndex = this.item_panel.RowCount++;
                            //MessageBox.Show(rowIndex.ToString());
                            int colIndecx = 0;
                            while (reader.Read())
                            {
                                BedDetails bed1 = new BedDetails();
                                bed1.setDetails(cmbcode.Text, reader["BedName"].ToString());

                                this.BeginInvoke((Action)(() =>
                                    {
                                        item_panel.Controls.Add(bed1, colIndecx, rowIndex);
                                    }));
                                System.Threading.Thread.Sleep(100);
                                listControl.Add(bed1);
                                colIndecx++;
                                if (colIndecx > 3)
                                {
                                    colIndecx = 0;
                                    rowIndex++;
                                    this.item_panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                                }
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
                pb.Close();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
        }
        private void fillWardCode()
        {
            if (cmbcode.Items.Count > 0)
                cmbcode.Items.Clear();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {

                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT WardCode FROM Wards WHERE CenterType='" + lbtype.Text + "' and CenterName='" + lbname.Text + "'";
                    cmbcode.Items.Add("--WARD CODE--");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("WardCode");
                                if (!cmbcode.Items.Contains(name))
                                    cmbcode.Items.Add(name);
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
                cmbcode.SelectedIndex = 0;
            };
            bwConn.RunWorkerAsync();
        }

        private void cmbcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbcode.SelectedIndex != 0)
                fillCombo();
        }

        private void item_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Red;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.Black;
        }
    }
}
