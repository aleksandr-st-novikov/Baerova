ALTER TABLE [dbo].[Articles] ADD [Category] [nvarchar](max)
ALTER TABLE [dbo].[Articles] ADD [IsFixed] [bit] NOT NULL DEFAULT 0

update articles set category='main', IsFixed=0