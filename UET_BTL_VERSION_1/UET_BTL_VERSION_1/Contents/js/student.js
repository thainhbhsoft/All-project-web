$(document).ready(function () {
   
    var rowCurrent = null;
    var idCurrent = null;
   
    //start  ajax delete student
   
    $(".delete-student").on("click", function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/Students/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (student) {
                $("#DstudentName").val(student.Name);
                $("#DstudentEmail").val(student.Email);
                $("#DstudentUsername").val(student.UserName);
                $("#DstudentPassword").val(student.PassWord);
               
                $(".delete-student-form h5:eq(0)").text("Xóa sinh viên");
                $(".delete-student-form .bottom-button:eq(0)").html("<a class='btn btn-danger btn-delete'>Xóa</a>");
                $(".delete-student-form .bottom-button:eq(1)").html("<a class='btn btn-success btn-cancle'>Hủy bỏ</a>");
                $(".delete-student-form").show();
            }
        });
    });

    $(".bottom-button").on("click", ".btn-delete", function () {
        $(".delete-student-form").fadeOut(700);
        rowCurrent.hide(1000);
        urldetail = '/Students/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });
    $(".bottom-button").on("click", ".btn-cancle", function () {
        $(".delete-student-form").fadeOut(700);
    });
    //end  ajax delete student
    //end  ajax create student
    $('.btn-close').click(function () {
        $('.create-student').hide();
        $('.edit-tab').hide();
        $('.input2').removeClass('border-red');
        $('[data-toggle="tooltip"]').tooltip("hide"); 
    });

    $('#add-button').click(function () {
        $('.input2').val("");
        $('.create-student').show();
    });

    $('#studentName').blur(function () {
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
     // Kiểm tra email vnu
    function isStudentEmail(e) {
        var regex = /(^(\d{8}))((@vnu.edu.vn)$)/;
        if (regex.test(e)) return true;
        return false;
    }
    // Kiểm tra mã minh viên (8 chữ số)
    function isStudentId(e) {
        var regex = /(^(\d{8})$)/;
        if (regex.test(e)) return true;
        return false;
    }
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
            if ($(this).val() === "") {
                $(this).addClass('border-red');
                check = false;
            }
        });
        if (!isStudentEmail($("#studentEmail").val())) {
             $('#studentEmail[data-toggle="tooltip"]').tooltip("show");  
            check = false;
            }
        if (!isStudentId($("#studentId").val())) {
            $('#studentId[data-toggle="tooltip"]').tooltip("show");  
            check = false;
        }
        if (check) {
            PeformAjaxCreateStudent();
        }
    });

    function PeformAjaxCreateStudent() {
        var frm = $('#addStudent');
            $.ajax({
                type: "POST",
                url: "/Students/Create",
                data: frm.serialize(),
                success: function (data) {
                    if (data.status === 0) {
                        $('#studentUsername[data-toggle="tooltip"]').tooltip("show");
                    } else {
                        $('.create-student').hide();
                        alert("create success");
                    }
                }
            });
   
    }
    
    //end  ajax create student




});

