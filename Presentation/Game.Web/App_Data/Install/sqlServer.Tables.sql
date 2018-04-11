CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerGuid] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](1000) NULL,
	[Email] [nvarchar](1000) NULL,
	[EmailToRevalidate] [nvarchar](1000) NULL,
	[AdminComment] [nvarchar](max) NULL,
	[RequireReLogin] [bit] NOT NULL,
	[FailedLoginAttempts] [int] NOT NULL,
	[CannotLoginUntilDateUtc] [datetime] NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[IsSystemAccount] [bit] NOT NULL,
	[SystemName] [nvarchar](400) NULL,
	[LastIpAddress] [nvarchar](max) NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[LastLoginDateUtc] [datetime] NULL,
	[LastActivityDateUtc] [datetime] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[CustomerPassword](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Password] [nvarchar](max) NULL,
	[PasswordFormatId] [int] NOT NULL,
	[PasswordSalt] [nvarchar](max) NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[CustomerRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[FreeShipping] [bit] NOT NULL,
	[TaxExempt] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[IsSystemRole] [bit] NOT NULL,
	[SystemName] [nvarchar](255) NULL,
	[EnablePasswordLifetime] [bit] NOT NULL,
	[PurchasedWithProductId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Customer_CustomerRole_Mapping](
	[CustomerId] [int] NOT NULL,
	[CustomerRoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[CustomerRoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[GenericAttribute](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[KeyGroup] [nvarchar](400) NOT NULL,
	[Key] [nvarchar](400) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[StoreId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LogLevelId] [int] NOT NULL,
	[ShortMessage] [nvarchar](max) NOT NULL,
	[FullMessage] [nvarchar](max) NULL,
	[IpAddress] [nvarchar](200) NULL,
	[CustomerId] [int] NULL,
	[PageUrl] [nvarchar](max) NULL,
	[ReferrerUrl] [nvarchar](max) NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Setting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Value] [nvarchar](2000) NOT NULL,
	[StoreId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Language](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LanguageCulture] [nvarchar](20) NOT NULL,
	[UniqueSeoCode] [nvarchar](2) NULL,
	[FlagImageFileName] [nvarchar](50) NULL,
	[Rtl] [bit] NOT NULL,
	[LimitedToStores] [bit] NOT NULL,
	[DefaultCurrencyId] [int] NOT NULL,
	[Published] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LocaleStringResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LanguageId] [int] NOT NULL,
	[ResourceName] [nvarchar](200) NOT NULL,
	[ResourceValue] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LocalizedProperty](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[LanguageId] [int] NOT NULL,
	[LocaleKeyGroup] [nvarchar](400) NOT NULL,
	[LocaleKey] [nvarchar](400) NOT NULL,
	[LocaleValue] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ActivityLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ActivityLogTypeId] [int] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[IpAddress] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ActivityLogType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SystemKeyword] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Enabled] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PermissionRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[SystemName] [nvarchar](255) NOT NULL,
	[Category] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ScheduleTask](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Seconds] [int] NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[StopOnError] [bit] NOT NULL,
	[LeasedByMachineName] [nvarchar](max) NULL,
	[LeasedUntilUtc] [datetime] NULL,
	[LastStartUtc] [datetime] NULL,
	[LastEndUtc] [datetime] NULL,
	[LastSuccessUtc] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[UrlRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EntityId] [int] NOT NULL,
	[EntityName] [nvarchar](400) NOT NULL,
	[Slug] [nvarchar](400) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[LanguageId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

















ALTER TABLE [dbo].[CustomerPassword]  WITH CHECK ADD  CONSTRAINT [CustomerPassword_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CustomerPassword] CHECK CONSTRAINT [CustomerPassword_Customer]
GO

ALTER TABLE [dbo].[Customer_CustomerRole_Mapping]  WITH CHECK ADD  CONSTRAINT [Customer_CustomerRoles_Source] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Customer_CustomerRole_Mapping] CHECK CONSTRAINT [Customer_CustomerRoles_Source]
GO

ALTER TABLE [dbo].[Customer_CustomerRole_Mapping]  WITH CHECK ADD  CONSTRAINT [Customer_CustomerRoles_Target] FOREIGN KEY([CustomerRoleId])
REFERENCES [dbo].[CustomerRole] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Customer_CustomerRole_Mapping] CHECK CONSTRAINT [Customer_CustomerRoles_Target]
GO

ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [Log_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [Log_Customer]
GO

ALTER TABLE [dbo].[LocaleStringResource]  WITH CHECK ADD  CONSTRAINT [LocaleStringResource_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LocaleStringResource] CHECK CONSTRAINT [LocaleStringResource_Language]
GO

ALTER TABLE [dbo].[LocalizedProperty]  WITH CHECK ADD  CONSTRAINT [LocalizedProperty_Language] FOREIGN KEY([LanguageId])
REFERENCES [dbo].[Language] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[LocalizedProperty] CHECK CONSTRAINT [LocalizedProperty_Language]
GO

ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [ActivityLog_ActivityLogType] FOREIGN KEY([ActivityLogTypeId])
REFERENCES [dbo].[ActivityLogType] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [ActivityLog_ActivityLogType]
GO

ALTER TABLE [dbo].[ActivityLog]  WITH CHECK ADD  CONSTRAINT [ActivityLog_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ActivityLog] CHECK CONSTRAINT [ActivityLog_Customer]
GO
