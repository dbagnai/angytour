<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="SchedaOffertaMaster.aspx.cs"
    EnableTheming="true" Culture="it-IT" Inherits="_SchedaOffertaMaster" Title="" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <div class="row">
        <div class="col-md-1 col-sm-1">
        </div>
        <div class="col-md-10 col-sm-10 col-xs-12">
            <h2 class="h1-body-title" style="color: #5c5c5c; margin-bottom: 10px">
                <asp:Literal Text="" runat="server" ID="litSezione" /></h2>
        </div>
        <div class="col-md-1 col-sm-1">
        </div>
    </div>
    <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
    <div class="container" style="padding-left: 35px; padding-right: 50px; text-align: center; margin-top: 0px">
        <asp:Panel runat="server" ID="pnlButtonsnav" Visible="false">
            <div class="row">
                <div class="col-sm-4">
                    <div class="<%= Categoria=="prod000037"?"divbuttonstylered":"divbuttonstyle" %>" style="font-size: 0.8em; padding: 4px; margin-bottom: 3px">
                        <a href='<%= ReplaceAbsoluteLinks( "~/Aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta"  + "&Lingua=" + Lingua + "&idOfferta=" + idOfferta) %>' title="">
                            <%= ImpostaTestoRichiesta() %>
                       
                        </a>
                    </div>
                </div>
                <div class="col-sm-offset-4 col-sm-2 hidden-xs">
                    <div class="<%= Categoria=="prod000037"?"divbuttonstylered":"divbuttonstyle" %>" style="font-size: 0.8em; padding: 4px; margin-bottom: 3px">
                        <%= "<a  target=\"_blank\" href=\"" + PercorsoAssolutoApplicazione +"/aspnetpages/SchedaOffertaStampa.aspx?idOfferta=" + idOfferta + "&CodiceTipologia=" + CodiceTipologia + "&Lingua=" + Lingua
            + "\"><i class=\"fa fa-print\"></i>&nbsp;Stampa</a>"  %>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="<%= Categoria=="prod000037"?"divbuttonstylered":"divbuttonstyle" %>" style="font-size: 0.8em; padding: 4px; margin-bottom: 3px">
                        <%= "<a  href=\"" +  GeneraBackLink()  + "\"><i class=\"fa fa-reply-all\"></i>&nbsp;" + Resources.Common.testoIndietro + "</a>" %>
                    </div>
                </div>
            </div>
            <br />
        </asp:Panel>
    </div>
    <script type="text/javascript">
        function inizializzaFlexsliderSchedaGallery() {


            //Plugin: flexslider con funzione di animazione dei messaggi o oggetti sopra
            // ------------------------------------------------------------------
            if ($('#gallery-slider') != null)
                $('#gallery-slider').each(function () {
                    var sliderSettings = {
                        animation: $(this).attr('data-transition'),
                        easing: "swing",
                        selector: ".slides > .slide",
                        smoothHeight: false,
                        // controlsContainer: ".flex-container",
                        animationLoop: false,             //Boolean: Should the animation loop? If false, directionNav will received "disable" classes at either end
                        slideshow: false,                //Boolean: Animate slider automatically
                        //slideshowSpeed: 4000,           //Integer: Set the speed of the slideshow cycling, in milliseconds
                        //animationSpeed: 1500,            //Integer: Set the speed of animations, in milliseconds

                        // Usability features
                        pauseOnAction: true,            //Boolean: Pause the slideshow when interacting with control elements, highly recommended.
                        pauseOnHover: false,            //Boolean: Pause the slideshow when hovering over slider, then resume when no longer hovering
                        useCSS: true,                   //{NEW} Boolean: Slider will use CSS3 transitions if available
                        touch: true,                    //{NEW} Boolean: Allow touch swipe navigation of the slider on touch-enabled devices
                        video: false,                   //{NEW} Boolean: If using video in the slider, will prevent CSS3 3D Transforms to avoid graphical glitches
                        sync: "#carousel-gallery-slider",

                        // Primary Controls
                        // controlNav: "thumbnails",       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        controlNav: false,       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        directionNav: true,             //Boolean: Create navigation for previous/next navigation? (true/false)
                        prevText: "",           //String: Set the text for the "previous" directionNav item
                        nextText: "",               //String: Set the text for the "next" directionNav item

                        start: function (slider) {
                            //hide all animated elements
                            slider.find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in first slide
                            slider.find('.slide').eq(1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                    function () {
                                        $(this).removeClass($(this).data('animate-in'));
                                    }
                                );
                            });
                        },
                        before: function (slider) {
                            //hide next animate element so it can animate in
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });
                        },
                        after: function (slider) {
                            //hide animtaed elements so they can animate in again
                            slider.find('.slide').find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in next slide
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                    function () {
                                        $(this).removeClass($(this).data('animate-in'));
                                    }
                                );
                            });
                            /* auto-restart player if paused after action */
                            if (!slider.playing) {
                                slider.play();
                            }

                        }
                    };

                    var sliderNav = $(this).attr('data-slidernav');
                    if (sliderNav !== 'auto') {
                        sliderSettings = $.extend({}, sliderSettings, {
                            //  controlsContainer: '.flex-container',
                            manualControls: sliderNav + ' li a'

                        });
                    }

                    $('html').addClass('has-flexslider');
                    $(this).flexslider(sliderSettings);
                });
            // $('#scheda-slider').resize(); //make sure height is right load assets loaded


            if ($('#carousel-gallery-slider') != null)
                $('#carousel-gallery-slider').flexslider({
                    animation: "slide",
                    controlNav: false,
                    animationLoop: false,
                    slideshow: false,
                    itemWidth: 120,
                    itemHeight: 80,
                    itemMargin: 5,
                    asNavFor: '#gallery-slider'
                });
        }

        function registrastatistiche() {
            $get("ctl00_ContentPlaceHolder1_rptOfferta_ctl00_btnRegistrastatistiche").click();
        }
        function goBack() {
            window.history.back()
        }
        $(document).ready(function () {
            inizializzaFlexsliderSchedaGallery();
            inizializzaFlexsliderScheda();
            inizializzaFlexsliderSchedaSoci();
        });
        function inizializzaFlexsliderScheda() {
            //Plugin: flexslider con funzione di animazione dei messaggi o oggetti sopra
            // ------------------------------------------------------------------
            if ($('#scheda-slider') != null)
                $('#scheda-slider').each(function () {
                    var sliderSettings = {
                        animation: $(this).attr('data-transition'),
                        easing: "swing",
                        selector: ".slides > .slide",
                        smoothHeight: false,
                        // controlsContainer: ".flex-container",
                        animationLoop: false,             //Boolean: Should the animation loop? If false, directionNav will received "disable" classes at either end
                        slideshow: false,                //Boolean: Animate slider automatically
                        //   slideshowSpeed: 4000,           //Integer: Set the speed of the slideshow cycling, in milliseconds
                        //  animationSpeed: 600,            //Integer: Set the speed of animations, in milliseconds

                        // Usability features
                        pauseOnAction: true,            //Boolean: Pause the slideshow when interacting with control elements, highly recommended.
                        pauseOnHover: true,            //Boolean: Pause the slideshow when hovering over slider, then resume when no longer hovering
                        useCSS: true,                   //{NEW} Boolean: Slider will use CSS3 transitions if available
                        touch: true,                    //{NEW} Boolean: Allow touch swipe navigation of the slider on touch-enabled devices
                        video: false,                   //{NEW} Boolean: If using video in the slider, will prevent CSS3 3D Transforms to avoid graphical glitches
                        sync: "#carousel-scheda-slider",

                        // Primary Controls
                        // controlNav: "thumbnails",       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        controlNav: false,       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        directionNav: true,             //Boolean: Create navigation for previous/next navigation? (true/false)
                        prevText: "",           //String: Set the text for the "previous" directionNav item
                        nextText: "",               //String: Set the text for the "next" directionNav item

                        start: function (slider) {
                            //hide all animated elements
                            slider.find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in first slide
                            slider.find('.slide').eq(1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                    function () {
                                        $(this).removeClass($(this).data('animate-in'));
                                    }
                                );
                            });
                        },
                        before: function (slider) {
                            //hide next animate element so it can animate in
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });
                        },
                        after: function (slider) {
                            //hide animtaed elements so they can animate in again
                            slider.find('.slide').find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in next slide
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                    function () {
                                        $(this).removeClass($(this).data('animate-in'));
                                    }
                                );
                            });
                            /* auto-restart player if paused after action */
                            if (!slider.playing) {
                                slider.play();
                            }

                        }
                    };

                    var sliderNav = $(this).attr('data-slidernav');
                    if (sliderNav !== 'auto') {
                        sliderSettings = $.extend({}, sliderSettings, {
                            //  controlsContainer: '.flex-container',
                            manualControls: sliderNav + ' li a'
                        });
                    }

                    $('html').addClass('has-flexslider');
                    $(this).flexslider(sliderSettings);
                });
            //    $('#scheda-slider').resize(); //make sure height is right load assets loaded
            if ($('#carousel-scheda-slider') != null)
                $('#carousel-scheda-slider').flexslider({
                    animation: "slide",
                    controlNav: false,
                    animationLoop: false,
                    slideshow: false,
                    itemWidth: 120,
                    itemHeight: 80,
                    itemMargin: 5,
                    asNavFor: '#scheda-slider'
                });

        }

        function inizializzaFlexsliderSchedaSoci() {
            //Plugin: flexslider con funzione di animazione dei messaggi o oggetti sopra
            // ------------------------------------------------------------------
            if ($('#scheda-slider-soci') != null)
                $('#scheda-slider-soci').each(function () {
                    var sliderSettings = {
                        animation: $(this).attr('data-transition'),
                        easing: "swing",
                        selector: ".slides > .slide",
                        smoothHeight: false,
                        // controlsContainer: ".flex-container",
                        animationLoop: false,             //Boolean: Should the animation loop? If false, directionNav will received "disable" classes at either end
                        slideshow: false,                //Boolean: Animate slider automatically
                        //   slideshowSpeed: 4000,           //Integer: Set the speed of the slideshow cycling, in milliseconds
                        //  animationSpeed: 600,            //Integer: Set the speed of animations, in milliseconds

                        // Usability features
                        pauseOnAction: true,            //Boolean: Pause the slideshow when interacting with control elements, highly recommended.
                        pauseOnHover: false,            //Boolean: Pause the slideshow when hovering over slider, then resume when no longer hovering
                        useCSS: true,                   //{NEW} Boolean: Slider will use CSS3 transitions if available
                        touch: true,                    //{NEW} Boolean: Allow touch swipe navigation of the slider on touch-enabled devices
                        video: false,                   //{NEW} Boolean: If using video in the slider, will prevent CSS3 3D Transforms to avoid graphical glitches
                        sync: "#carousel-scheda-slider-soci",

                        // Primary Controls
                        // controlNav: "thumbnails",       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        controlNav: false,       //Boolean: Create navigation for paging control of each clide? Note: Leave true for manualControls usage
                        directionNav: true,             //Boolean: Create navigation for previous/next navigation? (true/false)
                        prevText: "",           //String: Set the text for the "previous" directionNav item
                        nextText: "",               //String: Set the text for the "next" directionNav item

                        start: function (slider) {
                            //hide all animated elements
                            slider.find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in first slide
                            slider.find('.slide').eq(1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                    function () {
                                        $(this).removeClass($(this).data('animate-in'));
                                    }
                                );
                            });
                        },
                        before: function (slider) {
                            //hide next animate element so it can animate in
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });
                        },
                        after: function (slider) {
                            //hide animtaed elements so they can animate in again
                            slider.find('.slide').find('[data-animate-in]').each(function () {
                                $(this).css('visibility', 'hidden');
                            });

                            //animate in next slide
                            slider.find('.slide').eq(slider.animatingTo + 1).find('[data-animate-in]').each(function () {
                                if ($(this).data('animate-delay')) {
                                    $(this).addClass($(this).data('animate-delay'));
                                }
                                if ($(this).data('animate-duration')) {
                                    $(this).addClass($(this).data('animate-duration'));
                                }
                                $(this).css('visibility', 'visible').addClass('animated').addClass($(this).data('animate-in'));
                                $(this).one('webkitAnimationEnd oanimationend msAnimationEnd animationend',
                                    function () {
                                        $(this).removeClass($(this).data('animate-in'));
                                    }
                                );
                            });
                            /* auto-restart player if paused after action */
                            if (!slider.playing) {
                                slider.play();
                            }

                        }
                    };

                    var sliderNav = $(this).attr('data-slidernav');
                    if (sliderNav !== 'auto') {
                        sliderSettings = $.extend({}, sliderSettings, {
                            //  controlsContainer: '.flex-container',
                            manualControls: sliderNav + ' li a'

                        });
                    }

                    $('html').addClass('has-flexslider');
                    $(this).flexslider(sliderSettings);
                });
            //    $('#scheda-slider').resize(); //make sure height is right load assets loaded
            if ($('#carousel-scheda-slider-soci') != null)
                $('#carousel-scheda-slider-soci').flexslider({
                    animation: "slide",
                    controlNav: false,
                    animationLoop: false,
                    slideshow: false,
                    itemWidth: 120,
                    itemHeight: 80,
                    itemMargin: 5,
                    asNavFor: '#scheda-slider-soci'
                });

        }

    </script>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        makeRevLower = true;
    </script>
    <div class="row" runat="server" id="divGalleryDetail" visible="false">
        <div class="col-md-1 col-sm-1">
        </div>
        <div class="col-md-10 col-sm-10 col-xs-12">
            <asp:Repeater ID="rptOfferteGalleryDetail" runat="server">
                <ItemTemplate>
                    <div class="blog-post" style="text-align: justify; background-color: transparent; border: none">


                        <div style="width: 100%" runat="server"
                            visible='<%# ControlloVisibilita(Eval("FotoCollection_M"), Eval("linkVideo") )  %>'>
                            <div class="flexslider" data-transition="slide" data-slidernav="auto" id="gallery-slider" style="width: 100%; overflow: hidden; margin-bottom: 10px; margin-top: 10px;">
                                <div class="slides" runat="server" id="divFlexScheda">
                                    <%# CreaSlide(Container.DataItem,0,700) %>
                                </div>
                            </div>
                        </div>
                        <div class="flexslider" id="carousel-gallery-slider" style="height: 80px; padding-top: 0px; padding-bottom: 0px; overflow: hidden">
                            <ul class="slides">
                                <%# CreaSlideNavigation(Container.DataItem,0,0) %>
                            </ul>
                            <div style="clear: both"></div>
                        </div>

                        <div class="responsive-video" id="divVideo" runat="server" visible='<%# ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima"), Eval("linkVideo")  ) %>'>
                            <iframe frameborder="0" allowfullscreen src='<%#  SorgenteVideo(  Eval("linkVideo") ) %>'></iframe>
                        </div>

                    </div>

                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="col-md-1 col-sm-1">
        </div>
    </div>


    <div style="width: 100%; padding: 0px; margin: 0px" runat="server" id="divContenutiGallery" visible="false">
        <div class="row" style="margin-bottom: 20px; margin-top: 20px">
            <div class="col-sm-12">
                <ul class="works-grid works-grid-gut works-grid-3 works-hover-lw">
                    <asp:Literal Text="" ID="litGalleryDetails" runat="server" />
                </ul>
            </div>
        </div>
    </div>


