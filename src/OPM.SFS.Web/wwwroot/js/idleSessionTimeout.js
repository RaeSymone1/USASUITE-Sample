var notificationInterval,
    logoutInterval,
    logoutCounterSpan;

function startNotificationCounter() {
    var counter = 20; //session timeout in 20 minutes
    notificationInterval = setInterval(function () {
        counter--;
        if (counter === 5) { //show warning 5 minutes before expiring
            $('#session-expire-warning').modal();
            startLogoutCounter();
        }
    },
        60000);
}

function startLogoutCounter() {
    var counter = 300; //5 minute counter for logout 5 * 60
    logoutInterval = setInterval(function () {
        counter--;
        if (counter < 0) {
            //session has timed out so redirect to home page
            window.location = '/';
        } 
    },
        1000);
}
function resetCounters() {
    clearInterval(notificationInterval);
    clearInterval(logoutInterval);
    startNotificationCounter();
}
function onSessionExpireNotificationClose() {
    resetCounters();
}

$(document).ready(function () {
    logoutCounterSpan = $("#logout-counter-span");
    startNotificationCounter();
    $("#stay-logged-in-button").click(function () {
        $.get("/Index/?handler=ExtendSession",
            null,
            function (data) {
                resetCounters();
                $.modal.close();
            }
        );
    });
    $("#signout-button").click(function () {
        $.get("/Index/?handler=SignOut",
            null,
            function (data) {
                $.modal.close();
                window.location = '/';
            }
        );
    });
});