$(document).ready(function () {
   
    var rowCurrent = null;
    var idCurrent = null;
    //start  ajax delete ContentSurvey
    $(".delete-ContentSurvey").on("click", function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/ContentSurveys/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {
                $("#DsurveyName").val(data.Text);
                $(".delete-survey-form").show();
            }
        });
    });
    $(".bottom-button").on("click", ".btn-delete", function () {
        $(".delete-survey-form").fadeOut(700);
        rowCurrent.hide(1000);
        urldetail = '/ContentSurveys/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });
    $(".bottom-button").on("click", ".btn-cancle", function () {
        $(".delete-survey-form").fadeOut(700);
    });
    //end  ajax delete ContentSurvey

});

