CREATE TABLE [dbo].[AcademiaUser] (
    [AcademiaUserID] INT           IDENTITY (1, 1) NOT NULL,
    [UserName]       VARCHAR (50)  NULL,
    [Email]          VARCHAR (100) NULL,
    [Password]       VARCHAR (500) NULL,
    CONSTRAINT [PK_AcademiaUser] PRIMARY KEY CLUSTERED ([AcademiaUserID] ASC)
);

