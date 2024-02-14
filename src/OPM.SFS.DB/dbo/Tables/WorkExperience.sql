CREATE TABLE [dbo].[WorkExperience] (
    [WorkExperienceID]   INT            IDENTITY (1, 1) NOT NULL,
    [StudentID]          INT            NULL,
    [AddressID]          INT            NULL,
    [Employer]           VARCHAR (100)  NULL,
    [StartDate]          DATE           NULL,
    [EndDate]            DATE           NULL,
    [Title]              VARCHAR (100)  NULL,
    [Series]             VARCHAR (5)    NULL,
    [PayPlan]            VARCHAR (3)    NULL,
    [Grade]              VARCHAR (5)    NULL,
    [Salary]             MONEY          NULL,
    [HoursPerWeek]       INT            NULL,
    [SupervisorName]     VARCHAR (100)  NULL,
    [SupervisorPhone]    VARCHAR (30)   NULL,
    [SupervisorPhoneExt] VARCHAR (10)   NULL,
    [Duties]             VARCHAR (1000) NULL,
    [UserID]             VARCHAR (100)  NULL,
    CONSTRAINT [PK_WorkExperience] PRIMARY KEY CLUSTERED ([WorkExperienceID] ASC)
);

