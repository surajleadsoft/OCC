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
    public partial class Remark : UserControl
    {
        public static string created_by = "";
        public static string centerType = "";
        public static string centerName = "";
        string gender = "";
        string status = "";
        string adhar = "";
        string test = "";
        public Remark()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            RadioButton.CheckForIllegalCrossThreadCalls = false;
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            DateTimePicker.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            DataGridView.CheckForIllegalCrossThreadCalls = false;
        }
        public void setDetails(string user, string type, string name)
        {
            Remark.created_by = user;
            Remark.centerType = type;
            Remark.centerName = name;
        }
        private void fillCenterType()
        {
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT CenterType FROM centerType";
                    cmbtype.Items.Add("--CENTER TYPE--");
                    cmbname.Items.Add("--CENTER NAME--");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("CenterType");
                                if (!cmbtype.Items.Contains(name))
                                    cmbtype.Items.Add(name);
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
                cmbtype.SelectedIndex = 0;
            };
            if (cmbtype.Items.Count > 0)
                cmbtype.Items.Clear();
            if (cmbname.Items.Count > 0)
                cmbname.Items.Clear();
            bwConn.RunWorkerAsync();          
            
        }
        private void fillCenterName()
        {
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT CenterName FROM Centers WHERE CenterType='" + cmbtype.Text + "'";
                    cmbname.Items.Add("--CENTER NAME--");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("CenterName");
                                if (!cmbname.Items.Contains(name))
                                    cmbname.Items.Add(name);
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
                cmbname.SelectedIndex = 0;
                pb.Close();
                
            };
            if (cmbname.Items.Count > 0)
                cmbname.Items.Clear();
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
        }
        public void clearAll()
        {
            lbid.Text = "";
            txtadd.Text = "";
            lbtestname.Text = "";
            txtadhar.Text = "";
            txtage.Text = "";
            txtcontact.Text = "";
            txtdate.Text = DateTime.Now.ToShortDateString();
            txtname.Text = "";
            txtother.Text = "";
            gender = "";
            txtremark.Text = "";
            txtremark.ReadOnly = true;
            txtother.ReadOnly = true;
            fillCenterType();
        }
        public void fillgrid()
        {
            if (gridRemarkList.Rows.Count > 0)
                gridRemarkList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM PatientsRemark WHERE CenterType='" + Remark.centerType + "' and CenterName='"+Remark.centerName+"'";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date, fn, cn, gender, add, age, adhar;
                            string id = reader["ID"].ToString();
                            date = reader["Date"].ToString();
                            fn = reader["Name"].ToString();
                            cn = reader["ContactNo"].ToString();
                            gender = reader["Gender"].ToString();
                            add = reader["Address"].ToString();
                            age = reader["Age"].ToString();
                            adhar = reader["AdharCard"].ToString();
                            string type = reader["CenterType"].ToString();
                            string name = reader["CenterName"].ToString();
                            string remark = reader["Remark"].ToString();
                            gridRemarkList.Rows.Add(id, DateTime.Parse(date), fn, cn, gender, add, age, adhar, remark, type, name);
                        }
                    }
                }
            }            
        }
        public void unlockAll(bool b)
        {
            panelInput.Enabled = b;
        }
        public void actionButtonNormalStage()
        {
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnDel.Enabled = false;
            btnUp.Enabled = false;
            btnCan.Enabled = false;
        }
        public void actionButtonEditStage()
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnDel.Enabled = false;
            btnUp.Enabled = false;
            btnCan.Enabled = true;
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }
        private void getMaxID()
        {
            int srno = 0;
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT MAX(ID) FROM PatientsRemark";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string id = reader[0].ToString();
                                if(id!="")
                                    srno = Int32.Parse(id);
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
                srno += 1;
                lbid.Text = srno.ToString();
            };
            bwConn.RunWorkerAsync();
            
            
        }
       
        private bool validation()
        {

            if (txtremark.Text=="" || cmbtype.SelectedIndex==0 || cmbname.SelectedIndex==0)
                return false;
            else
                return true;
        }
        private void getMaxID(int srno)
        {
            string id = "";
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT MAX(ID) FROM Patients";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = reader[0].ToString();
                        }
                    }
                }
            }
            if (id != "")
                srno = Int32.Parse(id);
            srno = srno + 1;
            lbid.Text = srno.ToString();
        }
        private void saveDetails()
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
                if (status.Equals("ADD NEW"))
                {

                    DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Add This Patient's Remark ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (drs == DialogResult.No)
                        return;
                    MySqlConnection cn = new MySqlConnection();
                    cn.ConnectionString = DbConnect.conString;
                    cn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    string[] address = txtadd.Text.Split(',');
                    int flag = 1;
                    if (txtremark.Text == "Discharged")
                    {
                        flag = 0;
                        string bedname = getBedDetails("SELECT BedName From Patients WHERE Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "'");
                        string WardCode = getBedDetails("SELECT WardCode From Patients WHERE Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "'");
                        cmd.CommandText = "UPDATE Beds SET Flag=0 WHERE BedName='"+bedname+"' and WardCode='"+WardCode+"'";
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "INSERT INTO PatientsRemark (Name,ContactNo,Gender,Address,Age,AdharCard,Date,Remark,CenterType,CenterName,CreatedBy,CreatedAt,Area,District,Flag) VALUES ('" + txtname.Text + "','" + txtcontact.Text + "','" + gender + "','" + txtadd.Text + "'," + txtage.Text + ",'" + txtadhar.Text + "',@date,'" + txtremark.Text + "','" + centerType + "','" + centerName + "','" + created_by + "','" + DateTime.Now.ToString("hh:mm:ss tt") + "','" + address[0] + "','" + address[1] + "'," + flag + ")";
                    cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    MessageBox.Show("Patient's Remark Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();
                }
                else if (status.Equals("UPDATE"))
                {
                    MySqlConnection cn = new MySqlConnection();
                    cn.ConnectionString = DbConnect.conString;
                    cn.Open();
                    DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Update This Patient's Remark ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (drs == DialogResult.No)
                        return;
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    int flag = 1;
                    if (txtremark.Text == "Discharged")
                    {
                        flag = 0;
                        string bedname = getBedDetails("SELECT BedName From Patients WHERE Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "'");
                        string WardCode = getBedDetails("SELECT WardCode From Patients WHERE Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "'");
                        cmd.CommandText = "UPDATE Beds SET Flag=0 WHERE BedName='" + bedname + "' and WardCode='" + WardCode + "'";
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "UPDATE PatientsRemark SET Name='" + txtname.Text + "',ContactNo='" + txtcontact.Text + "',Gender='" + gender + "',Address='" + txtadd.Text + "',Age=" + txtage.Text + ",AdharCard='" + txtadhar.Text + "',Date=@date,Remark='" + txtremark.Text + "',CenterType='" + centerType + "',CenterName='" + centerName + "',CreatedBy='" + created_by + "',CreatedAt='" + DateTime.Now.ToString("hh:mm:ss tt") + "',Flag="+flag+" WHERE ID=" + lbid.Text;
                    cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();                    
                    MessageBox.Show("Patient's Remark Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cn.Close();
                }
                
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                pb.Close();
                
                clearAll();
                actionButtonNormalStage();
                unlockAll(false);
                fillgrid();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
        }
        private int getCount1()
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(ID) FROM HighRiskPatients WHERE Name='" + txtname.Text + "' and AdharCard='" + txtadhar.Text + "'";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader[0].ToString();
                            srno = Int32.Parse(id);
                        }
                    }
                }
            }
            return srno;
        }
        private string getBedDetails(string query)
        {
            string srno = "";
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();                
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader[0].ToString();
                            srno = id;
                        }
                    }
                }
            }
            return srno;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDetails();   
        }
        private void deleteDetails()
        {
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Delete This Patient's Remark ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM PatientsRemark WHERE ID=" + lbid.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient's Remark Deleted Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();
               
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                pb.Close();
                
                clearAll();
                actionButtonNormalStage();
                unlockAll(false);
                fillgrid();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();            
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            deleteDetails();
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            clearAll();
            actionButtonNormalStage();
            unlockAll(false);
            fillgrid();
        }

        private void gridRemarkList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            status = "UPDATE";
            actionButtonEditStage();
            unlockAll(true);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            actionButtonEditStage();
            clearAll();
            unlockAll(true);
            getMaxID();
            status = "ADD NEW";
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtype.SelectedIndex != 0)
                fillCenterName();
        }

        private void gridRemarkList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            unlockAll(false);
            status = "VIEW";
            lbid.Text = gridRemarkList.CurrentRow.Cells["ID"].Value.ToString();
            txtname.Text = gridRemarkList.CurrentRow.Cells["FullName"].Value.ToString();
            txtadd.Text = gridRemarkList.CurrentRow.Cells["Address"].Value.ToString();
            txtadhar.Text = gridRemarkList.CurrentRow.Cells["AdharCard"].Value.ToString();
            txtage.Text = gridRemarkList.CurrentRow.Cells["Age"].Value.ToString();
            txtcontact.Text = gridRemarkList.CurrentRow.Cells["ContactNo"].Value.ToString();
            txtdate.Text = gridRemarkList.CurrentRow.Cells["Date"].Value.ToString();
            cmbtype.Text = gridRemarkList.CurrentRow.Cells["CenterType1"].Value.ToString();
            cmbname.Text = gridRemarkList.CurrentRow.Cells["CenterName1"].Value.ToString();
            string remark= gridRemarkList.CurrentRow.Cells["Remark1"].Value.ToString();
            gender = gridRemarkList.CurrentRow.Cells["Gender1"].Value.ToString();
            getDetails(txtname.Text, txtcontact.Text);
            if (gender == "Male")
                radioButton4.Checked = true;
            else
                radioButton3.Checked = true;
            txtremark.Text = remark;
            actionButtonEditStage();
            btnSave.Enabled = false;
            btnDel.Enabled = true;
            btnUp.Enabled = true;
        }
        private void setDetails()
        {
            txtadd.Text = getDetails("SELECT Address FROM Patients Where (Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "') and (AdharCard='" + adhar + "')");
            txtage.Text = getDetails("SELECT Age FROM Patients Where (Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "') and (AdharCard='" + adhar + "')");
            txtother.Text = getDetails("SELECT OtherDiseases FROM Patients Where (Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "') and (AdharCard='" + adhar + "')");
            lbtestname.Text = getDetails("SELECT TestName FROM Patients Where (Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "') and (AdharCard='" + adhar + "')");
            test = getDetails("SELECT isTest FROM Patients Where (Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "') and (AdharCard='" + adhar + "')");
            gender = getDetails("SELECT Gender FROM Patients Where (Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "') and (AdharCard='" + adhar + "')");
            if (gender.Equals("Male"))
                radioButton4.Checked = true;
            else
                radioButton3.Checked = true;
            if (test.Equals("Yes"))
                radioButton6.Checked = true;
            else
                radioButton5.Checked = true;
            panel19.Enabled = false;
            panel11.Enabled = false;
            txtremark.Focus();
            txtname.Enabled = false;
            txtcontact.Enabled = false;
            txtage.Enabled = false;
            txtadhar.Enabled = false;
            txtadd.Enabled = false;
            lbtestname.Enabled = false;
            

        }
        private void txtname_MouseEnter(object sender, EventArgs e)
        {
            
        }
        private string getDetails(string query)
        {
            string details = "";
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            details = reader[0].ToString();
                        }
                    }
                }
            }
            return details;
        }
        private void getDetails(string name,string contact)
        {            
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT isTest,TestName,OtherDiseases From Patients WHERE Name='"+name+"' and ContactNo='"+contact+"'", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lbtestname.Text = reader["TestName"].ToString();
                            string isTest = reader["isTest"].ToString();
                            txtother.Text = reader["OtherDiseases"].ToString();
                            if(isTest.Equals("Yes"))
                                radioButton6.Checked=true;
                            else
                                radioButton5.Checked=true;
                        }
                    }
                }
            }
        }
        private void cmbaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void search()
        {
            if (gridRemarkList.Rows.Count > 0)
                gridRemarkList.Rows.Clear();

            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM PatientsRemark WHERE (CenterType='" + Remark.centerType + "' and CenterName='" + Remark.centerName + "') and (Name LIKE '%" + textBox1.Text + "%')";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date, fn, cn, gender, add, age, adhar;
                            string id = reader["ID"].ToString();
                            date = reader["Date"].ToString();
                            fn = reader["Name"].ToString();
                            cn = reader["ContactNo"].ToString();
                            gender = reader["Gender"].ToString();
                            add = reader["Address"].ToString();
                            age = reader["Age"].ToString();
                            adhar = reader["AdharCard"].ToString();
                            string type = reader["CenterType"].ToString();
                            string name = reader["CenterName"].ToString();
                            string remark = reader["Remark"].ToString();
                            gridRemarkList.Rows.Add(id, DateTime.Parse(date), fn, cn, gender, add, age, adhar, remark, type, name);
                        }
                    }
                }
            }
            gridRemarkList.ClearSelection();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            search();           
        }

        private void txtname_KeyDown(object sender, KeyEventArgs e)
        {
            if (status.Equals("ADD NEW"))
            {
                FullnameForm ob = new FullnameForm(created_by);
                ob.ShowDialog();
                string result = ob.getResult();
                string[] details = result.Split('-');
                txtname.Text = details[0].ToString();
                adhar = details[2].ToString();
                if (adhar == "Not Available")
                    checkBox1.Checked = true;
                else
                {
                    checkBox1.Checked = false;
                    txtadhar.Text = adhar;
                }
                txtadhar.Text = details[2].ToString();
                txtcontact.Text = details[1].ToString();
                setDetails();
                txtname.ReadOnly = true;
            }
            else
                txtname.ReadOnly = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (panelBg.Size == panelBg.MaximumSize)
                panelBg.Size = panelBg.MinimumSize;
            else
                panelBg.Size = panelBg.MaximumSize;
        }

        private void txtremark_KeyDown(object sender, KeyEventArgs e)
        {
            SelectRemark select = new SelectRemark();
            select.ShowDialog(this);
            txtremark.Text = select.getRemark();
        }

        private void panelInput_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
