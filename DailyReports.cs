using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace OCC
{
    public partial class DailyReports : UserControl
    {
        string query = "";
        public static string centerName="";
        public static string centerType = "";
        public DailyReports(string q)
        {
            InitializeComponent();
            query = q;
        }
        public void setDetails(string name, string type)
        {
            DailyReports.centerName = name;
            DailyReports.centerType = type;
        }
        public void loadData()
        {
            string wanted_path = "";
            ReportParameter[] rp = new ReportParameter[4];
            ReportDataSource rds = new ReportDataSource();
            ReportDataSource rds1 = new ReportDataSource();
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,CreatedAt,Flag FROM PatientsRemark WHERE (CenterType='" + DailyReports.centerType + "' and CenterName='" + DailyReports.centerName + "') and (Flag=1 and Date=@date)";
                cmd.Parameters.AddWithValue("@date", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd"));
                cmd.Connection = cn;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable("Patients");
                da.Fill(table1);
                DataSet ds = new DataSet();
                ds.Tables.Add(table1);
                rds.Name = "DataSet1";
                rds.Value = table1;

                
                MySqlCommand cmd1 = new MySqlCommand();
                cmd1.Connection = cn;
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "SELECT Name,ContactNo,Date,Address,Age,AdharCard,Remark,CenterType,CenterName,CreatedBy,CreatedAt,Flag FROM PatientsRemark WHERE (CenterType='" + DailyReports.centerType + "' and CenterName='" + DailyReports.centerName + "') and (Flag=0 and Date=@date)";
                cmd1.Parameters.AddWithValue("@date", DateTime.Parse(DateTime.Now.ToShortDateString()).ToString("yyyy-MM-dd"));
                MySqlDataAdapter da1 = new MySqlDataAdapter(cmd1);
                DataTable table2 = new DataTable("Discharged");
                da1.Fill(table2);
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(table2);
                rds1.Name = "DataSet2";
                rds1.Value = table2;             
               
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var subFolderPath = Path.Combine(path, "OCC Reports");
                if (!System.IO.Directory.Exists(subFolderPath))
                    System.IO.Directory.CreateDirectory(subFolderPath);
                var cur_folder = DateTime.Now.ToString("dd-MM-yyyy");
                subFolderPath = subFolderPath + "//" + cur_folder;
                if (!System.IO.Directory.Exists(subFolderPath))
                    System.IO.Directory.CreateDirectory(subFolderPath);
                wanted_path = subFolderPath;
                rp[0] = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                rp[1] = new ReportParameter("CenterType", DailyReports.centerType);
                rp[2] = new ReportParameter("CenterName", DailyReports.centerName);
                rp[3] = new ReportParameter("FilteredBy", "Daily");
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                ReportViewer1.LocalReport.ReportPath = path + "\\Report1.rdlc";
                ReportViewer1.LocalReport.SetParameters(rp);
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.LocalReport.DataSources.Add(rds1);
                ReportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                ReportViewer1.LocalReport.Refresh();                
                pb.Close();
                
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

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
            string wanted_path = "";
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var subFolderPath = Path.Combine(path, "OCC Reports");
            if (!System.IO.Directory.Exists(subFolderPath))
                System.IO.Directory.CreateDirectory(subFolderPath);
            var cur_folder = DateTime.Now.ToString("dd-MM-yyyy");
            subFolderPath = subFolderPath + "//" + cur_folder;
            if (!System.IO.Directory.Exists(subFolderPath))
                System.IO.Directory.CreateDirectory(subFolderPath);
            wanted_path = subFolderPath;
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = ReportViewer1.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);
            string filename = @wanted_path + "//" + DateTime.Now.ToString("hh-mm-ss") + ".pdf";
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            MessageBox.Show("File Created Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(filename);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
