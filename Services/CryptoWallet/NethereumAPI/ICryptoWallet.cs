using Microsoft.JSInterop;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.Web3;

namespace NFTMint.Services.CryptoWallet.NethereumAPI
{
    public abstract class ICryptoWallet : IEthereumHostProvider, IMetamaskInterop
    {
        public string Name => throw new NotImplementedException();

        public bool Available => throw new NotImplementedException();

        public string SelectedAccount => throw new NotImplementedException();

        public int SelectedNetwork => throw new NotImplementedException();

        public bool Enabled => throw new NotImplementedException();

        public event Func<string, Task> SelectedAccountChanged;
        public event Func<bool, Task> AvailabilityChanged;
        public event Func<int, Task> NetworkChanged;

        private readonly IJSRuntime _jsRuntime;

        public ICryptoWallet(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<bool> CheckMetamaskAvailability()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckProviderAvailabilityAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> EnableEthereumAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> EnableProviderAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> GetChainID()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetProviderSelectedAccountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetProviderSelectedNetworkAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> GetSelectedAddress()
        {
            throw new NotImplementedException();
        }

        public Task<Web3> GetWeb3Async()
        {
            throw new NotImplementedException();
        }

        public ValueTask<RpcResponseMessage> SendAsync(RpcRequestMessage rpcRequestMessage)
        {
            throw new NotImplementedException();
        }

        public ValueTask<RpcResponseMessage> SendTransactionAsync(MetamaskRpcRequestMessage rpcRequestMessage)
        {
            throw new NotImplementedException();
        }

        public ValueTask<string> SignAsync(string utf8Hex)
        {
            throw new NotImplementedException();
        }

        public Task<string> SignMessageAsync(string utf8Hex)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> SwitchEthereumChain(string chainId, string chainName, string symbol, int decimals, string[] rpcUrls, string[] blockExplorerUrls)
        {
            throw new NotImplementedException();
        }

        Task<int> IEthereumHostProvider.GetChainID()
        {
            throw new NotImplementedException();
        }

        Task<string> IEthereumHostProvider.SignAsync(string utf8Hex)
        {
            throw new NotImplementedException();
        }

        Task<bool> IEthereumHostProvider.SwitchEthereumChain(string chainId, string chainName, string symbol, int decimals, string[] rpcUrls, string[] blockExplorerUrls)
        {
            throw new NotImplementedException();
        }
    }
}
