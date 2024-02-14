var idleTime = 0;
$(document).ready(function () {
    // Increment the idle time counter every minute.
    var idleInterval = setInterval(timerIncrement, 60000); // 1 minute

    // Zero the idle timer on mouse movement.
    $(this).mousemove(function (e) {
        idleTime = 0;
    });
    $(this).keypress(function (e) {
        idleTime = 0;
    });
});

function timerIncrement() {
    idleTime = idleTime + 1;
    if (idleTime > 15) { // 15 minutes
        console.log('timed out!');
        $('#timeoutWarning').modal('hide');
        window.location.reload();
    }
    if (idleTime > 11) { //warn after 10 minutes
        console.log('timeout warning!');
        $('#timeoutWarning').modal('show')
    }
}