$(function () {
    $('.navbar-toggle').click(function () {
        $('.navbar-nav').toggleClass('slide-in');
        $('.side-body').toggleClass('body-slide-in');
        $('#search').removeClass('in').addClass('collapse').slideUp(200);

        /// uncomment code for absolute positioning tweek see top comment in css
        //$('.absolute-wrapper').toggleClass('slide-in');

    });

    // Remove menu for searching
    $('#search-trigger').click(function () {
        $('.navbar-nav').removeClass('slide-in');
        $('.side-body').removeClass('body-slide-in');

        /// uncomment code for absolute positioning tweek see top comment in css
        //$('.absolute-wrapper').removeClass('slide-in');
    });
});

//Обновление результата
function onCompleteUpdate(controller, action) {
    $.ajax({
        url: '/' + controller + '/' + action,
        cache: false,
        success: function (html) {
            $('#' + action).html(html);
        }
    });
};

function onCompleteUpdatePage(controller, action, page) {
    $.ajax({
        url: '/' + controller + '/' + action + '/' + page,
        cache: false,
        success: function (html) {
            $('#' + action).html(html);
        }
    });
}

//очистка формы
function OnSuccess(idForm) {
    $('#' + idForm)[0].reset();
};

//включаем показ ограничения
$('input[maxlength]').maxlength({
    threshold: 20
});

