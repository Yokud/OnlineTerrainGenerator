import Dispatcher from "../dispatcher/dispatcher.js";

export const actionTerrain = {
    send(func, alg, options) {
        Dispatcher.dispatch({
            actionName: 'send',
            func,
            alg,
            options,
        });
    },
    download() {
        Dispatcher.dispatch({
            actionName: 'download',
        });
    },
    update(func, alg, options) {
        Dispatcher.dispatch({
            actionName: 'update',
            func,
            alg,
            options,
        });
    },
};
