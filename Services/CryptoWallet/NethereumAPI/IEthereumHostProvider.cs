using System;
using System.Threading.Tasks;

namespace NFTMint.Services.CryptoWallet.NethereumAPI
{
    public interface IEthereumHostProvider
    {
        string Name { get; }

        bool Available { get; }
        string SelectedAccount { get;}
        int SelectedNetwork { get; }
        bool Enabled { get; }
        
        event Func<string, Task> SelectedAccountChanged;
        event Func<bool, Task> AvailabilityChanged;
		event Func<int, Task> NetworkChanged;
		//event Func<bool, Task> EnabledChanged;

        Task<bool> CheckProviderAvailabilityAsync();
        Task<Web3> GetWeb3Async();
        Task<string> EnableProviderAsync();
        Task<string> GetProviderSelectedAccountAsync();
        Task<int> GetProviderSelectedNetworkAsync();
        Task<string> SignAsync(string utf8Hex);
		Task<string> SignMessageAsync(string utf8Hex);
		Task<int> GetChainID();
		Task<bool> SwitchEthereumChain(string chainId, string chainName, string symbol, int decimals, string[] rpcUrls, string[] blockExplorerUrls);
	}
}