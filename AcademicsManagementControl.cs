using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using SkulMaster.Data;

namespace SkulMaster
{
    public partial class AcademicsManagementControl : UserControl
    {
        private DatabaseHelper _dbHelper;
        private ComboBox cmbYear, cmbTerm, cmbLevel, cmbSubject;
        private Button btnLoadClass, btnSaveGrades;
        private DataGridView dgvGrades;

        public AcademicsManagementControl()
        {
            _dbHelper = new DatabaseHelper();
            InitializeUI();
            LoadSubjectsIntoDropdown();
        }

        private void InitializeUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            // --- TOP FILTER PANEL ---
            Panel pnlFilters = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(20) };

            Label lblTitle = new Label { Text = "Gradebook Entry", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(40, 40, 40), Location = new Point(20, 15), AutoSize = true };
            pnlFilters.Controls.Add(lblTitle);

            int startX = 20; int y = 55; int comboWidth = 140; int gap = 15;

            // Filters
            cmbYear = new ComboBox { Location = new Point(startX, y), Width = comboWidth, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            cmbYear.Items.AddRange(new string[] { "2025/2026", "2026/2027", "2027/2028" }); cmbYear.SelectedIndex = 0;

            cmbTerm = new ComboBox { Location = new Point(startX + comboWidth + gap, y), Width = comboWidth, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            cmbTerm.Items.AddRange(new string[] { "Term 1", "Term 2", "Term 3" }); cmbTerm.SelectedIndex = 0;

            cmbLevel = new ComboBox { Location = new Point(startX + (comboWidth * 2) + (gap * 2), y), Width = comboWidth, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };
            cmbLevel.Items.AddRange(new string[] { "JSS 1", "JSS 2", "JSS 3", "SSS 1", "SSS 2", "SSS 3" }); cmbLevel.SelectedIndex = 0;

            cmbSubject = new ComboBox { Location = new Point(startX + (comboWidth * 3) + (gap * 3), y), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat };

            btnLoadClass = new Button { Text = "Load Roster", Location = new Point(startX + (comboWidth * 3) + 200 + (gap * 4), y - 2), Width = 120, Height = 30, FlatStyle = FlatStyle.Flat, BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnLoadClass.FlatAppearance.BorderSize = 0;
            btnLoadClass.Click += BtnLoadClass_Click;

            pnlFilters.Controls.Add(cmbYear); pnlFilters.Controls.Add(cmbTerm); pnlFilters.Controls.Add(cmbLevel); pnlFilters.Controls.Add(cmbSubject); pnlFilters.Controls.Add(btnLoadClass);
            this.Controls.Add(pnlFilters);

            // --- GRADEBOOK DATAGRID ---
            Panel pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };

            dgvGrades = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                GridColor = Color.LightGray
            };

            dgvGrades.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 50, 70);
            dgvGrades.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGrades.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvGrades.ColumnHeadersHeight = 40;
            dgvGrades.DefaultCellStyle.Padding = new Padding(5);
            dgvGrades.RowTemplate.Height = 35;

            // Create explicitly typed columns for perfect control
            dgvGrades.Columns.Add(new DataGridViewTextBoxColumn { Name = "StudentId", HeaderText = "Student ID", ReadOnly = true, DefaultCellStyle = { BackColor = Color.WhiteSmoke } });
            dgvGrades.Columns.Add(new DataGridViewTextBoxColumn { Name = "FullName", HeaderText = "Full Name", ReadOnly = true, DefaultCellStyle = { BackColor = Color.WhiteSmoke } });
            dgvGrades.Columns.Add(new DataGridViewTextBoxColumn { Name = "Test1", HeaderText = "Test 1 Score" });
            dgvGrades.Columns.Add(new DataGridViewTextBoxColumn { Name = "Test2", HeaderText = "Test 2 Score" });
            dgvGrades.Columns.Add(new DataGridViewTextBoxColumn { Name = "Mean", HeaderText = "Subject Mean", ReadOnly = true, DefaultCellStyle = { BackColor = Color.FromArgb(240, 248, 255), Font = new Font("Segoe UI", 10, FontStyle.Bold) } });

            // Automatically calculate the Mean whenever a teacher types a score!
            dgvGrades.CellValueChanged += DgvGrades_CellValueChanged;
            dgvGrades.CurrentCellDirtyStateChanged += (s, e) => { if (dgvGrades.IsCurrentCellDirty) dgvGrades.CommitEdit(DataGridViewDataErrorContexts.Commit); };

