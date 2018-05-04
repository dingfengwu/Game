CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerGuid] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](1000) NULL,
	[Email] [nvarchar](1000) NULL,
	[PhoneNumber] [nvarchar](1000) NULL,
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



CREATE TABLE [dbo].[CustomerAttribute](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](400) NOT NULL,
	[IsRequired] [BIT] NOT NULL,
	[AttributeControlTypeId] [INT] NOT NULL,
	[DisplayOrder] [INT] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[CustomerAttributeValue](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[CustomerAttributeId] [INT] NOT NULL,
	[Name] [NVARCHAR](400) NOT NULL,
	[IsPreSelected] [BIT] NOT NULL,
	[DisplayOrder] [INT] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CustomerAttributeValue]  WITH CHECK ADD  CONSTRAINT [CustomerAttributeValue_CustomerAttribute] FOREIGN KEY([CustomerAttributeId])
REFERENCES [dbo].[CustomerAttribute] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CustomerAttributeValue] CHECK CONSTRAINT [CustomerAttributeValue_CustomerAttribute]
GO



CREATE TABLE [dbo].[Order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderGuid] [uniqueidentifier] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[OrderStatusId] [int] NOT NULL,
	[PaymentStatusId] [INT] NOT NULL,
	[PaymentMethodSystemName] [nvarchar](max) NULL,
	[CustomerCurrencyCode] [nvarchar](max) NULL,
	[CurrencyRate] [decimal](18, 8) NOT NULL,
	[OrderTotal] [DECIMAL](18, 4) NOT NULL,
	[CustomerLanguageId] [int] NOT NULL,
	[CustomerIp] [nvarchar](max) NULL,
	[AllowStoringCreditCardNumber] [bit] NOT NULL,
	[CardType] [nvarchar](max) NULL,
	[CardName] [nvarchar](max) NULL,
	[CardNumber] [nvarchar](max) NULL,
	[MaskedCreditCardNumber] [nvarchar](max) NULL,
	[CardCvv2] [nvarchar](max) NULL,
	[CardExpirationMonth] [nvarchar](max) NULL,
	[CardExpirationYear] [nvarchar](max) NULL,
	[AuthorizationTransactionId] [nvarchar](max) NULL,
	[AuthorizationTransactionCode] [nvarchar](max) NULL,
	[AuthorizationTransactionResult] [nvarchar](max) NULL,
	[CaptureTransactionId] [nvarchar](max) NULL,
	[CaptureTransactionResult] [nvarchar](max) NULL,
	[SubscriptionTransactionId] [nvarchar](max) NULL,
	[PaidDateUtc] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedOnUtc] [datetime] NOT NULL,
	[CustomOrderNumber] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [Order_Customer]
GO



CREATE TABLE [dbo].[Currency](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](50) NOT NULL,
	[CurrencyCode] [NVARCHAR](5) NOT NULL,
	[Rate] [DECIMAL](18, 4) NOT NULL,
	[DisplayLocale] [NVARCHAR](50) NULL,
	[CustomFormatting] [NVARCHAR](50) NULL,
	[LimitedToStores] [BIT] NOT NULL,
	[Published] [BIT] NOT NULL,
	[DisplayOrder] [INT] NOT NULL,
	[CreatedOnUtc] [DATETIME] NOT NULL,
	[UpdatedOnUtc] [DATETIME] NOT NULL,
	[RoundingTypeId] [INT] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[OrderItem](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[OrderItemGuid] [UNIQUEIDENTIFIER] NOT NULL,
	[OrderId] [INT] NOT NULL,
	[MatchId] [INT] NOT NULL,
	[PlayId] [INT] NOT NULL,
	[UnitPrice] DECIMAL(18,4) NOT NULL,
	[Quantity] [INT] NOT NULL,
	[TeamId] [INT] NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [OrderItem_Order]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [OrderItem_Match] FOREIGN KEY([MatchId])
REFERENCES [dbo].[Match] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [OrderItem_Match]
GO



CREATE TABLE dbo.[Match](
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	MatchName NVARCHAR(500) NOT NULL,
	MatchTimeUtc DATETIME NOT NULL,
	GameId INT NOT NULL,
	MasterTeamId INT NOT NULL,
	MasterTeamRate DECIMAL(18,2) NOT NULL,
	MasterTeamScore DECIMAL(18,2) NOT NULL,
	SlaveTeamId INT NOT NULL,
	SlaverTeamRate DECIMAL(18,2) NOT NULL,
	SlaverTeamScore DECIMAL(18,2) NOT NULL,
	LiveUrl NVARCHAR(1000) NULL,
	Memo NVARCHAR(max) NULL,
	Enabled BIT NOT NULL,
	CreateTimeUtc DATETIME NOT NULL,
	CreateUserId INT NOT NULL,
	UpdateTimeUtc DATETIME NULL,
	UpdateUserId INT NULL
)
GO


CREATE TABLE MatchGame(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	Icon NVARCHAR(MAX) NOT NULL,
	CreateTimeUtc DATETIME NOT NULL,
	CreateUserId INT NOT NULL,
	UpdateTimeUtc DATETIME NULL,
	UpdateUserId INT NULL,
	Enabled BIT NOT NULL
)
GO



CREATE TABLE MatchTeam(
	Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	Name NVARCHAR(100) NOT NULL,
	Icon NVARCHAR(MAX) NOT NULL,
	CreateTimeUtc DATETIME NOT NULL,
	CreateUserId INT NOT NULL,
	UpdateTimeUtc DATETIME NULL,
	UpdateUserId INT NULL,
	Enabled BIT NOT NULL
)
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
CREATE TABLE CustomerRole_PermissionRecord_Mapping
(
	CustomerRoleId INT NOT NULL,
	PermissionRecordId INT NOT NULL
)
GO
ALTER TABLE [dbo].[CustomerRole_PermissionRecord_Mapping] ADD  CONSTRAINT [PK_CustomerRole_PermissionRecord_Mapping] PRIMARY KEY CLUSTERED 
(
	[CustomerRoleId] ASC,
	[PermissionRecordId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE = OFF,  IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
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
