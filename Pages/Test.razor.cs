﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Nethereum.Util;
using NFTMint.Services;
using NFTMint.Services.MetaMask;
using System.Numerics;

namespace NFTMint.Pages
{
    public partial class Test
    {
        [Inject]
        MetaMaskService _MetaMaskService { get; set; } = null!;
        [Inject]
        AccountSessionService _AccountSessionService { get; set; } = null!;
        [Inject]
        MyWalletService _MyWalletService { get; set; } = null!;

        [Inject]
        IJSRuntime JSRuntime { get; set; } = null!;

        decimal _coinBalance;

        bool _isInit = false;

        async Task Login()
        {
            Console.WriteLine("Hello World!");

            var isProviderAvailable = await _MetaMaskService.CheckProviderAvailabilityAsync();

            if (false == isProviderAvailable)
            {
                Console.WriteLine("Provider 가 안 보입니다!");
            }
            else
            {
                bool isVaildMainNet = await _MetaMaskService.CheckMainnet(
                    onWrongNetwork: async () =>
                    {
                        Console.WriteLine("Network 가 다릅니다!");
                    }
                );


                await _AccountSessionService.LoginWithWallet(
                        onWrongNetwork: async () =>
                        {
                            Console.WriteLine("Network 가 다릅니다!");
                        },
                        onNotLinkedAccount: async () =>
                        {
                            Console.WriteLine("연결되지 않았습니다!");
                        }
                );
            }

            /*
            List<Task> tasks = new List<Task>() { _MyWalletService.GetCoinBalance()};

            var coinBalance = ((Task<BigInteger>)tasks[0]).Result;
            _coinBalance = UnitConversion.Convert.FromWei(coinBalance);

            _coinBalance = Math.Truncate(_coinBalance * 1000) / 1000;*/

            _isInit = true;

            StateHasChanged();
        }
    }
}