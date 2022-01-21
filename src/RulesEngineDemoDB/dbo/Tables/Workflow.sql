CREATE TABLE [dbo].[Workflow] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [WorkflowName]      VARCHAR (255)  NOT NULL,
    [Seq]               INT            NOT NULL,
    [WorkflowsToInject] VARCHAR (1024) NULL,
    CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED ([Id] ASC)
);

