using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SkulMaster.Data;

namespace SkulMaster
{
    public partial class MainForm : Form
    {
        private DatabaseHelper _dbHelper;
        private PictureBox picSchoolLogo;
        private Label lblSchoolName, lblSchoolDetails, lblFooterText;
        private TabControl mainTabControl;

        public MainForm()
        {
            _dbHelper = new DatabaseHelper();
            InitializeUI();
            LoadSchoolHeader();
        }

        private void InitializeUI()
        {
            this.Text = "SkulMaster - School Management System";
            this.Size = new Size(1200, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- HEADER PANEL ---
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.White };
            picSchoolLogo = new PictureBox { Location = new Point(20, 10), Size = new Size(70, 70), SizeMode = PictureBoxSizeMode.Zoom };
            lblSchoolName = new Label { Location = new Point(100, 20), Size = new Size(800, 30), Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.DarkSlateBlue };
            lblSchoolDetails = new Label { Location = new Point(105, 50), Size = new Size(800, 20), Font = new Font("Segoe UI", 10, FontStyle.Regular), ForeColor = Color.Gray };
            pnlHeader.Controls.Add(picSchoolLogo); pnlHeader.Controls.Add(lblSchoolName); pnlHeader.Controls.Add(lblSchoolDetails);
            pnlHeader.Controls.Add(new Label { Dock = DockStyle.Bottom, Height = 2, BackColor = Color.LightGray });

            // --- FOOTER PANEL ---
            Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 30, BackColor = Color.WhiteSmoke };
            lblFooterText = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9), ForeColor = Color.DimGray };
            pnlFooter.Controls.Add(lblFooterText); pnlFooter.Controls.Add(new Label { Dock = DockStyle.Top, Height = 1, BackColor = Color.LightGray });

            // --- TAB CONTROL & USER CONTROLS ---
            mainTabControl = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10) };

            TabPage tabDashboard = new TabPage("Analytics Dashboard");
            TabPage tabManage = new TabPage("Student Management");
            TabPage tabStaff = new TabPage("Staff & Faculty");
            TabPage tabAcademics = new TabPage("Academics & Grades");

            // Instantiate our new modular UserControls
            var dashboardControl = new DashboardControl();
            var managementControl = new StudentManagementControl();
            var staffControl = new StaffManagementControl();
            var academicsControl = new AcademicsManagementControl();

            // Wire them together: When management data changes, tell the dashboard to refresh
            managementControl.OnDataChanged = () => dashboardControl.RefreshData();

            // Do an initial refresh to populate the dashboard on launch
            dashboardControl.RefreshData();

            tabDashboard.Controls.Add(dashboardControl);
            tabManage.Controls.Add(managementControl);
            tabStaff.Controls.Add(staffControl);
            tabAcademics.Controls.Add(academicsControl);

            mainTabControl.TabPages.Add(tabDashboard);
            mainTabControl.TabPages.Add(tabManage);
            mainTabControl.TabPages.Add(tabStaff);
            mainTabControl.TabPages.Add(tabAcademics);

            this.Controls.Add(mainTabControl);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlHeader);
        }

        private void LoadSchoolHeader()
        {
            var school = _dbHelper.GetSchoolInfo();
            if (school != null)
            {
                lblSchoolName.Text = string.IsNullOrEmpty(school.SchoolName) ? "SKULMASTER ACADEMY" : school.SchoolName.ToUpper();
                lblSchoolDetails.Text = $"Ministry Sync ID: {school.SchoolCode}   |   District: {school.District}   |   Level: {school.Level}";
                lblFooterText.Text = $"Contact: {school.Contact}   |   Address: {school.Address}   |   © {DateTime.Now.Year} SkulMaster. Developed by You";

                if (school.Logo != null && school.Logo.Length > 0)
                {
                    try { using (var ms = new MemoryStream(school.Logo)) { picSchoolLogo.Image = Image.FromStream(ms); } } catch { }
                }
            }
        }
    }
}