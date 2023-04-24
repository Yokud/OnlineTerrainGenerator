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
            case 'getTerrain':
                await this._getTerrain(action.data);
                break;
            default:
                return;
        }
    }

    async _getTerrain(data) {
        const request = await Ajax.getTerrain(data);

        if (request.status === 200) {
            optionsConst.result = 'static/img/testImg.svg';
        } else {
            alert('error');
        }
        this._refreshStore();
    }
}

export default new terrainStore();
