import terrainStore from "../stores/terrainStore.js";
import {optionsConst} from "../static/htmlConst.js";
import {actionTerrain} from "../actions/actionTerrain.js";

export default class TerrainView {
    constructor() {
        this._addHandlebarsPartial();

        this._rootElement = document.getElementById('root');

        terrainStore.registerCallback(this.updatePage.bind(this))
    }

    _addHandlebarsPartial() {
        Handlebars.registerPartial('terrain', Handlebars.templates.terrain);
        Handlebars.registerPartial('button', Handlebars.templates.button);
        Handlebars.registerPartial('inputField', Handlebars.templates.inputField);
    }

    _addPagesElements() {
        this._func = window.document.getElementById('js-func');
        this._funcError = window.document.getElementById('js-func-error');
        this._algoritm = window.document.getElementById('js-alg');
        this._algoritmError = window.document.getElementById('js-alg-error');

        this._send = document.getElementById("js-send");
    }

    _addPagesListener() {
        this._send.addEventListener('click', () => {
            if (this._algoritm.value === '0') {
                this._algoritm.classList.remove('menu__field-correct');
                this._algoritm.classList.add('menu__field-incorrect');
                this._algoritmError.textContent = 'Выберите алгоритм генерации ландшавта';
            } else {
                let flagCorrect = true;
                optionsConst.inputOptionsField.forEach((field) => {
                    const fieldElem = window.document.getElementById(field.jsIdInput);
                    const fieldElemError = window.document.getElementById(field.jsIdError);

                    console.log(fieldElem, fieldElem.necessarily);
                    if (!fieldElem.value && field.necessarily === true) {
                        fieldElem.classList.remove('input-block__field-correct');
                        fieldElem.classList.add('input-block__field-incorrect');
                        fieldElemError.textContent = 'Это поле не может быть пустым';
                        flagCorrect = false;
                    }

                    if (fieldElem.value) {
                        field.text = fieldElem.value;
                    }
                });

                if (flagCorrect) {
                    actionTerrain.getTerrain('test');
                }
            }

        });

        this._func.addEventListener('change', () => {
            // ToDo: validation
        });

        this._func.addEventListener('change', () => {
            optionsConst.inputFields.func.text = this._func.value;
        });

        let that = this;
        this._algoritm.addEventListener("change", function() {
            that.updatePage();
        });
    }

    updatePage() {
        this._render();
    }

    _preRender() {
        this._template = Handlebars.templates.terrain;

        if (this._algoritm) {
            this._curCombo = this._algoritm.value;
            if (this._algoritm.value === '1') {
                optionsConst.inputOptionsField = optionsConst.inputOptionsFieldFirst;
            } else if (this._algoritm.value === '2') {
                optionsConst.inputOptionsField = optionsConst.inputOptionsFieldSecond;
            } else if (this._algoritm.value === '3') {
                optionsConst.inputOptionsField = optionsConst.inputOptionsFieldThird;
            } else {
                optionsConst.inputOptionsField = null;
            }
        }

        this._context = optionsConst;
    }

    _render() {
        this._preRender();
        this._rootElement.innerHTML = this._template(this._context);
        this._addPagesElements();
        this._addPagesListener();

        if (this._curCombo) {
            this._algoritm.value = this._curCombo;
        }
    }
}
