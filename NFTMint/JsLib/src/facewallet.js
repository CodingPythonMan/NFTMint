import { Face, Network } from '@haechi-labs/face-sdk';
import { ethers } from 'ethers';

export const FacewalletInterop = {
    Init: (apikey) => {
        FacewalletInterop.face = new Face({
            network: Network.BNB_SMART_CHAIN_TESTNET,
            apiKey: apikey
        });

        FacewalletInterop.provider = new ethers.providers.Web3Provider(FacewalletInterop.face.getEthLikeProvider());
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
    }
}