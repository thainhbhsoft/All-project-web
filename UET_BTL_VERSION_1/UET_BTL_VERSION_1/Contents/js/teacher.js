$(document).ready(function () {
   
    //start  ajax delete teacher
    var rowCurrent = null;
    var idCurrent = null;
   
    $(".delete-teacher").on("click", function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/Teachers/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {
                $("#DteacherName").val(data.teacher.Name);
                $("#DteacherEmail").val(data.teacher.Email);
                $("#DteacherUsername").val(data.teacher.UserName);
                $("#DteacherPassword").val(data.teacher.PassWord);
                if (data.status === 0) {
                    $(".delete-teacher-form h5:eq(0)").text(" Không thể xóa ");
                    $(".delete-teacher-form tr:eq(0)").html("<td style='text-align: center;font-weight:bold; color: red; font-size: 17px; ' colspan='2'>GIẢNG VIÊN NÀY ĐANG DẠY MỘT HỌC PHẦN</td>");
                    $(".delete-teacher-form .bottom-button:eq(0)").html("");
                    $(".delete-teacher-form .bottom-button:eq(1)").html("<a class='btn btn-success btn-cancle'>Đóng</a>");
                } else {
                    $(".delete-teacher-form h5:eq(0)").text("Xóa giảng viên");
                    $(".delete-teacher-form tr:eq(0)").html("");
                    $(".delete-teacher-form .bottom-button:eq(0)").html("<a class='btn btn-danger btn-delete'>Xóa</a>");
                    $(".delete-teacher-form .bottom-button:eq(1)").html("<a class='btn btn-success btn-cancle'>Hủy bỏ</a>");
                }
                $(".delete-teacher-form").show();
            }
        });
    });

    $(".bottom-button").on("click", ".btn-delete", function () {
        $(".delete-teacher-form").fadeOut(700);
        rowCurrent.hide(1000);
        urldetail = '/Teachers/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });

   
    $(".bottom-button").on("click", ".btn-cancle", function () {
        $(".delete-teacher-form").fadeOut(700);
    });
    //end  ajax delete teacher
    //end  ajax create student
    $('.btn-close').click(function () {
        $('.create-teacher').hide();
        $('.input-information').removeClass('border-red');
        $('[data-toggle="tooltip"]').tooltip("hide");
    });

    $('#add-button').click(function () {
        $('.input-information').val("");
        $('.create-teacher').show();
    });

    $('#teacherName').blur(function () {
        $(this).val(ChuanhoaTen($(this).val()));
    });

    function ChuanhoaTen(ten) {
        dname = ten;
        ss = dname.split(' ');
        dname = "";
        for (i = 0; i < ss.length; i++)
            if (ss[i].length > 0) {
                if (dname.length > 0) dname = dname + " ";
                dname = dname + ss[i].substring(0, 1).toUpperCase();
                dname = dname + ss[i].substring(1).toLowerCase();
            }
        return dname;
    }
    // kiểm tra email giáo viên (4-10 chữ cái + @vnu.edu)
    function isTeacherEmail(e) {
        var regex = /(^([a-zA-Z]{4,10}))((@vnu.edu.vn)$)/;
        if (regex.test(e)) return true;
        return false;
    }
   
    $('.input-information').blur(function () {
        if ($(this).val() !== "") {
            $(this).removeClass('border-red');
        } else {
            $(this).addClass('border-red');
        }
    });
    $(".btn-submit").click(function (event) {

        let check = true;
        $('.input-information').removeClass('border-red');
        $('.input-information').each(function () {
            if ($(this).val() === "") {
                $(this).addClass('border-red');
                check = false;
            }
        });
        if (!isTeacherEmail($("#teacherEmail").val())) {
            $('#teacherEmail[data-toggle="tooltip"]').tooltip("show");
            check = false;
        }
       
        if (check) {
            PeformAjaxCreateTeacher();
        }
    });

    function PeformAjaxCreateTeacher() {
        var frm = $('#addTeacher');
        $.ajax({
            type: "POST",
            url: "/Teachers/Create",
            data: frm.serialize(),
            success: function (data) {
                if (data.status === 0) {
                    $('#teacherUsername[data-toggle="tooltip"]').tooltip("show");
                } else {
                    $('.create-teacher').hide();
                    alert("create success");
                }
            }
        });

    }

    //end  ajax create student
});

