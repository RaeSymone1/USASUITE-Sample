CREATE TABLE [dbo].[AdminUser] (
    [AdminUserID] INT           NOT NULL,
    [UserName]    VARCHAR (50)  NULL,
    [Email]       VARCHAR (100) NULL,
    [Password]    VARCHAR (100) NULL,
    CONSTRAINT [PK_AdminUser] PRIMARY KEY CLUSTERED ([AdminUserID] ASC)
);

