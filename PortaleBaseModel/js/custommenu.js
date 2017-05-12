

/* ------------------------------------------------

    Navigation

--------------------------------------------------- */


/* ------------------------------------------------
    Responsive Scripts for Navigation
--------------------------------------------------- */


function navResponsive() {

    if (screenWidth > 990) {

        $(".main-nav .navbar-nav > .dropdown > a").attr("data-toggle", "");
        $(".main-nav .navbar-nav.nav-search > .dropdown > a").attr("data-toggle", "dropdown");
        $('.main-nav .navbar-nav > .dropdown').removeClass('open');
        $('.main-nav .navbar-nav .dropdown-submenu').removeClass('open');
        $('.main-nav .navbar-nav > li').find(':focus').blur();
        if ($('.main-nav .navbar-collapse').hasClass('in')) {
            $('.main-nav .navbar-collapse').removeClass('in');
        }
        if ($('.navbar-toggle').hasClass('active')) {
            $('.navbar-toggle').removeClass('active');
        }

    }
    else if (screenWidth <= 991) {

        $(".main-nav .navbar-nav > .dropdown > a").attr("data-toggle", "dropdown");
        $('.main-nav .nav > li .dropdown-menu').removeAttr('style');
        $('.main-nav .nav > li > .dropdown-menu').removeAttr('style');

    }
}


/* ------------------------------------------------
    Navigation's Click, Hover and Keyup Events
--------------------------------------------------- */


