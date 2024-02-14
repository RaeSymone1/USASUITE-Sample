CREATE TABLE [dbo].[Institution] (
    [InstitutionID]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]              NCHAR (200)   NOT NULL,
    [IsActive]          BIT           NOT NULL,
    [City]              VARCHAR (200) NULL,
    [StateID]           INT           NULL,
    [PostalCode]        VARCHAR (10)  NULL,
    [HomePage]          VARCHAR (300) NULL,
    [ProgramPage]       VARCHAR (300) NULL,
    [InstitutionTypeID] INT           NULL,
    CONSTRAINT [PK_Institution] PRIMARY KEY CLUSTERED ([InstitutionID] ASC)
);

