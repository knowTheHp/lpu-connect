$(document).ready(function () {
    //Toggle Registration Form
    $('#Role').on('change', function () {
        if (this.value === '1') {
            $(".facultyInput").show();
            $(".studentInput").hide();
        } else if (this.value === '2') {
            $(".facultyInput").hide();
            $(".studentInput").show();
        }
    });
    //End

    //Toggle Checkbox

    //

    //Start Branch
    //$('#Courses').change(function () {
    //    var courseId = $('#Courses').val();
    //    $.ajax({
    //        type: "get",
    //        //data: JSON.stringify({courseId: courseId }),
    //        url: "/Account/FillBranch?courseId=" + courseId,
    //        datatype: "json",
    //        traditional: true,
    //        success: function (data) {
    //            var branch = $('#Branches');
    //            $.each(data, function (val, text) {
    //                branch.append(
    //                    $('<option></option>').val(val).html(text)
    //                );
    //            });
    //        }
    //    });
    //});
    //End

    //Start Image Script
    function readURL(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $("img#profilePic").attr("src", e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }
    $("#ImageUpload").change(function () {
        readURL(this);
    });
    //End Image Script

    //Start Login Form
    $("form#loginform").submit(function (e) {
        e.preventDefault();
        var username = $("#Username").val();
        var password = $("#Password").val();
        var url = "/account/login";

        $.post(url, { username: username, password: password }, function (data) {
            var response = data.trim();
            document.location.href = "/";
            if (response == "ok") {

            } else {
                $("#validationVisible").show();
            }
        });
    });
    //End Login Form
});