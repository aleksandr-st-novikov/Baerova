CREATE TABLE [dbo].[MailArticles] (
    [Id] [uniqueidentifier] NOT NULL,
    [ArticleId] [uniqueidentifier] NOT NULL,
    [DateMailing] [datetime] NOT NULL,
    [CountRecipient] [int] NOT NULL,
    CONSTRAINT [PK_dbo.MailArticles] PRIMARY KEY ([Id])
)