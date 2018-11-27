<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="Content_Tipo1.aspx.cs" Inherits="AspNetPages_Content_Tipo1" Title=""
    MaintainScrollPositionOnPostback="true" EnableViewState="false" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">
    <script type="text/javascript">
        var makeRevLower = true;
    </script>
    <div id="divTitleContainer">
    <div class="container" style="text-align: center;" runat="server" id="divTitle">
        <div class="row">
            <div class="col-md-1 col-sm-1">
            </div>
            <div class="col-md-10 col-sm-10 col-xs-12">
                <h1 class="title-block" style="line-height: normal;">
                    <asp:Literal Text="" runat="server" ID="litNomeContenuti" /></h1>
            </div>
            <div class="col-md-1 col-sm-1">
            </div>
        </div>
    </div>
    </div>
    <asp:Literal ID="litMainContent" runat="server"></asp:Literal>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Panel runat="server" ID="pnlNewsletter" Visible="false">
        <br />
        <h2 class="h2-section-title animated" data-animtype="flipInY"
            data-animrepeat="0"
            data-speed="1s"
            data-delay="0.2s">
            <asp:Literal Text='<%# references.ResMan("Common", Lingua,"titoloNewsletterc") %>' runat="server" />
        </h2>
        <div class="section-subscribe animated" data-animtype="flipInX"
            data-animrepeat="0"
            data-speed="1s"
            data-delay="0.5s">
            <p>
                <asp:Literal Text='<%# references.ResMan("Common", Lingua,"TestoNewsletterForm") %>' runat="server" />
            </p>
            <input type="text" name="q" class="subscribe-input text-input" validationgroup="newsletter1" placeholder="Name .." runat="server" id="txtNome" />
            <br />
            <br />
            <input type="text" name="q" class="subscribe-input text-input" validationgroup="newsletter1" placeholder="Email .." runat="server" id="txtEmail" />
            <button class="subscribe-button icon-email-plane" runat="server" validationgroup="newsletter1" onserverclick="btnNewsletter_Click"></button>
            <asp:RequiredFieldValidator ErrorMessage='<%# references.ResMan("Common", Lingua,"FormTesto2Err") %>' ValidationGroup="newsletter1" ControlToValidate="txtEmail" runat="server" />
        </div>
    </asp:Panel>




    <script type="text/javascript">
        $(document).ready(function () {
            inizializzaFlexsliderSchedaGallery();
        });
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


    </script>

    <div class="row" runat="server" id="divGalleryDetail" visible="false">
        <div class="col-md-1 col-sm-1">
        </div>
        <div class="col-md-10 col-sm-10 col-xs-12">
            <asp:Repeater ID="rptOfferteGalleryDetail" runat="server">
                <ItemTemplate>
                    <div class="blog-post" style="text-align: justify; background-color: transparent; border: none">
                        <div class="blog-span">

                            <div style="width: 100%" runat="server"
                                visible='<%# ControlloVisibilita(Eval("FotoCollection_M"))  %>'>
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
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="col-md-1 col-sm-1">
        </div>
    </div>
    
</asp:Content>

