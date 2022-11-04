CREATE TABLE [dbo].[tWalletAccount]
(
    [cWalletAddress]	[dbo].[ADDRESS_LENGTH] NOT NULL,
    [cIdentityUnique]	BIGINT CONSTRAINT [DF_tWalletAccount_cIdentityUnique] DEFAULT 0 NOT NULL,
    [cAccountUnique] BIGINT CONSTRAINT [DF_tWalletAccount_cAccountUnique] DEFAULT 0 NOT NULL,  
    [cResetTimeStamp] BIGINT CONSTRAINT [DF_tWalletAccount_cResetTimeStamp] DEFAULT 0 NOT NULL, 
    [cFreeCount] INT CONSTRAINT [DF_tWalletAccount_cFreeCount] DEFAULT 0 NOT NULL, 
    CONSTRAINT [PK_tWalletAccount] PRIMARY KEY CLUSTERED ([cWalletAddress])
)

GO
CREATE NONCLUSTERED INDEX [IX_tWalletAccount_cIdentityUnique]
    ON [dbo].[tWalletAccount]([cIdentityUnique]);

GO
CREATE NONCLUSTERED INDEX [IX_tWalletAccount_cAccountUnique]
    ON [dbo].[tWalletAccount]([cAccountUnique]);