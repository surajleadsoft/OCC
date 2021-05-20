using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace OCC
{
    public partial class Dashboard : UserControl
    {
        List<string> months = new List<string>();
        public Dashboard()
        {
            InitializeComponent();
            months.Add("Jan");
            months.Add("Feb");
            months.Add("Mar");
            months.Add("Apr");
            months.Add("May");
            months.Add("June");
            months.Add("July");
            months.Add("Aug");
            months.Add("Sep");
            months.Add("Nov");
            months.Add("Dec");
        }
        private int getCount(String q)
        {
            int srno = 0;
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = q;
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
        private void fillCharts()
        {

        }
        public void setCount()
        {
            lbActive.Text = getCount("SELECT DISTINCT COUNT(Name) FROM PatientsRemark WHERE Flag=1").ToString();
            lbDischarge.Text = getCount("SELECT DISTINCT COUNT(Name) FROM PatientsRemark WHERE Flag=0").ToString();
            lbQurantine.Text = getCount("SELECT DISTINCT COUNT(Name) FROM PatientsRemark WHERE CenterType='Quarantine Center'").ToString();
            lbrisk.Text = getCount("SELECT DISTINCT COUNT(Name) FROM HighRiskPatients").ToString();
            lbTotal.Text = getCount("SELECT DISTINCT COUNT(Name) FROM Patients").ToString();
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 1;
            lbtotalBeds.Text = getCount("SELECT COUNT(ID) FROM Beds WHERE CenterType='CCC'").ToString();
            lbQtotalbeds.Text = getCount("SELECT COUNT(ID) FROM Beds WHERE CenterType='Quarantine Center'").ToString();
            lbfree.Text = getCount("SELECT COUNT(ID) FROM Beds WHERE CenterType='CCC' and Flag=0").ToString();
            lballocated.Text = getCount("SELECT COUNT(ID) FROM Beds WHERE CenterType='CCC' and Flag=1").ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart.CheckForIllegalCrossThreadCalls = false;
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            chart1.ChartAreas[0].AxisX.Title = "Date";
            chart1.ChartAreas[0].AxisY.Title = "Patients";
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender1, e1) =>
            {
                if (comboBox2.SelectedIndex == 1 && comboBox1.SelectedIndex != 0)
                {
                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
                if (comboBox2.SelectedIndex == 2 && comboBox1.SelectedIndex != 0)
                {

                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1 and Remark='Active'";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
                if (comboBox2.SelectedIndex == 3 && comboBox1.SelectedIndex != 0)
                {
                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1 and Remark='Discharged'";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
                if (comboBox2.SelectedIndex == 4 && comboBox1.SelectedIndex != 0)
                {

                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1 and CenterType='Qurantine Center'";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
            };
            bwConn.RunWorkerCompleted += (sender1, e1) =>
            {
                if (e1.Error != null)
                {
                    MessageBox.Show(e1.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                pb.Close();
                
                for (int i = 0; i < chart1.Series["Patients"].Points.Count; i++)
                {
                    var val = chart1.Series["Patients"].Points[i].GetValueByName("Y");

                    int value = Int32.Parse(val.ToString());
                    if (Int32.Parse(value.ToString()) >= 15)
                        chart1.Series["Patients"].Points[i].Color = Color.Red;
                    else if (Int32.Parse(value.ToString()) >= 5 && Int32.Parse(value.ToString()) < 15)
                        chart1.Series["Patients"].Points[i].Color = Color.Orange;
                    else if (Int32.Parse(value.ToString()) >= 0 && Int32.Parse(value.ToString()) < 5)
                        chart1.Series["Patients"].Points[i].Color = Color.Green;
                }
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.Chart.CheckForIllegalCrossThreadCalls = false;
            ComboBox.CheckForIllegalCrossThreadCalls = false;
            chart1.ChartAreas[0].AxisX.Title = "Date";
            chart1.ChartAreas[0].AxisY.Title = "Patients";
            PleaseWait pb = new PleaseWait();
            BackgroundWorker bwConn = new BackgroundWorker();
            bwConn.DoWork += (sender1, e1) =>
            {
                if (comboBox2.SelectedIndex == 1 && comboBox1.SelectedIndex !=0)
                {
                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
                if (comboBox2.SelectedIndex == 2 && comboBox1.SelectedIndex != 0)
                {

                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1 and Remark='Active'";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
                if (comboBox2.SelectedIndex == 3 && comboBox1.SelectedIndex != 0)
                {
                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1 and Remark='Discharged'";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
                if (comboBox2.SelectedIndex == 4 && comboBox1.SelectedIndex != 0)
                {

                    DateTime date1 = DateTime.Parse(DateTime.Now.ToShortDateString());
                    TimeSpan span = new TimeSpan(Int32.Parse(comboBox1.Text.Replace("Days", "")), 0, 0, 0);
                    DateTime date2 = DateTime.Now.Subtract(span);
                    foreach (var series in chart1.Series)
                    {
                        series.Points.Clear();
                    }
                    while (date2 <= date1)
                    {
                        var connectionString = DbConnect.conString;
                        using (var connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            var query = "SELECT  DISTINCT COUNT(Name) From PatientsRemark WHERE Date=@date1 and CenterType='Qurantine Center'";
                            using (var command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@date1", date2.ToString("yyyy-MM-dd"));
                                using (var reader = command.ExecuteReader())
                                {
                                    int rows = 0;
                                    while (reader.Read())
                                    {
                                        string dateChart = date2.Day.ToString();
                                        string count = reader[0].ToString();
                                        chart1.Series["Patients"].Points.AddXY(dateChart + " " + months[date2.Month - 1], count);
                                        chart1.ChartAreas[0].AxisX.Interval = 1;
                                        chart1.ChartAreas[0].AxisY.Interval = 1;
                                        rows++;
                                    }
                                }
                            }
                        }
                        date2 = date2.AddDays(1);
                    }
                }
            };
            bwConn.RunWorkerCompleted += (sender1, e1) =>
            {
                if (e1.Error != null)
                {
                    MessageBox.Show(e1.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                pb.Close();
                
                for (int i = 0; i < chart1.Series["Patients"].Points.Count; i++)
                {
                    var val = chart1.Series["Patients"].Points[i].GetValueByName("Y");

                    int value = Int32.Parse(val.ToString());
                    if (Int32.Parse(value.ToString()) >= 15)
                        chart1.Series["Patients"].Points[i].Color = Color.Red;
                    else if (Int32.Parse(value.ToString()) >= 5 && Int32.Parse(value.ToString()) < 15)
                        chart1.Series["Patients"].Points[i].Color = Color.Orange;
                    else if (Int32.Parse(value.ToString()) >= 0 && Int32.Parse(value.ToString()) < 5)
                        chart1.Series["Patients"].Points[i].Color = Color.Green;
                }
            };
            bwConn.RunWorkerAsync();
            pb.ShowDialog();                                       
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbTotal_Click(object sender, EventArgs e)
        {
            AllPatients ob = new AllPatients("SELECT * FROM Patients ORDER BY Name","All Patients");
            ob.ShowDialog();
        }

        private void lbDischarge_Click(object sender, EventArgs e)
        {
            AllPatients ob = new AllPatients("SELECT * FROM PatientsRemark WHERE Flag=0 ORDER BY Name","Discharge Patients");
            ob.ShowDialog();
        }

        private void lbActive_Click(object sender, EventArgs e)
        {
            AllPatients ob = new AllPatients("SELECT * FROM PatientsRemark WHERE Flag=1 ORDER BY Name","Active Patients");
            ob.ShowDialog();
        }

        private void lbQurantine_Click(object sender, EventArgs e)
        {
            AllPatients ob = new AllPatients("SELECT * FROM Patients WHERE CenterType='Quarantine Center' ORDER BY Name","Quarantine Patients");
            ob.ShowDialog();
        }

        private void lbrisk_Click(object sender, EventArgs e)
        {
            AllPatients ob = new AllPatients("SELECT * FROM HighRiskPatients  ORDER BY Name","High Risk Patients");
            ob.ShowDialog();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbtotalBeds_Click(object sender, EventArgs e)
        {
            TotalBeds ob = new TotalBeds(Form1.centerType,Form1.centerName);
            ob.ShowDialog();
        }
        
    }
}
