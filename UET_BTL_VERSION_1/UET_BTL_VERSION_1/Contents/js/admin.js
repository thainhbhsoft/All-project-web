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
                if (data.status === 0) {
                    $(".confirm-delete-teacher").html("<h2>Giảng viên này đang dạy một học phần, không thể xóa được</h2>");
                    $(".confirm-delete-teacher").append("<button class='close1'>Close</button>");
                    $(".confirm-delete-teacher").show();
                } else {
                var content = "";
                var name = " </br> <span>Họ tên :</span>";
                var email = " </br> <span>Email :</span>";
                var username = "</br> <span>Tên đăng nhập :</span>";
                var password = "</br> <span>Mật khẩu :</span>";

                    content += name + data.teacher.Name;
                    content += email + data.teacher.Email;
                    content += username + data.teacher.UserName;
                    content += password + data.teacher.PassWord;
                    $(".confirm-delete-teacher").html(content);
                    $(".confirm-delete-teacher").append("</br></br><button class='delete'>Delete</button>");
                    $(".confirm-delete-teacher").append("<button class='close1'>Close</button>");
                    $(".confirm-delete-teacher").show();
                }
            }
        });
    });

    $(".confirm-delete-teacher").on("click", "button.delete", function () {
        $(".confirm-delete-teacher").slideUp(700);
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
    $(".confirm-delete-teacher").on("click", "button.close1", function () {
        $(".confirm-delete-teacher").slideUp(700);
    });
    //end  ajax delete teacher
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
    //start  ajax delete subject

    $(".delete-subject").on("click", function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/Subjects/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (subject) {

                var content = "";
                var name = " </br> <span>Tên học phần :</span>";
                var subjectCode = " </br> <span>Mã học phần :</span>";
                var creditNumber = "</br> <span>Số tín chỉ :</span>";
                var room = "</br> <span>Phòng học :</span>";
                var time = "</br> <span>Thời gian,tiết :</span>";

                content += name + subject.Name;
                content += subjectCode + subject.SubjectCode;
                content += creditNumber + subject.CreditNumber;
                content += room + subject.ClassRoom;
                content += time + subject.TimeTeach;
                $(".confirm-delete-subject").html(content);
                $(".confirm-delete-subject").append("</br></br><button class='delete'>Delete</button>");
                $(".confirm-delete-subject").append("<button class='close1'>Close</button>");
                $(".confirm-delete-subject").show();

            }
        });
    });

    $(".confirm-delete-subject").on("click", "button.delete", function () {
        $(".confirm-delete-subject").slideUp(700);
        rowCurrent.hide(1000);
        urldetail = '/Subjects/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });
    $(".confirm-delete-subject").on("click", "button.close1", function () {
        $(".confirm-delete-subject").slideUp(700);
    });
    //end  ajax delete subject

});

