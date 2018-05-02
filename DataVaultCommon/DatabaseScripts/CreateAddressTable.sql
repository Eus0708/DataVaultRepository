CREATE TABLE [dbo].[AddressTable] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Address1] NVARCHAR (50) NULL,
    [Address2] NVARCHAR (50) NULL,
    [City]     NVARCHAR (50) NULL,
    [State]    NVARCHAR(50)           NULL,
    [ZipCode]  NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

