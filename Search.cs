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
    public partial class Search : UserControl
    {
        public static string created_by = "";
        public static string centerType = "";
        public static string centerName = "";
        public Search()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        public void setDetails(string created,string type,string name)
        {
            Search.created_by = created;
            Search.centerType = type;
            Search.centerName = name;
        }
        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void fillgrid()
        {
            MySqlConnection cn = new MySqlConnection();
            cn.ConnectionString = DbConnect.conString;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cn;
            cmd.CommandText = "Select Name,ContactNo,AdharCard,Date,Remark,CenterType,CenterName From Patients WHERE CenterType='"+Search.centerType+"' and CenterName='"+Search.centerName+"'";
           // MessageBox.Show(cmd.CommandText);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable table1 = new DataTable("Patients");
            da.Fill(table1);
            cmd.CommandText = "Select Name,ContactNo,Address,Date,Remark,CenterType,CenterName From PatientsRemark WHERE CenterType='" + Search.centerType + "' and CenterName='" + Search.centerName + "'";
           // MessageBox.Show(cmd.CommandText);
            DataTable table2 = new DataTable("Remarks");
            da.Fill(table2);
            DataSet ds = new DataSet();
            ds.Tables.Add(table1);
            ds.Tables.Add(table2);
            
            //DataRelation relation = new DataRelation ("Remark Relation",
          //  DataRelation Datatablerelation = new DataRelation("Remark Details", ds.Tables[0].Columns[2], ds.Tables[1].Columns[0], false);
            DataRelation Datatablerelation = new DataRelation("Remark Details", new DataColumn[] { ds.Tables[0].Columns[0], ds.Tables[0].Columns[1] }, new DataColumn[] { ds.Tables[1].Columns[0], ds.Tables[1].Columns[1] }, false);
            ds.Relations.Add(Datatablerelation);
            dataGrid1.DataSource = ds.Tables[0];
            cn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex==1)
            {
                ByCenter ob = new ByCenter();
                ob.ShowDialog();
                string query = ob.getQuery();
                string[] centers = query.Split(':');                
                if (centers[0] == "--CENTER TYPE--" || centers[1] == "--CENTER NAME--")
                {
                    MessageBox.Show("Wrong parameter to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select Name,ContactNo,AdharCard,Date,Remark,CenterType,CenterName From Patients WHERE (CenterType='"+Search.centerType+"' and CenterName='"+Search.centerName+"') and (CenterType='"+centers[0]+"' and CenterName='"+centers[1]+"')";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable("Patients");
                da.Fill(table1);
                cmd.CommandText = "Select AdharCard,Address,Date,Remark,CenterType,CenterName From PatientsRemark WHERE (CenterType='" + Search.centerType + "' and CenterName='" + Search.centerName + "')";
                DataTable table2 = new DataTable("Remarks");
                da.Fill(table2);
                DataSet ds = new DataSet();
                ds.Tables.Add(table1);
                ds.Tables.Add(table2);
                DataRelation Datatablerelation = new DataRelation("Remark Details", ds.Tables[0].Columns[2], ds.Tables[1].Columns[0], false);
                ds.Relations.Add(Datatablerelation);
                dataGrid1.DataSource = ds.Tables[0];
                cn.Close();
            }
            if (comboBox1.SelectedIndex == 2)
            {
                ByName ob = new ByName();
                ob.ShowDialog();
                string name = ob.getQuery();
                if (name == "")
                {
                    MessageBox.Show("Wrong parameter to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select Name,ContactNo,AdharCard,Date,Remark,CenterType,CenterName From Patients WHERE (CenterType='" + Search.centerType + "' and CenterName='" + Search.centerName + "') and (Name LIKE '%" + name + "%')";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable("Patients");
                da.Fill(table1);
                cmd.CommandText = "Select AdharCard,Address,Date,Remark,CenterType,CenterName From PatientsRemark WHERE CreatedBy='" + created_by + "'";
                DataTable table2 = new DataTable("Remarks");
                da.Fill(table2);
                DataSet ds = new DataSet();
                ds.Tables.Add(table1);
                ds.Tables.Add(table2);
                DataRelation Datatablerelation = new DataRelation("Remark Details", ds.Tables[0].Columns[2], ds.Tables[1].Columns[0], false);
                ds.Relations.Add(Datatablerelation);
                dataGrid1.DataSource = ds.Tables[0];
                cn.Close();
            }
            if (comboBox1.SelectedIndex == 3)
            {
                ByGender ob = new ByGender();
                ob.ShowDialog();
                string gender = ob.getQuery();
                if (gender == "")
                {
                    MessageBox.Show("Please select gender to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select Name,ContactNo,AdharCard,Date,Remark,CenterType,CenterName From Patients WHERE (CenterType='" + Search.centerType + "' and CenterName='" + Search.centerName + "') and (Gender='" + gender + "')";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable("Patients");
                da.Fill(table1);
                cmd.CommandText = "Select AdharCard,Address,Date,Remark,CenterType,CenterName From PatientsRemark WHERE CreatedBy='" + created_by + "'";
                DataTable table2 = new DataTable("Remarks");
                da.Fill(table2);
                DataSet ds = new DataSet();
                ds.Tables.Add(table1);
                ds.Tables.Add(table2);
                DataRelation Datatablerelation = new DataRelation("Remark Details", ds.Tables[0].Columns[2], ds.Tables[1].Columns[0], false);
                ds.Relations.Add(Datatablerelation);
                dataGrid1.DataSource = ds.Tables[0];
                cn.Close();
            }
            if (comboBox1.SelectedIndex == 4)
            {
                ByDate ob = new ByDate();
                ob.ShowDialog();
                string query = ob.getQuery();
                string[] dates = query.Split(':');
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select Name,ContactNo,AdharCard,Date,Remark,CenterType,CenterName From Patients WHERE (CenterType='" + Search.centerType + "' and CenterName='" + Search.centerName + "') and (Date BETWEEN @date1 and @date2)";
                cmd.Parameters.AddWithValue("@date1", DateTime.Parse(dates[0]).ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@date2", DateTime.Parse(dates[1]).ToString("yyyy-MM-dd"));
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable("Patients");
                da.Fill(table1);
                cmd.CommandText = "Select AdharCard,Address,Date,Remark,CenterType,CenterName From PatientsRemark WHERE CreatedBy='" + created_by + "'";
              
                DataTable table2 = new DataTable("Remarks");
                da.Fill(table2);
                DataSet ds = new DataSet();
                ds.Tables.Add(table1);
                ds.Tables.Add(table2);
                DataRelation Datatablerelation = new DataRelation("Remark Details", ds.Tables[0].Columns[2], ds.Tables[1].Columns[0], false);
                ds.Relations.Add(Datatablerelation);
                dataGrid1.DataSource = ds.Tables[0];
                cn.Close();
            }
            if (comboBox1.SelectedIndex == 5)
            {
                //age
                ByAge ob = new ByAge();
                ob.ShowDialog();
                string query = ob.getQuery();
                if (query == "" || query==":")
                {
                    MessageBox.Show("Wrong parameters to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string[] ages = query.Split(':');
                if (!ages[0].All(char.IsDigit) || !ages[1].All(char.IsDigit))
                {
                    MessageBox.Show("Wrong parameters to search !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cn;
                cmd.CommandText = "Select Name,ContactNo,AdharCard,Age,Date,Remark,CenterType,CenterName From Patients WHERE (CenterType='" + Search.centerType + "' and CenterName='" + Search.centerName + "') and (Age >=" + ages[0] + " and Age <=" + ages[1] + ")";
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable("Patients");
                da.Fill(table1);
                cmd.CommandText = "Select AdharCard,Address,Age,Date,Remark,CenterType,CenterName From PatientsRemark WHERE CreatedBy='" + created_by + "'";
                DataTable table2 = new DataTable("Remarks");
                da.Fill(table2);
                DataSet ds = new DataSet();
                ds.Tables.Add(table1);
                ds.Tables.Add(table2);
                DataRelation Datatablerelation = new DataRelation("Remark Details", ds.Tables[0].Columns[2], ds.Tables[1].Columns[0], false);
                ds.Relations.Add(Datatablerelation);
                dataGrid1.DataSource = ds.Tables[0];
                cn.Close();
            }
        }
    }
}
