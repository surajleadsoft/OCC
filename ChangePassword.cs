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
    public partial class ChangePassword : Form
    {
        string user = "";
        public ChangePassword(string username)
        {
            InitializeComponent();
            user = username;
            TextBox.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
        }
        private void getUsername()
        {
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                string[] names = user.Split(' ');
                connection.Open();
                var query = "SELECT Email FROM EmpRegister Where FirstName='" + names[0].Trim() + "' and LastName='"+names[1].Trim()+"'";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader row = command.ExecuteReader())
                    {
                        while (row.Read())
                        {
                            txtuser.Text = row[0].ToString();
                            txtuser.ReadOnly = true;
                        }
                    }
                }

            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            getUsername();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private bool validation()
        {
            if (txtold.Text == "" || txtnew.Text == "" || txtconfirm.Text == "" )
                return false;
            else
                return true;
        }
        private void ChangePass()
        {
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {

                if (!validation())
                {
                    MessageBox.Show("Please fill all fields !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtnew.Text != txtconfirm.Text)
                {
                    MessageBox.Show("Password doesn't match !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtconfirm.Clear();
                    txtnew.Clear();
                    txtnew.Focus();
                    return;
                   
                }
                    DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Change Password ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (drs == DialogResult.No)
                        return;
                    MySqlConnection cn = new MySqlConnection();
                    cn.ConnectionString = DbConnect.conString;
                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE Login SET Password='"+txtconfirm.Text+"' WHERE Username='"+txtuser.Text+"'";
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Password Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();                

            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Hide();
                pb.Close();
                pb.Dispose();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ChangePass();   
        }
    }
}
