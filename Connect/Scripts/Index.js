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

    //Toggle Currently working for Signup
    $(function () {
        $('#currentlyWorking').change(function () {
            $('#toggleTo').toggle(!this.checked);
        }).change(); //ensure visible state matches initially
    });
    //

    //Toggle Currently Working for WorkXP
    $(function () {
        $('#isWorking').change(function () {
            $('#toggleToMonthAndYear').toggle(!this.checked);
        }).change(); //ensure visible state matches initially
    });

    $(function () {
        $('#updateIsWorking').change(function () {
            $('#updateWorkXpToggleToMonthAndYear').toggle(!this.checked);
        }).change(); //ensure visible state matches initially
    });

    //Toggle Project On-Going
    $(function () {
        $('#projectOnGoing').change(function () {
            $('#toggleMonthAndYear').toggle(!this.checked);
        }).change(); //ensure visible state matches initially
    });

    //Toggle Project Update On-Going
    $(function () {
        $('#projectUpdateOnGoing').change(function () {
            $('#toggleProjectUpdateMonthAndYear').toggle(!this.checked);
        }).change(); //ensure visible state matches initially
    });

    //Start Branch
    $('#Course').change(function () {
        var courseId = $('#Course').val();
        var branchId = $('#Branch');
        branchId.empty();
        $.ajax({
            type: "post",
            url: "account/branches/",
            datatype: "json",
            data: { courseId: courseId },
            success: function (branches) {
                $.each(branches, function (index, branch) {
                    branchId.append('<option value="' + branch.BranchId + '">' + branch.BranchName + '</option>');
                });
            }
        });
    });
    //End

    //send chat on enter key
    $("body").on("keypress", ".chatbox textarea", function (e) {
        if (e.which === 13) {
            $(this).parent().find("a.sendchat").click();
            setTimeout(function () {
                $(this).parent().find("textarea").focus();
            }, 0);
        }
    });
    //end

    //Start Image Script
    function readURL(input) {

        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $("img#profilePic").attr("src", e.target.result);
            };
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
            if (response === "ok") {
                //
            } else {
                $("#validationVisible").removeClass("hidden");
            }
        });
    });
    //End Login Form
});