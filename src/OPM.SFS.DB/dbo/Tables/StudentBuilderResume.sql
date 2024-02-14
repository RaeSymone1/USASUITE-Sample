CREATE TABLE [dbo].[StudentBuilderResume] (
    [StudentBuilderResumeID] INT            IDENTITY (1, 1) NOT NULL,
    [StudentID]              INT            NOT NULL,
    [Objective]              VARCHAR (1000) NULL,
    [OtherQualification]     VARCHAR (1000) NULL,
    [JobRelatedSkill]        VARCHAR (1000) NULL,
    [Certificate]            VARCHAR (1000) NULL,
    [HonorsAwards]           VARCHAR (1000) NULL,
    [Supplemental]           VARCHAR (1000) NULL,
    CONSTRAINT [PK_StudentResume] PRIMARY KEY CLUSTERED ([StudentBuilderResumeID] ASC),
    CONSTRAINT [FK_StudentBuilderResume_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Student] ([StudentID])
);

