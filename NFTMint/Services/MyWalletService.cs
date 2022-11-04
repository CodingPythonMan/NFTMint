using Nethereum.Web3;
using NFTMint.Services.MetaMask;
using System.Numerics;

namespace NFTMint.Services
{
    public class MyWalletService
    {
        MetaMaskService _MetaMaskService { get; set; }

        public string WalletAccount { get => _MetaMaskService.SelectedAccount; }

        public MyWalletService(MetaMaskService metaMaskService)
        {
            _MetaMaskService = metaMaskService;
        }

        public async Task<BigInteger> GetCoinBalance()
        {
            Web3 web3 = await _MetaMaskService.GetWeb3Async();

            var balance = await web3.Eth.GetBalance.SendRequestAsync(_MetaMaskService.SelectedAccount);

            return balance.Value;
        }
    }
}
