//Tooltip 
$(function () {
	$('.ui-tooltip').tooltip();
});
$(document).ready(function () {
	$("a.ui-btn").click(function (e) {
		e.preventDefault();
		if (!($(this).hasClass("active"))) {
			$(".ui-list").animate({
				height: "260px",
				opacity: "1"
			});
			$(this).addClass("active");
		}
		else {
			$(".ui-list").animate({
				height: "0px",
				opacity: "0"
			});
			$(this).removeClass("active");
		}
	});
});