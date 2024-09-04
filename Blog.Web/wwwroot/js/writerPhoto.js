$(document).ready(function () {
    var writerPPUrl = app.Urls.writerPPUrl;

    $.ajax({
        url: writerPPUrl,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                console.log(response.filename);
                $("img#writerPhoto").attr("src", "/images/" + response.filename);
            } else {
                console.error('Failed to get the image: ' + response.message);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error occurred: ' + error);
        }
    });
});