function navEvents() {

    /*---- Dropdown Menu Events ----*/

    $('.main-nav .navbar-nav > .dropdown > .dropdown-menu').click(function (event) {
        if (screenWidth <= 991) {
            event.stopPropagation();
        }
    });

    $(".main-nav .navbar-nav>.dropdown>.dropdown-menu>.dropdown-submenu").click(function (event) {
        if (screenWidth < 991) {
            $this = $(this);
            $this.siblings(".dropdown-submenu").removeClass("open").end();
            $this.parents(".dropdown-submenu").addClass('open');
            $this.toggleClass('open');
            event.stopPropagation();
        }
    });

    $('.main-nav .navbar-nav > .dropdown > a').click(function (event) {
        $('.main-nav .navbar-nav .dropdown-submenu').removeClass('open');
    });

    $('.navbar-toggle').click(function (event) {
        $(this).toggleClass('active')
    })

    $('.main-nav .nav > li .dropdown-submenu > a').click(function (event) {
        if (screenWidth > 991) {
            event.stopPropagation();
        }
    });

    $('.main-nav .nav > li').hover(function () {
        var dropdownList = $(this).find("> .dropdown-menu");

        if (screenWidth > 991) {

            /*---- Dropdown Animation on Hover ----*/

            dropdownList.addClass('animated fadeIn');
            window.setTimeout(function () {
                dropdownList.removeClass('animated fadeIn');
            }, 500);

            /*---- Positioning Dropdown Menu ----*/

            if (!dropdownList.hasClass('megamenu')) {
                var childDropdownList = $(this).find(".dropdown-submenu .dropdown-menu"),
                dropdownOffset = $(this).offset(),
                offsetLeft = dropdownOffset.left,
                dropdownWidth = dropdownList.width(),
                childWidth = childDropdownList.width(),
                docWidth = $(window).width(),
                aWidth = $(this).children("a").outerWidth(),
                shiftWidth = Math.abs(dropdownWidth - aWidth),
                childShiftWidth = dropdownWidth + childWidth - 1,
                isDropdownVisible = (offsetLeft + dropdownWidth <= docWidth),
                isChildDropdownVisible = (offsetLeft + dropdownWidth + childWidth <= docWidth);
                if (!isDropdownVisible) {
                    dropdownList.css('margin-left', '-' + shiftWidth + 'px')
                    childDropdownList.css('margin-left', '-' + childShiftWidth + 'px')
                } else if (!isChildDropdownVisible) {
                    childDropdownList.css('margin-left', '-' + childShiftWidth + 'px')
                }
                else {
                    dropdownList.removeAttr('style')
                    childDropdownList.removeAttr('style')
                }
            }

                /*---- Positioning Mega Menu ----*/

            else if (dropdownList.hasClass('megamenu')) {
                var dropdownOffset = $(this).offset(),
                linkWidth = $(this).width(),
                dropdownListOffset = dropdownList.offset(),
                offsetLeft = dropdownOffset.left,
                dropdownListoffsetLeft = dropdownListOffset.left,
                dropdownWidth = dropdownList.width(),
                docWidth = $(window).width(),
                shiftOffset = (($('.navigation').hasClass('transparent')) ? 30 : 30),
                positionedValue = Math.abs(offsetLeft),
                shiftWidth = Math.abs(positionedValue + dropdownWidth + shiftOffset),
                isDropdownVisible = (shiftWidth <= docWidth);
                if (!isDropdownVisible) {
                    calculateOffset = docWidth - dropdownWidth - shiftOffset;
                    dropdownList.css('left', +calculateOffset + 'px');
                }
                else {
                    dropdownList.css('left', +positionedValue + 'px');
                }
            }
        }
    });

    /*---- Full-screen Menu Events ----*/

    $('.full-screen-menu-trigger').click(function (event) {
        event.preventDefault();
        $('.full-screen-header').fadeToggle();
        $(this).toggleClass('active');
        $('html, body').toggleClass('full-screen-header-active');
    });

    /*---- Side Menu Events ----*/

    $('.side-menu-trigger').click(function (event) {
        event.preventDefault();
        $(this).toggleClass('active');
        $('body').toggleClass('in');
        $('.side-header').toggleClass('active');
    });

    $(document).mouseup(function (e) {
        var container = $(".main-nav");
        if (!container.is(e.target) && container.has(e.target).length === 0 && $('.side-header').hasClass('active')) {
            $('.side-menu-trigger').removeClass('active');
            $('.side-header').removeClass('active');
            $('body').removeClass('in');
        }
    });

    $('.side-header-close').click(function (event) {
        event.preventDefault();
        if ($('.side-header').hasClass('active')) {
            $('.side-menu-trigger').removeClass('active');
            $('.side-header').removeClass('active');
            $('body').removeClass('in');
        }
    });


    /*---- Sub-menu Events ----*/

    $(".menu-dropdown-link").click(function (event) {
        $(this)
            .parent(".with-dropdown")
            .siblings(".with-dropdown")
            .children(".menu-dropdown.collapse")
            .removeClass("in")
            .end();
        $(this).parents(".with-dropdown").children(".menu-dropdown.collapse").toggleClass('in');
        event.stopPropagation();
    });

    $('li.with-dropdown a.menu-dropdown-link').click(function () {
        var dh = $(this).parents(".with-dropdown").children(".menu-dropdown.collapse").outerHeight();
        if (!$(this).hasClass('active-dropdown')) {
            $(this).parents(".with-dropdown").children(".menu-dropdown.collapse").css('height', dh + 'px');
        }
        else {
            $(this).parents(".with-dropdown").children(".menu-dropdown.collapse").attr('style', '');
        }
        $('.active-dropdown').not($(this)).removeClass('active-dropdown');
        $(this).toggleClass('active-dropdown');
    });

    /*---- Search Box Events ----*/

    $('.search-box-trigger').click(function (event) {
        if ($(window).width() < 992) {
            if ($('.navbar-collapse').hasClass('in')) {
                $('.navbar-collapse').removeClass('in');
            }
        }
        event.preventDefault();
        $('.full-screen-search').fadeToggle();
        $(this).toggleClass('active');
    });

    $(".search-field").keyup(function (e) {
        if (e.keyCode == 13) {
            $('#searchForm').submit();
        }
    });

}


if (document.getElementsByClassName('corner-navigation') || document.getElementsByClassName('.padded-fixed-footer')) {
    window.addEventListener('scroll', function (e) {
        if ($(window).scrollTop() > 50) {
            $('.corner-navigation, .padded-fixed-footer').addClass('fill-in');
        }
        else {
            $('.corner-navigation, .padded-fixed-footer').removeClass('fill-in');
        }
    });
}


/* ------------------------------------------------
    Sticky Navigation
--------------------------------------------------- */


/*---- Sticky Nav's Global Variables ----*/

var headerHeight = 0,
    headerVisiblePos = 0,
	headerFixedPos = 0,
	isHeaderFixed = false,
	isHeaderVisible = false;

function stickyMenu1() {
    if ($('.main-nav').hasClass('sticky')) {
        window.addEventListener('scroll', function (e) {
            var screenTop = $(window).scrollTop();
            if (screenTop > 0) {
                $('.main-nav').addClass('shrink');
               // $('#VerticalSpacer').attr('style', 'height: 90px');
                //$('#VerticalSpacer1').attr('style', 'height: 120px');

            }
            if (screenTop <= 0) {
                $('.main-nav').removeClass('shrink');
                //$('#VerticalSpacer').attr('style', 'height:90px');
                //$('#VerticalSpacer1').attr('style', 'height: 120px');
            }
        });
        var screenTop = $(window).scrollTop();
        if (screenTop > 0) {
            $('.main-nav').addClass('shrink');

            //$('.navigation').attr('style', 'height: 60px');

            //$('#VerticalSpacer').attr('style', 'height: 60px');
            //$('#VerticalSpacer1').attr('style', 'height: 120px');
        }
        else {
            //$('#VerticalSpacer').attr('style', 'height: 90px');
        }
    }
}



