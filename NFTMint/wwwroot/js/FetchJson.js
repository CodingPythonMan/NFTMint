export const FetchJson = {
    Init: () => {
        fetch('../../../Config/WebServerConfigDev.json')
            .then((response) => response.json())
            .then((json) => console.log(json));
    }
}