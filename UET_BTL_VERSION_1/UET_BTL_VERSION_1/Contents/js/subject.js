$(document).ready(function () {
   
    var rowCurrent = null;
    var idCurrent = null;
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

