$(document).ready(function () {
	
 /** home page cards section **/
    var activeCardNumber = 1;
    var movingCards = null;
    var windowsize = false;
    if($(window).width()>1366){
        var first = '40px';
        var second = '12px';
        var thrid = '-30px';
        var four ='-70px';
        var five= '0px';
        var six ='0px';
    }
    else{
        var first = '0em';
        var second = '-1.2em';
        var thrid = '-3em';
        var four ='-4.7em';
        var five= '-4.5em';
        var six ='-5em';
    }
    $('.card1_click').click(function () {
        activeCardNumber = 1;
            stopTimer();
            startTimer();
        
        $(this).addClass('active-card');
        $(this).attr('rel', 'active-card');
        $(this).parent().siblings().children().removeClass('active-card');
        $(this).parent().siblings().children().removeAttr('rel');
        
        /** card functionality start here **/
        $('.first_card').css({
            'transform': 'translateY('+first+') scale(1)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '99'
        });
        $('.second_card').css({
            'transform': 'translateY('+second+') scale(0.95)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '98'
        });
        $('.third_card').css({
            'transform': 'translateY('+thrid+') scale(0.85)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '97'
        });
        $('.fourth_card').css({
            'transform': 'translateY('+four+') scale(0.75)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '96'
        });
		$('.five_card').css({
            'transform': 'translateY('+five+') scale(0.65)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '95'
        });
		$('.six_card').css({
            'transform': 'translateY('+six+') scale(0.55)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '94'
        });
        
    });
    $('.card2_click').click(function () {
        activeCardNumber = 2;
        stopTimer();
        startTimer();
        $(this).addClass('active-card');
        $(this).attr('rel', 'active-card');
        $(this).parent().siblings().children().removeClass('active-card');
        $(this).parent().siblings().children().removeAttr('rel');
        
            $('.first_card').css({
            'transform': 'translateY('+six+') scale(0.55)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '94'
        });
        $('.second_card').css({
            'transform': 'translateY('+first+') scale(1)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '99'
        });
        $('.third_card').css({
            'transform': 'translateY('+second+') scale(0.95)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '98'
        });
        $('.fourth_card').css({
            'transform': 'translateY('+thrid+') scale(0.85)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '97'
        });
		$('.five_card').css({
            'transform': 'translateY('+four+') scale(0.75)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '96'
        });
		$('.six_card').css({
            'transform': 'translateY('+five+') scale(0.65)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '95'
        });
        
    });
    $('.card3_click').click(function () {
        activeCardNumber = 3;
        stopTimer();
        startTimer();
        $(this).addClass('active-card');
        $(this).attr('rel', 'active-card');
        $(this).parent().siblings().children().removeClass('active-card');
        $(this).parent().siblings().children().removeAttr('rel');

        /** card functionality start here **/
        $('.first_card').css({
            'transform': 'translateY('+five+') scale(0.65)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '95'

        });
        $('.second_card').css({
            'transform': 'translateY('+six+') scale(0.55)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '94'
        });
        $('.third_card').css({
            'transform': 'translateY('+first+') scale(1)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '99'
        });
        $('.fourth_card').css({
            'transform': 'translateY('+second+') scale(0.95)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '98'
        });
		 $('.five_card').css({
            'transform': 'translateY('+thrid+') scale(0.85)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '97'
        });
		 $('.six_card').css({
            'transform': 'translateY('+four+') scale(0.75)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '96'
        });
    });
    $('.card4_click').click(function () {
        activeCardNumber = 4;
        stopTimer();
        startTimer();
        $(this).addClass('active-card');
        $(this).attr('rel', 'active-card');
        $(this).parent().siblings().children().removeClass('active-card');
        $(this).parent().siblings().children().removeAttr('rel');

        /** card functionality start here **/
        $('.first_card').css({
            'transform': 'translateY('+four+') scale(0.75)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '96'
        });
        $('.second_card').css({
            'transform': 'translateY('+five+') scale(0.65)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '95'
        });
        $('.third_card').css({
            'transform': 'translateY('+six+') scale(0.55)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '94'
        });
        $('.fourth_card').css({
            'transform': 'translateY('+first+') scale(1)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '99'
        });
		$('.five_card').css({
            'transform': 'translateY('+second+') scale(0.95)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '98'
        });
		$('.six_card').css({
            'transform': 'translateY('+thrid+') scale(0.85)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '97'
        });

    });
	 $('.card5_click').click(function () {
        activeCardNumber = 5;
        stopTimer();
        startTimer();
        $(this).addClass('active-card');
        $(this).attr('rel', 'active-card');
        $(this).parent().siblings().children().removeClass('active-card');
        $(this).parent().siblings().children().removeAttr('rel');

        /** card functionality start here **/
        $('.first_card').css({
            'transform': 'translateY('+thrid+') scale(0.85)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '97'
        });
        $('.second_card').css({
            'transform': 'translateY('+four+') scale(0.75)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '96'
        });
        $('.third_card').css({
            'transform': 'translateY('+five+') scale(0.65)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '95'
        });
        $('.fourth_card').css({
            'transform': 'translateY('+six+') scale(0.55)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '94'
        });
		$('.five_card').css({
            'transform': 'translateY('+first+') scale(1)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '99'
        });
		$('.six_card').css({
            'transform': 'translateY('+second+') scale(0.95)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '98'
        });

    });
	 $('.card6_click').click(function () {
        activeCardNumber = 6;
        stopTimer();
        startTimer();
        $(this).addClass('active-card');
        $(this).attr('rel', 'active-card');
        $(this).parent().siblings().children().removeClass('active-card');
        $(this).parent().siblings().children().removeAttr('rel');

        /** card functionality start here **/
        $('.first_card').css({
            'transform': 'translateY('+second+') scale(0.95)',
            'opacity': '1',
			
            'visibility': 'visible',
            'z-index': '98'
        });
        $('.second_card').css({
            'transform': 'translateY('+thrid+') scale(0.85)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '97'
        });
        $('.third_card').css({
            'transform': 'translateY('+four+') scale(0.75)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '96'
        });
        $('.fourth_card').css({
            'transform': 'translateY('+five+') scale(0.65)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '95'
        });
		$('.five_card').css({
            'transform': 'translateY('+six+') scale(0.55)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '94'
        });
		$('.six_card').css({
            'transform': 'translateY('+first+') scale(1)',
            'opacity': '1',
            'visibility': 'visible',
            'z-index': '99'
        });

    });

 if ($('.card_list').length > 0) {
        function startTimer() {
                movingCards = setInterval(function () {
                    moveRight();
                }, 7000);
           
        }
     

        function stopTimer() {
            clearInterval(movingCards);
         
        }

        function moveRight() {
             if($( window ).width()<1200){
                var len = $('.card_list ul.card_list_box li');
                if (len.length == activeCardNumber) {
                    $(".card_list ul.card_list_box li:nth-child(1)").children('a').trigger('click');
                } else {
                    $(".card_list ul.card_list_box li:nth-child(" + activeCardNumber + ")").next().children('a').trigger('click');
                }
            }
            
        }
        startTimer();

    }

 
});

