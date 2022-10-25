export const NethereumMetamaskInterop = {
	Init: (provider) => {
		NethereumMetamaskInterop.Provider = provider;
		NethereumMetamaskInterop.SetListener();
	},

	SetListener: () => {
		if (window.ethereum) {
			console.log("SetListener");
			ethereum.on("accountsChanged",
				(accounts) => {
					if (accounts.length === 0) {
						console.log("all disconnect");
						//DotNet.invokeMethodAsync('Market', 'SelectedAccountChanged', "");
						NethereumMetamaskInterop.Provider.invokeMethodAsync('ChangeSelectedAccountAsync', "");
					}
					else {
						console.log(accounts[0]);
						//DotNet.invokeMethodAsync('Market', 'SelectedAccountChanged', accounts[0]);
						NethereumMetamaskInterop.Provider.invokeMethodAsync('ChangeSelectedAccountAsync', accounts[0]);
					}
				}
			);

			ethereum.on('chainChanged',
				(chainId) => {
					//DotNet.invokeMethodAsync('Market', 'ChainChanged', parseInt(chainId));
					NethereumMetamaskInterop.Provider.invokeMethodAsync('ChainChangedAsync', parseInt(chainId));
				}
			);
		}
	},

	EnableEthereum: async () => {
		try {
			const accounts = await ethereum.request({ method: 'eth_requestAccounts' });
			ethereum.autoRefreshOnNetworkChange = false;
			//ethereum.on("accountsChanged",
			//	function (accounts) {
			//		if (accounts.length === 0) {
			//			console.log("all disconnect");
			//			DotNet.invokeMethodAsync('Market', 'SelectedAccountChanged', "");
			//		}
			//		else {
			//			console.log(accounts[0]);
			//			DotNet.invokeMethodAsync('Market', 'SelectedAccountChanged', accounts[0]);
			//		}
			//	});
			//ethereum.on("networkChanged",
			//	function (networkId) {

			//	});
			//ethereum.on("disconnect",
			//	function (error) {
			//		console.log(error);
			//	});

			return accounts[0];
		} catch (error) {
			console.log(error)
			if (error.code === 4001) {
				DotNet.invokeMethodAsync('Market', 'MetamaskUserDenied');
			}
			return null;
		}
	},

	IsConnected: () => {
		return ethereum.isConnected();
	},

	IsMetamaskAvailable: () => {
		if (window.ethereum) return true;
		return false;
	},
	GetSelectedAddress: () => {
		console.log(ethereum);
		return ethereum.selectedAddress;
	},

	Request: async (message) => {
		try {

			let parsedMessage = JSON.parse(message);
			console.log(parsedMessage);
			const response = await ethereum.request(parsedMessage);
			let rpcResonse = {
				jsonrpc: "2.0",
				result: response,
				id: parsedMessage.id,
				error: null
			}
			console.log(rpcResonse);

			return JSON.stringify(rpcResonse);
		} catch (e) {
			console.log(e);
			console.log(message);
			let parsedMessage = JSON.parse(message);
			//DotNet.invokeMethodAsync('Market', 'MetamaskUserDenied');
			let rpcResonseError = {
				jsonrpc: "2.0",
				id: parsedMessage.id,
				Error: {
					Message: e.message,
				}
			}
			return JSON.stringify(rpcResonseError);
		}
	},

	Send: async (message) => {
		return new Promise(function (resolve, reject) {
			console.log(JSON.parse(message));
			ethereum.send(JSON.parse(message), function (error, result) {
				console.log(result);
				console.log(error);
				resolve(JSON.stringify(result));
			});
		});
	},

	Sign: async (utf8HexMsg) => {
		const from = ethereum.selectedAddress;
		const params = [utf8HexMsg, from];
		const method = 'personal_sign';

		return await ethereum.request({
			method, params
		}).then((result) => {
			return result;
		}).catch((error) => {
			return '';
		});
	},

	GetChainID: async () => {
		const chainId = await ethereum.request({ method: 'eth_chainId' });
		return parseInt(chainId);
	},

	SwitchEthereumChain: async (
		chainId,
		chainName,
		symbol,
		decimals,
		rpcUrls,
		blockExplorerUrls
	) => {
		return await ethereum
			.request({
				method: 'wallet_switchEthereumChain',
				params: [{ chainId: chainId }],
			})
			.then((result) => {
				return true;
			})
			.catch((error) => {
				if (error.code === 4001) {
					// EIP-1193 userRejectedRequest error
					console.log("wallet_switchEthereumChain userRejectedRequest");
				}
				else if (error.code === 4902) {
					//Unrecognized chain ID
					 return ethereum
						.request({
							method: 'wallet_addEthereumChain',
							params: [
								{
									chainId: chainId,
									chainName: chainName,
									nativeCurrency: {
										symbol: symbol,
										decimals: decimals,
									},
									rpcUrls: rpcUrls,
									blockExplorerUrls: blockExplorerUrls
								},
							],
						})
						.then((result) => {
							return true;
						})
						.catch((error) => {
							console.log(error);
							return false;
						});
				}
				else {
					console.log(error);
				}
				return false;
			});
	},
}