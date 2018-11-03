
function OnlyTabWork(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode == 9)return true;return false;
}
function isBackDelKey(event, obj) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode == 8 || charCode == 127 || charCode == 9 || charCode == 11) { if (charCode == 8 || charCode == 127) obj.value = ""; return true; } else return false; return true;
}
function isNumberKey(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) return false; return true;
}
function isNumberKeyDot(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) return false; return true;
}
function isNumberKeySls(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode < 47 || charCode > 57) return false; return true;
}
function CapFirstAllWords(event, obj) { str = obj.value; var pieces = str.split(" "); for (var i = 0; i < pieces.length; i++) { var j = pieces[i].charAt(0).toUpperCase(); pieces[i] = j + pieces[i].substr(1).toLowerCase(); } obj.value = pieces.join(" "); }//sourabh 19-sep-2016
//function CapAllWords(event, obj) { str = obj.value; var pieces = str.split(" "); for (var i = 0; i < pieces.length; i++) { var j = pieces[i].charAt(0).toUpperCase(); pieces[i] = j + pieces[i].substr(1).toUpperCase(); } obj.value = pieces.join(" "); }//sourabh 19-sep-2016
function getQueryString(name) {//Use in MCPL
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}
function isDate(txtDate) {
    var currVal = txtDate.value;
    if (currVal == '' || currVal == '__/__/____')
        return false;

    //Declare Regex 
    var rxDatePattern = /^(\d{1,2})(\/|.)(\d{1,2})(\/|.)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?
    if (dtArray == null) {
        alert('Invalid Date.');
        txtDate.value = '';
    }
    //Checks for dd/mm/yyyy format.
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12) 
    { alert('Invalid Date'); txtDate.value = ''; }
    else if (dtDay < 1 || dtDay > 31) {
        alert('Invalid Date');
        txtDate.value = '';
    }
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31) {
        alert('Invalid Date');
        txtDate.value = '';
    }
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap)) {
            alert('Invalid Date');
            txtDate.value = '';
        }
    }
}
function Comma(Numb) {//Use in MCPL
    var Num = Numb.value;
    Num += '';
    Num = Num.replace(/,/g, '');
    x = Num.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d)((\d)(\d{2}?)+)$/;
    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    Numb.value = x1 + x2;
}
function formatNumber(number) {
    if (number != "") {
        number = number.toFixed(2) + '';
        x = number.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }

        return x1 + x2;
    }
    return "0";
}