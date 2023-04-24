import terrainStore from "../stores/terrainStore.js";
import {optionsConst} from "../static/htmlConst.js";

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
        this._algoritm = window.document.getElementById('js-alg');
        this._algoritmMenu = window.document.getElementById('js-alg-menu');
        this._myCombo = document.getElementById("myCombo");
    }

    _addPagesListener() {
        this._func.addEventListener('change', () => {
            // ToDo: validation
        });

        this._myCombo.addEventListener("change", function() {

        });
    }

    updatePage() {
        this._render();
    }

    _preRender() {
        this._template = Handlebars.templates.terrain;

        if (this._myCombo) {
            if (this._myCombo.value === 'Diamond-Square') {
                optionsConst.inputOptionsField = optionsConst.inputOptionsFieldFirst;
            } else if (this._myCombo.value === 'Шум Перлина') {
                optionsConst.inputOptionsField = optionsConst.inputOptionsFieldSecond;
            } else if (this._myCombo.value === 'Симплексный шум') {
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
    }
}
