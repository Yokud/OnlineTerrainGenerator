export const optionsConst = {
    title: 'Данные',
    jsIdError: 'js-error-text',
    info: 'Введите данные для получения чего-то там. Поля со звездочкой обязательны для заполнения',
    inputFields: {
        func: { help: 'Ф-я преобразования карты высот', jsIdInput: 'js-func', jsIdError: 'js-func-error', necessarily: true, text: ''},
        alg: { help: 'Алгоритм', jsIdInput: 'js-alg', jsIdError: 'js-alg-error'}
    },
    algFields: {
        first: 'Diamond-Square',
        second: 'Шум Перлина',
        third: 'Симплексный шум',
    },
    inputOptionsField: null,
    inputOptionsFieldFirst: [
        {help: 'шероховатость', jsIdInput: 'js-1', jsIdError: 'js-1-error', necessarily: true, text: ''},
        {help: 'зерно генерации', jsIdInput: 'js-2', jsIdError: 'js-2-error', necessarily: false, text: ''}],
    inputOptionsFieldSecond: [
        {help: 'масштаб', jsIdInput: 'js-3', jsIdError: 'js-3-error', necessarily: true, text: ''},
        {help: 'кол-во октав', jsIdInput: 'js-4', jsIdError: 'js-4-error', necessarily: true, text: ''},
        {help: 'лакунарность', jsIdInput: 'js-5', jsIdError: 'js-5-error', necessarily: true, text: ''},
        {help: 'стойкость', jsIdInput: 'js-6', jsIdError: 'js-6-error', necessarily: true, text: ''},
        {help: 'зерно генерации', jsIdInput: 'js-7', jsIdError: 'js-7-error', necessarily: false, text: ''}],
    inputOptionsFieldThird: [
        {help: 'масштаб', jsIdInput: 'js-3', jsIdError: 'js-3-error', necessarily: true, text: ''},
        {help: 'кол-во октав', jsIdInput: 'js-4', jsIdError: 'js-4-error', necessarily: true, text: ''},
        {help: 'лакунарность', jsIdInput: 'js-5', jsIdError: 'js-5-error', necessarily: true, text: ''},
        {help: 'стойкость', jsIdInput: 'js-6', jsIdError: 'js-6-error', necessarily: true, text: ''},
        {help: 'зерно генерации', jsIdInput: 'js-7', jsIdError: 'js-7-error', necessarily: false, text: ''}],
    buttonInfo: { text: 'Сгенерировать ландшафт', jsId: 'js-send'},
    logo: 'static/img/logo.svg',
    smile: 'static/img/sad.svg',
    logoText: 'OnlineTerrainGenerator',
    noImgText: 'Вы еще не отправили данные для генерации',
    result: false,
    //result: 'static/img/testImg.svg',
}
