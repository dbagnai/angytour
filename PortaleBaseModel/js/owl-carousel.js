var OwlCarousel = function () {

    return {
        
        //Owl Carousel
        initOwlCarousel: function () {
		    jQuery(document).ready(function() {
		        //Owl Slider v1
		        var owl = jQuery(".owl-slider");
		            owl.owlCarousel({
		                itemsDesktop : [1199,3], // i/tems between 1000px and 601px
		                itemsTablet: [979,2], // items between 600 and 0;
		                itemsMobile : [479,1] // itemsMobile disabled - inherit from itemsTablet option
		            });

		            // Custom Navigation Events
		            jQuery(".next-v1").click(function(){
		                owl.trigger('owl.next');
		            })
		            jQuery(".prev-v1").click(function(){
		                owl.trigger('owl.prev');
		            })
		        });

		        //Owl Slider v2
		        jQuery(document).ready(function() {
		            var owl = jQuery("#carousel1");
		            owl.owlCarousel({
		            	items: [3],
		            	itemsDesktop : [1199,3], // i/tems between 1000px and 601px
		            	itemsTablet: [979,2], // items between 600 and 0;
		            	itemsMobile : [479,1], // itemsMobile disabled - inherit from itemsTablet option
		                slideSpeed: 1000
		            });
		            // Custom Navigation Events
		            jQuery("#carousel1next").click(function () {
		                owl.trigger('owl.next');
		            })
		            jQuery("#carousel1prev").click(function () {
		                owl.trigger('owl.prev');
		            })
		        });

            //Owl Slider v2
		        jQuery(document).ready(function () {
		            var owl = jQuery("#carousel2");
		            owl.owlCarousel({
		                items: [3],
		                itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
		                itemsTablet: [979, 2], // items between 600 and 0;
		                itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
		                slideSpeed: 1000
		            });
		            // Custom Navigation Events
		            jQuery("#carousel2next").click(function () {
		                owl.trigger('owl.next');
		            })
		            jQuery("#carousel2prev").click(function () {
		                owl.trigger('owl.prev');
		            })
		        });


		        jQuery(document).ready(function () {
		            var owl = jQuery("#carousel2a");
		            owl.owlCarousel({
		                items: [4],
		                autoPlay: 5000,
		                itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
		                itemsTablet: [979, 2], // items between 600 and 0;
		                itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
		                slideSpeed: 1000
		            });

		            // Custom Navigation Events
		            jQuery("#carousel2anext").click(function () {
		                owl.trigger('owl.next');
		            })
		            jQuery("#carousel2aprev").click(function () {
		                owl.trigger('owl.prev');
		            })
		        });

		        jQuery(document).ready(function () {
		            var owl = jQuery("#carousel2b");
		            owl.owlCarousel({
		                items: [3],
		                itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
		                itemsTablet: [979, 2], // items between 600 and 0;
		                itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
		                slideSpeed: 1000,
		                autoPlay: 5000 
		            });

		            // Custom Navigation Events
		            jQuery("#carousel2bnext").click(function () {
		                owl.trigger('owl.next');
		            })
		            jQuery("#carousel2bprev").click(function () {
		                owl.trigger('owl.prev');
		            })
		        });

		        jQuery(document).ready(function () {
		            var owl = jQuery("#carousel2c");
		            owl.owlCarousel({
		                items: [3],
		                autoPlay: 5000,
		                itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
		                itemsTablet: [979, 2], // items between 600 and 0;
		                itemsMobile: [479, 1], // itemsMobile disabled - inherit from itemsTablet option
		                slideSpeed: 1000
		            });

		            // Custom Navigation Events
		            jQuery("#carousel2cnext").click(function () {
		                owl.trigger('owl.next');
		            })
		            jQuery("#carousel2cprev").click(function () {
		                owl.trigger('owl.prev');
		            })
		        });

		        //Owl Slider v3
		        jQuery(document).ready(function() {
		        var owl = jQuery(".owl-slider-v3");
		            owl.owlCarousel({
		            	items : 9,
		            	autoPlay : 5000,
		            	itemsDesktop: [1199, 3], // i/tems between 1000px and 601px
		            	itemsTablet: [979, 2], // items between 600 and 0;
		            	itemsMobile: [479, 1] // itemsMobile disabled - inherit from itemsTablet option
		            });

		            // Custom Navigation Events
		            jQuery(".next-v3").click(function () {
		                owl.trigger('owl.next');
		            })
		            jQuery(".prev-v3").click(function () {
		                owl.trigger('owl.prev');
		            })
		        });

		        //Owl Slider v4
		        jQuery(document).ready(function() {
		        var owl = jQuery(".owl-slider-v4");
		            owl.owlCarousel({
		                items:3,
		                itemsDesktop : [1000,3], //3 items between 1000px and 901px
		                itemsTablet: [600,2], //2 items between 600 and 0;
		                itemsMobile: [479, 1], //1 itemsMobile disabled - inherit from itemsTablet option
		                slideSpeed: 1000
		            });

		            // Custom Navigation Events
		            jQuery(".next-v4").click(function () {
		                owl.trigger('owl.next');
		            })
		            jQuery(".prev-v4").click(function () {
		                owl.trigger('owl.prev');
		            })
		    });
		}
    };
    
}();