$(function () {

    const now = new Date();
    const datepicker = $('#datepicker').datepicker({
        setDate: now,
        maxDate: now,
        defaultDate: now,
        closeText: 'Закрыть',
        prevText: '',
        currentText: 'Сегодня',
        monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
            'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
        monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн',
            'Июл', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
        dayNames: ['воскресенье', 'понедельник', 'вторник', 'среда', 'четверг', 'пятница', 'суббота'],
        dayNamesShort: ['вск', 'пнд', 'втр', 'срд', 'чтв', 'птн', 'сбт'],
        dayNamesMin: ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'],
        weekHeader: 'Не',
        dateFormat: 'dd.mm.yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    }
    );

    const alert = $('#alert');
    const result = $('#result');
    const loader = $('#loader');



    $('#form').click(function (e) {
        e.preventDefault();

        const target = e.target;
        if (target.type == "button") {

            alert.empty();
            result.empty();



            const cur = datepicker.val();

            if (cur.length == 10) {

                loader.show();

                const d = cur.substr(6, 4) + cur.substr(3, 2) + cur.substr(0, 2);
                let jqxhr;

                if (target.id == "downloadBtn") {
                    jqxhr = $.post('/Home/Download/', { date: d }, function (data) {

                        if (data.success) {
                            alert.html(`<div class="alert alert-success">Файл от  ${cur}  успешно загружен</div >`);
                        }
                        else {
                            alert.html(`<div class="alert alert-danger">${data.message}</div>`);
                        }
                    });
                    //console.log(jqxhr);
                }
                else if (target.id == "loadBtn")
                    jqxhr = $.post('/Entity/Add/', { date: d }, function (data) {


                        if (data.success) {
                            alert.html(`<div class="alert alert-success">В БД успешно загружено  ${data.count} новых записей  от  ${cur} </div >`);
                        }
                        else {
                            alert.html(`<div class="alert alert-danger">${data.message}</div>`);
                        }
                    });
                else if (target.id == "displayBtn")
                    jqxhr = $.post('/Entity/Get/', { date: d }, function (data) {
                        if (data.success) {
                            if (data.data.length > 0) {
                                showData(data.data, cur);
                            }
                            else {
                                alert.html(`<div class="alert alert-success">В БД нет записей от  ${cur}. Скачайте и загрузите их</div >`);
                            }
                        }
                        else {
                            alert.html(`<div class="alert alert-danger">${data.message}</div>`);
                        }
                    });

                jqxhr.always(function () { loader.hide(); });

            }
            else {
                alert.html(`<div class="alert alert-danger">Поле "Выберите дату" обязательно для заполнения</div >`);
            }


        }


    });

    function showData(data, date) {
        let s = `<h3>Отчет об объемах суммарных нагрузочных потерь (агрегированно по субъектам РФ)</h3>
        <p>Дата: ${date} </p>
        <table class="table"><tr><th>Субъект РФ</th><th>Объем потерь, МВтЧ.</th></tr>`;

        let i, len;
        for (i = 0, len = data.length; i < len; ++i) {
            s += '<tr><td>' + data[i].Region + '</td><td>' + data[i].Amount + '</td></tr>\n';
        }
        s += '</table>';

        result.html(s);

    }

});