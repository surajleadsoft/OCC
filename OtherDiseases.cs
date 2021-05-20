using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OCC
{
    public partial class OtherDiseases : Form
    {
        List<CheckBox> listControl = new List<CheckBox>();
        public OtherDiseases()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            //label3.ForeColor = Color.Red;
        }
        private void fillCombo()
        {
            if(item_panel.Controls.Count > 0)
                item_panel.Controls.Clear();
            var connectionString = DbConnect.conString;
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * From Diseases Order By ID";                
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
                            chk.Text = reader["Name"].ToString();
                            chk.CheckedChanged += new EventHandler(CheckBox_Checked);
                            item_panel.Controls.Add(chk, colIndecx, rowIndex);
                            listControl.Add(chk);
                            colIndecx++;
                            if (colIndecx > 1)
                            {
                                colIndecx = 0;
                                rowIndex ++;
                                this.item_panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                            }
                        }                        
                    }
                }                
            }
        }
        public string getDiseases()
        {
            return lbResult.Text;
        }
        private void CheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox chk = (sender as CheckBox);
            if (chk.Checked)
            {
                if (lbResult.Text == "")
                    lbResult.Text = chk.Text;
                else
                {
                    if (!lbResult.Text.Contains(chk.Text))
                        lbResult.Text = lbResult.Text + "," + chk.Text;
                    if (chk.Text == "No Diseases")
                    {
                        lbResult.Text = chk.Text;
                        for(int i=0;i<listControl.Count;i++)
                        {
                            CheckBox checkBox = (CheckBox)listControl[i];
                            if (checkBox.Text != "No Diseases")
                                checkBox.Checked = false;
                        }   
                    }
                }
            }
            else if(!chk.Checked)
            {
                if (lbResult.Text == chk.Text)
                    lbResult.Text = "";
                else {
                    string find = chk.Text + ",";
                    lbResult.Text=lbResult.Text.Replace(find, "");
                    find = ","+chk.Text;
                    lbResult.Text = lbResult.Text.Replace(find, "");
                }
            }
        }
        private void label3_MouseLeave(object sender, EventArgs e)
        {
            //label3.ForeColor = Color.Black;
        }

        private void OtherDiseases_Load(object sender, EventArgs e)
        {
            lbResult.Text = "";
            fillCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(lbResult.Text=="")
            {
                MessageBox.Show("Please select diseases of patients !!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Hide();
        }
    }
}
