CREATE TABLE [dbo].[State] (
    [StateID]      INT          IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (50) NULL,
    [Abbreviation] VARCHAR (5)  NULL,
    [DateInserted] DATETIME     CONSTRAINT [DF_State_DateInserted] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED ([StateID] ASC)
);

