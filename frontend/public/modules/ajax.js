class Ajax {
    constructor() {
        this.backendHostname = '127.0.0.1';
        this.backendPort = '8080';
        this._backendUrl = 'http://' + this.backendHostname + ':' + this.backendPort;

        this._apiUrl = {
            sendParam: '',
        }

        this._requestType = {
            GET: 'GET',
            POST: 'POST',
            DELETE: 'DELETE',
            PATCH: 'PATCH',
        }
    }

    _request(apiUrlType, requestType, body) {
        const requestUrl = this._backendUrl + apiUrlType;

        return fetch(requestUrl, {
            method: requestType,
            mode: "cors",
            credentials: "include",
            headers: a,
            body,
        });
    }

    async getTerrain(data) {
        return {text: 'test'};
    }
}

export default new Ajax();
