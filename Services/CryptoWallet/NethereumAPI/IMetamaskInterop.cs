using System.Threading.Tasks;
using Nethereum.JsonRpc.Client.RpcMessages;

namespace NFTMint.Services.CryptoWallet.NethereumAPI
{
    public interface IMetamaskInterop
    {
        ValueTask<string> EnableEthereumAsync();
        ValueTask<bool> CheckMetamaskAvailability();
        ValueTask<string> GetSelectedAddress();
        ValueTask<RpcResponseMessage> SendAsync(RpcRequestMessage rpcRequestMessage);
        ValueTask<RpcResponseMessage> SendTransactionAsync(MetamaskRpcRequestMessage rpcRequestMessage);
        ValueTask<string> SignAsync(string utf8Hex);
        ValueTask<int> GetChainID();
        ValueTask<bool> SwitchEthereumChain(string chainId, string chainName, string symbol, int decimals, string[] rpcUrls, string[] blockExplorerUrls);
	}
}