$(document).ready(function () {
    var socialLinksUrl = app.Urls.socialLinksUrl;
    console.log("URL: " + socialLinksUrl);

    $.ajax({
        url: socialLinksUrl,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $("a#socialLinksLinkedin").attr("href", data[0]);
            $("a#socialLinksInstagram").attr("href", data[1]);
            $("a#socialLinksFacebook").attr("href", data[2]);
            $("a#socialLinksX").attr("href", data[3]);
        },
        error: function (xhr, status, error) {
            console.error('Error occurred: ' + error);
        }
    });
});