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
});

