using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingDegreeMinorTableAndData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
             name: "DegreeMinor",
             columns: table => new
             {
                 DegreeMinorId = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                 Name = table.Column<string>(type: "varchar(100)", nullable: true),
             },
             constraints: table =>
             {
                 table.PrimaryKey("PK_StudentInstitueFunding", x => x.DegreeMinorId);
             });


            migrationBuilder.Sql(@"
                    Insert Into DegreeMinor (Name) Values('Accounting')
                    Insert Into DegreeMinor (Name) Values('Advanced Certificate in Information Assurance')
                    Insert Into DegreeMinor (Name) Values('Advanced Cybersecurity Experience for Students')
                    Insert Into DegreeMinor (Name) Values('Aerospace Engineering')
                    Insert Into DegreeMinor (Name) Values('Analytics')
                    Insert Into DegreeMinor (Name) Values('Application Security Risk Analysis')
                    Insert Into DegreeMinor (Name) Values('Applied Computer Science')
                    Insert Into DegreeMinor (Name) Values('Applied Data Science')
                    Insert Into DegreeMinor (Name) Values('Applied Mathematics')
                    Insert Into DegreeMinor (Name) Values('Artificial Intelligence')
                    Insert Into DegreeMinor (Name) Values('Business')
                    Insert Into DegreeMinor (Name) Values('Certificate of Advanced Study Information Security Management')
                    Insert Into DegreeMinor (Name) Values('Communications')
                    Insert Into DegreeMinor (Name) Values('Computer and Information Sciences')
                    Insert Into DegreeMinor (Name) Values('Computer Engineering')
                    Insert Into DegreeMinor (Name) Values('Computer Engineering/Cyber Risk Management')
                    Insert Into DegreeMinor (Name) Values('Computer Forensics')
                    Insert Into DegreeMinor (Name) Values('Computer Game Design')
                    Insert Into DegreeMinor (Name) Values('Computer Information Systems')
                    Insert Into DegreeMinor (Name) Values('Computer Network and System Administration')
                    Insert Into DegreeMinor (Name) Values('Computer Science')
                    Insert Into DegreeMinor (Name) Values('Computer Science and Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Computer Science and Statistics')
                    Insert Into DegreeMinor (Name) Values('Computer Science Digital Forensics')
                    Insert Into DegreeMinor (Name) Values('Computer Security')
                    Insert Into DegreeMinor (Name) Values('Computer Security and Information Assurance')
                    Insert Into DegreeMinor (Name) Values('Computing Security')
                    Insert Into DegreeMinor (Name) Values('Concentration in Networking and Security')
                    Insert Into DegreeMinor (Name) Values('Control Systems Security')
                    Insert Into DegreeMinor (Name) Values('Crime, Law, and Psychology')
                    Insert Into DegreeMinor (Name) Values('Criminal Investigations')
                    Insert Into DegreeMinor (Name) Values('Criminal Justice')
                    Insert Into DegreeMinor (Name) Values('Criminal Justice and Cyber Criminology')
                    Insert Into DegreeMinor (Name) Values('Criminology')
                    Insert Into DegreeMinor (Name) Values('Cyber')
                    Insert Into DegreeMinor (Name) Values('Cyber Components')
                    Insert Into DegreeMinor (Name) Values('Cyber Criminology')
                    Insert Into DegreeMinor (Name) Values('Cyber Defense')
                    Insert Into DegreeMinor (Name) Values('Cyber Intel')
                    Insert Into DegreeMinor (Name) Values('Cyber Operations')
                    Insert Into DegreeMinor (Name) Values('Cyber Operations and Applied Computer Science')
                    Insert Into DegreeMinor (Name) Values('Cyber Systems and Operations')
                    Insert Into DegreeMinor (Name) Values('Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Cybersecurity and Privacy')
                    Insert Into DegreeMinor (Name) Values('Cybersecurity Business Analytics and Supply Chains')
                    Insert Into DegreeMinor (Name) Values('Cybersecurity Concentration and Advanced Cybersecurity Experience for Students')
                    Insert Into DegreeMinor (Name) Values('Cybersecurity Concentration and Certificate')
                    Insert Into DegreeMinor (Name) Values('Cybersecurity Undergraduate Academic Certificate')
                    Insert Into DegreeMinor (Name) Values('Digital Forensics')
                    Insert Into DegreeMinor (Name) Values('Electrical Engineering')
                    Insert Into DegreeMinor (Name) Values('Emphasis in Cyber')
                    Insert Into DegreeMinor (Name) Values('Emphasis in Information Assurance')
                    Insert Into DegreeMinor (Name) Values('Energy Systems')
                    Insert Into DegreeMinor (Name) Values('Engineering')
                    Insert Into DegreeMinor (Name) Values('English')
                    Insert Into DegreeMinor (Name) Values('Finance')
                    Insert Into DegreeMinor (Name) Values('Foreign Language Chinese')
                    Insert Into DegreeMinor (Name) Values('Foreign Language French')
                    Insert Into DegreeMinor (Name) Values('Foreign Language Russian')
                    Insert Into DegreeMinor (Name) Values('Foreign Language Spanish')
                    Insert Into DegreeMinor (Name) Values('Global Information Assurance Certification')
                    Insert Into DegreeMinor (Name) Values('Global Security Studies')
                    Insert Into DegreeMinor (Name) Values('Health Policy')
                    Insert Into DegreeMinor (Name) Values('Healthcare Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Informatics')
                    Insert Into DegreeMinor (Name) Values('Information Assurance')
                    Insert Into DegreeMinor (Name) Values('Information Assurance and Criminal Justice')
                    Insert Into DegreeMinor (Name) Values('Information Assurance and Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Information Assurance and Security')
                    Insert Into DegreeMinor (Name) Values('Information Assurance and Security Option')
                    Insert Into DegreeMinor (Name) Values('Information Assurance in Criminal Justice System')
                    Insert Into DegreeMinor (Name) Values('Information Assurance Management')
                    Insert Into DegreeMinor (Name) Values('Information Emphasis')
                    Insert Into DegreeMinor (Name) Values('Information Science Technology and Arabic Language')
                    Insert Into DegreeMinor (Name) Values('Information Science Technology and Global Security')
                    Insert Into DegreeMinor (Name) Values('Information Security')
                    Insert Into DegreeMinor (Name) Values('Information Security and Information Assurance')
                    Insert Into DegreeMinor (Name) Values('Information Security Management')
                    Insert Into DegreeMinor (Name) Values('Information Security Policy')
                    Insert Into DegreeMinor (Name) Values('Information Systems and Security Management')
                    Insert Into DegreeMinor (Name) Values('Information Systems Track')
                    Insert Into DegreeMinor (Name) Values('Information Technology')
                    Insert Into DegreeMinor (Name) Values('Information Technology Administration and Security')
                    Insert Into DegreeMinor (Name) Values('Information Technology Cyber Security')
                    Insert Into DegreeMinor (Name) Values('Integration')
                    Insert Into DegreeMinor (Name) Values('Intelligence')
                    Insert Into DegreeMinor (Name) Values('International Affairs')
                    Insert Into DegreeMinor (Name) Values('International Business')
                    Insert Into DegreeMinor (Name) Values('International Politics')
                    Insert Into DegreeMinor (Name) Values('Law')
                    Insert Into DegreeMinor (Name) Values('Leadership')
                    Insert Into DegreeMinor (Name) Values('Linguistics')
                    Insert Into DegreeMinor (Name) Values('Machine Learning')
                    Insert Into DegreeMinor (Name) Values('Management')
                    Insert Into DegreeMinor (Name) Values('Management of Technology')
                    Insert Into DegreeMinor (Name) Values('Mathematics')
                    Insert Into DegreeMinor (Name) Values('Mathematics and Applied Computer Science')
                    Insert Into DegreeMinor (Name) Values('Modern Languages')
                    Insert Into DegreeMinor (Name) Values('National and International Security Studies')
                    Insert Into DegreeMinor (Name) Values('National Information Security')
                    Insert Into DegreeMinor (Name) Values('Network Administration and Security')
                    Insert Into DegreeMinor (Name) Values('Network Security')
                    Insert Into DegreeMinor (Name) Values('Network Security and Information Assurance')
                    Insert Into DegreeMinor (Name) Values('Networking and Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Networking and Security')
                    Insert Into DegreeMinor (Name) Values('Next generation wireless networks')
                    Insert Into DegreeMinor (Name) Values('Nonprofit Management')
                    Insert Into DegreeMinor (Name) Values('Philiosphy')
                    Insert Into DegreeMinor (Name) Values('Physics')
                    Insert Into DegreeMinor (Name) Values('Policy and Management')
                    Insert Into DegreeMinor (Name) Values('Political Science')
                    Insert Into DegreeMinor (Name) Values('Political Science Minor: Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Pre-Law')
                    Insert Into DegreeMinor (Name) Values('Privacy')
                    Insert Into DegreeMinor (Name) Values('Professional Writing')
                    Insert Into DegreeMinor (Name) Values('Public Health')
                    Insert Into DegreeMinor (Name) Values('Quantum Information Sciences')
                    Insert Into DegreeMinor (Name) Values('Robotics Engineering')
                    Insert Into DegreeMinor (Name) Values('Science and Technology')
                    Insert Into DegreeMinor (Name) Values('Secure and Dependable Systems')
                    Insert Into DegreeMinor (Name) Values('Secure Computing')
                    Insert Into DegreeMinor (Name) Values('Secure Computing and Networks')
                    Insert Into DegreeMinor (Name) Values('Security')
                    Insert Into DegreeMinor (Name) Values('Security and Risk Analysis')
                    Insert Into DegreeMinor (Name) Values('Security Assured Information Systems')
                    Insert Into DegreeMinor (Name) Values('Security Management Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Security Track')
                    Insert Into DegreeMinor (Name) Values('Social Data Analytics')
                    Insert Into DegreeMinor (Name) Values('Software Engineering')
                    Insert Into DegreeMinor (Name) Values('System Administration and Security')
                    Insert Into DegreeMinor (Name) Values('Technical Cybersecurity')
                    Insert Into DegreeMinor (Name) Values('Technology')

               ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "DegreeMinor");
        }
    }
}
