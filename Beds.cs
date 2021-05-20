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
    public partial class Beds : UserControl
    {
        string status = "";
        public static string centerType = "";
        public static string centerName = "";
        public Beds()
        {
            InitializeComponent();
        }
        public void setDetails(string type, string name)
        {
            Beds.centerName = name;
            Beds.centerType = type;
            fillCenterType();
            cmbtype.Text = type;
            fillCenterName(cmbtype.Text);
            cmbname.Text = name;
            cmbname.Enabled = false;
            cmbtype.Enabled = false;
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
        private void fillWardCode()
        {
            List<string> centerTypes = new List<string>();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                if (cmbcode.Items.Count > 0)
                    cmbcode.Items.Clear();
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT WardCode FROM Wards WHERE CenterType='"+cmbtype.Text+"' and CenterName='"+cmbname.Text+"'";
                    centerTypes.Add("--WARD CODE--");
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            //Iterate through the rows and add it to the combobox's items
                            while (reader.Read())
                            {
                                string name = reader.GetString("WardCode");
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
                cmbcode.Items.AddRange(centerTypes.ToArray());
                cmbcode.SelectedIndex = 0;
            };
            bwConn.RunWorkerAsync();
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
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void clearAll()
        {
            lbid.Text = "";
            txtname.Text = "";
            txtfrom.Text = "0";
            txtto.Text = "0";
            lbinstruction.Text = "";
            txtname.Text = "";
            gridEmailList.Update();
            gridEmailList.ClearSelection();
            fillCenterType();
            cmbtype.Text = Beds.centerType;
            fillCenterName(cmbtype.Text);
            cmbname.Text = Beds.centerName; ;
            cmbname.Enabled = false;
            cmbtype.Enabled = false;
        }
        public void unlockAll(bool b)
        {
            panelInput.Enabled = b;
            cmbname.Enabled = false;
            cmbtype.Enabled = false;
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
                var query = "SELECT * From Beds Order By ID";

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
                            string code = reader["WardCode"].ToString();
                            string name = reader["BedName"].ToString();
                            gridEmailList.Rows.Add(id, type, cname, code, name);
                        }
                    }
                }
                lbinstruction.Text = "Total Beds :" + gridEmailList.Rows.Count;
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
                var query = "SELECT MAX(ID) From Beds";

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
        private int getCount(string bedname)
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(ID) From Beds WHERE BedName='"+bedname+"'";

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
            return srno;
        }
        private bool validation()
        {
            if (lbid.Text == "" || txtfrom.Text == "0" || txtto.Text == "0" || cmbtype.SelectedIndex == 0 || cmbname.SelectedIndex == 0 || cmbcode.SelectedIndex==0)
                return false;
            else
                return true;
        }
        private void panelInput_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            status = "ADD NEW";
            clearAll();
            getID();
            unlockAll(true);
            actionButtonEditStage();
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
                drs = MessageBox.Show("Are You Sure Do You Want To Add This Beds ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                PleaseWait pb = new PleaseWait();
                BackgroundWorker bwConn = new BackgroundWorker();
                int from = Int32.Parse(txtfrom.Text);
                int to = Int32.Parse(txtto.Text);
                string result = "";
                for (int i = from; i <= to; i++)
                {
                    string bedname1 = cmbcode.Text + "/" + i.ToString();
                    int count = getCount(bedname1);
                    if (count > 0)
                    {
                        result = "exists";
                        break;
                    }
                }
                bwConn.DoWork += (sender1, e1) =>
                {
                    if (result == "")
                    {
                        for (int i = from; i <= to; i++)
                        {
                            string bedname = cmbcode.Text + "/" + i.ToString();
                            txtname.Text = bedname;
                            lbinstruction.Text = "Please wait !! We are adding beds for you.";
                            cmd.CommandText = "INSERT INTO Beds(CenterType,CenterName,WardCode,BedName,Flag) VALUES ('" + cmbtype.Text + "','" + cmbname.Text + "','" + cmbcode.Text + "','" + bedname + "',0)";
                            cmd.ExecuteNonQuery();
                            fillgrid();    
                        }
                        result = "success";
                    }
                };
                bwConn.RunWorkerCompleted += (sender1, e1) =>
                {
                    if (e1.Error != null)
                    {
                        MessageBox.Show(e1.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    pb.Close();
                    if(result=="success")
                        MessageBox.Show("Beds Added Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if(result=="exists")
                        MessageBox.Show("Beds Already Exists !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cn.Close();
                };
                bwConn.RunWorkerAsync();
                pb.ShowDialog();
            }
            else if (status.Equals("UPDATE"))
            {
                drs = MessageBox.Show("Are You Sure Do You Want To Update This Ward ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (drs == DialogResult.No)
                    return;
                cmd.CommandText = "";
                cmd.ExecuteNonQuery();
                MessageBox.Show("Beds Updated Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            drs = MessageBox.Show("Are You Sure Do You Want To Delete This Bed ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            cmd.CommandText = "DELETE FROM Beds WHERE BedName='" + txtname.Text+"'";
            cmd.ExecuteNonQuery();
            MessageBox.Show("Bed Deleted Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void cmbname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbname.SelectedIndex != 0)
                fillWardCode();
        }
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        private void txtfrom_Validating(object sender, CancelEventArgs e)
        {
            if (!IsDigitsOnly(txtfrom.Text))
            {
                MessageBox.Show("Please enter starting bed no in NUMBERS only !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtfrom.Clear();
                txtfrom.Focus();
            }
        }

        private void txtto_Validating(object sender, CancelEventArgs e)
        {
            if (!IsDigitsOnly(txtto.Text))
            {
                MessageBox.Show("Please enter ending bed no in NUMBERS only !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtto.Clear();
                txtto.Focus();
            }
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gridEmailList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lbid.Text = gridEmailList.CurrentRow.Cells["ID"].Value.ToString();
            txtname.Text = gridEmailList.CurrentRow.Cells["BedName"].Value.ToString();
            cmbcode.Text = gridEmailList.CurrentRow.Cells["WardCode"].Value.ToString();
            actionButtonEditStage();
            btnSave.Enabled = false;
            btnUp.Enabled = true;
            btnDel.Enabled = true;
            unlockAll(false);
        }

        private void label2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
