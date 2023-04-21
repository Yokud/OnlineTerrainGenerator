import Dispatcher from "../dispatcher/dispatcher.js";

export const actionTerrain = {
    getTerrain(data) {
        Dispatcher.dispatch({
            actionName: 'getTerrain',
            data,
        });
    },
};
