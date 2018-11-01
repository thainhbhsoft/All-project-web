$(document).ready(function () {
    $(".list-group a.list-group-item").click(function () {
        $(".list-group a.list-group-item").removeClass("Active");
        $(this).addClass("Active");
    });
});