/**
 * —оздание структуры карусели
 
 использование
  <div class="directoryCarousel" data-directory="/Content/UserFiles/Images/Publishing/2016/10/01/" data-numslides="4" data-extension="jpg"></div>
 $(document).ready(function () {
        var dc = $('.directoryCarousel');
        $('.directoryCarousel').directoryCarousel({ directory: dc.data('directory'), numslides: dc.data('numslides'), extension: dc.data('extension'), width: '100%' });
    });
 */

(function ($) {
    var directoryCarousel = function (element, options) {
        var elem = $(element),
            obj = this,
            elemId = elem[0].id;

        var config = $.extend({
            filebase: 'slide_',
            extension: 'jpg',
            directory: null,
            numslides: null,
            width: null
        }, options || {});

        if (config.width) {
            $(elem).css('width', config.width);
        }

        var slides = [];
        var indicators = [];

        slideNumber = 0;
        while (slideNumber <= config.numslides - 1) {
            var active = '';
            if (slideNumber == 0) {
                active = 'clas="active"';
            }
            indicators.push('<li data-target="#carousel" data-slide-to="' + slideNumber + '" ' + active + '"></li>');
            slideNumber++;
        }

        slideNumber = 1;
        while (slideNumber <= config.numslides) {
            var active;
            if (slideNumber == 1) {
                active = 'clas="item active"';
            } else {
                active = 'class="item"';
            }
            slides.push('<div ' + active + '><img src="' + config.directory + config.filebase + slideNumber + '.' + config.extension + '" style="max-width:' + config.width + ';" alt="" /></div>');
            slideNumber++;
        }

        var carouselWrap = $('<div class="carousel slide" id="carousel" data-ride="carousel"></div>');
        carouselWrap.appendTo(elem);
        var indicatorsWrap = $('<ol class="carousel-indicators" style="margin-bottom: -10px;"></ol>');
        indicatorsWrap.appendTo(carouselWrap);

        $.each(indicators, function (index, val) {
            $(val).appendTo(indicatorsWrap);
        });

        var innerWrap = $('<div class="carousel-inner" role="listbox"></div>');
        innerWrap.appendTo(carouselWrap);

        $.each(slides, function (index, val) {
            $(val).appendTo(innerWrap);
        });

        var buttons = $('<a class="leftCarousel carousel-control" href="#carousel" data-slide="prev"><span class="fa fa-arrow-circle-o-left" style="font-size: 1.8em;"></span></a><a class="rightCarousel carousel-control" href="#carousel" data-slide="next"><span class="fa fa-arrow-circle-o-right" style="font-size: 1.8em;"></span></a>');
        buttons.appendTo(carouselWrap);

        $(elem).removeAttr('data-directory').removeAttr('data-numslides').removeAttr('data-extension');
    };

    $.fn.directoryCarousel = function (options) {
        return this.each(function () {
            var element = $(this);
            // Return early if this element already has a plugin instance
            if (element.data('directorycarousel')) return;
            // pass options to plugin constructor
            var directorycarousel = new directoryCarousel(this, options);
            // Store plugin object in this element's data
            element.data('directorycarousel', directorycarousel);
        });
    };
})(jQuery);