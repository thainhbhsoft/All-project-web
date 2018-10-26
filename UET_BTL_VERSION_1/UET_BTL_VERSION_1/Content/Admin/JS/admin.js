$(document).ready(function () {
    $("button").click(function () {
        $.ajax({
            url: "demo_test.txt", success: function (result) {
                $(".ContentAjax").html(result);
            }
        });
    });
});