
SkulMaster 
An Offline-First School Management System designed for the Sierra Leonean Educational Sector.
SkulMaster is a robust, Windows-based desktop application built to handle the complete academic lifecycle—from student registration and grading to advanced analytics and physical report card generation. Engineered specifically to accommodate local infrastructure realities, it operates flawlessly offline using a local SQLite database and features a manual, asynchronous synchronization engine to securely upload data to the Ministry of Basic and Senior Secondary Education (MBSSE) cloud API when internet access is available.

------------------------------------------------------------------------------------------------------------------------------------------
Core Features
•	Offline-First Architecture: Full functionality without requiring an active internet connection.
•	MBSSE Compliant Analytics: Automatically calculates pass/fail ratios based strictly on the national 50% minimum pass mark.
•	Automated Report Cards: Generates professional, printable student report cards natively.
•	Asynchronous Cloud Sync: Manual, button-triggered API payloads to sync lightweight student profiles and new grades to the central database without freezing the UI.

⚙️ Installation
Prerequisites
•	Windows 10 or 11
•	[.NET Framework / .NET Core] (Depending on your specific build)
•	Visual Studio 2022 (for development)

------------------------------------------------------------------------------------------------------------------------------------------

For End-Users (School Administration)
1.	Download the latest SkulMaster_Setup.exe from the Releases tab.
2.	Double-click the installer and follow the Setup Wizard.
3.	Launch the application from the desktop shortcut.

Note: The local database (SkulMaster.db) is safely generated in the hidden Windows AppData folder to prevent write-permission errors.

------------------------------------------------------------------------------------------------------------------------------------------
For Developers
1.	Clone the repository to your local machine:
Bash
git clone (https://github.com/kalokoh/SkulMaster.git)
2.	Open SkulMaster.sln in Visual Studio.
3.	Ensure the following NuGet packages are restored:
o	Microsoft.Data.Sqlite
o	System.Text.Json
4.	Set the build configuration to Debug or Release and hit F5 to run.

------------------------------------------------------------------------------------------------------------------------------------------
Usage
Here is a quick workflow of how a teacher or administrator navigates the system:
1. Managing Academics
Navigate to the Academics tab. Select a Class (e.g., "SSS 1 Science"), Academic Year, and Term. Enter the raw scores for Test 1 and Test 2. The system will automatically calculate the average and assign a Pass/Fail remark.
2. Viewing Analytics
Navigate to the Analytics Dashboard. Select a cohort to view auto-generated, interactive Key Performance Indicators (KPIs) and stacked bar charts visualizing the exact headcount of students who passed (≥50%) versus failed (<50%) in each subject.
3. Generating Report Cards
From the Master Broadsheet, select a specific student's row and click Print Student Report. The system will generate a formatted document complete with the school header, grades, and overall term status, ready to be printed or saved as a PDF.
4. Cloud Synchronization
When an internet connection is established, the administrator can navigate to the Utilities/Settings module and click ☁️ Upload Data to Cloud.


The system will only transmit records that have not been previously synced, minimizing bandwidth usage.
Contributing

------------------------------------------------------------------------------------------------------------------------------------------
Contributions are welcome to help expand the system's capabilities for broader public health and utility tracking integrations!
1.	Fork the Project
2.	Create your Feature Branch:
Bash
git checkout -b feature/AmazingFeature
3.	Commit your Changes:
Bash
git commit -m 'Add some AmazingFeature'
4.	Push to the Branch:
Bash
git push origin feature/AmazingFeature
5.	Open a Pull Request 
Please ensure any database schema changes include the appropriate IF NOT EXISTS and ALTER TABLE fallbacks to prevent breaking existing offline deployments. 

------------------------------------------------------------------------------------------------------------------------------------------
