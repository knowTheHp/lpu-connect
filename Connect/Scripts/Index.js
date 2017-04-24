$(document).ready(function () {
    $("#register").hide();
    //Toggle Registration Form
    $('#Role').on('change', function () {
        if (this.value === '1') {
            $("#register").show();
            $(".facultyInput").show();
            $(".studentInput").hide();
        } else if (this.value === '2') {
            $("#register").show();
            $(".facultyInput").hide();
            $(".studentInput").show();
        } else {
            $("#register").hide();
            $(".facultyInput").hide();
            $(".studentInput").hide();
        }
    });
    //End

    //Toggle Checkbox

    //

    //Start Branch
    $('#Course').change(function () {
        var courseId = $('#Course').val();
        $.ajax({
            type: "get",
            url: "Account/Branches/",
            data: { courseId: courseId },
            datatype: "json",
            traditional: true,
            success: function (branches) {
                var branchId = $('#Branch');
                branchId.empty();
                $.each(branches, function (index, branch) {
                    //alert(branch.text);
                    branchId.append($('<option/>', {
                            value: branch.value,
                            text: branch.text
                        }));
                });
            }
        });
    });
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