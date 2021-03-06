USE [immanuel_kv]
GO
/****** Object:  StoredProcedure [immanuel_sa].[sp_UpdatePubSubUser]    Script Date: 7/22/2017 9:37:24 PM ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [immanuel_sa].[PubSubUser]
GO

CREATE TABLE [immanuel_sa].[PubSubUser](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientKey] [varchar](8) NOT NULL,
	[User] [varchar](64) NOT NULL,
	[ConnectionId] [varchar](64) NOT NULL,
	[IpAddr] [varchar](64) NULL,
	[Agent] [varchar](128) NULL,
	[CreatedAt] [datetime2](7) NOT NULL default getdate(),
 CONSTRAINT [PK_PubSubUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Pubsub_User] ON [immanuel_sa].[PubSubUser]
(
	[User] ASC,
	[ConnectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO


DROP TABLE [immanuel_sa].[PubSubGroup]
GO

CREATE TABLE [immanuel_sa].[PubSubGroup](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ClientKey] [varchar](8) NOT NULL,
	[GroupId] [varchar](64) NOT NULL,
	[User] [varchar](64) NOT NULL,
	[ConnectionId] [varchar](64) NOT NULL,
	[IpAddr] [varchar](64) NULL,
	[Agent] [varchar](128) NULL,
	[CreatedAt] [datetime2](7) NOT NULL default getdate(),
 CONSTRAINT [PK_PubSubGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Pubsub_Group] ON [immanuel_sa].[PubSubGroup]
(
	[GroupId] ASC,
	[User] ASC,
	[ConnectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [immanuel_sa].[sp_UpdatePubsubUser]
(
	@ip_ClientKey varchar(8),
	@ip_UserName varchar(64),
	@ip_ConnectionId varchar(1024),
	@ip_IpAddr varchar(64),
	@ip_Agent varchar(128)
)
AS
BEGIN
	MERGE [PubSubUser] AS tgt
		using (select @ip_ClientKey ClientKey,
					  @ip_UserName [User],
					  @ip_ConnectionId ConnectionId,
					  @ip_IpAddr IpAddr,
					  @ip_Agent Agent) as src
			on tgt.[ClientKey] = src.[ClientKey] and 
			   tgt.[User] = src.[User]
	when MATCHED then
		Update set tgt.ConnectionId = @ip_ConnectionId
	when not matched by target then
		INSERT ([ClientKey]
           ,[User]
           ,[ConnectionId]
           ,[IpAddr]
           ,[Agent])
		VALUES
           (@ip_ClientKey
           ,@ip_UserName
           ,@ip_ConnectionId
           ,@ip_IpAddr
           ,@ip_Agent)
		;
		
END

GO

CREATE PROCEDURE [immanuel_sa].[sp_UpdatePubsubGroup]
(
	@ip_ClientKey varchar(8),
	@ip_GroupId varchar(64),
	@ip_UserName varchar(64),
	@ip_ConnectionId varchar(1024),
	@ip_IpAddr varchar(64),
	@ip_Agent varchar(128)
)
AS
BEGIN
	MERGE [PubSubGroup] AS tgt
		using (select @ip_ClientKey ClientKey,
					  @ip_UserName [User],
					  @ip_GroupId [GroupId],
					  @ip_ConnectionId ConnectionId,
					  @ip_IpAddr IpAddr,
					  @ip_Agent Agent) as src
			on tgt.[ClientKey] = src.[ClientKey] and 
			   tgt.[User] = src.[User]
	when MATCHED then
		Update set tgt.ConnectionId = @ip_ConnectionId
	when not matched by target then
		INSERT ([ClientKey]
           ,[User]
		   ,[GroupId]
           ,[ConnectionId]
           ,[IpAddr]
           ,[Agent])
		VALUES
           (@ip_ClientKey
           ,@ip_UserName
		   ,@ip_GroupId
           ,@ip_ConnectionId
           ,@ip_IpAddr
           ,@ip_Agent)
		;
		
END

