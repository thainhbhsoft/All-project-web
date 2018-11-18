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

                var content = "";
                var message = " </br> <span>Bạn có chắc chắn muốn xóa?</span>";
                var contentsurvey = " </br> <span>Tên tiêu chí :</span>";
               
                content += message;
                content += contentsurvey + data.Text;
                $(".confirm-delete-ContentSurvey").html(content);
                $(".confirm-delete-ContentSurvey").append("</br></br><button class='delete'>Delete</button>");
                $(".confirm-delete-ContentSurvey").append("<button class='close1'>Close</button>");
                $(".confirm-delete-ContentSurvey").show();

            }
        });
    });

    $(".confirm-delete-ContentSurvey").on("click", "button.delete", function () {
        $(".confirm-delete-ContentSurvey").slideUp(700);
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
    $(".confirm-delete-ContentSurvey").on("click", "button.close1", function () {
        $(".confirm-delete-ContentSurvey").slideUp(700);
    });
    //end  ajax delete ContentSurvey

});

