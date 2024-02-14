CREATE TABLE [dbo].[StudentSecurityCertification] (
    [SecurityCertificationID] INT NOT NULL,
    [StudentID]               INT NOT NULL,
    CONSTRAINT [PK_StudentSecurityCertification] PRIMARY KEY CLUSTERED ([SecurityCertificationID] ASC, [StudentID] ASC)
);

