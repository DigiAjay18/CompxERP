/*
Template Name: Soft Themez - Software Landing Page Template
Author: Askbootstrap
Author URI: https://themeforest.net/user/askbootstrap
Version: 1.0
*/
$(document).ready(function() {
    "use strict";

    // ===========Navbar============
    $('.navbar-nav li.dropdown').hover(function() {
        $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeIn(500);
    }, function() {
        $(this).find('.dropdown-menu').stop(true, true).delay(100).fadeOut(500);
    });

    // ===========Navbar Scroll============
    $(window).scroll(function() {
        var scroll = $(window).scrollTop();

        if (scroll >= 100) {
            $(".navbar").addClass("fixed-navbar inner-navbar fixed-top");
        } else {
            $(".navbar").removeClass("fixed-navbar inner-navbar fixed-top");
        }
    });

    // ===========Screens============
    var screensslider = $(".screens");
    if (screensslider) {
        screensslider.owlCarousel({
            center: true,
            items: 2,
            loop: true,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],


            responsive: {
                0: {
                    items: 1,
                    nav: true
                },
                800: {
                    items: 2,
                    nav: true
                },
            }
        });
    }

    // ===========Blogs Slider============
    var blogslider = $(".blogs-slider");
    if (blogslider.length > 0) {
        blogslider.owlCarousel({
            items: 3,


            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                800: {
                    items: 3,
                    nav: false
                },
            }
        });
    }

  
	// ===========wow============
	
	// ===========wow============
	new WOW().init();	
	
	$(function () {
    $('span').click(function () {
        $('#datalist li:hidden').slice(0, 2).show();
        if ($('#datalist li').length == $('#datalist li:visible').length) {
            $('span ').hide();
        }
    });
    });
	
	
	
	
	


jQuery(document).ready(function($){
 
    // Define a blank array for the effect positions. This will be populated based on width of the title.
    var bArray = [];
    // Define a size array, this will be used to vary bubble sizes
    var sArray = [4,6,8,10];
 
    // Push the header width values to bArray
    for (var i = 0; i < $('.bubbles').width(); i++) {
        bArray.push(i);
    }
     
    // Function to select random array element
    // Used within the setInterval a few times
    function randomValue(arr) {
        return arr[Math.floor(Math.random() * arr.length)];
    }
 
    // setInterval function used to create new bubble every 350 milliseconds
    setInterval(function(){
         
        // Get a random size, defined as variable so it can be used for both width and height
        var size = randomValue(sArray);
        // New bubble appeneded to div with it's size and left position being set inline
        // Left value is set through getting a random value from bArray
        $('.bubbles').append('<div class="individual-bubble" style="left: ' + randomValue(bArray) + 'px; width: ' + size + 'px; height:' + size + 'px;"></div>');
         
        // Animate each bubble to the top (bottom 100%) and reduce opacity as it moves
        // Callback function used to remove finsihed animations from the page
        $('.individual-bubble').animate({
            'bottom': '100%',
            'opacity' : '-=0.7'
        }, 5000, function(){
            $(this).remove()
        }
        );
 
 
    }, 350);
 
});

});