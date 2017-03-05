CREATE TABLE [dbo].[BannerLinks] (
    [Id] [uniqueidentifier] NOT NULL,
    [NumSlide] [int] NOT NULL,
    [Link] [nvarchar](500),
    CONSTRAINT [PK_dbo.BannerLinks] PRIMARY KEY ([Id])
)