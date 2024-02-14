CREATE TABLE [dbo].[Contact] (
    [ContactID] INT           IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (20)  NULL,
    [LastName]  VARCHAR (20)  NULL,
    [Email]     VARCHAR (256) NULL,
    [Phone]     VARCHAR (20)  NULL,
    [PhoneExt]  VARCHAR (20)  NULL,
    CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED ([ContactID] ASC)
);

