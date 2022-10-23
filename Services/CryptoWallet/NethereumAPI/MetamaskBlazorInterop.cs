using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace NFTMint.Services.CryptoWallet.NethereumAPI
{
    public class MetamaskBlazorInterop : IMetamaskInterop
    {
        private readonly IJSRuntime _jsRuntime;

        public MetamaskBlazorInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
		}

        public async ValueTask<string> EnableEthereumAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("NethereumMetamaskInterop.EnableEthereum");
        }

        public async ValueTask<bool> CheckMetamaskAvailability()
        {
            return await _jsRuntime.InvokeAsync<bool>("NethereumMetamaskInterop.IsMetamaskAvailable");
        }

        public async ValueTask<RpcResponseMessage> SendAsync(RpcRequestMessage rpcRequestMessage)
        {
            var response = await _jsRuntime.InvokeAsync<string>("NethereumMetamaskInterop.Request", JsonConvert.SerializeObject(rpcRequestMessage));
            return JsonConvert.DeserializeObject<RpcResponseMessage>(response);
        }

        public async ValueTask<RpcResponseMessage> SendTransactionAsync(MetamaskRpcRequestMessage rpcRequestMessage)
        {
            var response = await _jsRuntime.InvokeAsync<string>("NethereumMetamaskInterop.Request", JsonConvert.SerializeObject(rpcRequestMessage));
            return JsonConvert.DeserializeObject<RpcResponseMessage>(response);
        }

        public async ValueTask<string> SignAsync(string utf8Hex)
        {
            return await _jsRuntime.InvokeAsync<string>("NethereumMetamaskInterop.Sign", utf8Hex);
        }

        public async ValueTask<string> GetSelectedAddress()
        {
            return await _jsRuntime.InvokeAsync<string>("NethereumMetamaskInterop.GetSelectedAddress");
        }

		public async ValueTask<int> GetChainID()
		{
			return await _jsRuntime.InvokeAsync<int>("NethereumMetamaskInterop.GetChainID");
		}

		public async ValueTask<bool> SwitchEthereumChain(string chainId, string chainName, string symbol, int decimals, string[] rpcUrls, string[] blockExplorerUrls)
        {
			return await _jsRuntime.InvokeAsync<bool>("NethereumMetamaskInterop.SwitchEthereumChain", chainId, chainName, symbol, decimals, rpcUrls, blockExplorerUrls);
        }

		//[JSInvokable]
  //      public static async Task MetamaskAvailableChanged(bool available)
  //      {
  //          await MetamaskHostProvider.Current.ChangeMetamaskAvailableAsync(available);
  //      }

        //[JSInvokable]
        //public static async Task SelectedAccountChanged(string selectedAccount)
        //{
        //    await MetamaskHostProvider.Current.ChangeSelectedAccountAsync(selectedAccount);
        //}

		[JSInvokable]
		public static async Task MetamaskUserDenied()
		{
            LOG.Trace("MetamaskUserDenied");
			await Task.CompletedTask;
			//await MetamaskHostProvider.Current.ChangeMetamaskAvailableAsync();
		}

		//[JSInvokable]
		//public static async Task SigndData(string result)
		//{
		//	await Task.CompletedTask;
		//	//await MetamaskHostProvider.Current.ChangeMetamaskAvailableAsync();
		//}

		//[JSInvokable]
		//public static async Task ChainChanged(int chainId)
		//{
		//	await MetamaskHostProvider.Current.ChainChangedAsync(chainId);
		//}

	}
}