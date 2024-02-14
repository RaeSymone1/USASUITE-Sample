CREATE TABLE [dbo].[SecurityCertification] (
    [SecurityCertificationID]   INT           IDENTITY (1, 1) NOT NULL,
    [SecurityCertificationCode] VARCHAR (10)  NOT NULL,
    [SecurityCertificationName] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_SecurityCertification] PRIMARY KEY CLUSTERED ([SecurityCertificationID] ASC)
);

