$(document).ready(function () {
   
    var rowCurrent = null;
    var idCurrent = null;
    //start  ajax delete ContentSurvey
    $("tbody").on("click",".delete-ContentSurvey",function () {
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
        rowCurrent.remove();
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
    //end  ajax create survey -----------------------------------------------------------------------------------
    $('.btn-close').click(function () {
        $('.create-survey').hide();
        $('.input2').removeClass('border-red');
    });
    $('#add-button').click(function () {
        $('.input2').val("");
        $('.create-survey').show();
    });
    $('.input2').blur(function () {
        if ($(this).val() !== "") {
            $(this).removeClass('border-red');
        } else {
            $(this).addClass('border-red');
        }
    });
    $(".btn-submit").click(function (event) {
        let check = true;
        $('.input2').removeClass('border-red');
        $('.input2').each(function () {
            if ($(this).val().trim() === "") {
                $(this).addClass('border-red');
                check = false;
            }
        });
        if (check) {
            PeformAjaxCreateSurvey();
        }
    });
    function appendRowSurvey(id, data, index) {
        $(".class-content-clone td:eq(2)").html("");
        $(".class-content-clone td:eq(2)").append("<a class='btn btn-warning' id='" + id + "'>Sửa</a>");
        $(".class-content-clone td:eq(2)").append(" <a class='btn btn-danger delete-ContentSurvey' id='" + id + "'>Xóa</a>");
        $(".class-content-clone td:eq(0)").text(index);
        $(".class-content-clone td:eq(1)").text(data);
        var rownew = $(".class-content-clone tr:eq(0)").clone(true);
        $(".content-teacher tbody").append(rownew);
    }
    function PeformAjaxCreateSurvey() {
        var frm = $('#addSurvey');
        $.ajax({
            type: "POST",
            url: "/ContentSurveys/Create",
            data: frm.serialize(),
            success: function (data) {
                if (data.status === 0) {
                    $('#surveyName[data-toggle="tooltip"]').tooltip("show");
                } else {
                    $('.create-survey').hide();
                    var index = parseInt($(".content-teacher .table-striped:last td:eq(0)").text()) + 1;
                    appendRowSurvey(data.id, data.content.Text,index);
                }
            }
        });

    }

    //end  ajax create survey

});

