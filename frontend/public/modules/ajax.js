class Ajax {
    constructor() {
        this.backendHostname = 'localhost';
        this.backendPort = '7252';
        this._backendUrl = 'https://' + this.backendHostname + ':' + this.backendPort;

        this._apiUrl = {
            send: '/api/HeightMap/colored',
            download: '/api/HeightMap/grayscaled',
            update: '/api/HeightMap',
        }

        this._requestType = {
            GET: 'GET',
            PUT: 'PUT',
        }
    }

    _request(apiUrlType, requestType, body) {
        const requestUrl = this._backendUrl + apiUrlType;

        return fetch(requestUrl, {
            method: requestType,
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body,
        });
    }

    async send(func, alg, options) {
        if (alg === "1") {
            alg = 'DiamondSquare';
        } else if (alg === "2") {
            alg = 'PerlinNoise';
        } else  if (alg === "3") {
            alg = 'SimplexNoise';
        }
        let body = {func: func, alg: alg, options: options};
        return this._request(this._apiUrl.send + '?heightMapParams=' + JSON.stringify(body), this._requestType.GET);
    }

    async download() {
        return this._request(this._apiUrl.download, this._requestType.GET);
    }

    async update(func, alg, options) {
        if (alg === "1") {
            alg = 'DiamondSquare';
        } else if (alg === "2") {
            alg = 'PerlinNoise';
        } else  if (alg === "3") {
            alg = 'SimplexNoise';
        }
        let body = {func: func, alg: alg, options: options};
        return this._request(this._apiUrl.update + '?heightMapParams=' + JSON.stringify(body), this._requestType.PUT);
    }
}

export default new Ajax();
