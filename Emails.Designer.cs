namespace OCC
{
    partial class Emails
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.gridEmailList = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameOfPerson = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.panelInput = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.txtemail = new System.Windows.Forms.TextBox();
            this.line2 = new DevComponents.DotNetBar.Controls.Line();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtname = new System.Windows.Forms.TextBox();
            this.line1 = new DevComponents.DotNetBar.Controls.Line();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbid = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEmailList)).BeginInit();
            this.panel3.SuspendLayout();
            this.panelInput.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panelInput);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(675, 444);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 249);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(675, 195);
            this.panel4.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridEmailList);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(675, 195);
            this.panel5.TabIndex = 0;
            // 
            // gridEmailList
            // 
            this.gridEmailList.AllowUserToAddRows = false;
            this.gridEmailList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.gridEmailList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridEmailList.BackgroundColor = System.Drawing.Color.White;
            this.gridEmailList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridEmailList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.NameOfPerson,
            this.Email});
            this.gridEmailList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEmailList.Location = new System.Drawing.Point(0, 0);
            this.gridEmailList.Name = "gridEmailList";
            this.gridEmailList.ReadOnly = true;
            this.gridEmailList.RowHeadersVisible = false;
            this.gridEmailList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridEmailList.Size = new System.Drawing.Size(675, 195);
            this.gridEmailList.TabIndex = 1;
            this.gridEmailList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridEmailList_CellContentClick);
            this.gridEmailList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gridEmailList_RowsAdded);
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 80;
            // 
            // NameOfPerson
            // 
            this.NameOfPerson.HeaderText = "Name Of Person";
            this.NameOfPerson.Name = "NameOfPerson";
            this.NameOfPerson.ReadOnly = true;
            this.NameOfPerson.Width = 270;
            // 
            // Email
            // 
            this.Email.HeaderText = "Email of Person";
            this.Email.Name = "Email";
            this.Email.ReadOnly = true;
            this.Email.Width = 320;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnDel);
            this.panel3.Controls.Add(this.btnUp);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnNew);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 207);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(675, 42);
            this.panel3.TabIndex = 5;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(465, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 30);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "CANCEL";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDel
            // 
            this.btnDel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDel.FlatAppearance.BorderSize = 0;
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDel.ForeColor = System.Drawing.Color.White;
            this.btnDel.Location = new System.Drawing.Point(384, 5);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 30);
            this.btnDel.TabIndex = 1;
            this.btnDel.Text = "DELETE";
            this.btnDel.UseVisualStyleBackColor = false;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.SaddleBrown;
            this.btnUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUp.FlatAppearance.BorderSize = 0;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.ForeColor = System.Drawing.Color.White;
            this.btnUp.Location = new System.Drawing.Point(303, 5);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 30);
            this.btnUp.TabIndex = 0;
            this.btnUp.Text = "UPDATE";
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.OrangeRed;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(222, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Green;
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(119, 5);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(97, 30);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "ADD NEW";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // panelInput
            // 
            this.panelInput.Controls.Add(this.panel7);
            this.panelInput.Controls.Add(this.panel6);
            this.panelInput.Controls.Add(this.label3);
            this.panelInput.Controls.Add(this.label4);
            this.panelInput.Controls.Add(this.lbid);
            this.panelInput.Controls.Add(this.label5);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInput.Location = new System.Drawing.Point(0, 41);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(675, 166);
            this.panelInput.TabIndex = 4;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.txtemail);
            this.panel7.Controls.Add(this.line2);
            this.panel7.Location = new System.Drawing.Point(10, 117);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(542, 35);
            this.panel7.TabIndex = 2;
            // 
            // txtemail
            // 
            this.txtemail.BackColor = System.Drawing.Color.White;
            this.txtemail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtemail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtemail.Location = new System.Drawing.Point(0, 0);
            this.txtemail.Name = "txtemail";
            this.txtemail.Size = new System.Drawing.Size(542, 22);
            this.txtemail.TabIndex = 3;
            // 
            // line2
            // 
            this.line2.BackColor = System.Drawing.Color.White;
            this.line2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.line2.Location = new System.Drawing.Point(0, 25);
            this.line2.Name = "line2";
            this.line2.Size = new System.Drawing.Size(542, 10);
            this.line2.TabIndex = 2;
            this.line2.Text = "line2";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtname);
            this.panel6.Controls.Add(this.line1);
            this.panel6.Location = new System.Drawing.Point(131, 42);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(421, 35);
            this.panel6.TabIndex = 1;
            // 
            // txtname
            // 
            this.txtname.BackColor = System.Drawing.Color.White;
            this.txtname.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtname.Location = new System.Drawing.Point(0, 0);
            this.txtname.Name = "txtname";
            this.txtname.Size = new System.Drawing.Size(421, 22);
            this.txtname.TabIndex = 3;
            // 
            // line1
            // 
            this.line1.BackColor = System.Drawing.Color.White;
            this.line1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.line1.Location = new System.Drawing.Point(0, 25);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(421, 10);
            this.line1.TabIndex = 2;
            this.line1.Text = "line1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "EMAIL OF PERSON";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(127, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "NAME OF PERSON";
            // 
            // lbid
            // 
            this.lbid.AutoSize = true;
            this.lbid.BackColor = System.Drawing.Color.Yellow;
            this.lbid.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbid.Location = new System.Drawing.Point(6, 51);
            this.lbid.Name = "lbid";
            this.lbid.Size = new System.Drawing.Size(26, 21);
            this.lbid.TabIndex = 0;
            this.lbid.Text = "ID";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 21);
            this.label5.TabIndex = 0;
            this.label5.Text = "ID";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(675, 41);
            this.panel2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 41);
            this.label1.TabIndex = 0;
            this.label1.Text = "EMAIL MASTER";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(645, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 41);
            this.label2.TabIndex = 1;
            this.label2.Text = "X";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            this.label2.MouseEnter += new System.EventHandler(this.label2_MouseEnter);
            this.label2.MouseLeave += new System.EventHandler(this.label2_MouseLeave);
            // 
            // Emails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Emails";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(695, 464);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEmailList)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.TextBox txtemail;
        private DevComponents.DotNetBar.Controls.Line line2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtname;
        private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView gridEmailList;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameOfPerson;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email;
    }
}
