$(document).ready(function () {
    var aboutGetUrl = app.Urls.aboutGetUrl;

    $.ajax({
        url: aboutGetUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $("h2#aboutTitleText").text(data.title);
            $("p#aboutDescriptionText").text(data.description);
        },
        error: function (xhr, status, error) {
            console.error('Error occurred: ' + error);
        }
    });
});