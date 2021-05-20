using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace OCC
{
    public partial class ReportPreview : Form
    {
        String query = "";
        String centerType = "";
        String centerName = "";
        string filterBy = "";
        public ReportPreview(string query,string type,string name,string filter)
        {
            InitializeComponent();
            this.query = query;
            centerType = type;
            centerName = name;
            filterBy = filter;
            ReportViewer.CheckForIllegalCrossThreadCalls = false;
        }

        private void loadData()
        {
            string wanted_path = "";
            ReportParameter[] rp = new ReportParameter[4];
            ReportDataSource rds = new ReportDataSource();
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender, e) =>
            {
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = DbConnect.conString;
                cn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.Connection = cn;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable("Patients");
                da.Fill(table1);
                DataSet ds = new DataSet();
                ds.Tables.Add(table1);
                rds.Name = "DataSet1";
                rds.Value = table1;
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
                rp[1] = new ReportParameter("centerType", centerType);
                rp[2] = new ReportParameter("centerName", centerName);
                rp[3] = new ReportParameter("FilteredBy", filterBy);
                ReportViewer1.LocalReport.DataSources.Clear();
                
            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                ReportViewer1.LocalReport.ReportPath = path + "//Report2.rdlc";
                ReportViewer1.LocalReport.SetParameters(rp);
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                ReportViewer1.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] bytes = ReportViewer1.LocalReport.Render(
                    "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);
                string filename = @wanted_path + "//"+DateTime.Now.ToString("hh-mm-ss") + ".pdf";
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                System.Diagnostics.Process.Start(filename);
                pb.Close();
                
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();
            
        }
        private void ReportPreview_Load(object sender, EventArgs e)
        {
            loadData();
            this.ReportViewer1.RefreshReport();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Black;
        }
    }
}
