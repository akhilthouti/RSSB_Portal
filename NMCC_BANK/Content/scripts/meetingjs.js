//var baseurl = "https://localhost/VKYCWebAPI";
var baseurl = "https://api.indofinnet.com";
var meetingOptions = null;
var chatGUID = "";
var userGUID1 = "";
var userGUID2 = "";
var user1SDP = "";
var user2SDP = "";
var questions = [];
var curQuestionIndex = 0;

function init(options) {
    meetingOptions = options;

    const questionButton = document.getElementById(meetingOptions.btnSendQuestion);
    questionButton.addEventListener('click', sendQuestion);

    $.get(baseurl + "/api/Meeting/" + meetingOptions.meetingId).done(function (result) {
        if (result != null) {
            //alert(JSON.stringify(result));
            questions = JSON.parse(result.questions);
            chatGUID = result.webrtcSesssionId;
            userGUID1 = result.userGUID1;
            userGUID2 = result.userGUID2;
            user1SDP = JSON.parse(result.user1SDP);
            //alert("Id: " + JSON.stringify(result));
            //alert("Id: " + result.isPrivate);
            if (result.isPrivate == true) {
                //alert("private");
                //document.getElementById("btnJoinMeeting").style.visibility = "visible";
                $("#" + meetingOptions.txtPasscode).show();
                $("#" + meetingOptions.btnJoinMeetingAsHostP).show();
                $("#" + meetingOptions.btnJoinMeetingAsGuestP).show();
            }

            if (result.isPrivate == false) {
                $("#" + meetingOptions.txtPasscode).show();
                $("#" + meetingOptions.btnJoinMeetingAsHost).show();
                $("#" + meetingOptions.btnJoinMeetingAsGuest).show();
            }
            start();
        }
        else
            alert("unsucessful!!!");
    });
}

function sendQuestion() {
    if (curQuestionIndex == questions.length) {
        alert("all questions sent");
    }
    else {
        webRTC.send({ From: myNo, To: "", SignalType: 'question', Raw: questions[curQuestionIndex] });
        curQuestionIndex = curQuestionIndex + 1;
    }
}

function getFormattedDate() {
    var date = new Date();
    var str = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate() + " " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();

    return str;
}


function btnJoinMeetingAsHostOnClick(meetingId) {
    $.get(baseurl + "/api/Meeting/checkmeetingpasscode/" + meetingOptions.meetingId + "?pass=" + $("#" + meetingOptions.txtPasscode).val()).done(function (result) {
        if (result.isHost == true) {
            //alert("Meeting joined as HOST");
            $("#" + meetingOptions.camera).show();

        }
        else {
            alert("You cant enter meeting.")
        }
    });

}

function btnJoinMeetingAsGuestOnClick(meetingId) {
    $.get(baseurl + "/api/Meeting/checkmeetingpasscode/" + meetingOptions.meetingId + "?pass=" + $("#" + meetingOptions.txtPasscode).val()).done(function (result) {
        if (result.isGuest == true) {
            alert("Meeting joined as GUEST");
            $("#div123").hide();
        }

    });

}

function btnJoinMeetingAsHostPOnClick(meetingId) {
    $.get(baseurl + "/api/Meeting/checkmeetingpasscode/" + meetingOptions.meetingId + "?pass=" + $("#" + meetingOptions.txtPasscode).val()).done(function (result) {
        if (result.isHost == true) {
            //alert("Meeting joined as HOST");
            $("#" + meetingOptions.container).show();
            $("#div123").hide();
        }
        else {
            alert("You cant enter meeting.")
        }
    });

}

function btnJoinMeetingAsGuestPOnClick(meetingId) {
    $.get(baseurl + "/api/Meeting/checkmeetingpasscode/" + meetingOptions.meetingId + "?pass=" + $("#" + meetingOptions.txtPasscode).val()).done(function (result) {
        if (result.isGuest == true) {
            alert("Meeting joined as GUEST");
            $("#div123").hide();
        }
        else {
            alert("You cant enter meeting.")
        }
    });

}