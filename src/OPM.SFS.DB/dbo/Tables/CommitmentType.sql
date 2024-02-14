CREATE TABLE [dbo].[CommitmentType] (
    [CommitmentTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (100) NOT NULL,
    [Code]             VARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_CommitmentType] PRIMARY KEY CLUSTERED ([CommitmentTypeID] ASC)
);

