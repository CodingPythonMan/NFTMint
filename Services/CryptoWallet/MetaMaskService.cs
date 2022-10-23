namespace NFTMint.Services.MetaMask
{
	public class MetaMaskService : IDisposable
	{
		IEthereumHostProvider _metamaskHostProvider { get; set; }

		public string SelectedAccount { get; private set; }

		public event Action<bool> OnAccountChange;
		public event Action<int> OnNetworkChange;

		void NotifyAccountChanged(bool isChanged) => OnAccountChange?.Invoke(isChanged);
		void NotifyNetworkChanged(int chainId) => OnNetworkChange?.Invoke(chainId);

		public MetaMaskService(IEthereumHostProvider ethereumHostProvider)
		{
			_metamaskHostProvider = ethereumHostProvider;

			_metamaskHostProvider.SelectedAccountChanged += _SelectedAccountChanged;
			_metamaskHostProvider.NetworkChanged += _NetworkChanged;
		}

		public bool IsAccountSelected => !string.IsNullOrWhiteSpace(SelectedAccount);

		async Task _SelectedAccountChanged(string account)
		{
			bool isChanged = false;

			if (string.IsNullOrWhiteSpace(SelectedAccount))
			{
				SelectedAccount = account;
			}
			else if (string.IsNullOrWhiteSpace(account) || SelectedAccount != account)
			{
				SelectedAccount = account;

				isChanged = true;
			}

			NotifyAccountChanged(isChanged);

			await Task.CompletedTask;
		}

		Task _NetworkChanged(int chainId)
		{
			NotifyNetworkChanged(chainId);

			return Task.CompletedTask;
		}

		public async Task<bool> CheckMainnet(Func<Task> onWrongNetwork)
		{
			int chainId = await GetChainID();

			int vaildChainId = ConfigService._webServerConfig.blockchain[ConfigService._webServerConfig.env.marketType].network.chainId;

			if (chainId != vaildChainId)
			{
				await onWrongNetwork();

				return await SwitchEthereumChain();
			}

			return true;
		}

		public async Task<int> GetChainID()
		{
			return await _metamaskHostProvider.GetChainID();
		}
		
		public async Task<bool> SwitchEthereumChain()
		{ 
			return await _metamaskHostProvider.SwitchEthereumChain(
				$"0x{ConfigService._webServerConfig.blockchain[ConfigService._webServerConfig.env.marketType].network.chainId.ToString("X")}",
				ConfigService._webServerConfig.blockchain[ConfigService._webServerConfig.env.marketType].network.chainName,
				ConfigService._webServerConfig.blockchain[ConfigService._webServerConfig.env.marketType].network.Symbol,
				ConfigService._webServerConfig.blockchain[ConfigService._webServerConfig.env.marketType].network.decimals,
				ConfigService._webServerConfig.blockchain[ConfigService._webServerConfig.env.marketType].network.rpcUrls,
				ConfigService._webServerConfig.blockchain[ConfigService._webServerConfig.env.marketType].network.blockExplorerUrls);
		}

		public async Task<string> ConnectMetaMask()
		{
			return await _metamaskHostProvider.EnableProviderAsync();
		}

		public async Task<bool> CheckProviderAvailabilityAsync()
		{
			return await _metamaskHostProvider.CheckProviderAvailabilityAsync();
		}

		public async Task GetProviderSelectedAccountAsync()
		{
			await _metamaskHostProvider.GetProviderSelectedAccountAsync();
		}

		public async Task<Web3> GetWeb3Async()
		{
			return await _metamaskHostProvider.GetWeb3Async();
		}

		public async Task<string> SignMessageAsync(string message)
		{
			return await _metamaskHostProvider.SignMessageAsync(message);
		}

		public async Task<string> SignAsync(string hex)
		{
			return await _metamaskHostProvider.SignAsync(hex);
		}

		public void Dispose()
		{
			_metamaskHostProvider.SelectedAccountChanged -= _SelectedAccountChanged;
		}
	}
}
