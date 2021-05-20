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
    public partial class Wards : UserControl
    {
        string status = "";
        public Wards()
        {
            InitializeComponent();
        }
        private void fillCenterType()
        {
            List<string> centerTypes = new List<string>();
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
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
                    centerTypes.Add("--CENTER TYPE--");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("CenterType");
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
                cmbtype.Items.AddRange(centerTypes.ToArray());
                cmbtype.SelectedIndex = 0;
                pb.Close();
                pb.Dispose();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        private void fillCenterName(string type)
        {
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            List<string> centerTypes = new List<string>();
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                if (cmbname.Items.Count > 0)
                    cmbname.Items.Clear();
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT CenterName FROM Centers WHERE CenterType='" + type + "'";
                    centerTypes.Add("--CENTER NAME--");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("CenterName");
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

                cmbname.Items.AddRange(centerTypes.ToArray());
                cmbname.SelectedIndex = 0;
                pb.Close();
                pb.Dispose();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void clearAll()
        {
            lbid.Text = "";
            txtcode.Text = "";
            fillCenterType();
            txtname.Text = "";
            gridEmailList.Update();
            gridEmailList.ClearSelection();
        }
        public void unlockAll(bool b)
        {
            panelInput.Enabled = b;
        }
        public void actionButtonNormalStage()
        {
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
            btnExit.Enabled = true;
        }
        public void actionButtonEditStage()
        {
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
            btnExit.Enabled = true;
        }
        public void fillgrid()
        {
            if (gridEmailList.Rows.Count > 0)
                gridEmailList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From Wards Order By ID";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            string type = reader["CenterType"].ToString();
                            string cname = reader["CenterName"].ToString();
                            string name = reader["WardName"].ToString();
                            string code = reader["WardCode"].ToString();
                            gridEmailList.Rows.Add(id, type,cname,code,name);
                        }
                    }
                }
            }
            gridEmailList.Update();
            gridEmailList.ClearSelection();
        }
        private void getID()
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT MAX(ID) From Wards";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string id = reader[0].ToString();
                            if (id != "")
                                srno = Int32.Parse(id);
                        }
                    }
                }
            }
            lbid.Text = (srno + 1).ToString();
        }
        private bool validation()
        {
            if (lbid.Text == "" || txtname.Text == "" || txtcode.Text=="" || cmbtype.SelectedIndex==0 || cmbname.SelectedIndex==0)
                return false;
            else
                return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validation())
            {
                MessageBox.Show("Please fill all fields !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MySqlConnection cn = new MySqlConnection();
            cn.ConnectionString = DbConnect.conString;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            DialogResult drs;
            if (status.Equals("ADD NEW"))
            {
                drs = MessageBox.Show("Are You Sure Do You Want To Add This Ward ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                cmd.CommandText = "INSERT INTO Wards(CenterType,CenterName,WardName,WardCode) VALUES('" + cmbtype.Text + "','"+cmbname.Text+"','"+txtname.Text+"','"+txtcode.Text+"')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ward Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
            }
            else if (status.Equals("UPDATE"))
            {
                drs = MessageBox.Show("Are You Sure Do You Want To Update This Ward ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                cmd.CommandText = "UPDATE Wards SET WardName='" + txtname.Text + "',CenterType='"+cmbtype.Text+"',CenterName='"+cmbname.Text+"',WardCode='"+txtcode.Text+"' WHERE ID=" + lbid.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ward Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
            }
            clearAll();
            fillgrid();
            unlockAll(false);
            actionButtonNormalStage();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            status = "ADD NEW";
            clearAll();
            getID();
            unlockAll(true);
            actionButtonEditStage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            status = "UPDATE";
            unlockAll(true);
            actionButtonEditStage();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection();
            cn.ConnectionString = DbConnect.conString;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            DialogResult drs;
            drs = MessageBox.Show("Are You Sure Do You Want To Delete This Ward ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            cmd.CommandText = "DELETE FROM Wards WHERE ID=" + lbid.Text;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Ward Deleted Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cn.Close();
            clearAll();
            fillgrid();
            unlockAll(false);
            actionButtonNormalStage();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clearAll();
            unlockAll(false);
            actionButtonNormalStage();
            fillgrid();
        }

        private void gridEmailList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lbid.Text = gridEmailList.CurrentRow.Cells["ID"].Value.ToString();
            txtname.Text = gridEmailList.CurrentRow.Cells["WardName"].Value.ToString();
            txtcode.Text = gridEmailList.CurrentRow.Cells["WardCode"].Value.ToString();
            fillCenterType();
            cmbtype.Text = gridEmailList.CurrentRow.Cells["CenterType"].Value.ToString();
            fillCenterName(cmbtype.Text);
            cmbname.Text = gridEmailList.CurrentRow.Cells["CenterName"].Value.ToString();
            actionButtonEditStage();
            btnSave.Enabled = false;
            btnUp.Enabled = true;
            btnDel.Enabled = true;
            unlockAll(false);
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtype.SelectedIndex != 0)
                fillCenterName(cmbtype.Text);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void lbid_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
