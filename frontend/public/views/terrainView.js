import terrainStore from "../stores/terrainStore.js";

export default class TerrainView {
    constructor() {
        this._addHandlebarsPartial();

        this._jsId = 'terrain';
        this._rootElement = document.getElementById('root');

        this._validateEmail = false;

        terrainStore.registerCallback(this.updatePage.bind(this))
    }

    _addHandlebarsPartial() {
        Handlebars.registerPartial('terrain', Handlebars.templates.terrain);
        Handlebars.registerPartial('button', Handlebars.templates.button);
        Handlebars.registerPartial('inputField', Handlebars.templates.inputField);
    }

    _addPagesElements() {

    }

    _addPagesListener() {

    }

    updatePage() {
        this._render();
    }

    _preRender() {
        this._template = Handlebars.templates.terrain;

        this._context = {
            logoData: 'test',
            result: 'static/img/testImg.svg'
        }
    }

    _render() {
        this._preRender();
        this._rootElement.innerHTML = this._template(this._context);
        this._addPagesElements();
        this._addPagesListener();
    }
}
