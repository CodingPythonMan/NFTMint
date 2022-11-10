using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Nethereum.Util;
using NFTMint.Services;
using NFTMint.Services.Config;
using NFTMint.Services.CryptoWallet.NethereumAPI;
using NFTMint.Services.MetaMask;
using System.Numerics;

namespace NFTMint.Pages
{
    public partial class Facewallet
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; } = null!;

        decimal _coinBalance { get; set; }
        string _walletAddress { get; set; } = "nothing";

        public string selectNetwork { get; set; } = "GOELRI";

        public string FacewalletStatusAttr { get; set; } = "danger";
        public string FacewalletStatus { get; set; } = "로그아웃";
        public string message { get; set; } = null!;
        public string signedMessage { get; set; } = null!;


        public enum Network
        {
            GOELRI = 0,
            BNB_SMART_CHAIN_TESTNET,
            BAOBAB,
            BORA_TESTNET,
            SOLANA_DEVNET,
            NEAR_TESTNET
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(true == firstRender)
            {
                await JSRuntime.InvokeVoidAsync("FacewalletInterop.Init", ConfigService._webServerConfig.env.facewalletTestnetApiKey);
                bool result = await JSRuntime.InvokeAsync<bool>("FacewalletInterop.IsLoggedIn");
                if(result)
                {
                    FacewalletStatusAttr = "success";
                    FacewalletStatus = "로그인";

                    await FacewalletGetBalance();
                    await FacewalletGetAddress();
                }
            }
        }

        async Task FacewalletLogin()
        {
            await JSRuntime.InvokeVoidAsync("FacewalletInterop.Login");
            FacewalletStatusAttr = "success";
            FacewalletStatus = "로그인";
            await FacewalletGetBalance();
            await FacewalletGetAddress();
        }

        async Task FacewalletLogout()
        {
            await JSRuntime.InvokeVoidAsync("FacewalletInterop.Logout");
            FacewalletStatusAttr = "danger";
            FacewalletStatus = "로그아웃";
            _coinBalance = 0;
            _walletAddress = "nothing";
            signedMessage = "";
            message = "";
            StateHasChanged();
        }

        async Task FacewalletGetBalance()
        {
            string balance = await JSRuntime.InvokeAsync<string>("FacewalletInterop.GetBalance");
            _coinBalance = decimal.Parse(balance);
            StateHasChanged();
        }

        async Task FacewalletGetAddress()
        {
            _walletAddress = await JSRuntime.InvokeAsync<string>("FacewalletInterop.GetAddress");
            StateHasChanged();
        }

        async Task FacewalletSendTransaction()
        {
            await JSRuntime.InvokeVoidAsync("FacewalletInterop.SendTransaction", "0xe4C91859B035Ad4f052948ce36a2434efc144e08", "0.01");
        }

        async Task FacewalletSwitchNetwork()
        {
            Console.WriteLine(selectNetwork);
            await JSRuntime.InvokeVoidAsync("FacewalletInterop.SwitchNetwork", selectNetwork);
        }

        async Task FacewalletSignMessage()
        {
            signedMessage = await JSRuntime.InvokeAsync<string>("FacewalletInterop.SignMessage", message);
            StateHasChanged();
        }

        async Task FacewalletGetContract()
        {
            Console.WriteLine(await JSRuntime.InvokeAsync<object>("FacewalletInterop.GetContract", null));
            StateHasChanged();
        }

        async Task FacewalletTransfer()
        {
            Console.WriteLine(await JSRuntime.InvokeAsync<string>("FacewalletInterop.Transfer", null, null));
            StateHasChanged();
        }

        async Task AnalyzeFacewalletSigner()
        {
            await JSRuntime.InvokeVoidAsync("FacewalletInterop.GetMethods");
        }
    }
}
