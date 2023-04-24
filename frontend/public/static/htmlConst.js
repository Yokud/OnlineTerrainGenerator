export const optionsConst = {
    title: 'Данные',
    jsIdError: 'js-error-text',
    info: 'Введите данные для получения чего-то там. Поля со звездочкой обязательны для заполнения',
    inputFields: {
        func: { help: 'Ф-я преобразования карты высот', jsIdInput: 'js-func', jsIdError: 'js-func-error', necessarily: true},
        alg: { help: 'Алгоритм', jsIdInput: 'js-alg', jsIdError: 'js-alg-error'}
    },
    algFields: {
        first: 'Diamond-Square',
        second: 'Шум Перлина',
        third: 'Симплексный шум',
    },
    inputOptionsField: null,
    inputOptionsFieldFirst: [{help: 'шероховатость', necessarily: true}, {help: 'зерно генерации', necessarily: false}],
    inputOptionsFieldSecond: [{help: 'масштаб', necessarily: true}, {help: 'кол-во октав', necessarily: true}, {help: 'лакунарность', necessarily: true}, {help: 'стойкость', necessarily: true}, {help: 'зерно генерации', necessarily: false}],
    inputOptionsFieldThird: [{help: 'масштаб', necessarily: true}, {help: 'кол-во октав', necessarily: true}, {help: 'лакунарность', necessarily: true}, {help: 'стойкость', necessarily: true}, {help: 'зерно генерации', necessarily: false}],
    logo: 'static/img/logo.svg',
    smile: 'static/img/sad.svg',
    logoText: 'OnlineTerrainGenerator',
    noImgText: 'Вы еще не отправили данные для генерации',
    result: false,
    //result: 'static/img/testImg.svg',
}
