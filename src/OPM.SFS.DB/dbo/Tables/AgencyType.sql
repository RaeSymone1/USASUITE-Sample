CREATE TABLE [dbo].[AgencyType] (
    [AgencyTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (250) NULL,
    [Code]         VARCHAR (50)  NULL,
    [DateInserted] DATETIME      CONSTRAINT [DF_AgencyType_DateInserted] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_AgencyType] PRIMARY KEY CLUSTERED ([AgencyTypeID] ASC)
);

