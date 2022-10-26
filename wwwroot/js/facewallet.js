import { Face, Network } from '@haechi-labs/face-sdk';
import { ethers } from 'ethers';

const face = new Face({
    network: Network.BNB_SMART_CHAIN_TESTNET,
    apiKey: 'YOUR_DAPP_API_KEY'
});
const provider = new ethers.providers.Web3Provider(face.getEthLikeProvider());