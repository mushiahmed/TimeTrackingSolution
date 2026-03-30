
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