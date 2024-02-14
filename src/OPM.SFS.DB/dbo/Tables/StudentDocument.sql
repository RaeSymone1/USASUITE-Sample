CREATE TABLE [dbo].[StudentDocument] (
    [StudentDocumentID] INT           IDENTITY (1, 1) NOT NULL,
    [StudentID]         INT           NOT NULL,
    [DocumentTypeID]    INT           NULL,
    [FileName]          VARCHAR (100) NULL,
    [FilePath]          VARCHAR (256) NULL,
    [DateCreated]       DATETIME      NULL,
    [IsDeleted]         BIT           NULL,
    [UserID]            VARCHAR (100) NULL,
    CONSTRAINT [PK_StudentDocument] PRIMARY KEY CLUSTERED ([StudentDocumentID] ASC)
);

