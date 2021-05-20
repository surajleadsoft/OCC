using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace OCC
{
    public partial class AllEmails : UserControl
    {
        string main_query = "";
        List<string> emails = new List<string>();
        string filename = "",status="";
        public AllEmails()
        {
            InitializeComponent();
        }
        public void setQuery(string query,string st)
        {
            main_query = query;
            status = st;
        }
        private void loadData()
        {
            string wanted_path = "";
            ReportParameter rp = new ReportParameter();
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
                cmd.CommandText = main_query;
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
                rp = new ReportParameter("Date", DateTime.Now.ToShortDateString());
                reportViewer1.LocalReport.DataSources.Clear();

            };
            bwConn.RunWorkerCompleted += (sender, e) =>
            {
                if (e.Error != null)
                {
                    MessageBox.Show(e.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                reportViewer1.LocalReport.ReportPath = path + "//Report1.rdlc";
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.LocalReport.Refresh();
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] bytes = reportViewer1.LocalReport.Render(
                    "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);
                filename = @wanted_path  +"//"+DateTime.Now.ToString("hh-mm-ss") + ".pdf";
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                
                pb.Close();
                pb.Dispose();
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        public void fillCombo()
        {
            if (panel6.Controls.Count > 0)
                panel6.Controls.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From Emails Order By ID";
                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Label label = new Label();
                            label.AutoSize = true;
                            Font f = new System.Drawing.Font("Calibri", 12.0f, FontStyle.Bold);
                            label.Font = f;
                            label.Text = reader["NameOfPerson"].ToString();
                            label.Dock = DockStyle.Top;
                            panel6.Controls.Add(label);
                            if (!emails.Contains(reader["Email"].ToString()))
                                emails.Add(reader["Email"].ToString());
                        }
                    }
                }
            }
        }
        void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {

        }
        private void sendEmail(string emailto, string name)
        {
            string emailFromAddress = "suraj.leadsoft@gmail.com"; //Sender Email Address  
            string password = "Ledso@143"; //Sender Password  
            string emailToAddress = emailto; //Receiver Email Address  
            string subject = "Osmanabad Covid Care Patient Reports";
            string body = "Dear " + name + ",\n\nKindly check the attachment of patients Reports here !!!\n\n\nThanks & Regards,\nApp Development Team,\nLeadSoft IT Solutions,\n7028816463.";
            MailMessage message = new MailMessage();
            SmtpClient smtp;
            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(filename);
            message.Attachments.Add(attachment);
            message.Subject = subject;
            message.Body = body;
            message.To.Add(new MailAddress(emailto));
            message.From = new MailAddress(emailFromAddress, "LeadSoft IT Solutions");
            smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 25;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(emailFromAddress, password);
            smtp.SendAsync(message, message.Subject);
            smtp.SendCompleted += new SendCompletedEventHandler(smtp_SendCompleted);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            loadData();
            List<string> names = new List<string>();
            foreach (Control cntrl in panel6.Controls)
            {
                Label label = (Label)cntrl;
                names.Add(label.Text);
            }
            for (int i = 0; i < emails.Count; i++)
            {
                sendEmail(emails[i], names[i]);
            }
            MessageBox.Show("Email Sent Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult drs = MessageBox.Show("Do you wan to open the report file ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            System.Diagnostics.Process.Start(filename);
        }
    }
}
