$(window).load(function(){
	$("body").addClass("loaded");
})
$(window).scroll(function() {
	var height = $(window).scrollTop();
	if (height > 50) 
	{
		$('html').addClass('sticky');				
	} else {					
		$('html').removeClass('sticky');				
	}							
});						
			
$(document).ready(function() {				
	$(".scrollToTop").click(function(event) {
		event.preventDefault();					
		$("html, body").animate({ scrollTop: 0 }, "slow");
		return false;				
	});
	$('.course_link').mouseenter(function() {
		$("body").addClass("courses_show");
	});
	$('.close_menu').click(function() {
		$("body").removeClass("courses_show");
	});
	$('.plus-icon').click(function() {
		$('.menu-drodown > ul').slideToggle('slow', function() {});
		$(this).parent().toggleClass('submenu-show');
	});
	$('.sidebar-toggle').click(function() {
		$("html").toggleClass("sidebar-show");
	});
	$('.toggle-menu').click(function() {
		$("html").toggleClass("menu-show");
	});
	$('.o-BClose').click(function() {
		$("html").removeClass("menu-show");
	});
	$('.navbar-nav a').click(function() {
		$("html").removeClass("menu-show");
	});
	$('.custom_close').click(function() {
		$("body").removeClass("custom_show");
	});
	$('.info-btn a').click(function() {
		$(this).parent().parent().toggleClass('show');
	});
	$('#info-box-close').click(function() {
		$(".info-box").removeClass("show");
	});
});

