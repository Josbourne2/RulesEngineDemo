CREATE TABLE [dbo].[Rule] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [Seq]                INT            NOT NULL,
    [RuleDataId]         INT            NULL,
    [WorkflowDataId]     INT            NULL,
    [RuleName]           VARCHAR (255)  NOT NULL,
    [Properties]         VARCHAR (1024) NULL,
    [Operator]           VARCHAR (25)   NULL,
    [Enabled]            BIT            NOT NULL,
    [ErrorType]          INT            NOT NULL,
    [RuleExpressionType] INT            NOT NULL,
    [WorkflowsToInject]  VARCHAR (1024) NULL,
    [Expression]         VARCHAR (4000) NULL,
    [Actions]            VARCHAR (4000) NULL,
    [SuccessEvent]       VARCHAR (255)  NULL,
    CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Rule_Rule_RuleDataId] FOREIGN KEY ([RuleDataId]) REFERENCES [dbo].[Rule] ([Id]),
    CONSTRAINT [FK_Rule_Workflow_WorkflowDataId] FOREIGN KEY ([WorkflowDataId]) REFERENCES [dbo].[Workflow] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Rule_WorkflowDataId]
    ON [dbo].[Rule]([WorkflowDataId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Rule_RuleDataId]
    ON [dbo].[Rule]([RuleDataId] ASC);

