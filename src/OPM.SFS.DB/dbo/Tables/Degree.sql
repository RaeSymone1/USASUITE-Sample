CREATE TABLE [dbo].[Degree] (
    [DegreeID]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (150) NOT NULL,
    [Code]         VARCHAR (10)  NULL,
    [DateInserted] DATETIME      CONSTRAINT [DF_Degree_DateInserted] DEFAULT (getdate()) NULL,
    [LastModified] DATETIME      NULL,
    CONSTRAINT [PK_Degree] PRIMARY KEY CLUSTERED ([DegreeID] ASC)
);

