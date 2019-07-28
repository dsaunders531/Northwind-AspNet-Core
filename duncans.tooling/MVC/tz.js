$(document).ready(function ()
{
    // The .getTimezoneOffset function does not work in all browsers or returns bad results so work it out manually.
    var tzOffsetMins = 0;

    if (CookieMonster.HasCookie("UserAgentTzOffset") == false) {     
        var dtmUser = Date(0); // Mon Apr 01 2019 10:52:05 GMT+0100 (British Summer Time)
        var intStart = dtmUser.indexOf('GMT') + 3;
        var intEnd = dtmUser.indexOf(' ', intStart);
        var strGMTpart = dtmUser.substring(intStart, intEnd);
        var directionPart = strGMTpart.substring(0, 1);
        var hoursPart = Number(strGMTpart.substring(1, 3));
        var minutesPart = Number(strGMTpart.substring(3));

        tzOffsetMins = (hoursPart * 60) + minutesPart;

        if (directionPart == '-') {
            tzOffsetMins = tzOffsetMins * -1;
        }

        CookieMonster.SetCookie("UserAgentTzOffset", tzOffsetMins, "session");
    }

    // Now change the form values as these cannot be set in views and will be generated UTC serverside.
    // text datetime values will need to be set server side.
    if (tzOffsetMins != 0) {
        var formTimeEles = document.querySelectorAll('input[type="time"]');
        var formDateTimeEles = document.querySelectorAll('input[type="datetime"]');
        var formDateTimeLocalEles = document.querySelectorAll('input[type="datetime-local"]');

        for (var i = 0; i < formTimeEles.length; i++) { 
            if (Date.parse(formTimeEles[i].value.length > 0)) {
                if (Date.parse(formTimeEles[i].value) != NaN) {
                    var eleValue = new Date(formTimeEles[i].value);
                    var mins = eleValue.getMinutes();
                    eleValue = new Date(eleValue.setMinutes(mins + tzOffsetMins));
                    formTimeEles[i].attributes['value'].value = eleValue.toLocaleTimeString();                    
                }
            }           
        }

        for (var i = 0; i < formDateTimeEles.length; i++) {
            if (Date.parse(formDateTimeEles[i].value.length > 0)) {
                if (Date.parse(formDateTimeEles[i].value) != NaN) {
                    var eleValue = new Date(formDateTimeEles[i].value);
                    var mins = eleValue.getMinutes();
                    eleValue = new Date(eleValue.setMinutes(mins + tzOffsetMins));
                    formDateTimeEles[i].attributes['value'].value = eleValue.toString();
                }
            }               
        }

        for (var i = 0; i < formDateTimeLocalEles.length; i++) {
            if (Date.parse(formDateTimeLocalEles[i].value.length > 0)) {
                if (Date.parse(formDateTimeLocalEles[i].value) != NaN) {
                    var eleValue = new Date(formDateTimeLocalEles[i].value);
                    var mins = eleValue.getMinutes();
                    eleValue = new Date(eleValue.setMinutes(mins + tzOffsetMins));
                    formDateTimeLocalEles[i].attributes['value'].value = eleValue.toString();
                }
            }
        }
    }
});