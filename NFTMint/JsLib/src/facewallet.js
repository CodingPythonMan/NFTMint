import { Face, Network } from '@haechi-labs/face-sdk';
import { ethers } from 'ethers';

export const FacewalletInterop = {
    Init: (apikey) => {
        if (!FacewalletInterop.hasOwnProperty('face')) {
            FacewalletInterop.face = new Face({
                network: Network.BNB_SMART_CHAIN_TESTNET,
                apiKey: apikey
            });

            FacewalletInterop.provider = new ethers.providers.Web3Provider(FacewalletInterop.face.getEthLikeProvider());
        }
    },

    Login: async () => {
        await FacewalletInterop.face.auth.login();
    },

    IsLoggedIn: async () => {
        const loggedIn = await FacewalletInterop.face.auth.isLoggedIn();
        return loggedIn;
    },

    Logout: async () => {
        await FacewalletInterop.face.auth.logout();
        alert("Logout!");
    },

    SwitchNetwork: async (network) => {
        switch (network)
        {
            case "GOELRI":
                await FacewalletInterop.face.switchNetwork(Network.GOELRI);
                break;
            case "MUMBAI":
                await FacewalletInterop.face.switchNetwork(Network.MUMBAI);
                break;
            case "BNB_SMART_CHAIN_TESTNET":
                await FacewalletInterop.face.switchNetwork(Network.BNB_SMART_CHAIN_TESTNET);
                break;
            case "BAOBAB":
                await FacewalletInterop.face.switchNetwork(Network.BAOBAB);
                break;
            case "BORA_TESTNET":
                await FacewalletInterop.face.switchNetwork(Network.BORA_TESTNET);
                break;
            case "SOLANA_DEVNET":
                await FacewalletInterop.face.switchNetwork(Network.SOLANA_DEVNET);
                break;
            case "NEAR_TESTNET":
                await FacewalletInterop.face.switchNetwork(Network.NEAR_TESTNET);
                break;
        }
    },

    GetAddress: async () => {
        const signer = FacewalletInterop.provider.getSigner();

        const userAddress = await signer.getAddress();
        return userAddress;
    },

    GetBalance: async () => {
        const signer = FacewalletInterop.provider.getSigner();

        // Get user's BNB Smart Chain wallet address
        const userAddress = await signer.getAddress();
        const balance = ethers.utils.formatEther(
            // Get user's Binance Coin balance in wei
            await FacewalletInterop.provider.getBalance(userAddress)
        );

        return balance;
    },

    SendTransaction: async (ReceiverAddress, Amount) => {
        const signer = FacewalletInterop.provider.getSigner();

        // DApp should choose the input required to send transaction
        const receiverAddress = ReceiverAddress;
        const amount = ethers.utils.parseEther(Amount);

        // This transaction means user send his/her 0.1 ETH to receiver address
        // User would confirm the transaction information in Face wallet modal
        // Then user would enter the PIN code in Face wallet modal
        const tx = await signer.sendTransaction({
            to: receiverAddress,
            value: amount,
        });

        // If you want to use 12 confirmations, set 'confirms' parameter to 12
        const receipt = await tx.wait();
        const txHash = receipt.transactionHash;
    },

    SignMessage: async (message) => {
        const signer = FacewalletInterop.provider.getSigner();

        const originalMessage = message;
        const signedMessage = await signer.signMessage(originalMessage);

        return signedMessage;
    },

    GetContract: async (ContractAddress) => {
        const signer = FacewalletInterop.provider.getSigner();
        const userAddress = await signer.getAddress();

        // BEP20 smart contract
        const contractABI = '[{"constant":true,"inputs":[],"name":"name","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"_spender","type":"address"},{"name":"_value","type":"uint256"}],"name":"approve","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[],"name":"totalSupply","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"_from","type":"address"},{"name":"_to","type":"address"},{"name":"_value","type":"uint256"}],"name":"transferFrom","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[],"name":"decimals","outputs":[{"name":"","type":"uint8"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"_owner","type":"address"}],"name":"balanceOf","outputs":[{"name":"balance","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"symbol","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"_to","type":"address"},{"name":"_value","type":"uint256"}],"name":"transfer","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"_owner","type":"address"},{"name":"_spender","type":"address"}],"name":"allowance","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"payable":true,"stateMutability":"payable","type":"fallback"},{"anonymous":false,"inputs":[{"indexed":true,"name":"owner","type":"address"},{"indexed":true,"name":"spender","type":"address"},{"indexed":false,"name":"value","type":"uint256"}],"name":"Approval","type":"event"},{"anonymous":false,"inputs":[{"indexed":true,"name":"from","type":"address"},{"indexed":true,"name":"to","type":"address"},{"indexed":false,"name":"value","type":"uint256"}],"name":"Transfer","type":"event"}]';
        const contractAddress = ContractAddress;
        const contract = new ethers.Contract(contractAddress, contractABI, signer);

        // Get the data of smart contract
        const balance = await contract.balanceOf(userAddress);
        console.log(balance);

        return balance;
    },

    Transfer: async (ReceiverAddress, ContractAddress) => {
        const signer = FacewalletInterop.provider.getSigner();
        const userAddress = await signer.getAddress();

        // DApp should choose the input required to send transaction
        const receiverAddress = ReceiverAddress;
        const amount = ethers.utils.parseUnits('0.01', 18);

        // BEP20 smart contract
        const contractABI = '[{"constant":true,"inputs":[],"name":"name","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"_spender","type":"address"},{"name":"_value","type":"uint256"}],"name":"approve","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[],"name":"totalSupply","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"_from","type":"address"},{"name":"_to","type":"address"},{"name":"_value","type":"uint256"}],"name":"transferFrom","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[],"name":"decimals","outputs":[{"name":"","type":"uint8"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[{"name":"_owner","type":"address"}],"name":"balanceOf","outputs":[{"name":"balance","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":true,"inputs":[],"name":"symbol","outputs":[{"name":"","type":"string"}],"payable":false,"stateMutability":"view","type":"function"},{"constant":false,"inputs":[{"name":"_to","type":"address"},{"name":"_value","type":"uint256"}],"name":"transfer","outputs":[{"name":"","type":"bool"}],"payable":false,"stateMutability":"nonpayable","type":"function"},{"constant":true,"inputs":[{"name":"_owner","type":"address"},{"name":"_spender","type":"address"}],"name":"allowance","outputs":[{"name":"","type":"uint256"}],"payable":false,"stateMutability":"view","type":"function"},{"payable":true,"stateMutability":"payable","type":"fallback"},{"anonymous":false,"inputs":[{"indexed":true,"name":"owner","type":"address"},{"indexed":true,"name":"spender","type":"address"},{"indexed":false,"name":"value","type":"uint256"}],"name":"Approval","type":"event"},{"anonymous":false,"inputs":[{"indexed":true,"name":"from","type":"address"},{"indexed":true,"name":"to","type":"address"},{"indexed":false,"name":"value","type":"uint256"}],"name":"Transfer","type":"event"}]';
        const contractAddress = ContractAddress;
        const contract = new ethers.Contract(contractAddress, contractABI, signer);

        // Send a transaction to transfer BEP20 token
        const balance = await contract.transfer(userAddress, amount);
        console.log(balance);

        return balance;
    },

    GetMethods: () => {
        let signer = FacewalletInterop.provider.getSigner();
        let currentObj = signer;
        let properties = new Set();
        do {
            Object.getOwnPropertyNames(currentObj).map(item => properties.add(item))
        }
        while ((currentObj = Object.getPrototypeOf(currentObj)))

        console.log([...properties.keys()].filter(item => typeof signer[item] === 'function'));
    }
}