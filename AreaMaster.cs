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
    public partial class AreaMaster : UserControl
    {
        string status = "";
        public AreaMaster()
        {
            InitializeComponent();
        }
        private void getID()
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT MAX(ID) From Areas";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
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
        public void clearAll()
        {
            textBox1.Text = "";
            txtname.Text = "";
            lbid.Text = "";
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
            btnExit.Enabled = true;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
        }
        public void actionButtonEditStage()
        {
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnExit.Enabled = true;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
        }

        public void fillgrid()
        {
            if (gridEmailList.Rows.Count > 0)
                gridEmailList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From Areas Order By ID";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            string name = reader["NameOfArea"].ToString();
                            string name1 = reader["NameOfDistrict"].ToString();
                            gridEmailList.Rows.Add(id, name,name1);
                        }
                    }
                }
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clearAll();
            unlockAll(false);
            fillgrid();
            actionButtonNormalStage();
        }
        private bool validation()
        {
            if (lbid.Text == "" || txtname.Text == "")
                return false;
            else
                return true;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            getID();
            status = "ADD NEW";
            actionButtonEditStage();
            unlockAll(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!validation())
            {
                MessageBox.Show("Please enter all fields !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (status.Equals("ADD NEW"))
            {
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Add This Area ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;

                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                var query = "INSERT INTO Areas (NameOfArea,NameOfDistrict) VALUES ('" + txtname.Text + "','"+textBox1.Text+"')";
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Area Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (status.Equals("UPDATE"))
            {
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Update This Area ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;

                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                var query = "UPDATE Areas SET NameOfArea='" + txtname.Text + "',NameOfDistrict='" + textBox1.Text + "' WHERE ID=" + lbid.Text;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Area Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            fillgrid();
            clearAll();
            gridEmailList.ClearSelection();
            actionButtonNormalStage();
            unlockAll(false);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            status = "UPDATE";
            unlockAll(true);
            actionButtonEditStage();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Delete This Area ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;

            MySqlConnection cn = new MySqlConnection();
            cn.ConnectionString = DbConnect.conString;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            var query = "DELETE FROM Areas WHERE ID=" + lbid.Text;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("Area Deleted Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clearAll();
            fillgrid();
            actionButtonNormalStage();
            unlockAll(false);
        }

        private void gridEmailList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lbid.Text = gridEmailList.CurrentRow.Cells["ID"].Value.ToString();
            txtname.Text = gridEmailList.CurrentRow.Cells["NameOfArea"].Value.ToString();
            textBox1.Text = gridEmailList.CurrentRow.Cells["NameOfDistrict"].Value.ToString();
            actionButtonEditStage();
            btnSave.Enabled = false;
            btnUp.Enabled = true;
            btnDel.Enabled = true;
            unlockAll(false);
        }
    }
}
