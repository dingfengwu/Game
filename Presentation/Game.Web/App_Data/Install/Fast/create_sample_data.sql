SET IDENTITY_INSERT [dbo].[Match] ON 
INSERT INTO dbo.[Match]([Id], MatchName,MatchTimeUtc,GameId,MasterTeamId,MasterTeamRate,MasterTeamScore,SlaveTeamId,SlaverTeamRate,SlaverTeamScore,LiveUrl,Memo,Enabled,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId) VALUES(1,'测试比赛一','2018-09-10',1,1,1.9,0,2,2.0,0,'','',1,'2017-09-10',1,NULL,NULL)
INSERT INTO dbo.[Match]([Id], MatchName,MatchTimeUtc,GameId,MasterTeamId,MasterTeamRate,MasterTeamScore,SlaveTeamId,SlaverTeamRate,SlaverTeamScore,LiveUrl,Memo,Enabled,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId) VALUES(2,'测试比赛二','2018-08-10',1,3,1.1,0,2,1.0,0,'','',1,'2017-08-10',1,NULL,NULL)
INSERT INTO dbo.[Match]([Id], MatchName,MatchTimeUtc,GameId,MasterTeamId,MasterTeamRate,MasterTeamScore,SlaveTeamId,SlaverTeamRate,SlaverTeamScore,LiveUrl,Memo,Enabled,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId) VALUES(3,'测试比赛三','2018-07-10',1,4,1.2,0,2,0.0,0,'','',1,'2017-07-10',1,NULL,NULL)
INSERT INTO dbo.[Match]([Id], MatchName,MatchTimeUtc,GameId,MasterTeamId,MasterTeamRate,MasterTeamScore,SlaveTeamId,SlaverTeamRate,SlaverTeamScore,LiveUrl,Memo,Enabled,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId) VALUES(4,'测试比赛四','2018-06-10',1,2,1.3,0,4,1.9,0,'','',1,'2017-06-10',1,NULL,NULL)
INSERT INTO dbo.[Match]([Id], MatchName,MatchTimeUtc,GameId,MasterTeamId,MasterTeamRate,MasterTeamScore,SlaveTeamId,SlaverTeamRate,SlaverTeamScore,LiveUrl,Memo,Enabled,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId) VALUES(5,'测试比赛五','2018-05-10',1,3,1.4,0,1,1.8,0,'','',1,'2017-05-10',1,NULL,NULL)
GO



