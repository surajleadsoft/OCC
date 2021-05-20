using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using MySql.Data.MySqlClient;

namespace OCC
{
    public partial class Updates : Form
    {
        WebClient client;
        string filename1 = "";
        public Updates()
        {
            InitializeComponent();
        }

        private void Updates_Load(object sender, EventArgs e)
        {
            client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChange;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = "http://leadsoftsolutions.online/TernaQuarantineCenter/downloads/OCC.msi";

            if (!string.IsNullOrEmpty(url))
            {
                Thread thread = new Thread(() =>
                {
                    Uri uri = new Uri(url);
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    path = path + "\\OCC PATCH\\";
                    if (!System.IO.Directory.Exists(path))
                        System.IO.Directory.CreateDirectory(path);
                    string filename = System.IO.Path.GetFileName(uri.AbsolutePath);
                    filename1 = path + "\\" + filename;
                    client.DownloadFileAsync(uri, path + "\\" + filename);
                });
                thread.Start();
            }
        }
        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download Complete !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            System.Diagnostics.Process.Start(filename1);
            MySqlConnection cn = new MySqlConnection();
            cn.ConnectionString = DbConnect.conString;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE SoftwareUpdates SET isUpdate='No'";
            cmd.ExecuteNonQuery();
            cn.Close();
            Application.Exit();
        }
        private void Client_DownloadProgressChange(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate()
                {
                    progressBar1.Minimum = 0;
                    double receive = double.Parse(e.BytesReceived.ToString());
                    double total = double.Parse(e.TotalBytesToReceive.ToString());
                    double percentage = receive / total * 100;
                    button1.Text = "Downloaded "+string.Format("{0:0.##}",percentage)+"%";
                    progressBar1.Value = Int32.Parse(Math.Truncate(percentage).ToString());
                }));
        }
    }
}
