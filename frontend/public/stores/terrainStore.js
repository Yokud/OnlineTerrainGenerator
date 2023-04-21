import Dispatcher from '../dispatcher/dispatcher.js';
import Ajax from "../modules/ajax.js";

class terrainStore {
    constructor() {
        this._callbacks = [];

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
            alert('200');
        } else {
            alert('error');
        }
        this._refreshStore();
    }
}

export default new terrainStore();
