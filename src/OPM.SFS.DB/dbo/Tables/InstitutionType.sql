CREATE TABLE [dbo].[InstitutionType] (
    [InstitutionTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_InstitutionType] PRIMARY KEY CLUSTERED ([InstitutionTypeID] ASC)
);

