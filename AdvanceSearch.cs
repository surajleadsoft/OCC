using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DGVPrinterHelper;

namespace OCC
{
    public partial class AdvanceSearch : UserControl
    {
        public static string created_by = "";
        public static string centerType = "";
        public static string centerName = "";
        string main_query = "";
        string filterBy = "Overall";
        public AdvanceSearch()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        public void setDetails(string user,string type,string name)
        {
            AdvanceSearch.created_by = user;
            AdvanceSearch.centerType = type;
            AdvanceSearch.centerName = name;
        }
        public void fillgrid()
        {
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                var connectionString = DbConnect.conString;
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,CreatedAt FROM PatientsRemark WHERE CenterType='"+AdvanceSearch.centerType+"' and CenterName='"+AdvanceSearch.centerName+"'";                    
                    main_query = query;
                    using (var command = new MySqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            int srno = 1;
                            while (reader.Read())
                            {
                                string date, fn, cn, add, age, adhar;
                                date = reader["Date"].ToString();
                                fn = reader["Name"].ToString();
                                cn = reader["ContactNo"].ToString();
                                add = reader["Address"].ToString();
                                age = reader["Age"].ToString();
                                adhar = reader["AdharCard"].ToString();
                                string remark = reader["Remark"].ToString();
                                string type = reader["CenterType"].ToString();
                                string cname = reader["CenterName"].ToString();
                                string cat = reader["CreatedBy"].ToString();
                                gridPatientsList.Rows.Add(srno,fn, cn, DateTime.Parse(date), add, age, adhar, remark, type, cname, cat);
                                srno++;
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
                gridPatientsList.ClearSelection();
                pb.Close();
                
            };
            if (gridPatientsList.Rows.Count > 0)
                gridPatientsList.Rows.Clear();
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
        }
        public void fillgrid(string q)
        {
            if (gridPatientsList.Rows.Count > 0)
                gridPatientsList.Rows.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = q;
                main_query = query;
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        int srno = 1;
                        while (reader.Read())
                        {
                            string date, fn, cn, add, age, adhar;
                            date = reader["Date"].ToString();
                            fn = reader["Name"].ToString();
                            cn = reader["ContactNo"].ToString();
                            add = reader["Address"].ToString();
                            age = reader["Age"].ToString();
                            adhar = reader["AdharCard"].ToString();
                            string remark = reader["Remark"].ToString();
                            string type = reader["CenterType"].ToString();
                            string cname = reader["CenterName"].ToString();
                            string cat = reader["CreatedBy"].ToString();
                            gridPatientsList.Rows.Add(srno,fn, cn, DateTime.Parse(date), add, age, adhar, remark, type, cname, cat);
                            srno++;
                        }
                    }
                }
            }
            gridPatientsList.ClearSelection();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (panelBg.Size == panelBg.MaximumSize)
                panelBg.Size = panelBg.MinimumSize;
            else
                panelBg.Size = panelBg.MaximumSize;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReportPreview view = new ReportPreview(main_query,AdvanceSearch.centerType,AdvanceSearch.centerName,filterBy);
            view.ShowDialog();
        }
        private void byDate()
        {
            ByDate ob = new ByDate();
            ob.ShowDialog();
            if (gridPatientsList.Rows.Count > 0)
                gridPatientsList.Rows.Clear();
            string query = ob.getQuery();
            string[] dates = query.Split(':');
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                command.CommandText = "SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy FROM PatientsRemark WHERE (CenterType='" + AdvanceSearch.centerType + "' and CenterName='" + AdvanceSearch.centerName + "') and (Date BETWEEN @date1 and @date2)";
                command.Parameters.AddWithValue("@date1", DateTime.Parse(dates[0]).ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@date2", DateTime.Parse(dates[1]).ToString("yyyy-MM-dd"));

                using (var reader = command.ExecuteReader())
                {
                    int srno = 1;
                    while (reader.Read())
                    {
                        string date, fn, cn, add, age, adhar;
                        date = reader["Date"].ToString();
                        fn = reader["Name"].ToString();
                        cn = reader["ContactNo"].ToString();
                        add = reader["Address"].ToString();
                        age = reader["Age"].ToString();
                        adhar = reader["AdharCard"].ToString();
                        string remark = reader["Remark"].ToString();
                        string type = reader["CenterType"].ToString();
                        string cname = reader["CenterName"].ToString();
                        string cat = reader["CreatedBy"].ToString();
                        gridPatientsList.Rows.Add(srno,fn, cn, DateTime.Parse(date), add, age, adhar, remark, type, cname, cat);
                        srno++;
                    }
                }

            }
            gridPatientsList.ClearSelection();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                //By Center
                ByCenter ob = new ByCenter();
                ob.ShowDialog();
                string query = ob.getQuery();
                string[] centers = query.Split(':');
                if (centers[0] == "--CENTER TYPE--" || centers[1] == "--CENTER NAME--")
                {
                    MessageBox.Show("Wrong parameter to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                filterBy = "By Center";
                fillgrid("SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,CreatedAt FROM PatientsRemark WHERE (CenterType='" + centers[0] + "' and CenterName='" + centers[1] + "')");
            }
            if (comboBox1.SelectedIndex == 2)
            {
                //By Name
                ByName ob = new ByName();
                ob.ShowDialog();
                string query = ob.getQuery();
                if (query=="")
                {
                    MessageBox.Show("Wrong parameter to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                filterBy = "By Name";
                fillgrid("SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,CreatedAt FROM PatientsRemark WHERE (CenterType='" + AdvanceSearch.centerType + "' and CenterName='" + AdvanceSearch.centerName + "') and (Name LIKE '%" + query + "%')");
            }
            if (comboBox1.SelectedIndex == 3)
            {
                //By Gender
                ByGender ob = new ByGender();
                ob.ShowDialog();
                string gender = ob.getQuery();
                if (gender == "")
                {
                    MessageBox.Show("Please select gender to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                filterBy = "By Gender";
                fillgrid("SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,CreatedAt FROM PatientsRemark WHERE (CenterType='" + AdvanceSearch.centerType + "' and CenterName='" + AdvanceSearch.centerName + "') and (Gender='" + gender + "')");
            }
            if (comboBox1.SelectedIndex == 4)
            {
                //By Date
                filterBy = "By Date";
                byDate();   
            }
            if (comboBox1.SelectedIndex == 5)
            {
                ByAge ob = new ByAge();
                ob.ShowDialog();
                string query = ob.getQuery();
                if (query == "" || query == ":")
                {
                    MessageBox.Show("Wrong parameters to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                filterBy = "By Age";
                string[] ages = query.Split(':');
                if (!ages[0].All(char.IsDigit) || !ages[1].All(char.IsDigit))
                {
                    MessageBox.Show("Wrong parameters to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                fillgrid("SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,CreatedAt FROM PatientsRemark WHERE (CenterType='" + AdvanceSearch.centerType + "' and CenterName='" + AdvanceSearch.centerName + "') and (Age >=" + ages[0] + " and Age <=" + ages[1] + ")");
            }
            if (comboBox1.SelectedIndex == 6)
            {
                ByAdd ob = new ByAdd();
                ob.ShowDialog();
                string query = ob.getAddress();
                if (query == "" || query == ":")
                {
                    MessageBox.Show("Wrong parameters to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                filterBy = "By Location";
                string[] address = query.Split(',');
                fillgrid("SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,Area,District,CreatedAt FROM PatientsRemark WHERE (CenterType='" + AdvanceSearch.centerType + "' and CenterName='" + AdvanceSearch.centerName + "') and (Area ='" + address[0] + "' and District ='" + address[1] + "')");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EmailFiles ob = new EmailFiles(main_query);
            ob.ShowDialog();
        }

        private void gridPatientsList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
