import Dispatcher from '../dispatcher/dispatcher.js';
import Ajax from "../modules/ajax.js";
import {optionsConst} from "../static/htmlConst.js";

class terrainStore {
    constructor() {
        this._callbacks = [];

        this.result = null;

        Dispatcher.register(this._fromDispatch.bind(this));
    }

    registerCallback(callback) {
        this._callbacks.push(callback);
    }

    _refreshStore() {
        this._callbacks.forEach((callback) => {
            if (callback) {
                callback();
            }
        });
    }

    async _fromDispatch(action) {
        switch (action.actionName) {
            case 'send':
                await this._send(action.func, action.alg, action.options);
                break;
            case 'download':
                await this._download(action.data);
                break;
            case 'update':
                await this._update(action.data);
                break;
            default:
                return;
        }
    }

    async _send(func, alg, options) {
        const request = await Ajax.send(func, alg, options);

        if (request.status === 200) {
            const response = await request.json();
            console.log(response);
            optionsConst.result = 'static/img/testImg.svg';
        } else {
            alert('send error');
        }
        this._refreshStore();
    }

    async _download(data) {
        const request = await Ajax.download(data);

        if (request.status === 200) {
            const response = await request.json();
            console.log(response);
        } else {
            alert('download error');
        }
        this._refreshStore();
    }

    async _update(data) {
        const request = await Ajax.update(data);

        if (request.status === 200) {
            const response = await request.json();
            console.log(response);
        } else {
            alert('update error');
        }
        this._refreshStore();
    }
}

export default new terrainStore();
