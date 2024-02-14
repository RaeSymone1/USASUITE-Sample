CREATE TABLE [dbo].[SalaryType] (
    [SalaryTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (100) NOT NULL,
    [Code]         VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_SalaryType] PRIMARY KEY CLUSTERED ([SalaryTypeID] ASC)
);

