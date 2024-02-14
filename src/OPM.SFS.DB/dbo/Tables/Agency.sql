CREATE TABLE [dbo].[Agency] (
    [AgencyID]                  INT           IDENTITY (1, 1) NOT NULL,
    [AgencyTypeID]              INT           NULL,
    [ParentAgencyID]            INT           NULL,
    [Name]                      VARCHAR (512) NULL,
    [AddressID]                 INT           NULL,
    [RequirePayPlanSeriesGrade] BIT           NULL,
    [RequireSmartCardAuth]      BIT           NULL,
    [IsDisabled]                BIT           NULL,
    [MigrationID]               INT           NULL,
    [DateInserted]              DATETIME      CONSTRAINT [DF_Agency_DateInserted] DEFAULT (getdate()) NULL,
    [LastModified]              DATETIME      NULL,
    [ModifiedBy]                INT           NULL,
    CONSTRAINT [PK_Agency] PRIMARY KEY CLUSTERED ([AgencyID] ASC),
    CONSTRAINT [FK_Agency_AgencyType] FOREIGN KEY ([AgencyTypeID]) REFERENCES [dbo].[AgencyType] ([AgencyTypeID])
);

