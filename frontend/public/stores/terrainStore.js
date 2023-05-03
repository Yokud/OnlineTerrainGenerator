import Dispatcher from '../dispatcher/dispatcher.js';
import Ajax from "../modules/ajax.js";
import {optionsConst} from "../static/htmlConst.js";
import TerrainView from "../views/terrainView.js"

class terrainStore {
    constructor() {
        this._callbacks = [];

        this.result = null;
        this.flag = false;

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
                await this._download();
                break;
            case 'update':
                await this._update(action.func, action.alg, action.options);
                break;
            default:
                return;
        }
    }

    async _send(func, alg, options) {
        const request = await Ajax.send(func, alg, options);

        if (request.status === 200) {
            const response = await request.json();
            
            optionsConst.result = response.data;
            this.flag = true;
        } else {
            optionsConst.result = false;
        }

        this._refreshStore();
    }

    async _download() {
        const request = await Ajax.download();

        if (request.status === 200) {
            const response = await request.json();
        } else {
            alert('download error');
        }
        this._refreshStore();
    }

    async _update(func, alg, options) {
        const request = await Ajax.update(func, alg, options);

        if (request.status === 200) {
            const response = await request.json();
            optionsConst.result = response.data;
        } else {
            optionsConst.result = false;
        }
        this._refreshStore();
    }
}

export default new terrainStore();
