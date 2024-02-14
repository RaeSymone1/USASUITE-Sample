CREATE TABLE [dbo].[StudentJobActivity] (
    [StudentJobActivityID] INT            IDENTITY (1, 1) NOT NULL,
    [StudentID]            INT            NOT NULL,
    [AgencyID]             INT            NULL,
    [ContactID]            INT            NULL,
    [Description]          VARCHAR (1000) NULL,
    [CurrentStatus]        VARCHAR (1000) NULL,
    [DateApplied]          DATE           NULL,
    [PositionTitle]        VARCHAR (100)  NULL,
    [USAJOBSControlNumber] BIGINT         NULL,
    [DutyLocation]         VARCHAR (100)  NULL,
    CONSTRAINT [PK_StudentJobActivity] PRIMARY KEY CLUSTERED ([StudentJobActivityID] ASC)
);

