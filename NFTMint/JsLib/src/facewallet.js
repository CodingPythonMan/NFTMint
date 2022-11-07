import { Face, Network } from '@haechi-labs/face-sdk';
import { ethers } from 'ethers';

export async function login() {
    // Setting the network to Ethereum
    const face = new Face({
        network: Network.BNB_SMART_CHAIN_TESTNET,
        apiKey: 'API KEY'
        
    });
    const provider = new ethers.providers.Web3Provider(face.getEthLikeProvider());

    await face.auth.login();
}