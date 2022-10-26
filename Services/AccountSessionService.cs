using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Nethereum.Model;
using NFTMint.Services.MetaMask;

namespace NFTMint.Services
{
    public class AccountSessionService
    {
        MetaMaskService _MetaMaskService { get; set; }

        public AccountSessionService(MetaMaskService metaMaskService)
        {
            _MetaMaskService = metaMaskService;

            _MetaMaskService.OnAccountChange += _OnAccountChange;
            _MetaMaskService.OnNetworkChange += _MetaMaskService_OnNetworkChange;
        }

        void _MetaMaskService_OnNetworkChange(int chainId)
        {
            int vaildChainId = 97;

            if (vaildChainId != chainId)
            {
                Logout();
            }
        }

        void _OnAccountChange(bool isChanged)
        {
            if (isChanged)
            {
                Logout();
            }
        }

        public void Logout()
        {
            Console.WriteLine("Logout!");
        }

        public async ValueTask<bool> LoginWithWallet(
            Func<Task> onWrongNetwork,
            Action onNotLinkedAccount = null)
        {
            bool result = false;

            do
            {
                try
                {
                    bool isVaildMainnet = await _MetaMaskService.CheckMainnet(onWrongNetwork);

                    if (false == isVaildMainnet)
                    {
                        break;
                    }

                    //await _MetaMaskService.SwitchEthereumChain();
                    await _MetaMaskService.ConnectMetaMask();

                    if (false == _MetaMaskService.IsAccountSelected)
                    {
                        break;
                    }

                    //var isLinkedAccount = _AccountLinkService.IsLinkedAccount();

                    /*if (isLinkedAccount)
                    {
                        await _Login();
                    }
                    else
                    {
                        onNotLinkedAccount?.Invoke();
                    }*/

                    result = true;
                }
                catch (TaskCanceledException e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            } while (false);

            return result;
        }
    }
}
