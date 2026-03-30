$(document).ready(function () {

    //var url = window.location.pathname.toLowerCase();
    

    //$('.menu a').each(function () {
    //    if (url.indexOf($(this).attr('href').toLowerCase()) >= 0) {
    //        $(this).addClass('active');
    //    }
        
    //});

});
function showOverlay(text) {
    var $overlay = $('#ajaxOverlay');
        $('.overlay-text', $overlay).text(text || 'Processing...');
    $overlay.show();
        isSubmitting = true;
    }

function hideOverlay() {
    var $overlay = $('#ajaxOverlay');
    $overlay.hide();
        isSubmitting = false;
    }