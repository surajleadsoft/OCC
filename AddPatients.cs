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
    public partial class AddPatients : UserControl
    {
        public static string created_by = "";
        public static string centerType = "";
        public static string centerName = "";
        string gender = "", test = "", adhar = "";
        string status = "";
        public AddPatients()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            RadioButton.CheckForIllegalCrossThreadCalls = false;
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            DateTimePicker.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            DataGridView.CheckForIllegalCrossThreadCalls = false;
            clearAll();
        }
        public void setDetails(string user, string type, string name)
        {
            AddPatients.created_by = user;
            AddPatients.centerType = type;
            AddPatients.centerName = name;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (panel1.Size == panel1.MaximumSize)
                panel1.Size = panel1.MinimumSize;
            else
                panel1.Size = panel1.MaximumSize;
        }
        public void clearAll()
        {
            panelAddress.Size = panelAddress.MinimumSize;
            panelAContain.Size = panelAContain.MinimumSize;
            panelDiseases.Size = panelDiseases.MinimumSize;
            panelRemark.Size = panelRemark.MinimumSize;
            panelRContains.Size = panelRContains.MinimumSize;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
            radioButton5.Checked = false;
            radioButton6.Checked = false;
            fillWardCode();
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
            test = "";
            txtremark.Text = "";
            txtadd.ReadOnly = true;
            txtremark.ReadOnly = true;
            txtother.ReadOnly = true;
            fillgrid();
            gridPatientsList.Update();
            gridPatientsList.ClearSelection();
            checkBox1.Checked = false;
        }
        private void fillWardCode()
        {
            if (cmbcode.Items.Count > 0)
                cmbcode.Items.Clear();

            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT WardCode FROM Wards WHERE CenterType='" + AddPatients.centerType + "' and CenterName='" + AddPatients.centerName + "'";
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
            cmbcode.SelectedIndex = 0;
        }
        public void fillgrid()
        {
            if (gridPatientsList.Rows.Count > 0)
                gridPatientsList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT ID,Date,Name,ContactNo,Gender,Address,Age,AdharCard,TestName,OtherDiseases,Remark,WardCode,BedName FROM Patients WHERE CenterType='" + AddPatients.centerType + "' and CenterName='" + AddPatients.centerName + "'";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date, fn, cn, gender, add, age, adhar, testn, other;
                            string id = reader["ID"].ToString();
                            date = reader["Date"].ToString();
                            fn = reader["Name"].ToString();
                            cn = reader["ContactNo"].ToString();
                            gender = reader["Gender"].ToString();
                            add = reader["Address"].ToString();
                            age = reader["Age"].ToString();
                            adhar = reader["AdharCard"].ToString();
                            testn = reader["TestName"].ToString();
                            other = reader["OtherDiseases"].ToString();
                            string remark = reader["Remark"].ToString();
                            string wardcode = reader["WardCode"].ToString();
                            string bedname = reader["BedName"].ToString();
                            gridPatientsList.Rows.Add(id, DateTime.Parse(date), fn, cn, gender, add, age, adhar, testn, other, remark,wardcode,bedname);
                        }
                    }
                }
            }

            gridPatientsList.ClearSelection();
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
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

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
        private int getCount()
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(ID) FROM Patients WHERE Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "'";
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
        private int getCount1()
        {
            int srno = 0;
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
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
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            bwConn.RunWorkerAsync();
            return srno;
        }
        private bool validation()
        {
            if (adhar == "" || txtage.Text == "" || !txtcontact.MaskFull || txtname.Text == "" || txtother.Text == "" || txtremark.Text == "" || gender == "" || test == "" || cmbbedname.SelectedIndex==0 || cmbcode.SelectedIndex==0)
                return false;
            else
                return true;
        }
        private string saveDetails()
        {
            string bedname = cmbbedname.Text;
            string WardCode = cmbcode.Text;
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            string result = "";            
            bwConn.DoWork += (sender, e) =>
            {
                if (checkBox1.Checked)
                    adhar = "Not Available";
                else
                    adhar = txtadhar.Text;
                if (!validation())
                {
                    result = "error";
                }
                else
                {
                    if (status.Equals("ADD NEW"))
                    {

                        DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Add This Patient ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (drs == DialogResult.No)
                            return;
                        MySqlConnection cn = new MySqlConnection();
                        cn.ConnectionString = DbConnect.conString;
                        cn.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        string[] address = txtadd.Text.Split(',');                        
                        cmd.CommandText = "INSERT INTO Patients (Name,ContactNo,Gender,Address,Age,AdharCard,Date,Remark,CenterType,CenterName,isTest,TestName,OtherDiseases,CreatedBy,CreatedAt,Area,District,WardCode,BedName) VALUES ('" + txtname.Text + "','" + txtcontact.Text + "','" + gender + "','" + txtadd.Text + "'," + txtage.Text + ",'" + adhar + "',@date,'" + txtremark.Text + "','" + centerType + "','" + centerName + "','" + test + "','" + lbtestname.Text + "','" + txtother.Text + "','" + created_by + "','" + DateTime.Now.ToString("hh:mm:ss tt") + "','" + address[0] + "','" + address[1] + "','" + WardCode + "','" + bedname + "')";                        
                        cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        int flag = 1;
                        if (txtremark.Text == "Discharged")
                        {
                            flag = 0;                            
                            cmd.CommandText = "UPDATE Beds SET Flag=0 WHERE BedName='" + bedname + "' and WardCode='" + WardCode + "'";
                            cmd.ExecuteNonQuery();
                        }
                        cmd.CommandText = "INSERT INTO PatientsRemark (Name,ContactNo,Gender,Address,Age,AdharCard,Date,Remark,CenterType,CenterName,CreatedBy,CreatedAt,Area,District,Flag) VALUES ('" + txtname.Text + "','" + txtcontact.Text + "','" + gender + "','" + txtadd.Text + "'," + txtage.Text + ",'" + adhar + "',@date,'" + txtremark.Text + "','" + centerType + "','" + centerName + "','" + created_by + "','" + DateTime.Now.ToString("hh:mm:ss tt") + "','" + address[0] + "','" + address[1] + "'," + flag + ")";
                        cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cmd.CommandText = "UPDATE Beds SET Flag=1 WHERE (BedName='" + bedname + "') and (CenterType='" + centerType + "' and CenterName='" + centerName + "')";
                        cmd.ExecuteNonQuery();
                        string diseases = txtother.Text;
                        string[] diss = diseases.Split(',');
                        if (diss.Length > 1)
                        {
                            getMaxID(0);
                            cmd.CommandText = "INSERT INTO HighRiskPatients (ID,Name,ContactNo,Gender,Address,Age,AdharCard,Date,Remark,CenterType,CenterName,isTest,TestName,OtherDiseases,CreatedBy,CreatedAt,Area,District) VALUES (" + lbid.Text + ",'" + txtname.Text + "','" + txtcontact.Text + "','" + gender + "','" + txtadd.Text + "'," + txtage.Text + ",'" + adhar + "',@date,'" + txtremark.Text + "','" + centerType + "','" + centerName + "','" + test + "','" + lbtestname.Text + "','" + txtother.Text + "','" + created_by + "','" + DateTime.Now.ToString("hh:mm:ss tt") + "','" + address[0] + "','" + address[1] + "')";
                            cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                            cmd.ExecuteNonQuery();
                        }
                        cn.Close();
                        result = "success";
                    }
                    else if (status.Equals("UPDATE"))
                    {
                        MySqlConnection cn = new MySqlConnection();
                        cn.ConnectionString = DbConnect.conString;
                        cn.Open();
                        DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Update This Patient ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (drs == DialogResult.No)
                            return;
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        if (checkBox1.Checked)
                            adhar = "Not Available";
                        else
                            adhar = txtadhar.Text;
                        string[] address = txtadd.Text.Split(',');
                        int flag = 1;
                        if (txtremark.Text == "Discharged")
                        {
                            flag = 0;
                            cmd.CommandText = "UPDATE Beds SET Flag=0 WHERE BedName='" + bedname + "' and WardCode='" + WardCode + "'";
                            cmd.ExecuteNonQuery();
                        }
                       
                        cmd.CommandText = "UPDATE Patients SET Name='" + txtname.Text + "',ContactNo='" + txtcontact.Text + "',Gender='" + gender + "',Address='" + txtadd.Text + "',Age=" + txtage.Text + ",AdharCard='" + adhar + "',Date=@date,Remark='" + txtremark.Text + "',CenterType='" + centerType + "',CenterName='" + centerName + "',isTest='" + test + "',TestName='" + lbtestname.Text + "',OtherDiseases='" + txtother.Text + "',CreatedBy='" + created_by + "',CreatedAt='" + DateTime.Now.ToString("hh:mm:ss tt") + "',Area='" + address[0] + "',District='" + address[1] + "',WardCode='" + WardCode + "',BedName='" + bedname + "' WHERE ID=" + lbid.Text;
                        cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cmd.CommandText = "UPDATE Beds SET Flag=1 WHERE (BedName='" + bedname + "') and (CenterType='" + centerType + "' and CenterName='" + centerName + "')";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "UPDATE PatientsRemark SET Name='" + txtname.Text + "',ContactNo='" + txtcontact.Text + "',Gender='" + gender + "',Address='" + txtadd.Text + "',Age=" + txtage.Text + ",AdharCard='" + adhar + "',Date=@date,Remark='" + txtremark.Text + "',CenterType='" + centerType + "',CenterName='" + centerName + "',CreatedBy='" + created_by + "',CreatedAt='" + DateTime.Now.ToString("hh:mm:ss tt") + "',Area='" + address[0] + "',District='" + address[1] + "',Flag=" + flag + " WHERE Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "'";
                        cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        string diseases = txtother.Text;
                        string[] diss = diseases.Split(',');
                        if (diss.Length > 1 && getCount1() == 0)
                        {
                            getMaxID(0);
                            cmd.CommandText = "INSERT INTO HighRiskPatients (ID,Name,ContactNo,Gender,Address,Age,AdharCard,Date,Remark,CenterType,CenterName,isTest,TestName,OtherDiseases,CreatedBy,CreatedAt,Area,District) VALUES (" + lbid.Text + ",'" + txtname.Text + "','" + txtcontact.Text + "','" + gender + "','" + txtadd.Text + "'," + txtage.Text + ",'" + adhar + "',@date,'" + txtremark.Text + "','" + centerType + "','" + centerName + "','" + test + "','" + lbtestname.Text + "','" + txtother.Text + "','" + created_by + "','" + DateTime.Now.ToString("hh:mm:ss tt") + "','" + address[0] + "','" + address[1] + "')";
                            cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            // getMaxID(0);
                            cmd.CommandText = "UPDATE HighRiskPatients SET Name='" + txtname.Text + "',ContactNo='" + txtcontact.Text + "',Gender='" + gender + "',Address='" + txtadd.Text + "',Age=" + txtage.Text + ",AdharCard='" + adhar + "',Date=@date,Remark='" + txtremark.Text + "',CenterType='" + centerType + "',CenterName='" + centerName + "',isTest='" + test + "',TestName='" + lbtestname.Text + "',OtherDiseases='" + txtother.Text + "',CreatedBy='" + created_by + "',CreatedAt='" + DateTime.Now.ToString("hh:mm:ss tt") + "',Area='" + address[0] + "',District='" + address[1] + "' WHERE Name='" + txtname.Text + "' and ContactNo='" + txtcontact.Text + "'";
                            cmd.Parameters.AddWithValue("@date", DateTime.Parse(txtdate.Text).ToString("yyyy-MM-dd"));
                            cmd.ExecuteNonQuery();
                        }
                        cn.Close();
                        result = "success";
                    }
                }
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (result != "error")
                {
                    clearAll();
                    actionButtonNormalStage();
                    unlockAll(false);
                    fillgrid();
                }
                else
                {
                    if (txtadd.Text == "")
                    {
                        MessageBox.Show("Please Enter Address !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtadd.Focus();
                        
                    }
                    else if (adhar=="")
                    {
                        MessageBox.Show("Please Enter AdharCard No !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtadhar.Focus();
                        
                    }
                    else if (txtage.Text == "")
                    {
                        MessageBox.Show("Please Enter Age !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtage.Focus();
                        
                    }
                    else if(!txtcontact.MaskFull)
                    {
                        MessageBox.Show("Please Enter ContactNo !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtcontact.Focus();
                        
                    }
                    else if(txtname.Text=="")
                    {
                        MessageBox.Show("Please Enter Fullname Of Patients !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtname.Focus();
                        
                    }
                    else if(txtother.Text=="")
                    {
                        MessageBox.Show("Please Enter Other Diseases !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtother.Focus();
                        
                    }
                    else if(txtremark.Text=="")
                    {
                        MessageBox.Show("Please Enter Remark Of Patients !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtremark.Focus();
                        
                    }
                    else if(lbtestname.Text=="")
                    {
                        MessageBox.Show("Please Select Test Done or Not?? !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                        
                        
                    }
                    else if(gender=="")
                    {
                        MessageBox.Show("Please Gender Of Patients !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    else if (cmbcode.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Ward Code of Patients !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cmbcode.Focus();
                    }
                    else if (cmbbedname.SelectedIndex == 0)
                    {
                        MessageBox.Show("Please select Bed Name of Patients !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cmbbedname.Focus();
                    }
                }
                pb.Close();
                
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            string result = saveDetails();            
            if (result == "success" && status == "ADD NEW")
            {
                MessageBox.Show("Patient Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == "success" && status == "UPDATE")
            {
                MessageBox.Show("Patient Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (result == "exist" && status == "ADD NEW")
            {

                MessageBox.Show("Patients Already Exists !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            gridPatientsList.Update();
            gridPatientsList.ClearSelection();
        }

        private void panelInput_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                gender = "Male";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                gender = "Female";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked && (status == "ADD NEW" || status == "UPDATE"))
            {
                test = "Yes";
                TestName testname = new TestName();
                testname.ShowDialog();
                lbtestname.Text = testname.getTestName();
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                test = "No";
                lbtestname.Text = "-";
            }
        }

        private void txtother_Enter(object sender, EventArgs e)
        {
           
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            status = "UPDATE";
            actionButtonEditStage();
            unlockAll(true);
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
                DialogResult drs = MessageBox.Show("Are You Sure Do You Want To Delete This Patient ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Patients WHERE ID=" + lbid.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Patient Deleted Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cn.Close();

            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                clearAll();
                actionButtonNormalStage();
                unlockAll(false);
                fillgrid();
                pb.Close();
                
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            deleteDetails();
            gridPatientsList.Update();
            gridPatientsList.ClearSelection();
        }

        private void btnCan_Click(object sender, EventArgs e)
        {
            clearAll();
            actionButtonNormalStage();
            unlockAll(false);
            fillgrid();
            gridPatientsList.Update();
            gridPatientsList.ClearSelection();
        }

        private void gridPatientsList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gridPatientsList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            unlockAll(false);
            status = "VIEW";
            lbid.Text = gridPatientsList.CurrentRow.Cells["ID"].Value.ToString();
            txtname.Text = gridPatientsList.CurrentRow.Cells["FullName"].Value.ToString();
            txtadd.Text = gridPatientsList.CurrentRow.Cells["Address"].Value.ToString();
            adhar = gridPatientsList.CurrentRow.Cells["AdharCardNo"].Value.ToString();
            txtage.Text = gridPatientsList.CurrentRow.Cells["Age"].Value.ToString();
            txtcontact.Text = gridPatientsList.CurrentRow.Cells["ContactNo"].Value.ToString();
            txtdate.Text = gridPatientsList.CurrentRow.Cells["Date"].Value.ToString();
            txtother.Text = gridPatientsList.CurrentRow.Cells["OtherDiseases"].Value.ToString();
            txtremark.Text = gridPatientsList.CurrentRow.Cells["Remark"].Value.ToString();
            gender = gridPatientsList.CurrentRow.Cells["Gender1"].Value.ToString();
            test = gridPatientsList.CurrentRow.Cells["TestName"].Value.ToString();                        
            string wardcode = "", bedname = "";
            if(gridPatientsList.CurrentRow.Cells["WardCode"].Value != null)
                wardcode = gridPatientsList.CurrentRow.Cells["WardCode"].Value.ToString();
            if(gridPatientsList.CurrentRow.Cells["BedName"].Value !=null)
                bedname = gridPatientsList.CurrentRow.Cells["BedName"].Value.ToString();
            if (wardcode != "")
            {
                cmbcode.Text = wardcode;
                fillBedNames1();
            }
            else
                fillWardCode();
            if (bedname != "")
                cmbbedname.Text = bedname;
            if (adhar == "Not Available")
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
                txtadhar.Text = adhar;
            }
            if (test != "-")
            {
                radioButton6.Checked = true;
                lbtestname.Text = test;
            }
            else
            {
                lbtestname.Text = test;
                radioButton5.Checked = true;
            }
            if (gender == "Male")
            {
                radioButton4.Checked = true;
            }
            else
                radioButton3.Checked = true;
            actionButtonEditStage();
            btnSave.Enabled = false;
            btnDel.Enabled = true;
            btnUp.Enabled = true;


        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            actionButtonEditStage();
            clearAll();
            unlockAll(true);
            getMaxID(0);
            status = "ADD NEW";
        }

        private void txtremark_KeyDown(object sender, KeyEventArgs e)
        {
            SelectRemark select = new SelectRemark();
            select.ShowDialog(this);
            txtremark.Text = select.getRemark();
        }

        private void gridPatientsList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            gridPatientsList.ClearSelection();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                adhar = "Not Available";
                txtadhar.Text = "";
                txtadhar.ReadOnly = true;
            }
            else
            {
                adhar = txtadhar.Text;
                txtadhar.ReadOnly = false;
            }
        }

        private void txtadd_KeyDown(object sender, KeyEventArgs e)
        {
            Address ob = new Address(AddPatients.created_by);
            ob.ShowDialog();
            string address = ob.getAddress();
            txtadd.Text = address;
        }

        private void txtadd_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcontact_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtcontact_Validating(object sender, CancelEventArgs e)
        {
            if (getCount() > 0 && status.Equals("ADD NEW"))
            {
                MessageBox.Show("Patients Already Exists !!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtname.Clear();
                txtcontact.Clear();
                txtname.Focus();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (gridPatientsList.Rows.Count > 0)
                gridPatientsList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT ID,Date,Name,ContactNo,Gender,Address,Age,AdharCard,TestName,OtherDiseases,Remark FROM Patients WHERE (CenterType='" + AddPatients.centerType + "' and CenterName='" + AddPatients.centerName + "') and (Name LIKE '%" + textBox1.Text + "%')";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date, fn, cn, gender, add, age, adhar, testn, other;
                            string id = reader["ID"].ToString();
                            date = reader["Date"].ToString();
                            fn = reader["Name"].ToString();
                            cn = reader["ContactNo"].ToString();
                            gender = reader["Gender"].ToString();
                            add = reader["Address"].ToString();
                            age = reader["Age"].ToString();
                            adhar = reader["AdharCard"].ToString();
                            testn = reader["TestName"].ToString();
                            other = reader["OtherDiseases"].ToString();
                            string remark = reader["Remark"].ToString();
                            gridPatientsList.Rows.Add(id, DateTime.Parse(date), fn, cn, gender, add, age, adhar, testn, other, remark);
                        }
                    }
                }
            }
            gridPatientsList.ClearSelection();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (gridPatientsList.Rows.Count > 0)
                gridPatientsList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT ID,Date,Name,ContactNo,Gender,Address,Age,AdharCard,TestName,OtherDiseases,Remark FROM Patients WHERE (CenterType='" + AddPatients.centerType + "' and CenterName='" + AddPatients.centerName + "') and (Name LIKE '%" + textBox1.Text + "%')";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date, fn, cn, gender, add, age, adhar, testn, other;
                            string id = reader["ID"].ToString();
                            date = reader["Date"].ToString();
                            fn = reader["Name"].ToString();
                            cn = reader["ContactNo"].ToString();
                            gender = reader["Gender"].ToString();
                            add = reader["Address"].ToString();
                            age = reader["Age"].ToString();
                            adhar = reader["AdharCard"].ToString();
                            testn = reader["TestName"].ToString();
                            other = reader["OtherDiseases"].ToString();
                            string remark = reader["Remark"].ToString();
                            gridPatientsList.Rows.Add(id, DateTime.Parse(date), fn, cn, gender, add, age, adhar, testn, other, remark);
                        }
                    }
                }
            }
            gridPatientsList.ClearSelection();
        }
        private void fillBedNames()
        {
            if (cmbbedname.Items.Count > 0)
                cmbbedname.Items.Clear();
            cmbbedname.Items.Add("--BED NAME--");
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT BedName FROM Beds WHERE (CenterType='" + AddPatients.centerType + "' and CenterName='" + AddPatients.centerName + "') and (WardCode='" + cmbcode.Text + "' and Flag=0)";
                    
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("BedName");
                                if (!cmbbedname.Items.Contains(name))
                                    cmbbedname.Items.Add(name);
                            }
                        }
                    }
                }
                cmbbedname.DropDownHeight = 150;
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                cmbbedname.SelectedIndex = 0;
            };
            bwConn.RunWorkerAsync();
        }
        private void fillBedNames1()
        {
            if (cmbbedname.Items.Count > 0)
                cmbbedname.Items.Clear();

            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT BedName FROM Beds WHERE (CenterType='" + AddPatients.centerType + "' and CenterName='" + AddPatients.centerName + "') and (WardCode='" + cmbcode.Text + "' and Flag=1)";
                cmbbedname.Items.Add("--BED NAME--");
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        //Iterate through the rows and add it to the combobox's items
                        while (reader.Read())
                        {
                            string name = reader.GetString("BedName");
                            if (!cmbbedname.Items.Contains(name))
                                cmbbedname.Items.Add(name);
                        }
                    }
                }
            }
        }
        private void cmbcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (status.Equals("ADD NEW") || status.Equals("UPDATE"))
                fillBedNames();
            else if(status.Equals("VIEW"))
                fillBedNames1();
        }

        private void txtcontact_Validating_1(object sender, CancelEventArgs e)
        {
            txtcontact_Validating(sender, e);
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
            radioButton3_CheckedChanged(sender, e);
        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            radioButton4_CheckedChanged(sender, e);
        }

        private void txtadd_KeyDown_1(object sender, KeyEventArgs e)
        {
            panelAddress.Size = panelAddress.MaximumSize;
            addresses1.fillCenterType();
            addresses1.Visible = true;
            addresses1.Dock = DockStyle.Fill;
            addresses1.Visible = true;
            addresses1.clearAll();
            panelAContain.Controls.Add(addresses1);            
            panelAContain.Size = panelAContain.MaximumSize;
            panelInput.ScrollControlIntoView(panelAContain);
        }

        private void addresses1_VisibleChanged(object sender, EventArgs e)
        {
            if (addresses1.Visible == false)
            {
                txtadd.Text = addresses1.getAddress();
                panelAddress.Size = panelAddress.MinimumSize;
                panelAContain.Size = panelAContain.MinimumSize;
            }
            else
            {
                txtadd.Text = "";
                panelAddress.Size = panelAddress.MaximumSize;
                panelAContain.Size = panelAContain.MaximumSize;
                panelInput.ScrollControlIntoView(panelAContain);
            }
        }

        private void other1_VisibleChanged(object sender, EventArgs e)
        {
            if (other1.Visible == false)
            {
                txtother.Text = other1.getDiseases();
                panelDiseases.Size = panelDiseases.MinimumSize;
            }
            else
            {
                panelDiseases.Size = panelDiseases.MaximumSize;
                txtother.Text = "";
            }
        }

        private void txtother_KeyDown(object sender, KeyEventArgs e)
        {
            panelDiseases.Size = panelDiseases.MaximumSize;
            other1.Visible = true;
            other1.clearAll();
            panelInput.ScrollControlIntoView(panelDcontains);
        }

        private void txtremark_KeyDown_1(object sender, KeyEventArgs e)
        {
            panelRemark.Size = panelRemark.MaximumSize;
            panelRContains.Size = panelRContains.MaximumSize;
            selectR1.Visible = true;
            selectR1.clearAll();
            selectR1.fillCenterType();           
            panelInput.ScrollControlIntoView(panelRContains);
        }

        private void selectR1_VisibleChanged(object sender, EventArgs e)
        {
            if (selectR1.Visible == false)
            {
                txtremark.Text = selectR1.getRemark();
                panelRContains.Size = panelRContains.MinimumSize;
                panelRemark.Size = panelRemark.MinimumSize;                
            }
            else
            {
                panelRemark.Size = panelRemark.MaximumSize;
                panelRContains.Size = panelRContains.MaximumSize;
                txtremark.Text = "";
            }
        }

        private void selectR1_VisibleChanged_1(object sender, EventArgs e)
        {
            selectR1_VisibleChanged(sender, e);
        }

        private void radioButton6_CheckedChanged_1(object sender, EventArgs e)
        {
            radioButton6_CheckedChanged(sender, e);
        }

        private void radioButton5_CheckedChanged_1(object sender, EventArgs e)
        {
            radioButton5_CheckedChanged(sender, e);
        }

        private void panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbid_Click(object sender, EventArgs e)
        {

        }

        private void cmbcode_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbcode.SelectedIndex == 0)
            {
                cmbcode.BackColor = SystemColors.HotTrack;
                cmbcode.ForeColor = Color.White;
            }
            else
            {
                cmbcode.BackColor = Color.White;
                cmbcode.ForeColor = Color.Black;
            }
            fillBedNames();
        }

        private void cmbbedname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbbedname.SelectedIndex == 0)
            {
                cmbbedname.BackColor = SystemColors.HotTrack;
                cmbbedname.ForeColor = Color.White;
            }
            else
            {
                cmbbedname.BackColor = Color.White;
                cmbbedname.ForeColor = Color.Black;
            }
        }
    }
}

