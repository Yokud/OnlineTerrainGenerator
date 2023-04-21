/**
 * класс, реализующий диспетчер
 */
class Dispatcher {
    /**
     * @constructor
     * конструктор метода
     */
    constructor() {
        this._callbacks = [];
        this._isDispatching = false;
        this._pendingPayload = null;
    }

    /**
     * метод, регистрирующий новый коллбек в диспетчере
     * @param {Function} callback функция-коллбек
     */
    register(callback) {
        this._callbacks.push(callback);
    }

    /**
     * метод, удаляющий регистрацию коллбека
     * @param {int} id
     */
    unregister(id) {
        delete this._callbacks[id];
    }

    /**
     * метод, организующий рассылку
     * @param {Object} payload
     */
    dispatch(payload) {
        if (this._isDispatching) {
            throw new Error("Cannot dispatch in the middle of a dispatch");
        }
        this._isDispatching = true;
        this._pendingPayload = payload;
        try {
            this._callbacks.forEach((callback) => {
                if (callback) {
                    callback(this._pendingPayload);
                }
            });
        } finally {
            this._pendingPayload = null;
            this._isDispatching = false;
        }
    }
}

export default new Dispatcher();
