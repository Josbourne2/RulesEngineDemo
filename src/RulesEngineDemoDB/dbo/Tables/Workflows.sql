CREATE TABLE [dbo].[Workflows] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (255)  NOT NULL,
    [Workflow]               VARCHAR(8000)            NOT NULL
    CONSTRAINT [PK_Workflows] PRIMARY KEY CLUSTERED ([Id] ASC)
);

