CREATE TABLE [dbo].[tMarketplaceRune]
(
	[cOwnerAddress]		[dbo].[ADDRESS_LENGTH] ,	
	[cIndex]			INT NOT NULL,  
	[cStack]			INT CONSTRAINT [DF_tMarketplaceRune_cStack] DEFAULT 0 NOT NULL, 

	CONSTRAINT [PK_tMarketplaceRune] PRIMARY KEY ([cOwnerAddress],[cIndex] )
)