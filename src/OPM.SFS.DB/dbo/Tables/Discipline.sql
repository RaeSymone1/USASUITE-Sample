CREATE TABLE [dbo].[Discipline] (
    [DisciplineID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (100) NULL,
    [Code]         VARCHAR (10)  NULL,
    [DateInserted] DATETIME      CONSTRAINT [DF_Discipline_DateInserted] DEFAULT (getdate()) NULL,
    [LastModified] DATETIME      NULL,
    CONSTRAINT [PK_Discipline] PRIMARY KEY CLUSTERED ([DisciplineID] ASC)
);

