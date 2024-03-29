﻿CREATE TABLE [dbo].[Student] (
    [StudentID]           INT           IDENTITY (1, 1) NOT NULL,
    [UserName]            VARCHAR (120) NOT NULL,
    [Email]               VARCHAR (256) NOT NULL,
    [FirstName]           VARCHAR (20)  NOT NULL,
    [MiddleName]          VARCHAR (20)  NULL,
    [LastName]            VARCHAR (20)  NULL,
    [Password]            VARCHAR (256) NOT NULL,
    [IsDisabled]          BIT           NOT NULL,
    [FailedLoginCount]    INT           NOT NULL,
    [FailedLoginDate]     DATETIME      NULL,
    [LastLoginDate]       DATETIME2 (7) NULL,
    [MotherMaidenName]    VARCHAR (255) NOT NULL,
    [CurrentAddressID]    INT           NOT NULL,
    [PermanentAddressID]  INT           NULL,
    [SSN]                 VARCHAR (50)  NOT NULL,
    [AlternateEmail]      VARCHAR (256) NULL,
    [EmergencyContactID]  INT           NULL,
    [InstitutionID]       INT           NULL,
    [DisciplineID]        INT           NULL,
    [DegreeID]            INT           NULL,
    [ExpectedGradDate]    DATETIME2 (7) NULL,
    [InternshipAvailDate] DATETIME      NULL,
    [PostGradAvailDate]   DATETIME      NULL,
    [UserID]              VARCHAR (100) NULL,
    [UserIP]              VARCHAR (50)  NULL,
    [PasswordExpiration]  DATE          NULL,
    CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED ([StudentID] ASC),
    CONSTRAINT [FK_Student_AddressCurrent] FOREIGN KEY ([CurrentAddressID]) REFERENCES [dbo].[Address] ([AddressID]),
    CONSTRAINT [FK_Student_AddressPerm] FOREIGN KEY ([PermanentAddressID]) REFERENCES [dbo].[Address] ([AddressID]),
    CONSTRAINT [FK_Student_Contact] FOREIGN KEY ([EmergencyContactID]) REFERENCES [dbo].[Contact] ([ContactID]),
    CONSTRAINT [FK_Student_Degree] FOREIGN KEY ([DegreeID]) REFERENCES [dbo].[Degree] ([DegreeID]),
    CONSTRAINT [FK_Student_Discipline] FOREIGN KEY ([DisciplineID]) REFERENCES [dbo].[Discipline] ([DisciplineID]),
    CONSTRAINT [FK_Student_Institution] FOREIGN KEY ([InstitutionID]) REFERENCES [dbo].[Institution] ([InstitutionID])
);