</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHoldermastercenter" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHoldermasternorow" runat="Server">
    <div class="row" style="padding-top: 0; padding-left: 0px; padding-right: 0px;">
        <div class="col-md-1 col-sm-1" runat="server" id="column1" visible="false">
        </div>
        <div class="col-md-9 col-sm-9" runat="server" id="column2">

            <div class="row">
                <asp:Label ID="output" runat="server"></asp:Label>
            </div>
            <div id="divItemContainter1" style="position: relative; display: none"></div>

            <div class="row">
                <div class="col-md-12 col-sm-12">
                    <%--   <asp:UpdatePanel ID="UserPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                    <asp:Repeater ID="rptSocio" runat="server" OnItemDataBound="rptOfferta_ItemDataBound">
                        <ItemTemplate>
                            <div class="blog-post" style="text-align: justify">
                                <h2 style="text-transform: capitalize">
                                    <asp:Literal ID="litTitolo" Text='<%# Eval("Cognome_dts").ToString() + " "  + Eval("Nome_dts").ToString()  %>'
                                        runat="server"></asp:Literal>
                                </h2>
                                <div runat="server" id="div1" visible='<%# AttivaContatto(Eval("Abilitacontatto")) %>' style="margin-bottom: 15px">
                                    <a id="A6" runat="server" href='<%# "~/Aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta"  + "&Lingua=" + Lingua + "&idOfferta=" + Eval("Id").ToString()  %>'
                                        target="_blank" title="" class="buttonstyle">
                                        <%= ImpostaTestoRichiesta() %>
                        
                                    </a>
                                </div>

                                <div style="width: 100%" runat="server"
                                    visible='<%# ControlloVisibilita(Eval("FotoCollection_M"), Eval("linkVideo") )  %>'>
                                    <div class="flexslider" data-transition="slide" data-slidernav="auto" id="scheda-slider-soci" style="width: 100%; overflow: hidden; margin-bottom: 10px; margin-top: 10px;">
                                        <div class="slides" runat="server" id="divFlexScheda">
                                            <%# CreaSlide(Container.DataItem,0,400) %>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel runat="server" Visible='<%# ControlloVisibilitaMiniature(Eval("FotoCollection_M"))  %>'>
                                    <div class="flexslider" id="carousel-scheda-slider-soci" style="height: 90px; padding-top: 10px; overflow: hidden">
                                        <ul class="slides">
                                            <%# CreaSlideNavigation(Container.DataItem,0,0) %>
                                        </ul>
                                        <div style="clear: both"></div>
                                    </div>
                                </asp:Panel>
                                <div class="responsive-video" id="divVideo" runat="server" visible='<%# ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima"), Eval("linkVideo")  ) %>'>
                                    <iframe frameborder="0" allowfullscreen src='<%#  SorgenteVideo(  Eval("linkVideo") ) %>'></iframe>
                                </div>
                                <div class="blog-post-body">
                                    <p>
                                        <asp:Label ID="lbldescri" runat="server" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo( CommonPage.ReplaceLinks( Eval("Descrizione" + Lingua).ToString()) ) %>'></asp:Label>
                                    </p>
                                    <%--<p>
                                                <%# Resources.Common.TestoCategoriasocio.ToString() + ": " + TestoCaratteristica(2,Eval("Caratteristica3").ToString(),Lingua) %>
                                            </p>--%>
                                    <p>
                                        <asp:Literal ID="Literal5" Text='<%#  ControlloVuoto("Tel:", Eval("Telefono").ToString() )%>'
                                            runat="server"></asp:Literal><br />
                                        <asp:Literal ID="Literal6" Text='<%#   ControlloVuoto("Tel:",Eval("Fax").ToString())%>'
                                            runat="server"></asp:Literal>
                                    </p>
                                    <p>
                                        <a href='<%# "mailto:" + Eval("Email").ToString() %>'>
                                            <asp:Literal ID="Literal3" Text='<%#   ControlloVuotoEmail("Email:", Eval("Email").ToString(),Eval("Abilitacontatto")) %>'
                                                runat="server"></asp:Literal></a><br />
                                        <a href='<%# "http://" + Eval("Website").ToString() %>' target="_blank" style="text-decoration: underline;" onclick="javascript:registrastatistiche();">
                                            <asp:Literal ID="Literal4" Text='<%# ControlloVuoto("Website:", Eval("Website").ToString()) %>'
                                                runat="server"></asp:Literal></a><br />
                                        <asp:Button Style="display: none" Text="" ID="btnRegistrastatistiche" OnClick="btnRegistrastatistiche_Click" runat="server" CommandArgument='<%# Eval("Id") %>' />
                                    </p>
                                    <p>
                                        <asp:Literal ID="litPosizione" runat="server"
                                            Text='<%# VisualizzaPosizione( Container.DataItem ) %>'></asp:Literal>
                                    </p>

                                    <div runat="server" id="divContact" visible='<%# AttivaContatto(Eval("Abilitacontatto")) %>'>
                                        <a id="A1" runat="server" href='<%# "~/Aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta"  + "&Lingua=" + Lingua + "&idOfferta=" + Eval("Id").ToString()   %>'
                                            target="_blank" title="" class="buttonstyle">
                                            <%= ImpostaTestoRichiesta() %>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <!--Blog Post-->
                    <div class="blog-post" style="text-align: left" itemscope="" itemtype="http://schema.org/Article">
                        <asp:Repeater ID="rptOfferta" runat="server" OnItemDataBound="rptOfferta_ItemDataBound">
                            <ItemTemplate>
                                <%-- <div class="pull-right" runat="server" id="div1" visible='<%# AttivaContatto(Eval("Abilitacontatto")) %>'>
                                <a id="A2" runat="server" href='<%# "~/Aspnetpages/Content_Tipo3.aspx?idOfferta=" + Eval("Id").ToString() + "&TipoContenuto=Richiesta"  + "&Lingua=" + Lingua %>'
                                    target="_blank" title="" class="button btn-flat">
                                    <asp:Literal Text="<%$ Resources:Common,TestoDisponibilita  %>" runat="server" />
                                </a>
                            </div>--%>

                                <!-- Slider -->
                                <%-- <div class="blog-post-featured-img">

                                            <div class="blog-slider cycle-slideshow"
                                                 data-cycle-slides="> .slider-img"
                                                 data-cycle-swipe="true"
                                                 data-cycle-prev=".cycle-prev"
                                                 data-cycle-next=".cycle-next"
                                                 data-cycle-timeout=0
                                                 >

                                                <div class="fa fa-chevron-right cycle-next"></div>
                                                <div class="fa fa-chevron-left cycle-prev"></div>
                                                <div class="cycle-pager"></div>

                                                <!-- slider item -->
                                                <div class="slider-img">
                                                    <img src="images/placeholders/blog2.jpg" alt=""/>
                                                </div>
                                                <!-- //slider item// -->
                                                <!-- slider item -->
                                                <div class="slider-img">
                                                    <img src="images/placeholders/blog3.jpg" alt=""/>
                                                </div>
                                                <!-- //slider item// -->
                                                <!-- slider item -->
                                                <div class="slider-img">
                                                    <img src="images/placeholders/blog4.jpg" alt=""/>
                                                </div>
                                                <!-- //slider item// -->
                                            </div>
                                 </div>--%>
                                <!-- Slider -->

                                <h1 itemprop="name">
                                    <a id="a4" runat="server"
                                        href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                        target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                        <asp:Literal ID="Literal7" Text='<%# estraititolo(  Eval("Denominazione" + Lingua) ) %>'
                                            runat="server"></asp:Literal><br />
                                        <span style="font-size: 70%">
                                            <asp:Literal ID="Literal12" Text='<%# estraisottotitolo(  Eval("Denominazione" + Lingua) ) %>'
                                                runat="server"></asp:Literal></span>
                                    </a>
                                </h1>

                                <div class="blog-post-details" runat="server" id="divPostDetails" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString()) || !string.IsNullOrEmpty( ControlloVuotoPosizione( Eval("CodiceComune").ToString()  , Eval("CodiceProvincia").ToString(), Eval("CodiceTipologia").ToString(),Lingua )) 
                                    || VerificaPresenzaPrezzo( Eval("Prezzo") )  %>'>

                                    <div style="padding-left: 0px" class="blog-post-details-item  blog-post-details-item-left" runat="server" visible='<%# VerificaPresenzaPrezzo( Eval("Prezzo") ) %>'>
                                        <div style="font-weight: 800; font-size: 1.9em; float: left; line-height: normal">
                                            <asp:Literal ID="Literal15" runat="server"
                                                Text='<%#  ImpostaIntroPrezzo(Eval("CodiceTipologia").ToString()) + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("Prezzo")) + " €" %>'></asp:Literal>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div style="font-weight: 600; font-size: 1.4em; color: #888; float: left; text-decoration: line-through; line-height: normal">
                                            <em>
                                                <asp:Literal ID="Literal9" runat="server"
                                                    Text='<%#  ImpostaIntroPrezzo(Eval("CodiceTipologia").ToString()) + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("PrezzoListino")) + " €<br/><br/>" %>'></asp:Literal></em>
                                        </div>

                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="blog-post-details-item blog-post-details-item-left" runat="server" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString()) %>'>
                                        <meta itemprop="datePublished" content='<%#  string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) %>'>
                                        &nbsp;<asp:Literal ID="Literal10" Text="<%$ Resources:Common,TestoPubblicatodata  %>" runat="server" /><asp:Literal ID="Literal11"
                                            Text='<%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento"))   %>'
                                            runat="server" />
                                        <asp:Label ID="Literal14" itemprop="articleSection"
                                            Text='<%# " " + TestoSezione(Eval("CodiceTipologia").ToString()) %>'
                                            runat="server" />
                                    </div>

                                    <div class="blog-post-details-item blog-post-details-item-left">
                                        <asp:Literal ID="litPosizione" runat="server"
                                            Text='<%# ControlloVuotoPosizione( Eval("CodiceComune").ToString()  , Eval("CodiceProvincia").ToString(), Eval("CodiceTipologia").ToString(),Lingua ) %>'></asp:Literal>
                                    </div>
                                </div>

                                <div style="width: 100%" runat="server"
                                    visible='<%# ControlloVisibilita(Eval("FotoCollection_M"), Eval("linkVideo") )  %>'>
                                    <div class="flexslider" data-transition="slide" data-slidernav="auto" id="scheda-slider" style="width: 100%; overflow: hidden; margin-bottom: 10px; margin-top: 0px;">
                                        <div class="slides" runat="server" id="divFlexScheda">
                                            <%# CreaSlide(Container.DataItem,0,600) %>
                                        </div>
                                    </div>

                                </div>

                                <asp:Panel runat="server" Visible='<%# ControlloVisibilitaMiniature(Eval("FotoCollection_M"))  %>'>
                                    <div class="flexslider" id="carousel-scheda-slider" style="height: 80px; padding-top: 0px; padding-bottom: 0px; overflow: hidden">
                                        <ul class="slides">
                                            <%# CreaSlideNavigation(Container.DataItem,0,0) %>
                                        </ul>
                                        <div style="clear: both"></div>
                                    </div>
                                </asp:Panel>

                                <div class="responsive-video" id="divVideo" runat="server" visible='<%# ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima"), Eval("linkVideo")  ) %>'>
                                    <iframe frameborder="0" allowfullscreen src='<%#  SorgenteVideo(  Eval("linkVideo") ) %>'></iframe>
                                </div>
                                <p>
                                    <asp:Literal ID="Literal13" runat="server"
                                        Text='<%# VisualizzaPosizione( Container.DataItem ) %>'></asp:Literal>
                                </p>

                                <%#  CrealistaFiles(Eval("Id"),  Eval("FotoCollection_M")) %>
                                <p>
                                    <!-- Go to www.addthis.com/dashboard to customize your tools -->
                                    <div class="addthis_inline_share_toolbox"></div>
                                </p>

                                <div class="blog-post-body" style="text-align: justify" itemprop="description">

                                    <p>
                                        <asp:Label ID="lbldescri" runat="server" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo( CommonPage.ReplaceLinks( Eval("Descrizione" + Lingua).ToString()) ) %>'></asp:Label>
                                    </p>
                                    <p>
                                        <asp:Label ID="Label2" runat="server" Text='<%#  WelcomeLibrary.UF.Utility.SostituisciTestoACapo( CommonPage.ReplaceLinks( Eval("Datitecnici" + Lingua).ToString()) ) %>'></asp:Label>
                                    </p>
                                    <p>
                                        <asp:Literal ID="Literal1" Text='<%# ControlloVuoto("",   WelcomeLibrary.UF.Utility.SostituisciTestoACapo( Eval("Indirizzo").ToString())) %>'
                                            runat="server"></asp:Literal>
                                        <asp:Literal ID="Literal5" Text='<%#  ControlloVuoto("<br/>Tel:", Eval("Telefono").ToString()  )%>'
                                            runat="server"></asp:Literal>
                                        <asp:Literal ID="Literal6" Text='<%#   ControlloVuoto("<br/>Fax:",Eval("Fax").ToString()   ) %>'
                                            runat="server"></asp:Literal>
                                        <a href='<%# "mailto:" + Eval("Email").ToString() %>'>
                                            <asp:Literal ID="Literal3" Text='<%#   ControlloVuotoEmail("<br/>Email:", Eval("Email").ToString(),Eval("Abilitacontatto"))  %>'
                                                runat="server"></asp:Literal></a>
                                        <a href='<%# "http://" + Eval("Website").ToString() %>' target="_blank" style="text-decoration: underline;" onclick="javascript:registrastatistiche();">
                                            <asp:Literal ID="Literal4" Text='<%# ControlloVuoto("<br/>Website:", Eval("Website").ToString() )   %>'
                                                runat="server"></asp:Literal></a>
                                    </p>
                                    <div class="row">
                                        <div class="col-sm-4">
                                            <div runat="server" id="divContact" visible='<%# AttivaContatto(Eval("Abilitacontatto")) %>'>
                                                <%--   <a id="A1" runat="server" href='<%# "~/Aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta" + "&Lingua=" + Lingua+ "&idOfferta=" + Eval("Id").ToString()  %>'
                                                    target="_blank" title="" class="buttonstyle">
                                                    <%= ImpostaTestoRichiesta() %>
                                                </a>--%>
                                                <%= "<a class=\"buttonstyle\"  target=\"_blank\" href=\"" + PercorsoAssolutoApplicazione + "/aspnetpages/Content_Tipo3.aspx?TipoContenuto=Richiesta&Lingua=" + Lingua+ "&idOfferta=" + idOfferta + "\">" + ImpostaTestoRichiesta() + "</a>" %>
                                            </div>
                                        </div>
                                        <div class="col-sm-4">
                                            <%= "<a class=\"buttonstyle\"  target=\"_blank\" href=\"" + PercorsoAssolutoApplicazione + "/aspnetpages/SchedaOffertaStampa.aspx?idOfferta=" + idOfferta + "&CodiceTipologia=" + CodiceTipologia + "&Lingua=" + Lingua  + "\"><i class=\"fa fa-print\"></i>" + Resources.Basetext.Stampa + "</a>"%>
                                        </div>
                                        <div class="col-sm-4">
                                            <%= "<a class=\"buttonstyle\"    href=\"" + GeneraBackLink() + "\"><i class=\"fa fa-reply-all\"></i>&nbsp;" + Resources.Common.testoIndietro + "</a>"%>
                                        </div>
                                    </div>


                                    <asp:Button Style="display: none" Text="" ID="btnRegistrastatistiche" OnClick="btnRegistrastatistiche_Click" runat="server" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:Panel runat="server" ID="pnlMap" Visible='<%# ControllaVisibilitaValore(Eval("Latitudine1_dts").ToString()) %>'>
                                        <div id="map" style="height: 350px; width: 100%; border: 1px solid #ccc; margin: 0px auto">
                                        </div>
                                        <script type="text/javascript">
                                            var GooglePosizione1 = '<%# Eval("Latitudine1_dts").ToString().Replace(",",".") + "," +  Eval("Longitudine1_dts").ToString().Replace(",",".")  %>';
                                            var googleurl1 = "<%# Eval("Website").ToString()   %>";
                                            var googlepin1 = "";

                                        </script>
                                    </asp:Panel>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <div class="ui-15" runat="server" id="div1" visible="true">
                            <div class="ui-content">
                                <div class="container-fluid">
                                    <div class="row" style="padding-right: inherit">
                                        <div class="col-md-12 col-sm-12 ui-padd">
                                            <!-- Ui Form -->
                                            <div class="ui-form">
                                                <!-- Heading -->
                                                <h3 class="h3-sidebar-title sidebar-title">
                                                    <%= Resources.Common.TestoDisponibilita %>
                                                </h3>
                                                <!-- Form -->


                                                <!-- UI Input -->
                                                <div class="ui-input">
                                                    <!-- Input Box -->
                                                    <input class="form-control" type="text" name="uname" validationgroup="contattilateral" placeholder="Nome" runat="server" id="txtContactName" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Cognome" runat="server" id="txtContactCognome" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Telefono" runat="server" id="txtContactPhone" />
                                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input type="text" class="form-control" name="unname" validationgroup="contattilateral" placeholder="Email" runat="server" id="txtContactEmail" />
                                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contattilateral" placeholder="Messaggio .." runat="server" id="txtContactMessage" />
                                                </div>


                                                <button id="Button1" class="btn btn-blue btn-lg btn-block" runat="server" validationgroup="contattilateral" onserverclick="btnContatti_Click"><%= Resources.Common.TestoInvio %></button>
                                                <asp:CheckBox ID="chkContactPrivacy" runat="server" Style="font-weight: 300; font-size: 10px" Checked="true" Text="Acconsento al trattamento dei miei dati personali (D.Lgs 196/2003) " />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto2Err %>" ValidationGroup="contattilateral" ControlToValidate="txtContactName" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto16lErr %>" ValidationGroup="contattilateral" ControlToValidate="txtContactCognome" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto4Err %>" ValidationGroup="contattilateral" ControlToValidate="txtContactEmail" runat="server" />
                                                <div style="font-weight: 300; font-size: 10px; color: red">
                                                    <asp:Literal Text="" ID="outputContact" runat="server" />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>




                    </div>
                    <%--    </ContentTemplate>
                </asp:UpdatePanel>--%>
                </div>
            </div>
            <!-- Go to www.addthis.com/dashboard to customize your tools -->
            <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-58fdc705ad8f59e1"></script>

            <div runat="server" id="divSuggeriti" style="margin-bottom: 15px">

                <div runat="server" visible="false" id="divScrollerSuggeriti" style="margin-top: 0px; margin-bottom: 0px">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="title-block clearfix">
                                <div class="row" style="text-align: center; padding-bottom: 10px; padding-top: 10px; margin-bottom: 5px;">
                                    <div class="headline pull-left">
                                        <h2>
                                            <%= (CodiceTipologia=="rif000100" || CodiceTipologia=="rif000101") ?  GetGlobalResourceObject("Common", "titoloCollegati").ToString(): GetGlobalResourceObject("Common", "titoloCatalogoConsigliati").ToString() %>
                                        </h2>
                                    </div>
                                    <a class="owl-btn prev-v2 pull-left" id="carousel2cprev"><i style="color: #33332e" class="fa fa-chevron-left"></i></a>
                                    <a class="owl-btn next-v2 pull-left" id="carousel2cnext"><i style="color: #33332e" class="fa fa-chevron-right"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row" style="min-height: 250px; padding-top: 0px; padding-bottom: 0px; text-align: center">
                        <div class="col-md-12 col-sm-12 col-xs-12" style="padding-left: 0px; padding-right: 0px">
                            <div class="owl-carousel-v1 owl-work-v1 margin-bottom-40">
                                <div class="owl-slider-v2" id="carousel2a">
                                    <asp:Literal Text="" runat="server" ID="litScrollerSuggeriti" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <asp:Repeater ID="rptArticoliSuggeriti" runat="server"
                        ViewStateMode="Enabled">
                        <ItemTemplate>
                            <div class="col-sm-4" style="padding-left: 0px;">
                                <div class="blog-post">

                                    <h4 style="margin-bottom: 5px">
                                        <a id="a4" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                            href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                            <asp:Literal ID="Literal1" Text='<%# estraititolo(  Eval("Denominazione" + Lingua) ) %>'
                                                runat="server"></asp:Literal>
                                            <br />
                                            <i style="font-size: 70%">
                                                <asp:Literal ID="Literal8" Text='<%# estraisottotitolo(  Eval("Denominazione" + Lingua) ) %>'
                                                    runat="server"></asp:Literal></i></a>
                                    </h4>

                                    <div class="blog-post-tags">
                                        <ul class="list-unstyled list-inline blog-info" style="margin-bottom: 0px">
                                            <li runat="server" visible='<%# VerificaPresenzaPrezzo( Eval("Prezzo") ) %>'>
                                                <span style="font-weight: 500">
                                                    <asp:Literal ID="lblPrezzo" runat="server"
                                                        Text='<%#  Resources.Common.TitoloPrezzo  + String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"),"{0:N2}",Eval("Prezzo")) + " €<br/><br/>" %>'></asp:Literal></span>
                                            </li>
                                            <li runat="server" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString()) %>'>
                                                <asp:Literal ID="Literal2" Text="pubblicato il " runat="server" /><asp:Literal ID="Literal3"
                                                    Text='<%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) + TestoSezione(Eval("CodiceTipologia").ToString()) %>'
                                                    runat="server" /></li>
                                            <li>
                                                <asp:Literal ID="litPosizione" runat="server"
                                                    Text='<%# ControlloVuotoPosizione( Eval("CodiceComune").ToString()  , Eval("CodiceProvincia").ToString(), Eval("CodiceTipologia").ToString(),Lingua ) %>'></asp:Literal>
                                            </li>

                                        </ul>
                                    </div>
                                    <div class="blog-post-featured-img img-overlay" runat="server"
                                        style="max-height: 200px; overflow: hidden" visible='<%#  !ControlloVideo(Eval("FotoCollection_M.FotoAnteprima")) %>'>
                                        <a id="a3" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                            href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                            target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                            <asp:Image ID="Anteprima" runat="server" class="img-responsive"
                                                ImageUrl='<%#  ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>'
                                                Visible='<%#  !ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima") ) %>' /></a>
                                        <div class="item-img-overlay">
                                            <a id="a8" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                                href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                                target="_self" class="portfolio-zoom" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'></a>
                                        </div>
                                    </div>
                                    <div class="blog-post-featured-img img-overlay" runat="server" style="max-height: 100px; overflow: hidden">
                                        <div class="responsive-video" id="divVideo" runat="server" visible='<%#  ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima"), Eval("linkVideo")  ) %>'>
                                            <iframe id="Iframe2" src='<%#  SorgenteVideo(  Eval("linkVideo") ) %>'
                                                visible='<%#  ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima") ) %>'
                                                runat="server" frameborder="0" allowfullscreen></iframe>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%= SeparaRows() %>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>


            </div>
        </div>
        <div class="col-md-3 col-sm-3" runat="server" id="column3">
            <div class="sidebar">


                <!-- Sidebar Block -->
                <div class="sidebar-block" runat="server" id="divSearch" visible="false">
                    <div class="sidebar-content tags blog-search">
                        <div class="input-group">
                            <input class="form-control blog-search-input text-input" name="q" type="text" placeholder="<%$ Resources:Common,TestoCercaBlog %>" runat="server" id="inputCerca" />
                            <span class="input-group-addon">
                                <button onserverclick="Cerca_Click" id="BtnCerca" class="blog-search-button icon-reload" runat="server" clientidmode="Static" />
                            </span>
                        </div>
                    </div>
                </div>
                <!-- Sidebar Block -->
                <div class="sidebar-block" runat="server" id="divContact" visible="true">

                    <div class="sidebar-content">


                        <div class="ui-15">
                            <div class="ui-content">
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 ui-padd">
                                            <!-- Ui Form -->
                                            <div class="ui-form">
                                                <!-- Heading -->
                                                <h3 class="h3-sidebar-title sidebar-title">
                                                    <%= Resources.Common.TestoDisponibilita %>
                                                </h3>
                                                <!-- Form -->
                                                <!-- UI Input -->
                                                <div class="ui-input">
                                                    <!-- Input Box -->
                                                    <input class="form-control" type="text" name="uname" validationgroup="contattilateral" placeholder="Nome" runat="server" id="txtContactName1" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Cognome" runat="server" id="txtContactCognome1" />
                                                    <label class="ui-icon"><i class="fa fa-user"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input class="form-control" type="text" name="unname" validationgroup="contattilateral" placeholder="Telefono" runat="server" id="txtContactPhone1" />
                                                    <label class="ui-icon"><i class="fa fa-phone"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <input type="text" class="form-control" name="unname" validationgroup="contattilateral" placeholder="Email" runat="server" id="txtContactEmail1" />
                                                    <label class="ui-icon"><i class="fa fa-envelope-o"></i></label>
                                                </div>
                                                <div class="ui-input">
                                                    <textarea class="form-control" rows="4" cols="5" name="q" validationgroup="contattilateral" placeholder="Messaggio .." runat="server" id="txtContactMessage1" />
                                                </div>

                                                <button id="btnFormContatto1" class="btn btn-blue btn-lg btn-block" runat="server" validationgroup="contattilateral" onserverclick="btnContatti1_Click"><%= Resources.Common.TestoInvio %></button>

                                                <div style="clear: both"></div>
                                                <asp:CheckBox ID="chkContactPrivacy1" runat="server" Style="font-weight: 300; font-size: 10px" Checked="true" Text="Acconsento al trattamento dei miei dati personali (D.Lgs 196/2003) " />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto2Err %>" ValidationGroup="contattilateral" ControlToValidate="txtContactName1" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto16lErr %>" ValidationGroup="contattilateral" ControlToValidate="txtContactCognome1" runat="server" />
                                                <asp:RequiredFieldValidator ErrorMessage="<%$ Resources:Common,FormTesto4Err %>" ValidationGroup="contattilateral" ControlToValidate="txtContactEmail1" runat="server" />
                                                <div style="font-weight: 300; font-size: 10px; color: red">
                                                    <asp:Literal Text="" ID="outputContact1" runat="server" />
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Sidebar Block -->
                <div class="sidebar-block" id="divCategorie" runat="server" visible="false">
                    <h3 class="h3-sidebar-title sidebar-title">Categorie
                    </h3>
                    <div class="sidebar-content">
                        <ul class="posts-list">
                            <asp:Repeater ID="rptContenutiLink" runat="server"
                                ViewStateMode="Enabled">
                                <ItemTemplate>
                                    <li><i class="fa fa-check color-green"></i>
                                        <a id="linkRubriche" style="font-size: 14px" onclick="javascript:JsSvuotaSession(this)"
                                            href='<%# CreaLinkRoutes(Session,false,Lingua,CommonPage.CleanUrl(Eval("Descrizione").ToString()),"",Eval("Codice").ToString()) %>'
                                            runat="server">
                                            <asp:Label ID="Titolo" runat="server" Text='<%# Eval("Descrizione").ToString() %>'></asp:Label></a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptProdottiContenutiLink" runat="server"
                                ViewStateMode="Enabled">
                                <ItemTemplate>
                                    <li>
                                        <a id="linkRubriche" onclick="javascript:JsSvuotaSession(this)" href='<%# CommonPage.CreaLinkRoutes(Session, false, Lingua, CommonPage.CleanUrl(Eval("Descrizione").ToString()), "", Eval("CodiceTipologia").ToString(), Eval("CodiceProdotto").ToString())%>'
                                            runat="server">
                                            <asp:Label ID="Titolo" runat="server" Text='<%# Eval("Descrizione").ToString()%>'></asp:Label>
                                        </a>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
                <!-- Sidebar Block -->
                <div id="divLatestpostContainerTitle" class="row" style="display: none">
                    <div class="col-sm-12">
                        <div class="subtitle-block clearfix">
                            <div class="row" style="text-align: left; padding-bottom: 0px; padding-top: 30px; margin-bottom: 0px; line-height: 40px; color: #33332e; border-bottom: 1px solid #33332e">
                                <%= Resources.Common.TestoPanel1  %>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divLatestpostContainer"></div>
                <div id="divLatestpostContainerPager"></div>
                <div class="sidebar-block" runat="server" id="divLatestPost" visible="false">
                    <h3 class="h3-sidebar-title sidebar-title"><%=  Resources.Common.TestoPanel1 %>
                    </h3>
                    <div class="sidebar-content">
                        <ul class="posts-list">
                            <asp:Repeater ID="rtpLatestPost" runat="server"
                                ViewStateMode="Enabled">
                                <ItemTemplate>
                                    <li>
                                        <div class="posts-list-thumbnail">
                                            <a id="a5" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                                href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                                <asp:Image ID="Anteprima" runat="server" Style="max-width: 55px; max-height: 55px"
                                                    ImageUrl='<%#  ComponiUrlAnteprima(Eval("FotoCollection_M.FotoAnteprima"),Eval("CodiceTipologia").ToString(),Eval("Id").ToString()) %>'
                                                    Visible='<%#  !ControlloVideo ( Eval("FotoCollection_M.FotoAnteprima") ) %>' />
                                            </a>
                                        </div>
                                        <div class="posts-list-content">
                                            <a class="posts-list-title" id="a1" runat="server" onclick="javascript:JsSvuotaSession(this)"
                                                href='<%# CreaLinkRoutes(Session,false,Lingua,CleanUrl(Eval("Denominazione" + Lingua).ToString()),Eval("Id").ToString(),Eval("CodiceTipologia").ToString(), Eval("CodiceCategoria").ToString()) %>'
                                                target="_self" title='<%# CleanInput(ConteggioCaratteri(  Eval("Denominazione" + Lingua).ToString(),300,true )) %>'>
                                                <asp:Literal ID="litTitolo" Text='<%# WelcomeLibrary.UF.Utility.SostituisciTestoACapo(  Eval("Denominazione" + Lingua).ToString() ) %>'
                                                    runat="server"></asp:Literal></a>
                                            <span class="posts-list-meta" runat="server" visible='<%# ControllaVisibilitaPerCodice(Eval("CodiceTipologia").ToString()) %>'>
                                                <%-- <asp:Literal ID="lblBrDesc" Text='<%#  ReplaceLinks(WelcomeLibrary.UF.Utility.SostituisciTestoACapo( ConteggioCaratteri(   Eval("Descrizione" + Lingua).ToString() ,200,true  )  ) , false) %>'
                                            runat="server"></asp:Literal>--%>
                                                <i class="fa fa-calendar"></i>
                                                <asp:Literal ID="Literal2" Text="<%$ Resources:Common,TestoPubblicatodata  %>" runat="server" /><asp:Literal ID="Literal3"
                                                    Text='<%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) + TestoSezione(Eval("CodiceTipologia").ToString()) %>'
                                                    runat="server" />
                                            </span>
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
                <div id="divContainerBannerslat1"></div>
                <div class="sidebar-block" runat="server" id="div2" visible="false">
                    <div class="row" style="margin-bottom: 10px; margin-top: 0px">
                        <ul class="works-grid works-grid-gut works-grid-1 works-hover-lw">
                            <asp:Literal ID="litBannersLaterali" runat="server"></asp:Literal>
                        </ul>
                    </div>
                </div>
                <!-- Sidebar Block -->
                <div class="sidebar-block" runat="server" id="divArchivio" visible="false">
                    <h3 class="h3-sidebar-title sidebar-title"><%= Resources.Common.TestoArchivio %>
                    </h3>
                    <div class="sidebar-content" style="overflow-y: auto" id="divArchivioList">

                        <asp:Repeater ID="rptArchivio" runat="server">
                            <ItemTemplate>
                                <ul class="posts-list" id="ulAnno" runat="server" title='<%# Eval("Key").ToString() %>'>
                                    <asp:Repeater ID="rptArchivioMesi" runat="server" DataSource='<%# Eval("Value") %>'
                                        OnItemDataBound="rptArchivioMesi_ItemDataBound">
                                        <ItemTemplate>
                                            <li style="border-top: 1px dotted #e5e5e5; margin-top: 10px; padding-top: 10px">
                                                <i class="fa fa-check color-green"></i>
                                                <a id="alink" runat="server" href="#" target="_self">(<asp:Literal ID="NumeroElementi"
                                                    runat="server" Text='<%# Eval("Value").ToString() %>'></asp:Literal>)</a>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </ItemTemplate>
                        </asp:Repeater>

                    </div>
                </div>

            </div>
        </div>

    </div>

    <%--   <div class="row">
            <div id="fb-root"></div>
            <script type="text/javascript">
                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) return;
                    js = d.createElement(s); js.id = id;
                    js.src = "//connect.facebook.net/it_IT/all.js#xfbml=1&appId=435846069925856";
                    fjs.parentNode.insertBefore(js, fjs);
                }(document, 'script', 'facebook-jssdk'));
            </script>
            <div class="fb-comments" data-width="650" data-num-posts="5" runat="server" id="divComments"></div>
        </div>--%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderIndextext" runat="Server">
    <div style="background-color: #f2ece6;">
        <div class="container">
            <div class="row">
                <div id="divScrollerSuggeritiJsTitle" class="row" style="display: none; margin-left: 30px;margin-right:30px">
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="subtitle-block clearfix">
                                <div class="row" style="text-align: left; padding-bottom: 0px; padding-top: 30px; margin-bottom: 0px; line-height: 40px; color: #33332e; border-bottom: 1px solid #33332e">
                                    <div class="pull-left lead">
                                        <h2 style="margin-bottom:3px">
                                            <%= (CodiceTipologia=="rif000100" || CodiceTipologia=="rif000101" || CodiceTipologia=="rif000003") ?  GetGlobalResourceObject("Common", "titoloCollegati").ToString(): GetGlobalResourceObject("Common", "titoloCatalogoConsigliati").ToString() %>
                                        </h2>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div id="divScrollerSuggeritiJs"></div>
            </div>
        </div>
    </div>
</asp:Content>
