using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OPM.SFS.Data.Migrations
{
    public partial class AddingDataDiscipline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                Insert Into Discipline (Name) Values('Accounting')
                    Insert Into Discipline (Name) Values('Advanced Certificate in Information Assurance')
                    Insert Into Discipline (Name) Values('Advanced Cybersecurity Experience for Students')
                    Insert Into Discipline (Name) Values('Aerospace Engineering')
                    Insert Into Discipline (Name) Values('Application Security Risk Analysis')
                    Insert Into Discipline (Name) Values('Applied Computer Science')
                    Insert Into Discipline (Name) Values('Applied Data Science')
                    Insert Into Discipline (Name) Values('Applied Mathematics')
                    Insert Into Discipline (Name) Values('Artificial Intelligence')
                    Insert Into Discipline (Name) Values('Business')
                    Insert Into Discipline (Name) Values('Certificate of Advanced Study Information Security Management')
                    Insert Into Discipline (Name) Values('Communications')
                    Insert Into Discipline (Name) Values('Computer and Information Sciences')
                 
                    Insert Into Discipline (Name) Values('Computer Engineering/Cyber Risk Management')
                    Insert Into Discipline (Name) Values('Computer Forensics')
                    Insert Into Discipline (Name) Values('Computer Game Design')
                    Insert Into Discipline (Name) Values('Computer Information Systems')
                    Insert Into Discipline (Name) Values('Computer Network and System Administration')
                   
                    Insert Into Discipline (Name) Values('Computer Science and Cybersecurity')
                    Insert Into Discipline (Name) Values('Computer Science and Statistics')
                    Insert Into Discipline (Name) Values('Computer Science Digital Forensics')
                    Insert Into Discipline (Name) Values('Computer Security')
                    Insert Into Discipline (Name) Values('Computer Security and Information Assurance')
                    Insert Into Discipline (Name) Values('Computing Security')
                    Insert Into Discipline (Name) Values('Concentration in Networking and Security')
                    Insert Into Discipline (Name) Values('Control Systems Security')
                    Insert Into Discipline (Name) Values('Crime, Law, and Psychology')
                    Insert Into Discipline (Name) Values('Criminal Investigations')
                   
                    Insert Into Discipline (Name) Values('Criminal Justice and Cyber Criminology')
                    Insert Into Discipline (Name) Values('Criminology')
                    Insert Into Discipline (Name) Values('Cyber')
                    Insert Into Discipline (Name) Values('Cyber Components')
                    Insert Into Discipline (Name) Values('Cyber Criminology')
                    Insert Into Discipline (Name) Values('Cyber Defense')
                    Insert Into Discipline (Name) Values('Cyber Intel')
                    Insert Into Discipline (Name) Values('Cyber Operations')
                    Insert Into Discipline (Name) Values('Cyber Operations and Applied Computer Science')
                    Insert Into Discipline (Name) Values('Cyber Systems and Operations')
                 
                    Insert Into Discipline (Name) Values('Cybersecurity and Privacy')
                    Insert Into Discipline (Name) Values('Cybersecurity Business Analytics and Supply Chains')
                    Insert Into Discipline (Name) Values('Cybersecurity Concentration and Advanced Cybersecurity Experience for Students')
                    Insert Into Discipline (Name) Values('Cybersecurity Concentration and Certificate')
                    Insert Into Discipline (Name) Values('Cybersecurity Undergraduate Academic Certificate')
                    Insert Into Discipline (Name) Values('Digital Forensics')
          
                    Insert Into Discipline (Name) Values('Emphasis in Cyber')
                    Insert Into Discipline (Name) Values('Emphasis in Information Assurance')
                    Insert Into Discipline (Name) Values('Energy Systems')
                    Insert Into Discipline (Name) Values('Engineering')
                    Insert Into Discipline (Name) Values('English')
                    Insert Into Discipline (Name) Values('Finance')
                    Insert Into Discipline (Name) Values('Foreign Language Chinese')
                    Insert Into Discipline (Name) Values('Foreign Language French')
                    Insert Into Discipline (Name) Values('Foreign Language Russian')
                    Insert Into Discipline (Name) Values('Foreign Language Spanish')
                    Insert Into Discipline (Name) Values('Global Information Assurance Certification')
                    Insert Into Discipline (Name) Values('Global Security Studies')
                    Insert Into Discipline (Name) Values('Health Policy')
                    Insert Into Discipline (Name) Values('Healthcare Cybersecurity')
               
                  
                    Insert Into Discipline (Name) Values('Information Assurance and Criminal Justice')
                    Insert Into Discipline (Name) Values('Information Assurance and Cybersecurity')
                    Insert Into Discipline (Name) Values('Information Assurance and Security')
                    Insert Into Discipline (Name) Values('Information Assurance and Security Option')
                    Insert Into Discipline (Name) Values('Information Assurance in Criminal Justice System')
                    Insert Into Discipline (Name) Values('Information Assurance Management')
                    Insert Into Discipline (Name) Values('Information Emphasis')
                    Insert Into Discipline (Name) Values('Information Science Technology and Arabic Language')
                    Insert Into Discipline (Name) Values('Information Science Technology and Global Security')
                    Insert Into Discipline (Name) Values('Information Security')
                    Insert Into Discipline (Name) Values('Information Security and Information Assurance')
                    Insert Into Discipline (Name) Values('Information Security Management')
                    
                    Insert Into Discipline (Name) Values('Information Systems and Security Management')
                    Insert Into Discipline (Name) Values('Information Systems Track')
                   
                    Insert Into Discipline (Name) Values('Information Technology Administration and Security')
                    Insert Into Discipline (Name) Values('Information Technology Cyber Security')
                    Insert Into Discipline (Name) Values('Integration')
                    Insert Into Discipline (Name) Values('Intelligence')
                    Insert Into Discipline (Name) Values('International Affairs')
                    Insert Into Discipline (Name) Values('International Business')
                    Insert Into Discipline (Name) Values('International Politics')
                    Insert Into Discipline (Name) Values('Law')
                    Insert Into Discipline (Name) Values('Leadership')
                    Insert Into Discipline (Name) Values('Linguistics')
                    Insert Into Discipline (Name) Values('Machine Learning')
                    Insert Into Discipline (Name) Values('Management')
                    Insert Into Discipline (Name) Values('Management of Technology')
                    Insert Into Discipline (Name) Values('Mathematics')
                    Insert Into Discipline (Name) Values('Mathematics and Applied Computer Science')
                    Insert Into Discipline (Name) Values('Modern Languages')
                    Insert Into Discipline (Name) Values('National and International Security Studies')
                    Insert Into Discipline (Name) Values('National Information Security')
                    Insert Into Discipline (Name) Values('Network Administration and Security')
                    Insert Into Discipline (Name) Values('Network Security')
                    Insert Into Discipline (Name) Values('Network Security and Information Assurance')
                    Insert Into Discipline (Name) Values('Networking and Cybersecurity')
                    Insert Into Discipline (Name) Values('Networking and Security')
                    Insert Into Discipline (Name) Values('Next generation wireless networks')
                    Insert Into Discipline (Name) Values('Nonprofit Management')
                    Insert Into Discipline (Name) Values('Philiosphy')
                    Insert Into Discipline (Name) Values('Physics')
                    Insert Into Discipline (Name) Values('Policy and Management')
                    
                    Insert Into Discipline (Name) Values('Political Science Minor: Cybersecurity')
                    Insert Into Discipline (Name) Values('Pre-Law')
                    Insert Into Discipline (Name) Values('Privacy')
                    Insert Into Discipline (Name) Values('Professional Writing')
                    Insert Into Discipline (Name) Values('Public Health')
                    Insert Into Discipline (Name) Values('Quantum Information Sciences')
                    Insert Into Discipline (Name) Values('Robotics Engineering')
                    Insert Into Discipline (Name) Values('Science and Technology')
                    Insert Into Discipline (Name) Values('Secure and Dependable Systems')
                    Insert Into Discipline (Name) Values('Secure Computing')
                    Insert Into Discipline (Name) Values('Secure Computing and Networks')
                 
                    Insert Into Discipline (Name) Values('Security and Risk Analysis')
                    Insert Into Discipline (Name) Values('Security Assured Information Systems')
                    Insert Into Discipline (Name) Values('Security Management Cybersecurity')
                    Insert Into Discipline (Name) Values('Security Track')
                    Insert Into Discipline (Name) Values('Social Data Analytics')
                    
                    Insert Into Discipline (Name) Values('System Administration and Security')
                    Insert Into Discipline (Name) Values('Technical Cybersecurity')
                    Insert Into Discipline (Name) Values('Technology')

            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
