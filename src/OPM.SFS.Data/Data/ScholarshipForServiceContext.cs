using System;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OPM.SFS.Core.Data;
using OPM.SFS.Data;

#nullable disable

namespace OPM.SFS.Data
{
    public partial class ScholarshipForServiceContext : DbContext, IDataProtectionKeyContext
    {
        public ScholarshipForServiceContext()
        {
        }
        public ScholarshipForServiceContext(DbContextOptions<ScholarshipForServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademiaUser> AcademiaUsers { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressAgencyMapping> AddressAgencyMappings { get; set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<AgencyType> AgencyTypes { get; set; }
        public virtual DbSet<AgencyUser> AgencyUsers { get; set; }
        public virtual DbSet<ApplicationEventLog> ApplicationEventLogs { get; set; }
        public virtual DbSet<CommitmentType> CommitmentTypes { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Degree> Degrees { get; set; }
        public virtual DbSet<Discipline> Disciplines { get; set; }    
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }
        public virtual DbSet<InstitutionType> InstitutionTypes { get; set; }
        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<SalaryType> SalaryTypes { get; set; }
        public virtual DbSet<SecurityCertification> SecurityCertifications { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentBuilderResume> StudentBuilderResumes { get; set; }
        public virtual DbSet<StudentCommitment> StudentCommitments { get; set; }
        public virtual DbSet<StudentDocument> StudentDocuments { get; set; }
        public virtual DbSet<StudentJobActivity> StudentJobActivities { get; set; }
        public virtual DbSet<StudentRace> StudentRaces { get; set; }
        public virtual DbSet<StudentSecurityCertification> StudentSecurityCertifications { get; set; }
        public virtual DbSet<StatusOption> StatusOption { get; set; }
        public virtual DbSet<WorkExperience> WorkExperiences { get; set; }
        public virtual DbSet<StudentAccountPasswordHistory> StudentAccountUsedPasswords { get; set; }
        public virtual DbSet<AgencyUserPasswordHistory> AgencyUserPasswordHistories { get; set; }
        public virtual DbSet<AdminUserPasswordHistory> AdminUserPasswordHistories { get; set; }
        public virtual DbSet<AcademiaUserPasswordHistory> AcademiaUserPasswordHistories { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<StudentAccount> StudentAccount { get; set; }
        public virtual DbSet<RegistrationCode> RegistrationCodes { get; set; }
        public virtual DbSet<SchoolType> SchoolType { get; set; }
        public virtual DbSet<Citizenship> Citizenship { get; set; }
        public virtual DbSet<AgencyUserRole> AgencyUserRoles { get; set; }
        public virtual DbSet<AcademiaUserRole> AcademiaUserRoles { get; set; }
        public virtual DbSet<AdminUserRole> AdminUserRoles { get; set; }
        public virtual DbSet<JobSearchType> JobSearchTypes { get; set; }
        public virtual DbSet<CommitmentStatus> CommitmentStatus { get; set; }
        public virtual DbSet<CommitmentApprovalWorkflow> CommitmentApprovalWorkflow { get; set; }
        public virtual DbSet<CommitmentStudentDocument> CommitmentStudentDocument { get; set; }
        public virtual DbSet<ProfileStatus> ProfileStatus { get; set; }
        public virtual DbSet<CertificateStaging> CertificateStaging { get; set; }
        public virtual DbSet<GlobalConfiguration> GlobalConfiguration { get; set; }
        public virtual DbSet<EmailSentLog> EmailSentLog { get; set; }
        public virtual DbSet<JobActivityStatus> JobActivityStatus { get; set; }
        public virtual DbSet<AcademicSchedule> AcademicSchedule { get; set; }
        public virtual DbSet<InstitutionContact> InstitutionContact { get; set; }
        public virtual DbSet<InstitutionContactType> InstitutionContactType { get; set; }
        public virtual DbSet<AuditEventLog> AuditEventLog { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
        public virtual DbSet<LoginGovStaging> LoginGovStaging { get; set; }       
        public virtual DbSet<StudentInstitutionFunding> StudentInstitutionFundings { get; set; }
        public virtual DbSet<ExtensionType> ExtensionType { get; set; }
        public virtual DbSet<SessionList> SessionList { get; set; }
        public virtual DbSet<ProgramYear> ProgramYear { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
		public virtual DbSet<FollowUpTypeOption> FollowUpTypeOption { get; set; }
        public virtual DbSet<Feature> Feature { get; set; }
        public virtual DbSet<StudentFeature> StudentFeature { get; set; }
        public virtual DbSet<AdminFeature> AdminFeature { get; set; }
        public virtual DbSet<AcademiaUserFeature> AcademiaUserFeature { get; set; }
        public virtual DbSet<AgencyUserFeature> AgencyUserFeature { get; set; }
        public virtual DbSet<StudentDashboardLog> StudentDashboardLog { get; set; }
		public virtual DbSet<EmploymentVerification> EmploymentVerification { get; set; }
		public virtual DbSet<EmailQueue> EmailQueue { get; set; }
        public virtual DbSet<ScheduledTask> ScheduledTask { get; set; }
        public virtual DbSet<DocumentScanQueue> DocumentScanQueue { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScholarshipForService");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AcademiaUser>(entity =>
            {
                entity.ToTable("AcademiaUser");

                entity.Property(e => e.AcademiaUserId).HasColumnName("AcademiaUserID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LineOne)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LineTwo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_State");
            });

            modelBuilder.Entity<AddressAgencyMapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AddressAgencyMapping");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AgencyId).HasColumnName("AgencyID");
            });

            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.ToTable("AdminUser");

                entity.Property(e => e.AdminUserId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("AdminUserID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Agency>(entity =>
            {
                entity.ToTable("Agency");

                entity.Property(e => e.AgencyId).HasColumnName("AgencyID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AgencyTypeId).HasColumnName("AgencyTypeID");

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModified).HasColumnType("datetime");
                               
                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.ParentAgencyId).HasColumnName("ParentAgencyID");

                entity.HasOne(d => d.AgencyType)
                    .WithMany(p => p.Agencies)
                    .HasForeignKey(d => d.AgencyTypeId)
                    .HasConstraintName("FK_Agency_AgencyType");
            });

            modelBuilder.Entity<AgencyType>(entity =>
            {
                entity.ToTable("AgencyType");

                entity.Property(e => e.AgencyTypeId).HasColumnName("AgencyTypeID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AgencyUser>(entity =>
            {
                entity.ToTable("AgencyUser");

                entity.Property(e => e.AgencyUserId).HasColumnName("AgencyUserID");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ApplicationEventLog>(entity =>
            {
                entity.ToTable("ApplicationEventLog");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<CommitmentType>(entity =>
            {
                entity.ToTable("CommitmentType");

                entity.Property(e => e.CommitmentTypeId).HasColumnName("CommitmentTypeID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneExt)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Degree>(entity =>
            {
                entity.ToTable("Degree");

                entity.Property(e => e.DegreeId).HasColumnName("DegreeID");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Discipline>(entity =>
            {
                entity.ToTable("Discipline");

                entity.Property(e => e.DisciplineId).HasColumnName("DisciplineID");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.ToTable("DocumentType");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.ToTable("Education");

                entity.Property(e => e.EducationId).HasColumnName("EducationID");

                entity.Property(e => e.CityName)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Country).HasColumnName("Country");

                entity.Property(e => e.CreditType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Degree).HasColumnName("Degree");

                entity.Property(e => e.DegreeOther)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gpa)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("GPA");

                entity.Property(e => e.Gpamax)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("GPAMax");

                entity.Property(e => e.Honors)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Major)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Minor)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");
                               

                entity.Property(e => e.TotalCredits)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<Ethnicity>(entity =>
            {
                entity.ToTable("Ethnicity");

                entity.Property(e => e.EthnicityId).HasColumnName("EthnicityID");

                entity.Property(e => e.EthnicityName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.GenderId).HasColumnName("GenderID");

                entity.Property(e => e.GenderName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Institution>(entity =>
            {
                entity.ToTable("Institution");

                entity.Property(e => e.InstitutionId).HasColumnName("InstitutionID");

                entity.Property(e => e.City)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HomePage)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.InstitutionTypeId).HasColumnName("InstitutionTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsFixedLength(true);

                entity.Property(e => e.ParentInstitutionID).HasColumnName("ParentInstitutionID");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramPage)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.HasOne(d => d.InstitutionType)
                    .WithMany(p => p.Institutions)
                    .HasForeignKey(d => d.InstitutionTypeId)
                    .HasConstraintName("FK_Institution_InstitutionType");

            });

            modelBuilder.Entity<InstitutionType>(entity =>
            {
                entity.ToTable("InstitutionType");

                entity.Property(e => e.InstitutionTypeId).HasColumnName("InstitutionTypeID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("Race");

                entity.Property(e => e.RaceId).HasColumnName("RaceID");

                entity.Property(e => e.RaceName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalaryType>(entity =>
            {
                entity.ToTable("SalaryType");

                entity.Property(e => e.SalaryTypeId).HasColumnName("SalaryTypeID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SecurityCertification>(entity =>
            {
                entity.ToTable("SecurityCertification");

                entity.Property(e => e.SecurityCertificationId).HasColumnName("SecurityCertificationID");

                entity.Property(e => e.SecurityCertificationCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityCertificationName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("State");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.Abbreviation)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DateInserted)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.AlternateEmail)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.CitizenStatusId).HasColumnName("CitizenStatusID");

                entity.Property(e => e.CurrentAddressId).HasColumnName("CurrentAddressID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.EmergencyContactId).HasColumnName("EmergencyContactID");

                entity.Property(e => e.EthnicityId).HasColumnName("EthnicityID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GenderId).HasColumnName("GenderID");
                
                entity.Property(e => e.LastName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PermanentAddressId).HasColumnName("PermanentAddressID");                

                entity.Property(e => e.Ssn)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SSN");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.Property(e => e.UserIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserIP");               

                entity.HasOne(d => d.CurrentAddress)
                    .WithMany(p => p.StudentCurrentAddresses)
                    .HasForeignKey(d => d.CurrentAddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_AddressCurrent");

                entity.HasOne(d => d.EmergencyContact)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.EmergencyContactId)
                    .HasConstraintName("FK_Student_Contact");

                entity.HasOne(d => d.Ethnicity)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.EthnicityId)
                    .HasConstraintName("FK_Student_Ethnicity");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Student_Gender");

                entity.HasOne(d => d.PermanentAddress)
                    .WithMany(p => p.StudentPermanentAddresses)
                    .HasForeignKey(d => d.PermanentAddressId)
                    .HasConstraintName("FK_Student_AddressPerm");
            });

            modelBuilder.Entity<StudentBuilderResume>(entity =>
            {
                entity.ToTable("StudentBuilderResume");

                entity.Property(e => e.StudentBuilderResumeId).HasColumnName("StudentBuilderResumeID");

                entity.Property(e => e.Certificate)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.HonorsAwards)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.JobRelatedSkill)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Objective)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.OtherQualification)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.Supplemental)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentBuilderResumes)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentBuilderResume_Student");
            });

            modelBuilder.Entity<StudentCommitment>(entity =>
            {
                entity.ToTable("StudentCommitment");

                entity.Property(e => e.StudentCommitmentId).HasColumnName("StudentCommitmentID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.AgencyId).HasColumnName("AgencyID");

                entity.Property(e => e.CommitmentTypeId).HasColumnName("CommitmentTypeID");

                entity.Property(e => e.EndDate).HasColumnType("date");

                
                entity.Property(e => e.Grade)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.JobSearchTypeId).HasColumnName("JobSearchTypeID");

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Justification)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.MentorContactId).HasColumnName("MentorContactID");

                entity.Property(e => e.PayPlan)
                    .HasMaxLength(3)
                    .IsUnicode(false);               

                entity.Property(e => e.SalaryMaximum).HasColumnType("money");

                entity.Property(e => e.SalaryMinimum).HasColumnType("money");

                entity.Property(e => e.SalaryTypeId).HasColumnName("SalaryTypeID");

                entity.Property(e => e.Series)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.SupervisorContactId).HasColumnName("SupervisorContactID");


                entity.HasOne(d => d.Address)
                    .WithMany(p => p.StudentCommitments)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK_StudentCommitment_Address");

                entity.HasOne(d => d.Agency)
                    .WithMany(p => p.StudentCommitments)
                    .HasForeignKey(d => d.AgencyId)
                    .HasConstraintName("FK_StudentCommitment_Agency");

                entity.HasOne(d => d.CommitmentType)
                    .WithMany(p => p.StudentCommitments)
                    .HasForeignKey(d => d.CommitmentTypeId)
                    .HasConstraintName("FK_StudentCommitment_CommitmentType");               

                entity.HasOne(d => d.MentorContact)
                    .WithMany(p => p.StudentCommitmentMentorContacts)
                    .HasForeignKey(d => d.MentorContactId)
                    .HasConstraintName("FK_StudentCommitment_ContactMentor");                

                entity.HasOne(d => d.SalaryType)
                    .WithMany(p => p.StudentCommitments)
                    .HasForeignKey(d => d.SalaryTypeId)
                    .HasConstraintName("FK_StudentCommitment_SalaryType");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentCommitments)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentCommitment_Student");

                entity.HasOne(d => d.SupervisorContact)
                    .WithMany(p => p.StudentCommitmentSupervisorContacts)
                    .HasForeignKey(d => d.SupervisorContactId)
                    .HasConstraintName("FK_StudentCommitment_ContactSupervisor");
              
            });

            modelBuilder.Entity<StudentDocument>(entity =>
            {
                entity.ToTable("StudentDocument");

                entity.Property(e => e.StudentDocumentId).HasColumnName("StudentDocumentID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");

                entity.Property(e => e.FileName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<StudentJobActivity>(entity =>
            {
                entity.ToTable("StudentJobActivity");

                entity.Property(e => e.StudentJobActivityId).HasColumnName("StudentJobActivityID");
                               
                entity.Property(e => e.StatusOther)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.DateApplied).HasColumnType("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.DutyLocation)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PositionTitle)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.Property(e => e.UsajobscontrolNumber).HasColumnName("USAJOBSControlNumber");
            });

            modelBuilder.Entity<StudentRace>(entity =>
            {
                entity.HasKey(e => e.StudentRaceD);

                entity.ToTable("StudentRace");

                entity.Property(e => e.RaceId).HasColumnName("RaceID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.StudentRaces)
                    .HasForeignKey(d => d.RaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentRace_Race");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentRaces)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentRace_Student");
            });

            modelBuilder.Entity<StudentSecurityCertification>(entity =>
            {
                entity.HasKey(e => new { e.SecurityCertificationId, e.StudentId });

                entity.ToTable("StudentSecurityCertification");

                entity.Property(e => e.SecurityCertificationId).HasColumnName("SecurityCertificationID");

                entity.Property(e => e.StudentId).HasColumnName("StudentID");
            });

            modelBuilder.Entity<StatusOption>(entity =>
            {
                entity.HasKey(e => e.StudentStatusId);

                entity.Property(e => e.StudentStatusId).HasColumnName("StudentStatusID");

                entity.Property(e => e.Phase)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Option)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WorkExperience>(entity =>
            {
                entity.ToTable("WorkExperience");

                entity.Property(e => e.WorkExperienceId).HasColumnName("WorkExperienceID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Duties)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Employer)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.End).HasMaxLength(10);

                entity.Property(e => e.Grade)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PayPlan)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Salary).HasMaxLength(10);

                entity.Property(e => e.Series)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Start).HasMaxLength(10);
               

                entity.Property(e => e.SupervisorName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SupervisorPhone)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.SupervisorPhoneExt)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("UserID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
