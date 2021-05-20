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
    public partial class Remarks : UserControl
    {
        string status = "";
        public Remarks()
        {
            InitializeComponent();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }
        public void clearAll()
        {
            lbid.Text = "";
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
                var query = "SELECT * From Remarks Order By ID";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            string name = reader["Name"].ToString();
                            gridEmailList.Rows.Add(id, name);
                        }
                    }
                }
            }
        }
        private void getID()
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT MAX(ID) From Remarks";

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
        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            status = "ADD NEW";
            clearAll();
            getID();
            unlockAll(true);
            actionButtonEditStage();
        }

        private void gridEmailList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lbid.Text = gridEmailList.CurrentRow.Cells["ID"].Value.ToString();
            txtname.Text = gridEmailList.CurrentRow.Cells["Remark"].Value.ToString();
            actionButtonEditStage();
            btnSave.Enabled = false;
            btnUp.Enabled = true;
            btnDel.Enabled = true;
            unlockAll(false);
        }
        private bool validation()
        {
            if (lbid.Text == "" || txtname.Text == "")
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
                drs = MessageBox.Show("Are You Sure Do You Want To Add This Remark ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                cmd.CommandText = "INSERT INTO Remarks(Name) VALUES('" + txtname.Text + "')";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Remark Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
            }
            else if (status.Equals("UPDATE"))
            {
                drs = MessageBox.Show("Are You Sure Do You Want To Update This Test ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                cmd.CommandText = "UPDATE Remarks SET Name='" + txtname.Text + "' WHERE ID=" + lbid.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Remark Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
            }
            clearAll();
            fillgrid();
            unlockAll(false);
            actionButtonNormalStage();
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
            drs = MessageBox.Show("Are You Sure Do You Want To Delete This Remark ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            cmd.CommandText = "DELETE FROM Remarks WHERE ID=" + lbid.Text;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Remark Deleted Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cn.Close();
            clearAll();
            fillgrid();
            unlockAll(false);
            actionButtonNormalStage();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            actionButtonNormalStage();
            clearAll();
            unlockAll(false);
            fillgrid();

        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }
    }
}
