using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace NFTMint.Services.CryptoWallet.NethereumAPI
{
    public class MetamaskHostProvider: IEthereumHostProvider
    {
        private readonly IMetamaskInterop _metamaskInterop;
        //public static MetamaskHostProvider Current { get; private set; }
        public string Name { get; } = "Metamask";
        public bool Available { get; private set; }
        public string SelectedAccount { get; private set; }
        public int SelectedNetwork { get; }
        public bool Enabled { get; private set; }

        private MetamaskInterceptor _metamaskInterceptor;

        public event Func<string, Task> SelectedAccountChanged;
        public event Func<bool, Task> AvailabilityChanged;
		public event Func<int, Task> NetworkChanged;
		//public event Func<bool, Task> EnabledChanged;

		public MetamaskHostProvider(
            IMetamaskInterop metamaskInterop)
		{
			_metamaskInterop = metamaskInterop;
			_metamaskInterceptor = new MetamaskInterceptor(_metamaskInterop, this);

			//Current = this;
		}

		public async Task<bool> CheckProviderAvailabilityAsync()
        {
            var result = await _metamaskInterop.CheckMetamaskAvailability();
            await ChangeMetamaskAvailableAsync(result);
            return result;
        }

        public Task<Web3> GetWeb3Async()
        {
            var web3 = new Nethereum.Web3.Web3 {Client = {OverridingRequestInterceptor = _metamaskInterceptor}};
            return Task.FromResult(web3);
        }

        public async Task<string> EnableProviderAsync()
        {
            var selectedAccount = await _metamaskInterop.EnableEthereumAsync();
            Enabled = !string.IsNullOrEmpty(selectedAccount);

            //RingLog.LOG.Trace(_CircuitHandlerService.CircuitId);

            if (Enabled)
            {
				SelectedAccount = selectedAccount;
                if (SelectedAccountChanged != null)
                {
                    var a = SelectedAccountChanged.Target;
					await SelectedAccountChanged.Invoke(selectedAccount);
                }
                return selectedAccount;
            }

            return null;
        }

        public async Task<string> GetProviderSelectedAccountAsync()
        {
            var result = await _metamaskInterop.GetSelectedAddress();
            await ChangeSelectedAccountAsync(result);
            return result;
        }

        public Task<int> GetProviderSelectedNetworkAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<string> SignAsync(string utf8Hex)
        {
            return await _metamaskInterop.SignAsync(utf8Hex);
        }

		public async Task<string> SignMessageAsync(string message)
		{
			return await _metamaskInterop.SignAsync(message.ToHexUTF8());
		}

		[JSInvokable]
        public async Task ChangeSelectedAccountAsync(string selectedAccount)
        {
			if (SelectedAccount != selectedAccount)
			{
				SelectedAccount = selectedAccount;
				if (SelectedAccountChanged != null)
				{
					await SelectedAccountChanged.Invoke(selectedAccount);
				}
			}
		}

		[JSInvokable]
		public async Task ChainChangedAsync(int chainId)
		{
			if (NetworkChanged != null)
			{
				await NetworkChanged.Invoke(chainId);
			}
		}

		//public async Task ChangeSelectedAccountAsync(string selectedAccount)
		//      {
		//	RingLog.LOG.Info($"[{_CircuitHandlerService.CircuitId}] prev = {SelectedAccount}, curr = {selectedAccount}");

		//	if (SelectedAccount != selectedAccount)
		//          {
		//		SelectedAccount = selectedAccount;
		//              if (SelectedAccountChanged != null)
		//              {
		//                  await SelectedAccountChanged.Invoke(selectedAccount);
		//              }
		//          }
		//      }

		public async Task ChangeMetamaskAvailableAsync(bool available)
        {
            Available = available;
            if (AvailabilityChanged != null)
            {
                await AvailabilityChanged.Invoke(available);
            }
        }

		//public async Task ChainChangedAsync(int chainId)
		//{
		//	if (NetworkChanged != null)
		//	{
		//		await NetworkChanged.Invoke(chainId);
		//	}
		//}

		public async Task<int> GetChainID()
		{
            return await _metamaskInterop.GetChainID();
		}

		public async Task<bool> SwitchEthereumChain(string chainId, string chainName, string symbol, int decimals, string[] rpcUrls, string[] blockExplorerUrls)
        {
			return await _metamaskInterop.SwitchEthereumChain(chainId, chainName, symbol, decimals, rpcUrls, blockExplorerUrls);
		}
	}
}
