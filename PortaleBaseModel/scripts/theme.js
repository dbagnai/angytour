$(function() {
	$('.nav-tabs a').click(function (e) {
		e.preventDefault();
		$(this).tab('show');
	})
	$('.nav-tabs a:first').tab('show');
});