﻿$(document).ready(function () {
   
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
                
                    var content = "";
                    var name = " </br> <span>Họ tên :</span>";
                    var email = " </br> <span>Email :</span>";
                    var username = "</br> <span>Tên đăng nhập :</span>";
                    var password = "</br> <span>Mật khẩu :</span>";
                    var courses = "</br> <span>Khóa học :</span>";
                
                    content += name + student.Name;
                    content += email + student.Email;
                    content += courses + student.Course;
                    content += username + student.UserName;
                    content += password + student.PassWord;
                $(".confirm-delete-student").html(content);
                $(".confirm-delete-student").append("</br></br><button class='delete'>Delete</button>");
                $(".confirm-delete-student").append("<button class='close1'>Close</button>");
                $(".confirm-delete-student").show();
                
            }
        });
    });

    $(".confirm-delete-student").on("click", "button.delete", function () {
        $(".confirm-delete-student").slideUp(700);
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
    $(".confirm-delete-student").on("click", "button.close1", function () {
        $(".confirm-delete-student").slideUp(700);
    });
    //end  ajax delete student
    //end  ajax create student
    $('.btn-close').click(function () {
        $('.add-tab').hide();
        $('.edit-tab').hide();
        $('.input-information').removeClass('border-red');
    });

    $('#add-button').click(function () {
        $('.add-tab').show();
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

    function isStudentEmail(e) {
        var regex = /(^(\d{8}))((@vnu.edu.vn)$)/;
        if (regex.test(e)) return true;
    }
    // Kiểm tra mã minh viên (8 chữ số)
    function isStudentId(e) {
        var regex = /(^(\d{8})$)/;
        if (regex.test(e)) return true;
    }
    $("#addStudent").submit(function (event) {

        var check = true;
       
         if (isStudentEmail($("#studentEmail").val()) != true) {
             $('#studentEmail[data-toggle="tooltip"]').tooltip("show");  
            check = false;
        }
       
        if (isStudentId($("#studentId").val()) != true) {
            $('#studentId[data-toggle="tooltip"]').tooltip("show");  
            check = false;
        }
        if (check == false)
            event.preventDefault();
    });
    




    //end  ajax create student




});