SET IDENTITY_INSERT [dbo].[Order] ON 
INSERT [dbo].[Order] ([Id], [OrderGuid], [CustomerId], [OrderStatusId], [PaymentStatusId], [PaymentMethodSystemName], [CustomerCurrencyCode], [CurrencyRate], [OrderTotal], [CustomerLanguageId], [CustomerIp], [AllowStoringCreditCardNumber], [CardType], [CardName], [CardNumber], [MaskedCreditCardNumber], [CardCvv2], [CardExpirationMonth], [CardExpirationYear], [AuthorizationTransactionId], [AuthorizationTransactionCode], [AuthorizationTransactionResult], [CaptureTransactionId], [CaptureTransactionResult], [SubscriptionTransactionId], [PaidDateUtc], [Deleted], [CreatedOnUtc], [CustomOrderNumber]) VALUES (1, N'640ab776-f9ac-4121-aa45-fc785e60c7fc', 2, 20, 30, N'Payments.CheckMoneyOrder', N'USD', CAST(1.00000000 AS Decimal(18, 8)), N'3', 1, N'127.0.0.1', 0, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', CAST(N'2017-10-16T08:37:16.237' AS DateTime), 0, CAST(N'2017-10-16T08:37:16.237' AS DateTime), N'1')
INSERT [dbo].[Order] ([Id], [OrderGuid], [CustomerId], [OrderStatusId], [PaymentStatusId], [PaymentMethodSystemName], [CustomerCurrencyCode], [CurrencyRate], [OrderTotal], [CustomerLanguageId], [CustomerIp], [AllowStoringCreditCardNumber], [CardType], [CardName], [CardNumber], [MaskedCreditCardNumber], [CardCvv2], [CardExpirationMonth], [CardExpirationYear], [AuthorizationTransactionId], [AuthorizationTransactionCode], [AuthorizationTransactionResult], [CaptureTransactionId], [CaptureTransactionResult], [SubscriptionTransactionId], [PaidDateUtc], [Deleted], [CreatedOnUtc], [CustomOrderNumber]) VALUES (2, N'bbbdb9bb-cd89-47b1-94bf-100386f6aa80', 3, 10, 10, N'Payments.CheckMoneyOrder', N'USD', CAST(1.00000000 AS Decimal(18, 8)), N'2', 1, N'127.0.0.1', 0, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', NULL, 0, CAST(N'2017-10-16T08:37:16.477' AS DateTime), N'2')
INSERT [dbo].[Order] ([Id], [OrderGuid], [CustomerId], [OrderStatusId], [PaymentStatusId], [PaymentMethodSystemName], [CustomerCurrencyCode], [CurrencyRate], [OrderTotal], [CustomerLanguageId], [CustomerIp], [AllowStoringCreditCardNumber], [CardType], [CardName], [CardNumber], [MaskedCreditCardNumber], [CardCvv2], [CardExpirationMonth], [CardExpirationYear], [AuthorizationTransactionId], [AuthorizationTransactionCode], [AuthorizationTransactionResult], [CaptureTransactionId], [CaptureTransactionResult], [SubscriptionTransactionId], [PaidDateUtc], [Deleted], [CreatedOnUtc], [CustomOrderNumber]) VALUES (3, N'4c48a2a7-1ec3-4761-ba61-5101391ec3ef', 4, 10, 10, N'Payments.CheckMoneyOrder', N'USD', CAST(1.00000000 AS Decimal(18, 8)), N'3', 1, N'127.0.0.1', 0, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', NULL, 0, CAST(N'2017-10-16T08:37:16.557' AS DateTime), N'3')
INSERT [dbo].[Order] ([Id], [OrderGuid], [CustomerId], [OrderStatusId], [PaymentStatusId], [PaymentMethodSystemName], [CustomerCurrencyCode], [CurrencyRate], [OrderTotal], [CustomerLanguageId], [CustomerIp], [AllowStoringCreditCardNumber], [CardType], [CardName], [CardNumber], [MaskedCreditCardNumber], [CardCvv2], [CardExpirationMonth], [CardExpirationYear], [AuthorizationTransactionId], [AuthorizationTransactionCode], [AuthorizationTransactionResult], [CaptureTransactionId], [CaptureTransactionResult], [SubscriptionTransactionId], [PaidDateUtc], [Deleted], [CreatedOnUtc], [CustomOrderNumber]) VALUES (4, N'3c89ad33-13f6-4274-8545-058997a5acc9', 5, 20, 30, N'Payments.CheckMoneyOrder', N'USD', CAST(1.00000000 AS Decimal(18, 8)), N'3', 1, N'127.0.0.1', 0, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', CAST(N'2017-10-16T08:37:16.663' AS DateTime), 0, CAST(N'2017-10-16T08:37:16.663' AS DateTime), N'4')
INSERT [dbo].[Order] ([Id], [OrderGuid], [CustomerId], [OrderStatusId], [PaymentStatusId], [PaymentMethodSystemName], [CustomerCurrencyCode], [CurrencyRate], [OrderTotal], [CustomerLanguageId], [CustomerIp], [AllowStoringCreditCardNumber], [CardType], [CardName], [CardNumber], [MaskedCreditCardNumber], [CardCvv2], [CardExpirationMonth], [CardExpirationYear], [AuthorizationTransactionId], [AuthorizationTransactionCode], [AuthorizationTransactionResult], [CaptureTransactionId], [CaptureTransactionResult], [SubscriptionTransactionId], [PaidDateUtc], [Deleted], [CreatedOnUtc], [CustomOrderNumber]) VALUES (5, N'38b0f47e-4310-45c5-9027-4e907fffdd50', 6, 30, 30, N'Payments.CheckMoneyOrder', N'USD', CAST(1.00000000 AS Decimal(18, 8)), N'1', 1, N'127.0.0.1', 0, N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', N'', CAST(N'2017-10-16T08:37:16.877' AS DateTime), 0, CAST(N'2017-10-16T08:37:16.877' AS DateTime), N'5')
SET IDENTITY_INSERT [dbo].[Order] OFF
GO



SET IDENTITY_INSERT [dbo].[OrderItem] ON 
GO
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (1, N'609dd61e-965a-44d3-abf7-bede207a2510', 1, 1, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (2, N'e82cc07d-d665-45ef-a6e4-4f9d436d948c', 1, 2, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (3, N'51cf8597-46a2-4918-b952-ed3e1632eecd', 1, 3, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (4, N'809a96bb-df98-43ff-81f4-74ae5295954c', 2, 4, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (5, N'452235b1-df8b-4505-bcf8-49318369247c', 2, 5, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (6, N'a104a775-f78e-4e22-8de0-85e334988c91', 3, 1, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (7, N'0da92594-e97f-4fc7-a782-cb0735d216f0', 3, 2, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (8, N'5ec7f30d-67bf-4f8d-84d5-828d912ba229', 3, 3, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (9, N'eabd2aed-67d9-4d06-a21d-75c8bde0ed83', 4, 4, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (10, N'5eef6233-d9ff-4f37-9807-61861ccbddba', 4, 5, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (11, N'b2917ad7-4d0e-4c51-8fa3-dce382125753', 4, 3, 1)
INSERT [dbo].[OrderItem] ([Id], [OrderItemGuid], [OrderId], [MatchId], [Quantity]) VALUES (12, N'9d9c7132-e70d-4801-b1c0-ef9be2370a61', 5, 2, 1)
GO
SET IDENTITY_INSERT [dbo].[OrderItem] OFF
GO



INSERT INTO dbo.MatchGame(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled)VALUES('LOL','lol.png',GETUTCDATE(),1,NULL,NULL,1)
INSERT INTO dbo.MatchGame(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled)VALUES('CS','cs.png',GETUTCDATE(),1,NULL,NULL,1)
INSERT INTO dbo.MatchGame(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled)VALUES('DOTAII','dota2.png',GETUTCDATE(),1,NULL,NULL,1)
INSERT INTO dbo.MatchGame(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled)VALUES('炉石传说','lsqs.png',GETUTCDATE(),1,NULL,NULL,1)
INSERT INTO dbo.MatchGame(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled)VALUES('王者荣耀','wzry.png',GETUTCDATE(),1,NULL,NULL,1)
GO



INSERT INTO dbo.MatchTeam(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled) VALUES('测试团队一','1.png',GETUTCDATE(),1,NULL,NULL,1)
INSERT INTO dbo.MatchTeam(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled) VALUES('测试团队二','2.png',GETUTCDATE(),1,NULL,NULL,1)
INSERT INTO dbo.MatchTeam(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled) VALUES('测试团队三','3.png',GETUTCDATE(),1,NULL,NULL,1)
INSERT INTO dbo.MatchTeam(Name,Icon,CreateTimeUtc,CreateUserId,UpdateTimeUtc,UpdateUserId,Enabled) VALUES('测试团队四','4.png',GETUTCDATE(),1,NULL,NULL,1)
GO



INSERT INTO setting VALUES('captchasettings.enabled','True',0)
INSERT INTO setting VALUES('captchasettings.showonloginpage','True',0)
INSERT INTO setting VALUES('captchasettings.showonregistrationpage','True',0)
INSERT INTO setting VALUES('captchasettings.recaptchapublickey','6Lcht1QUAAAAANBy7JA_K_SwNTcn8Djdf4EwNbGm',0)
INSERT INTO setting VALUES('captchasettings.recaptchaprivatekey','6Lcht1QUAAAAAPOBeQgWgKx2vbGNwE93_mMl570K',0)
INSERT INTO setting VALUES('captchasettings.recaptchaversion','Version2',0)
INSERT INTO setting VALUES('captchasettings.recaptchatheme','white',0)
INSERT INTO setting VALUES('captchasettings.recaptchalanguage','zh-CN',0)
GO
