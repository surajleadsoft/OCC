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
    public partial class EmailMaster : Form
    {
        string createdBy = "";
        string status = "";
        public EmailMaster(string user)
        {
            InitializeComponent();
            createdBy = user;
        }
        private void clearAll()
        {
            txtemail.Text = "";
            txtname.Text = "";
            lbid.Text = "";
        }
        private void unlockAll(bool b)
        {
            panelInput.Enabled = b;
        }
        private void actionButtonNormalStage()
        {
            btnNew.Enabled = true;
            btnSave.Enabled = false;
            btnExit.Enabled = true;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
        }
        private void actionButtonEditStage()
        {
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnExit.Enabled = true;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
        }
        private void getID()
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT MAX(ID) From Emails";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string id = reader[0].ToString();
                            if(id!="")
                                srno = Int32.Parse(id);
                        }
                    }
                }
            }
            lbid.Text = (srno + 1).ToString();
        }
        private void fillgrid()
        {
            if (gridEmailList.Rows.Count > 0)
                gridEmailList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From Emails Order By ID";
                
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            string name = reader["NameOfPerson"].ToString();
                            string email = reader["Email"].ToString();
                            gridEmailList.Rows.Add(id, name, email);
                        }
                    }
                }                
            }
            gridEmailList.ClearSelection();
        }
        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            clearAll();
            unlockAll(false);
            fillgrid();
            actionButtonNormalStage();
            this.Hide();
        }

        private void EmailMaster_Load(object sender, EventArgs e)
        {
            clearAll();
            unlockAll(false);
            fillgrid();
            actionButtonNormalStage();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            getID();
            status = "ADD NEW";
            actionButtonEditStage();
            unlockAll(true);
        }
        private bool validation()
        {
            if (txtemail.Text == "" || lbid.Text == "" || txtname.Text == "")
                return false;
            else
                return true;
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
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Add This Email ??","Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;

                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                var query = "INSERT INTO Emails (NameOfPerson,Email) VALUES ('"+txtname.Text+"','"+txtemail.Text+"')";
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Email Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);               
            }
            else if (status.Equals("UPDATE"))
            {
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Update This Email ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;

                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                var query = "UPDATE Emails SET NameOfPerson='" + txtname.Text + "',Email='" + txtemail.Text + "' WHERE ID="+lbid.Text;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Email Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            clearAll();
            fillgrid();
            actionButtonNormalStage();
            unlockAll(false);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Delete This Email ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;

            MySqlConnection cn = new MySqlConnection();
            cn.ConnectionString = DbConnect.conString;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            var query = "DELETE FROM Emails WHERE ID="+lbid.Text;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            cn.Close();
            MessageBox.Show("Email Delete Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clearAll();
            fillgrid();
            actionButtonNormalStage();
            unlockAll(false);
        }

        private void gridEmailList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lbid.Text = gridEmailList.CurrentRow.Cells["ID"].Value.ToString();
            txtname.Text = gridEmailList.CurrentRow.Cells["NameOfPerson"].Value.ToString();
            txtemail.Text = gridEmailList.CurrentRow.Cells["Email"].Value.ToString();
            actionButtonEditStage();
            btnSave.Enabled = false;
            btnUp.Enabled = true;
            btnDel.Enabled = true;
            unlockAll(false);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            status = "UPDATE";
            unlockAll(true);
            actionButtonEditStage();
        }
    }
}
