using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net;
using System.IO;

namespace OCC
{
    public partial class Login : Form
    {
        static string username = "";
        static string centerType = "";
        static string centerName = "";
        static string userType = "";
        public Login()
        {
            InitializeComponent();
            txtuser.Focus();           
        }
        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        private string getUsername()
        {
            return txtuser.Text;
        }
        private string getPassword()
        {
            return txtpwd.Text;
        }
        private string getCenterType()
        {
            return cmbtype.Text;
        }
        private string getCenterName()
        {
            return cmbname.Text;
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
                txtuser.Focus();
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
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtype.SelectedIndex != 0)
                fillCenterName(cmbtype.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Exit Application ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            Application.Exit();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(DateTime.Now.Day.ToString());
            if (!CheckForInternetConnection())
            {
                MessageBox.Show("No Internet !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            if (getUpdates() == "Yes")
            {
                Updates ob = new Updates();
                ob.ShowDialog();
            }
            if (Properties.Settings.Default["Username"].ToString() != "" || Properties.Settings.Default["Password"].ToString() != "" || Properties.Settings.Default["CenterType"].ToString() != "" || Properties.Settings.Default["CenterName"].ToString() != "")
            {
                txtuser.Text = Properties.Settings.Default["Username"].ToString();
                txtpwd.Text = Properties.Settings.Default["Password"].ToString();
                fillCenterType();
                cmbtype.Text = Properties.Settings.Default["CenterType"].ToString();
                cmbname.Text = Properties.Settings.Default["CenterName"].ToString();
                txtuser.Enabled = false;
                txtpwd.Enabled = false;
                cmbname.Enabled = false;
                cmbtype.Enabled = false;
                checkBox1.Checked = true;
            }
            else
                fillCenterType();
        }
        private bool validation(string user,string password,string name,string type)
        {            
            if (user == "" || password == "" || name == "--CENTER NAME--" || type == "--CENTER TYPE--")
                return false;
            else
                return true;
           
        }
        public string getTime()
        {
            return DateTime.Now.ToString("hh:mm:ss tt");
        }
        public string getUser()
        {
            return txtuser.Text.Trim();
        }
        private void getLoginDetails()
        {
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT FirstName,LastName FROM EmpRegister Where Email='" + txtuser.Text.Trim() + "'";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader row = command.ExecuteReader())
                    {
                        while (row.Read())
                        {
                            Login.username = row["FirstName"] + " " + row["LastName"];
                            Login.centerType = cmbtype.Text;
                            Login.centerName = cmbname.Text;                            
                        }                       
                    }
                }

            }
        }
        private string getUpdates()
        {
            string updates = "";
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT isUpdate From SoftwareUpdates";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader row = command.ExecuteReader())
                    {
                        while (row.Read())
                        {
                            string up = row["isUpdate"].ToString();
                            updates = up;
                        }
                    }
                }

            }
            return updates;
        }
        public string getType()
        {
            return centerType;
        }
        public string getName()
        {
            return centerName;
        }
        public string User()
        {
            return username;
        }
        public string getUserType()
        {
            return Login.userType;
        }
        private string LoginUser(string username, string password)
        {
            if (!validation(getUsername(), getPassword(), getCenterName(), getCenterType()))
            {
                return "fields";
            }
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Login Where (Username='" + username.Trim() + "' and Password='" + password.Trim() + "') and (CenterType='"+cmbtype.Text.Trim()+"' and CenterName='"+cmbname.Text.Trim()+"')";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader row = command.ExecuteReader())
                    {
                        if (row.Read())
                        {
                            getLoginDetails();
                            getUserType1();
                            return "ok";

                        }
                        else
                        {
                            return "not ok";

                        }
                    }
                }

            }
        }
        private void getUserType1()
        {
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT UserType FROM EmpRegister Where Email='"+txtuser.Text.Trim()+"'";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader row = command.ExecuteReader())
                    {
                        if (row.Read())
                        {
                            Login.userType = row[0].ToString();
                        }
                        else
                        {
                            Login.userType = "";

                        }
                    }
                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TextBox.CheckForIllegalCrossThreadCalls = false;
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            string status = "";
            bwConn.DoWork += (sender1, e1) =>
            {
                status=LoginUser(txtuser.Text, txtpwd.Text);
            };
            bwConn.RunWorkerCompleted += (sender1, e1) =>
            {

                if (e1.Error != null)
                {
                    MessageBox.Show(e1.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (status=="ok")
                {
                    MessageBox.Show("Login Sucessfull !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                }
                else if(status=="not ok")
                {
                    MessageBox.Show("Login Failed !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtpwd.Clear();
                    txtuser.Clear();
                    cmbtype.SelectedIndex = 0;
                }
                else
                    MessageBox.Show("Please enter all fields !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pb.Close();
                pb.Dispose();

            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (txtpwd.Enabled == true || txtuser.Enabled == true || cmbtype.Enabled==true || cmbname.Enabled==true)
                {
                    if (txtuser.Text == "" || txtpwd.Text == "" || cmbname.SelectedIndex == 0 || cmbtype.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please enter all credentials !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Properties.Settings.Default["Username"] = txtuser.Text;
                    Properties.Settings.Default["Password"] = txtpwd.Text;
                    Properties.Settings.Default["CenterType"] = cmbtype.Text;
                    Properties.Settings.Default["CenterName"] = cmbname.Text;
                    Properties.Settings.Default.Save();                    
                }
            }
            else if (checkBox1.Checked == false)
            {
                Properties.Settings.Default["Username"] = "";
                Properties.Settings.Default["Password"] = "";
                Properties.Settings.Default["CenterType"] = "";
                Properties.Settings.Default["CenterName"] = "";
                Properties.Settings.Default.Save();
                txtuser.Enabled = true;
                txtpwd.Enabled = true;
                cmbname.Enabled = true;
                cmbtype.Enabled = true;
                txtuser.Clear();
                txtpwd.Clear();
                fillCenterType();
            }
        }

        private void cmbtype_TextChanged(object sender, EventArgs e)
        {
            if (cmbtype.Text != "")
                fillCenterName(cmbtype.Text);
        }
    }
}
