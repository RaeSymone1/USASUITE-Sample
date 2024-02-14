CREATE TABLE [dbo].[Country] (
    [CountryID]    INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NULL,
    [Abbreviation] VARCHAR (5)  NULL,
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([CountryID] ASC)
);

