using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace OCC
{
    public partial class SpecificEmail : UserControl
    {
        string main_query = "";
        List<string> emails = new List<string>();
        List<string> emailsTo = new List<string>();
        string filename = "", status = "";
        List<CheckBox> listControl = new List<CheckBox>();
        public SpecificEmail()
        {
            InitializeComponent();
        }
        public void setQuery(string query, string st)
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
                filename = @wanted_path +"//"+ DateTime.Now.ToString("hh-mm-ss") + ".pdf";                
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                pb.Close();
                
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();

        }
        public void fillCombo()
        {
            if (item_panel.Controls.Count > 0)
                item_panel.Controls.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From Emails Order By ID";
                using (var command = new MySqlCommand(query, connection))
                {

                    using (var reader = command.ExecuteReader())
                    {
                        int rowIndex = this.item_panel.RowCount++;
                        int colIndecx = 0;
                        while (reader.Read())
                        {
                            CheckBox chk = new CheckBox();
                            chk.Width = 150;
                            chk.Text = reader["NameOfPerson"].ToString();
                            if (!emails.Contains(reader["Email"].ToString()))
                                emails.Add(reader["Email"].ToString());
                            chk.CheckedChanged += new EventHandler(CheckBox_Checked);
                            item_panel.Controls.Add(chk, colIndecx, rowIndex);
                            listControl.Add(chk);
                            colIndecx++;
                            if (colIndecx > 1)
                            {
                                colIndecx = 0;
                                rowIndex++;
                                this.item_panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                            }
                        }
                    }
                }
            }
        }
        private string getEmailFromName(string name)
        {
            string email = "";
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT Email FROM Emails Where NameOfPerson='" + name + "'";

                using (var command = new MySqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            email = reader["Email"].ToString();
                        }
                    }
                }                
            }
            return email;
        }
        private void CheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.Checked)
            {
                string email = getEmailFromName(chk.Text);
                if (!emailsTo.Contains(email))
                    emailsTo.Add(email);
            }
            else
            {
                string email = getEmailFromName(chk.Text);
                emailsTo.Remove(email);
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
            foreach (Control cntrl in item_panel.Controls)
            {
                CheckBox label = (CheckBox)cntrl;
                names.Add(label.Text);
            }
            for (int i = 0; i < emailsTo.Count; i++)
            {
                sendEmail(emailsTo[i], names[i]);
            }
            MessageBox.Show("Email Sent Successfully !!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult drs = MessageBox.Show("Do you wan to open the report file ??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drs == DialogResult.No)
                return;
            System.Diagnostics.Process.Start(filename);
        }

        private void item_panel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }
    }
}
