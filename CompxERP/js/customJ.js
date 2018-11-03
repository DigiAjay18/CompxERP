(function (global) {



    var _hash = "!";
    var noBackPlease = function () {
        global.location.href += "#";

        // making sure we have the fruit available for juice....
        // 50 milliseconds for just once do not cost much (^__^)
        global.setTimeout(function () {
            global.location.href += "!";
        }, 50);
    };

    // Earlier we had setInerval here....
    global.onhashchange = function () {
        if (global.location.hash !== _hash) {
            global.location.hash = _hash;
        }
    };

    global.onload = function () {

        noBackPlease();


    };

})(window);

    window.onload = function () {
            var chart1 = document.getElementById("line-chart").getContext("2d");
            window.myLine = new Chart(chart1).Line(lineChartData, {
                responsive: true,
                scaleLineColor: "rgba(202, 55, 55, 0.41)",
                scaleGridLineColor: "rgba(0,0,0,.05)",
                scaleFontColor: "#c5c7cc"
             
            });
        };
	

(function () {
    // Get current date
    var date = new Date();

    // Format day/month/year to two digits
    var formattedDate = ('0' + date.getDate()).slice(-2);
    var formattedmonthNames = [
     "Jan", "Feb", "March",
     "April", "May", "June", "July",
     "Aug", "Sep", "Oct",
     "Nov", "Dec"
    ];
    var formattedmonthIndex = date.getMonth();
    var formattedYear = date.getFullYear().toString().substr(2, 2);

    // Combine and format date string
    var dateString = formattedDate + '-' + formattedmonthNames[formattedmonthIndex] + '-' + formattedYear;

    // Reference output DIV
    var output = document.querySelector('#output');

    // Output dateString
    output.innerHTML = dateString;
})();


          function checkTime(i) {
              if (i < 10) {
                  i = "0" + i;
              }
              return i;
          }

function startTime() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();
    // add a zero in front of numbers<10
    m = checkTime(m);
    s = checkTime(s);
    document.getElementById('time').innerHTML = h + ":" + m + ":" + s;
    t = setTimeout(function () {
        startTime()
    }, 500);
}
startTime();


