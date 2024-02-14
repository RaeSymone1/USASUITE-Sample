CREATE TABLE [dbo].[AgencyUser] (
    [AgencyUserID] INT           IDENTITY (1, 1) NOT NULL,
    [UserName]     VARCHAR (50)  NULL,
    [Email]        VARCHAR (100) NULL,
    [Password]     VARCHAR (500) NULL,
    CONSTRAINT [PK_AgencyUser] PRIMARY KEY CLUSTERED ([AgencyUserID] ASC)
);

