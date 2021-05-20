using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;

namespace OCC
{
    public partial class AddUsers : UserControl
    {
        string id = "";
        string status = "";
        string gender = "";
        public AddUsers()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
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
                
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        public void clearAll()
        {
            txtadhar.Text = "";
            txtcontact.Text = "";
            txtdob.Text = DateTime.Now.ToShortDateString();
            txtfn.Text = "";
            txtidno.Text = "";
            txtemail.Text="";
            txtln.Text = "";
            txtemail.ReadOnly = false;
            fillCenterType();
        }
        void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            
        }
        private void sendEmail()
        {
            string emailFromAddress = "suraj.leadsoft@gmail.com"; //Sender Email Address  
            string password = "Ledso@143"; //Sender Password  
            string emailToAddress = txtemail.Text; //Receiver Email Address  
            string subject = "Osmanabad Covid Care Application Download";
            string mobile = "http://leadsoftsolutions.online/TernaQuarantineCenter/downloads/download.html";
            string body = "Dear " + txtfn.Text + ",\n\nYou've registered for Osmanabad Covid Care Management System. Please find your login credentials here.\n\nUsername: " + txtemail.Text + "\nPassword:12345\n\nDownload Links\nMobile App: " + mobile + "\nDesktop App : " + mobile + "\n\n\nThanks & Regards,\nApp Development Team,\nLeadSoft IT Solutions,\n7028816463.";
            MailMessage message = new MailMessage();
            SmtpClient smtp;
            message.Subject = subject;
            message.Body = body;
            message.To.Add(new MailAddress(txtemail.Text));
            message.From = new MailAddress(emailFromAddress, "LeadSoft IT Solutions");
            smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(emailFromAddress, password);
            smtp.SendAsync(message, message.Subject);
            smtp.SendCompleted += new SendCompletedEventHandler(smtp_SendCompleted);
        }
        public void unlockAll(bool b)
        {
            panel4.Enabled = b;
            panel9.Enabled = b;
            panel17.Enabled = b;
        }
         public void fillgrid()
         { 
            if (gridUserList.Rows.Count > 0)
                gridUserList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From EmpRegister Order By ID";
                
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string id = reader["ID"].ToString();
                            string fn=reader["FirstName"].ToString();
                            string ln=reader["LastName"].ToString();
                            string dob=reader["DOB"].ToString();
                            string ge=reader["Gender"].ToString();
                            string email=reader["Email"].ToString();
                            string contact=reader["ContactNo"].ToString();
                            string adhar=reader["Adharcard"].ToString();
                            string gid=reader["GovtID"].ToString();
                            gridUserList.Rows.Add(id,fn,ln,dob,ge,email,contact,adhar,gid);
                        }
                    }                
                }            
            }
            gridUserList.ClearSelection();
         }
        public void actionButtonNormalStage()
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
            btnCan.Enabled = false;
        }
        public void actionButtonEditStage()
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnUp.Enabled = false;
            btnDel.Enabled = false;
            btnCan.Enabled = true;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (panelBg.Size == panelBg.MaximumSize)
                panelBg.Size = panelBg.MinimumSize;
            else
                panelBg.Size = panelBg.MinimumSize;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            unlockAll(true);
            clearAll();
            actionButtonEditStage();
            status = "ADD NEW";
        }

        private void panelBg_Paint()
        {
        
        }
        private bool validation()
        {
            if (!txtadhar.MaskFull || !txtcontact.MaskFull || txtfn.Text == "" || txtln.Text == "" || txtidno.Text == "" || txtln.Text == "" || gender == "" || cmbname.SelectedIndex==0 || cmbtype.SelectedIndex==0)
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
            if (status.Equals("ADD NEW"))
            {
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Add This User ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                sendEmail();
                cmd.CommandText = "INSERT INTO EmpRegister(FirstName,LastName,DOB,Gender,Email,ContactNo,AdharCard,GovtID) VALUES ('"+txtfn.Text+"','"+txtln.Text+"','"+txtdob.Text+"','"+gender+"','"+txtemail.Text+"','"+txtcontact.Text+"','"+txtadhar.Text+"','"+txtidno.Text+"')";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Login(Username,Password,CenterType,CenterName) VALUES ('"+txtemail.Text+"','12345','"+cmbtype.Text+"','"+cmbname.Text+"')";
                cmd.ExecuteNonQuery();              
                MessageBox.Show("User Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
            }
            if (status.Equals("UPDATE"))
            {
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Update This User ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Connection = cn;
                cmd.CommandText = "UPDATE EmpRegister SET FirstName='" + txtfn.Text + "',LastName='" + txtln.Text + "',DOB='" + txtdob.Text + "',Gender='" + gender + "',ContactNo='" + txtcontact.Text + "',AdharCard='" + txtadhar.Text + "',GovtID= '" + txtidno.Text + "' WHERE Email='" + txtemail.Text + "'";
                cmd.ExecuteNonQuery();
                MessageBox.Show("User Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
            }
            clearAll();
            unlockAll(false);
            fillgrid();
            actionButtonNormalStage();
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            clearAll();
            unlockAll(false);
            fillgrid();
            actionButtonNormalStage();
        }

        private void rdbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMale.Checked)
                gender = "Male";
        }

        private void rdbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFemale.Checked)
                gender = "Female";
        }
        private void getCenterDetails()
        {
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From Login WHERE Username='"+txtemail.Text+"'";// Order By ID";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cmbtype.Text = reader["CenterType"].ToString();
                            cmbname.Text = reader["CenterName"].ToString();
                        }
                        else
                        {
                            cmbname.Text = "";
                            cmbtype.Text = "";
                        }
                    }
                }
            }
        }
        private void gridUserList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtfn.Text = gridUserList.CurrentRow.Cells["FirstName"].Value.ToString();
            txtln.Text = gridUserList.CurrentRow.Cells["LastName"].Value.ToString();
            txtcontact.Text = gridUserList.CurrentRow.Cells["ContactNo"].Value.ToString();
            txtemail.Text = gridUserList.CurrentRow.Cells["Email"].Value.ToString();
            txtdob.Text = gridUserList.CurrentRow.Cells["DOB"].Value.ToString();
            gender = gridUserList.CurrentRow.Cells["Gender"].Value.ToString();
            id = gridUserList.CurrentRow.Cells["ID"].Value.ToString();
            txtadhar.Text = gridUserList.CurrentRow.Cells["AdharCard"].Value.ToString();
            txtidno.Text = gridUserList.CurrentRow.Cells["IDProof"].Value.ToString();
            getCenterDetails();
            actionButtonEditStage();
            unlockAll(false);
            btnSave.Enabled = false;
            btnUp.Enabled = true;
            btnDel.Enabled = true;
            if (gender == "Male")
                rdbMale.Checked = true;
            else
                rdbFemale.Checked = true;
            txtemail.ReadOnly =true;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Delete This User ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            MySqlConnection cn = new MySqlConnection();
            cn.ConnectionString = DbConnect.conString;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = cn;
            cmd.CommandText = "DELETE FROM EmpRegister WHERE Email='"+txtemail.Text+"'";            
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DELETE FROM Login WHERE Username='" + txtemail.Text + "'";
            cmd.ExecuteNonQuery();
            MessageBox.Show("User Deleted Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cn.Close();
            clearAll();
            unlockAll(false);
            fillgrid();
            actionButtonNormalStage();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            unlockAll(true);
            actionButtonEditStage();
            status = "UPDATE";
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtype.SelectedIndex != 0)
                fillCenterName(cmbtype.Text);
        }
    }
}