/* ------------------------------------------------

    Floating Sidebar

--------------------------------------------------- */


/*---- Sidebar's Global Variables ----*/


var isStickyElementFixed = false,
    stickyElementSetPoint = 0,
    stickyElementY = 0,
    screenWidth = window.innerWidth,
    screenHeight = window.innerHeight,
    stickyElementDisabled = false,
    winScrollY = 0,
    stickyElementTop = 0;

function stickElement() {
    if (document.getElementById('stickyElement')) {
        var elementW = $('.stickyElement').width(),
            relElement = $('.stick-to-side').offset().top + $('.stick-to-side').height(),
            elementW = $('.stickyElement').width(),
            elementH = $('.stickyElement').parent().height(),
            stickyContainer = document.getElementById("sticky-container");
        stickyContainer.style.width = elementW + 'px';
        stickyElementY = $('.stickyElement').offset().top - 60;
        stickyElementSetPoint = relElement - elementH - 60;
        stickyElementTop = $('.stick-to-side').innerHeight() - elementH;
        if (screenWidth < 991) {
            $('.stickyElement').removeClass('stickTop');
            $('.stickyElement').removeAttr('style');
        }
        else if ($(window).scrollTop() > stickyElementSetPoint) {
            $('.stickyElement').removeAttr('style');
            isStickyElementFixed = false;
        }
        window.addEventListener('scroll', function (e) {
            winScrollY = $(window).scrollTop();
            if (screenWidth > 991) {
                if ((winScrollY > stickyElementY) && (winScrollY < stickyElementSetPoint)) {
                    $('.stickyElement').addClass('stickTop');
                    $('.stickyElement').removeAttr('style');
                    isStickyElementFixed = false;
                }
                else if (winScrollY > stickyElementSetPoint && !isStickyElementFixed) {
                    $('.stickyElement').removeClass('stickTop');
                    $('.stickyElement').attr('style', 'position:absolute; top:' + stickyElementTop + 'px');
                    isStickyElementFixed = true;
                }
                else if (winScrollY < stickyElementY) {
                    $('.stickyElement').removeClass('stickTop');
                    isStickyElementFixed = false;
                }
            }
            else if ((screenWidth < 991) && (!stickyElementDisabled)) {
                $('.stickyElement').removeClass('stickTop');
                $('.stickyElement').removeAttr('style');
                stickyElementDisabled = true;
            }
        });
    }
}


/* ------------------------------------------------

    Vertically Centred Elements

--------------------------------------------------- */


function verticallyCentered() {
    if (document.getElementsByClassName("vertical-centred-element")) {
        $('.vertical-centred-element').each(function () {
            var $this = $(this),
                height = 0,
                width = 0,
                margin = 0;
            if ($this.hasClass('flipped-vertical')) {
                width = $this.outerWidth();
                margin = width / 2;
                $this.css('margin-top', margin + 'px');
            }
        });
    }
}



/* ------------------------------------------------

    Function Calls

--------------------------------------------------- */


var $win = $(window);


/* ------------------------------------------------
    Window Resize Events
--------------------------------------------------- */


$win.on('resize', function () {

    /*---- Resetting Variables ----*/

    isStickyElementFixed = false;
    winScrollY = 0;
    stickyElementSetPoint = 0;
    stickyElementY = 0;
    screenWidth = window.innerWidth;
    screenHeight = window.innerHeight;
    winScrollY = 0;
    stickyElementTop = 0;
    stickyElementDisabled = false;
    headerVisiblePos = 0;
    headerFixedPos = 0;
    isHeaderFixed = false;
    isHeaderVisible = false;
    headerHeight = 0;


    navResponsive();
     
    setTimeout(stickElement, 2000)

    stickyMenu1();

    verticallyCentered();

}).resize();


/* ------------------------------------------------
    Window Load Events
--------------------------------------------------- */



$win.on('load', function () {

    navEvents();
    stickyMenu1();

    /*---- Hide Page Loader ----*/

    $(".loader").fadeOut("slow");

});
