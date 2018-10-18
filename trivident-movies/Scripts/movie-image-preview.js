    

$(document).ready(function () {
        $("input#ImageLink").change(function () {
            $("img#cover_image").attr("src", $("input#ImageLink").val());
        });
    });
    