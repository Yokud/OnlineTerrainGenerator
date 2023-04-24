export const optionsConst = {
    title: 'Данные',
    jsIdError: 'js-error-text',
    info: 'Введите данные для получения чего-то там. Поля со звездочкой обязательны для заполнения',
    inputFields: {
        func: { help: 'Ф-я преобразования карты высот', type: 'text', jsIdInput: 'js-func', jsIdError: 'js-func-error'},
        alg: { help: 'Алгоритм', type: 'text', jsIdInput: 'js-alg', jsIdError: 'js-alg-error'}
    },
    algFields: {
        first: 'Diamond-Square',
        second: 'Шум Перлина',
        third: 'Симплексный шум',
    },
    inputOptionsField: null,
    inputOptionsFieldFirst: [{name: 'шероховатость', necessarily: true}, {name: 'зерно генерации', necessarily: false}],
    inputOptionsFieldSecond: [{name: 'масштаб', necessarily: true}, {name: 'кол-во октав', necessarily: true}, {name: 'лакунарность', necessarily: true}, {name: 'стойкость', necessarily: true}, {name: 'зерно генерации', necessarily: false}],
    inputOptionsFieldThird: [{name: 'масштаб', necessarily: true}, {name: 'кол-во октав', necessarily: true}, {name: 'лакунарность', necessarily: true}, {name: 'стойкость', necessarily: true}, {name: 'зерно генерации', necessarily: false}],
    logo: 'static/img/logo.svg',
    smile: 'static/img/sad.svg',
    logoText: 'OnlineTerrainGenerator',
    noImgText: 'Вы еще не отправили данные для генерации',
    result: false,
    //result: 'static/img/testImg.svg',
}
