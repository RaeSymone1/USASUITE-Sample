CREATE TABLE [dbo].[Address] (
    [AddressID]  INT           IDENTITY (1, 1) NOT NULL,
    [LineOne]    VARCHAR (100) NOT NULL,
    [LineTwo]    VARCHAR (100) NULL,
    [City]       VARCHAR (60)  NOT NULL,
    [StateID]    INT           NOT NULL,
    [PostalCode] VARCHAR (20)  NULL,
    [Country]    VARCHAR (100) NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED ([AddressID] ASC),
    CONSTRAINT [FK_Address_State] FOREIGN KEY ([StateID]) REFERENCES [dbo].[State] ([StateID])
);

