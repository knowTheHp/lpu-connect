$(document).ready(function () {
    //Start Live Search
    $("#searchText").keyup(function () {
        var searchVal = $("#searchText").val();
        $("#liveSearchul").empty();
        if (searchVal === "" || searchVal === " ") return false;
        var url = "Profile/Search";
        $.post(url, { searchVal: searchVal }, function (data) {
            for (var i = 0; i < data.length; i++) {
                var obj = data[i];
                $("#liveSearchul").append('<li><a href="/' + obj.Username + '"><img src="Uploads/' + obj.UserId + obj.Username + '.jpg"/>' + ' ' + obj.Firstname + ' ' + obj.Lastname + '</a></li>');
            }
        });
    });
    //End Live Search

    //Close button for search
    $("body").keydown(function (e) {
        if (e.keyCode === 27) {
            $("#searchText").val("");
            $("#liveSearchul").empty();
        }
    });
    //End close button for search

    //close button for request
    $("body").keydown(function (e) {
        if (e.keyCode === 27) {
            $("#reqNotifyul").fadeOut();
        }
    });
    $("body").on("click", ".notif", function () {
        $("ul#reqNotifyul").show();
    });
    //end close button

    //show connectLpu@123@ modal
    $("#connectModal").on("click", function () {
        $('#connectionRequestModal').modal('show');
    });
    //end show modal

    //hide connect modal
    $('#hideModal').on("click", function () {
        $('#connectionRequestModal').modal('hide');
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
    });
    //end hide connect modal
});