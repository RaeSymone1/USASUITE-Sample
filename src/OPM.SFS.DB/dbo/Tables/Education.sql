CREATE TABLE [dbo].[Education] (
    [EducationID]       INT           IDENTITY (1, 1) NOT NULL,
    [StudentID]         INT           NOT NULL,
    [InstitutionTypeID] INT           NOT NULL,
    [CityName]          VARCHAR (60)  NULL,
    [StateID]           INT           NULL,
    [PostalCode]        VARCHAR (20)  NULL,
    [CountryID]         INT           NULL,
    [DegreeID]          INT           NULL,
    [DegreeOther]       VARCHAR (100) NULL,
    [CompletionYear]    INT           NULL,
    [GPA]               VARCHAR (10)  NULL,
    [GPAMax]            VARCHAR (10)  NULL,
    [TotalCredits]      VARCHAR (60)  NULL,
    [CreditType]        VARCHAR (100) NULL,
    [Major]             VARCHAR (100) NULL,
    [Minor]             VARCHAR (100) NULL,
    [Honors]            VARCHAR (100) NULL,
    [UserID]            VARCHAR (100) NULL,
    CONSTRAINT [PK_Education] PRIMARY KEY CLUSTERED ([EducationID] ASC)
);

