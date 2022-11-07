import { Face, Network } from '@haechi-labs/face-sdk';
import { ethers } from 'ethers';

export async function facewallet() {
    // Setting the network to Ethereum
    const face = new Face({
        network: Network.GOERLI,
        apiKey: 'API KEY'
    });
    const provider = new ethers.providers.Web3Provider(face.getEthLikeProvider());

    await face.auth.login();
}