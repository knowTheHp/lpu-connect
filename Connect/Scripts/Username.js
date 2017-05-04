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

    //setup hub connection
    var hub = $.connection.echo;
    $.connection.hub.start().done(function () {
        hub.server.hello("SignalR working");
    });
    //end hub
});