            pnlGrid.Controls.Add(dgvGrades);
            this.Controls.Add(pnlGrid);

            // --- BOTTOM ACTION PANEL ---
            Panel pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.White };
            btnSaveGrades = new Button { Text = "💾 Save All Grades", Location = new Point(20, 15), Width = 180, Height = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.SeaGreen, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), Cursor = Cursors.Hand };
            btnSaveGrades.FlatAppearance.BorderSize = 0;
            btnSaveGrades.Click += BtnSaveGrades_Click;
            pnlBottom.Controls.Add(btnSaveGrades);
            this.Controls.Add(pnlBottom);
        }

        private void LoadSubjectsIntoDropdown()
        {
            var subjects = _dbHelper.GetSubjects();
            cmbSubject.DataSource = subjects;
            cmbSubject.DisplayMember = "SubjectName";
            cmbSubject.ValueMember = "Id";
        }

        private void BtnLoadClass_Click(object sender, EventArgs e)
        {
            if (cmbSubject.SelectedValue == null) { MessageBox.Show("Please add subjects to the database first."); return; }

            int subjectId = Convert.ToInt32(cmbSubject.SelectedValue);
            string level = cmbLevel.SelectedItem.ToString();
            string year = cmbYear.SelectedItem.ToString();
            string term = cmbTerm.SelectedItem.ToString();

            var roster = _dbHelper.GetClassRosterWithGrades(level, subjectId, year, term);

            dgvGrades.Rows.Clear();
            foreach (DataRow row in roster.Rows)
            {
                // Load data and calculate initial mean
                double? t1 = row["Test1"] != DBNull.Value ? Convert.ToDouble(row["Test1"]) : (double?)null;
                double? t2 = row["Test2"] != DBNull.Value ? Convert.ToDouble(row["Test2"]) : (double?)null;
                string meanStr = CalculateMean(t1, t2);

                dgvGrades.Rows.Add(row["StudentId"].ToString(), row["FullName"].ToString(), t1, t2, meanStr);
            }

            if (roster.Rows.Count == 0) MessageBox.Show($"No students found enrolled in {level}.", "Empty Class");
        }

        // Live calculation logic for Sierra Leone standard
        private void DgvGrades_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (dgvGrades.Columns[e.ColumnIndex].Name == "Test1" || dgvGrades.Columns[e.ColumnIndex].Name == "Test2"))
            {
                var row = dgvGrades.Rows[e.RowIndex];

                double? t1 = null; double? t2 = null;
                if (double.TryParse(row.Cells["Test1"].Value?.ToString(), out double parsedT1)) t1 = parsedT1;
                if (double.TryParse(row.Cells["Test2"].Value?.ToString(), out double parsedT2)) t2 = parsedT2;

                // Update the Mean cell instantly
                row.Cells["Mean"].Value = CalculateMean(t1, t2);
            }
        }

        private string CalculateMean(double? t1, double? t2)
        {
            if (t1 == null && t2 == null) return "-";
            double val1 = t1 ?? 0;
            double val2 = t2 ?? 0;

            // If only one test is taken, do we divide by 1 or 2? 
            // Standard practice: if a test is completely missing (null), it counts as 0 in the average.
            double mean = (val1 + val2) / 2.0;
            return Math.Round(mean, 1).ToString("0.0");
        }

        private void BtnSaveGrades_Click(object sender, EventArgs e)
        {
            if (dgvGrades.Rows.Count == 0) return;

            int subjectId = Convert.ToInt32(cmbSubject.SelectedValue);
            string year = cmbYear.SelectedItem.ToString();
            string term = cmbTerm.SelectedItem.ToString();

            int savedCount = 0;

            foreach (DataGridViewRow row in dgvGrades.Rows)
            {
                string studentId = row.Cells["StudentId"].Value.ToString();

                double? t1 = null; double? t2 = null;
                if (double.TryParse(row.Cells["Test1"].Value?.ToString(), out double parsedT1)) t1 = parsedT1;
                if (double.TryParse(row.Cells["Test2"].Value?.ToString(), out double parsedT2)) t2 = parsedT2;

                // Only save if at least one test score is entered
                if (t1 != null || t2 != null)
                {
                    _dbHelper.SaveGrade(studentId, subjectId, year, term, t1, t2);
                    savedCount++;
                }
            }

            MessageBox.Show($"Successfully saved grades for {savedCount} students!", "Grades Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}