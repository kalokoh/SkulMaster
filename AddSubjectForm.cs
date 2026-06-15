using System;
using System.Drawing;
using System.Windows.Forms;
using SkulMaster.Data;
using SkulMaster.Models;

namespace SkulMaster
{
    public partial class AddSubjectForm : Form
    {
        private DatabaseHelper _dbHelper;
        private TextBox txtCode, txtName;
        private ComboBox cmbLevel, cmbFaculty;
        private CheckBox chkIsCore;
        private Button btnSave, btnCancel;

        public AddSubjectForm()
        {
            _dbHelper = new DatabaseHelper();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Add New Subject";
            this.ClientSize = new Size(380, 480);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // Top Color Accent
            Panel topAccent = new Panel { Dock = DockStyle.Top, Height = 5, BackColor = Color.DarkOrange };
            this.Controls.Add(topAccent);

            Label lblTitle = new Label { Text = "Subject Details", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), Location = new Point(25, 25), AutoSize = true };
            this.Controls.Add(lblTitle);

            int y = 70; int gap = 60; int startX = 30; int width = 300;

            // Subject Code
            this.Controls.Add(new Label { Text = "Subject Code (e.g., MTH, ENG):", Location = new Point(startX, y), AutoSize = true, ForeColor = Color.DimGray });
            txtCode = new TextBox { Location = new Point(startX, y + 20), Width = width, BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtCode);

            // Subject Name
            y += gap;
            this.Controls.Add(new Label { Text = "Subject Name:", Location = new Point(startX, y), AutoSize = true, ForeColor = Color.DimGray });
            txtName = new TextBox { Location = new Point(startX, y + 20), Width = width, BorderStyle = BorderStyle.FixedSingle };
            this.Controls.Add(txtName);

            // Level
            y += gap;
            this.Controls.Add(new Label { Text = "Academic Level:", Location = new Point(startX, y), AutoSize = true, ForeColor = Color.DimGray });
            cmbLevel = new ComboBox { Location = new Point(startX, y + 20), Width = width, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            cmbLevel.Items.AddRange(new string[] { "All Levels", "JSS Only", "SSS Only" });
            cmbLevel.SelectedIndex = 0;
            this.Controls.Add(cmbLevel);

            // Faculty
            y += gap;
            this.Controls.Add(new Label { Text = "Faculty Track:", Location = new Point(startX, y), AutoSize = true, ForeColor = Color.DimGray });
            cmbFaculty = new ComboBox { Location = new Point(startX, y + 20), Width = width, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            cmbFaculty.Items.AddRange(new string[] { "General (All)", "Sciences", "Arts & Languages", "Commerce / Business" });
            cmbFaculty.SelectedIndex = 0;
            this.Controls.Add(cmbFaculty);

            // Is Core Checkbox
            y += gap;
            chkIsCore = new CheckBox { Text = "This is a Core Subject (Mandatory)", Location = new Point(startX, y + 10), Width = width, ForeColor = Color.SeaGreen, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            this.Controls.Add(chkIsCore);

            // Buttons
            y += 50;
            btnSave = new Button { Text = "Save Subject", Location = new Point(startX, y), Width = 140, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.SeaGreen, ForeColor = Color.White, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Cancel", Location = new Point(startX + 150, y), Width = 150, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.LightGray, ForeColor = Color.Black, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnSave; // Pressing 'Enter' saves
            this.CancelButton = btnCancel; // Pressing 'Esc' cancels
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please provide both a Subject Code and Name.", "Missing Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var subject = new Subject
            {
                SubjectCode = txtCode.Text.Trim().ToUpper(),
                SubjectName = txtName.Text.Trim(),
                Level = cmbLevel.SelectedItem.ToString(),
                Faculty = cmbFaculty.SelectedItem.ToString(),
                IsCoreSubject = chkIsCore.Checked
            };

            _dbHelper.AddSubject(subject);

            // Tell the parent window that we successfully saved data before closing
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}