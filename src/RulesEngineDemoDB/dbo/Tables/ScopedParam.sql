CREATE TABLE [dbo].[ScopedParam] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Seq]            INT            NOT NULL,
    [RuleDataId]     INT            NULL,
    [WorkflowDataId] INT            NULL,
    [Name]           VARCHAR (255)  NOT NULL,
    [Expression]     VARCHAR (4000) NOT NULL,
    CONSTRAINT [PK_ScopedParam] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScopedParam_Rule_RuleDataId] FOREIGN KEY ([RuleDataId]) REFERENCES [dbo].[Rule] ([Id]),
    CONSTRAINT [FK_ScopedParam_Workflow_WorkflowDataId] FOREIGN KEY ([WorkflowDataId]) REFERENCES [dbo].[Workflow] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ScopedParam_WorkflowDataId]
    ON [dbo].[ScopedParam]([WorkflowDataId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ScopedParam_RuleDataId]
    ON [dbo].[ScopedParam]([RuleDataId] ASC);

