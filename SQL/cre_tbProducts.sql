/****** Object:  Table [dbo].[Products]    Script Date: 18-3-2023 13:15:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Products](
	[ID] [uniqueidentifier] NOT NULL,
	[SKU] [varchar](20) NOT NULL,
	[Name] [varchar](100) NULL,
	[Price] [decimal](18, 2) NULL,
	[ChangeDate] [datetime] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Products-SKU]    Script Date: 18-3-2023 13:15:30 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = N'IX_Products-SKU')
CREATE UNIQUE NONCLUSTERED INDEX [IX_Products-SKU] ON [dbo].[Products]
(
	[SKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Products_ID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_ID]  DEFAULT (newid()) FOR [ID]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Products_Price]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_Price]  DEFAULT ((0)) FOR [Price]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Products_ChangeDate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_ChangeDate]  DEFAULT (getdate()) FOR [ChangeDate]
END
GO

/****** Object:  Trigger [dbo].[TR_Products_U]    Script Date: 18-3-2023 13:15:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TR_Products_U]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[TR_Products_U] ON [dbo].[Products] FOR UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	update Products
	set ChangeDate = getdate()
	from Products P inner join Inserted I on P.ID=I.ID
END
' 
GO
ALTER TABLE [dbo].[Products] ENABLE TRIGGER [TR_Products_U]
GO
