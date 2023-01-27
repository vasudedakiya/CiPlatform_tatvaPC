$(function () {

    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        var decodeUrl = decodeURIComponent(url);
        $.get(decodeUrl).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })
    })

    PlaceHolderElement.on('click', '[data-bs-save="modal"]', function (event) {

        var formData = new FormData($('#modalForm').get(0));
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        console.log(actionUrl);
        $.ajax({
            url: actionUrl,
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function(data){
                PlaceHolderElement.find('.modal').modal('hide');
            }

        })
        //PlaceHolderElement.find('.modal').modal('hide');
    })

})


function Validate() {
    var password = document.getElementById("txtPassword").value;
    var confirmPassword = document.getElementById("txtConfirmPassword").value;
    if (password != confirmPassword) {
        alert("Passwords do not match.");
        return false;
    }
    return true;
}

function isValidPassword(password) {
    var pattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/;
    return pattern.test(password);
}

$(function () {

    $('.custom-dropdown').on('show.bs.dropdown', function () {
        console.log("Hello");
        // $(this).find('.dropdown-menu').first().stop(false, false).slideDown();
        // $(this).find('.dropdown-menu').addClass('active');
        var that = $(this);
        setTimeout(function () {
            that.find('.dropdown-menu').addClass('active');
        }, 100);


    });
    $('.custom-dropdown').on('hide.bs.dropdown', function () {
        $(this).find('.dropdown-menu').removeClass('active');
    });

});

