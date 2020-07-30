var CookiesPolicy = {

    id_attivita: null,
    lang: 'it',
    link_privacy: null,
    html_element: null,
    css_element: null,
    view_element: true,
    cookie_name: 'CookiePolicy',
    cookie_duration: '259200', // durata in minuti 
    testo_introduttivo: 'Questo sito utilizza i cookies per offrirti un\'esperienza di navigazione migliore.Usando il nostro servizio accetti l\'impiego di cookie in accordo con la nostra cookie policy: <a href="#" id="link_open_cookie_policy">Scoprine di piu\' </a>.',
    bar_height: 13, // pixel 
    bar_padding: 3, // pixel 

    init: function (id_attivita, lang) {
        /* if (!id_attivita) return false;*/

        CookiesPolicy.id_attivita = id_attivita;
        CookiesPolicy.lang = lang;
        CookiesPolicy.link_privacy = 'http://' + document.domain + '/' + 'it/politica-cookie-5';
        CookiesPolicy.link_privacy_en = 'http://' + document.domain + '/' + 'en/politica-cookie-5';
        CookiesPolicy.html_element = CookiesPolicy.CreateHtml();
        CookiesPolicy.css_element = CookiesPolicy.CreateCSS();
        CookiesPolicy.view_element = CookiesPolicy.IsRequiredView();

        if (CookiesPolicy.view_element) CookiesPolicy.ViewCookiesPolicy();

        //////////////////////////////////////////////////////////
        //Spengo i cookies su scroll
        if (CookiesPolicy.isjQuery())
            jQuery(window).scroll(function () {
                if (jQuery(window).scrollTop() > 200) {
                    if (CookiesPolicy.isjQuery()) {
                        jQuery('#cookies_policy').remove();
                    } else {
                        var el = document.getElementById('cookies_policy');
                        el.parentNode.removeChild(el);
                    }
                }
            });
        //////////////////////////////////////////////////////////


    },

    CreateHtml: function () {

        var html = '';

        html += '<div id="cookies_policy" class="cookies_policy" >';
        html += ' <input id="link_close_cookie_policy" type="button" value="Ok">' + '' + CookiesPolicy.testo_introduttivo;
        html += '</div>';

        return html;

    },

    CreateCSS: function () {

        var css = '';

        css += '<style type="text/css">';
        css += '.ui-mobile [data-role=page]{top:25px !important}';
        css += '.cookies_policy{box-sizing: initial;position:fixed; bottom:0; padding-top:' + CookiesPolicy.bar_padding + 'px; padding-bottom:' + CookiesPolicy.bar_padding + 'px;  background-color:#ffffff; border-bottom:1px solid #ccc; color:#333; text-align:center;  width:100%; font-family:verdana; font-size:13px; z-index:19999}';
        css += '.cookies_policy a:link{color:#333; text-decoration:underline; padding:0px 0px; } .cookies_policy a:visited{color:#333; text-decoration:underline} .cookies_policy a:hover{color:#ccc; text-decoration:underline}';
        css += '.cookies_policy input[type=button]{background-color:#000000; color:white; cursor:pointer; border:gray;  padding:0px 0px; height: 35px; }';
        css += '</style>';

        return css;

    },

    SetBarPosition: function (position) {

        switch (position) {
            case "top":
                jQuery("#cookies_policy").css("top", 0).css('bottom', 'auto').css('position', 'relative').css('z-index', '99999');
                break;
            case "bottom":
                jQuery("#cookies_policy").css("bottom", 0).css('position', 'fixed').css('top', 'auto').css('z-index', '99999');
                break;
        }


    },

    IsRequiredView: function () {
        var cookie = CookiesPolicy.GetCookie(CookiesPolicy.cookie_name);
        if (cookie == '1') {
            return false;
        } else {
            return true;
        }
    },

    isjQuery: function () {
        if (typeof jQuery != 'undefined') {
            return true;
        } else {
            return false;
        }
    },

    isHighslide: function () {
        if (typeof hs != 'undefined' && typeof hs.htmlExpand != 'undefined') {
            return true;
        } else {
            return false;
        }
    },

    OpenPolicyLink: function () {

        if (CookiesPolicy.isHighslide()) {

            var w = window,
                d = document,
                e = d.documentElement,
                g = d.getElementsByTagName('body')[0],
                x = w.innerWidth || e.clientWidth || g.clientWidth,
                y = w.innerHeight || e.clientHeight || g.clientHeight;

            hs.height = y - 200;
            hs.width = x - 100;
            hs.align = 'center';
            hs.preserveContent = false;
            hs.marginTop = 60;
            hs.marginLeft = 50;
            hs.slideshows = ''; // fix perche va in conflitto
            return hs.htmlExpand(document.body, { objectType: 'iframe', src: CookiesPolicy.link_privacy });
        } else {
            if (CookiesPolicy.lang == 'it') { window.open(CookiesPolicy.link_privacy, '', ''); return false; }
            else { window.open(CookiesPolicy.link_privacy_en, '', ''); return false; }
        }

    },

    RemovePolicyBar: function () {

        if (CookiesPolicy.isjQuery()) {
            jQuery('#cookies_policy').remove();
        } else {
            var el = document.getElementById('cookies_policy');
            if (el) el.parentNode.removeChild(el);
        }

    },



    ViewCookiesPolicy: function () {

        CookiesPolicy.RemovePolicyBar();

        if (CookiesPolicy.isjQuery()) {
            jQuery('body').prepend(CookiesPolicy.html_element);
            jQuery('head').prepend(CookiesPolicy.css_element);


            jQuery('#link_open_cookie_policy').click(function () {
                CookiesPolicy.OpenPolicyLink();
            })

            jQuery('#link_close_cookie_policy').click(function () {
                CookiesPolicy.HideCookiesPolicy();
            })


            if (typeof cms == 'object') {

                var w = cms.getDocumentWidth();

                jQuery('div').filter(function () {
                    if (jQuery(this).css('position') == 'fixed') {
                        var pos = jQuery(this).position().top;
                        var display = jQuery(this).css('display')
                        if (pos == 0 && display != 'none') CookiesPolicy.SetBarPosition('bottom');
                    }
                });

                cms.onEvent('WindowResized', function (e) {


                    if (parseFloat(w) <= 992) {
                        jQuery('div').filter(function () {
                            if (jQuery(this).css('position') == 'fixed') {
                                var pos = jQuery(this).position().top;
                                if (pos == 0) CookiesPolicy.SetBarPosition('bottom');
                            }
                        });
                    } else {
                        CookiesPolicy.SetBarPosition('bottom');
                    }


                })


            }

        } else {

            function create(htmlStr) {
                var frag = document.createDocumentFragment(),
                    temp = document.createElement('div');
                temp.innerHTML = htmlStr;
                while (temp.firstChild) {
                    frag.appendChild(temp.firstChild);
                }
                return frag;
            }

            var fragment = create(CookiesPolicy.html_element);
            document.body.insertBefore(fragment, document.body.childNodes[0]);

            var fragment = create(CookiesPolicy.css_element);
            document.head.insertBefore(fragment, document.head.childNodes[0]);

            document.getElementById('link_open_cookie_policy').addEventListener('click', function () {
                CookiesPolicy.OpenPolicyLink();
            }, false);

            document.getElementById('link_close_cookie_policy').addEventListener('click', function () {
                CookiesPolicy.HideCookiesPolicy();
            }, false);

        }
    },

    HideCookiesPolicy: function () {

        CookiesPolicy.SetCookie(CookiesPolicy.cookie_name, '1', CookiesPolicy.cookie_duration, '/');

        if (CookiesPolicy.isjQuery()) {
            jQuery('#cookies_policy').remove();
        } else {
            var el = document.getElementById('cookies_policy');
            el.parentNode.removeChild(el);
        }



    },

    GetCookie: function (name) {

        var start = document.cookie.indexOf(name + "=");
        var len = start + name.length + 1;
        if ((!start) && (name != document.cookie.substring(0, name.length))) {
            return null;
        }
        if (start == -1) return null;
        var end = document.cookie.indexOf(";", len);
        if (end == -1) end = document.cookie.length;
        return unescape(document.cookie.substring(len, end));
    },

    SetCookie: function (name, value, expires, path, domain, secure) {

        // set time, it's in milliseconds
        var today = new Date();
        today.setTime(today.getTime());

        /*
        if the expires variable is set, make the correct
        expires time, the current script below will set
        it for x number of days, to make it for hours,
        delete * 24, for minutes, delete * 60 * 24
        */
        if (expires) {
            expires = expires * 1000 * 60 * 60 * 24;
        }
        var expires_date = new Date(today.getTime() + (expires));

        document.cookie = name + "=" + escape(value) +
            ((expires) ? ";expires=" + expires_date.toGMTString() : "") +
            ((path) ? ";path=" + path : "") +
            ((domain) ? ";domain=" + domain : "") +
            ((secure) ? ";secure" : "");
    }




};

CookiesPolicy.init('', 'it');
