
function pass() {
    debugger;

    var x = document.getElementById("Password");

    var y = document.getElementById("init_eye");

    if (x.type === "password") {
        x.type = "text";
        $("i").removeClass("fa-eye-slash");
        //$("i").css("color", " #555");
        $("i").addClass("fa-eye");
    } else {
        x.type = "password";
        $("i").removeClass("fa-eye");
        //$("i").css("color", "");
        $("i").addClass("fa-eye-slash");

    }
};

function OTPQEMobileAuthenticate() {
    debugger;
    $("#DIGIbag").show();
    var OTP = $('#QEMobileauth').val();
    var MobileNumber = $('#QEMobilecode').val();
    
    $(".hello").hide();
    $(".hell").css("display", "block");
    //swal("Authenticate Successfully... ");
    $("#otp").hide();
    //$("#QEEmailbaknew1").css("displ");
    $('#QEMobileauth').val('');
    $("")
    if (OTP == '') {
        swal("Enter valid OTP...");
        return false;
    }
    if (OTP.length > 6) {
        swal("Enter 6 digit Valid OTP...");
        $('#QEMobileauth').val('');

        return false;
    }
    $.ajax({
        //url: '/KYCQuickEnroll/OTPAutheticate',
        url: '/KYCQuickEnroll/QEMobileOTPAuthenticate',
        type: 'GET',
        data: {
            OTP: OTP,
            MobileNumber: MobileNumber
        },
        cache: false,
        success: function (result) {
            debugger;
            $("#DIGIbag").hide();
            if (result == "Verified Successfully") {
                swal("Verified Successfully");
                $("#DigiCerti").hide();

                
                window.location.href = "/OrgnisationDashBoard/MainDashboard";
            }
            else if (result == "You have reached the maximum number of attempts.\n Please try again after 10 minutes.") {
                swal({
                    title: result,
                    confirmButtonClass: "btn-danger",
                    confirmButtonText: "ok",
                },
                    function (isConfirm) {
                        //alert("hiii");
                        if (isConfirm) {
                            window.location.href = '/OrganisationLogin/OrganisationDetails';
                        }

                    }
                )
                //$('#Emailbaknew1').hide();

            } 
            else {
                swal(result);
                //$('#Emailbaknew1').hide();
                $('#emailauth').val('');
                $('#QEMobileVerify').hide();
                $('#QEisMobileVerify').hide();
                $('#QEAuthMobileGetOtp').show();
                //window.location.href = "/OrgnisationDashBoard/MainDashboard";
            }

        }
    });

}
function closeQEMobile() {
    $('#otp_div').css("display", "none");

}
function QEGetMobileOTP() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();


    var MobileNumber = $('#QEMobilecode').val();
    var last_digits = MobileNumber.substr(6, 11);
    //console.log(`Fifteen is ${a + b}.`);otp sent successfully to the registered mobile no
    var t = document.getElementById("text").innerHTML = `RSSB has sent a temporary OTP to your Registered Mobile No(Valid for 10 mins).`;
    //var t = document.getElementById("text").innerHTML = `CP-ALPHAF has sent a temporary OTP to your mobile ending in ******${last_digits} (Valid for 2 mins).`;
    $(".init_email").show();
    $.ajax({
        url: '/KYCQuickEnroll/QEMoblieOTPSend',
        type: 'POST',
        data: {
            MobileNumber: MobileNumber
        },
        cache: false,
        success: function (result) {
            debugger;
            $("#DigiCerti").hide();
            $("#DIGIbag").hide();
            //$("#QEEmailbaknew1").hide();
            if (result != null) {
                debugger;

                $(".init_mail").show();
                // var elem = document.getElementById("QEMobileGetOtp");
                // $('#QEMobileGetOtp').val('Resend OTP');
                $("#QEMobileGetOtp").html('RESEND OTP');
                $("#QEMobileGetOtp").prop('disabled', true);
                setTimeout(function () {
                    $("#QEMobileGetOtp").prop('disabled', false); // enable the button after 5 seconds
                }, 600000);
                // if (elem.value == "GET OTP") elem.value = "Resend OTP";
            } else {
                swal('OTP does not send.try again..');

            }
            // $('#DigitCaptureImage').html(result);
        }
    });

}
//function AgentLogin() {

//    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
//    $("#DigiCerti").show();
//    debugger;
//    var unm = $("#UserId").val();
//    var pwd = $("#Password").val();
//    if ($("#UserId").val() == "" && $("#Password").val() == "") {
//        $("#DigiCerti").hide();
//        swal("Please Enter Valid UserID Or Password");
//        return false;
//    }
//    else if ($("#UserId").val() == '') {
//        $("#DigiCerti").hide();
//        swal("Please Enter Valid UserID");
//        return false;
//    }
//    else if ($("#Password").val() == '') {
//        $("#DigiCerti").hide();
//        swal("Please Enter Valid Password ");
//        return false;
//    }
//    else {
//        $.ajax({
//            url: '/OrganisationLogin/OrganisationDetails?userId=' + unm + '&Password=' + pwd,
//            type: 'POST',
//            data: {
//            },
//            cache: false,
//            success: function (result) {
//                debugger;
//                var Message = result.split(',')[0];
//                var mobile = result.split(',')[1];
//                var LogStatus = result.split(',')[2];
//                var ip = result.split(',')[3];
//                var hostnm = result.split(',')[4];
//                var time = result.split(',')[5];
//                if (Message == "Success") {

//                    if (LogStatus == 'true' || LogStatus == 'True') {
//                        //swal({
//                        //    title: "User Already Logged In ",
//                        //    text: "IP Address:"+ ip +" \n Device Name:"+ hostnm +"\n Session Start Time: "+ time +" ",
//                        //    confirmButtonClass: "btn-danger",
//                        //    confirmButtonText: "ok",
//                        //},
//                        //    function (isConfirm) {
//                        //        if (isConfirm) {
//                        //            debugger;
//                        //            window.location.href = "/OrganisationLogin/OrganisationDetails";
//                        //        }
//                        //    })
//                        ActivLogin(ip, hostnm, time);

//                    }
//                    else {
//                        //swal("Verified Successfully");
//                        CheckCaptcha();
//                        $('#QEMobilecode').hide();
//                        //QEGetMobileOTP();
//                        //swal("User Verified Successfully");
//                        $("#DigiCerti").hide();
//                        $('#QEMobilecode').val(mobile);
//                        //$("#otp_div").show();

//                        //window.location.href = "/OrgnisationDashBoard/MainDashboard";
//                    }

//                }
//                else if (result == "Attempt1") {
//                    $("#DigiCerti").hide();
//                    //swal("Please Enter Valid UserID OR Passward");
//                    swal({
//                        title: "Incorrect UserID Or Password",
//                        text: "You Have Only 2 Attempts Remaining",
//                        type: "warning",
//                        showCancelButton: false,
//                        confirmButtonColor: "#DD6B55",
//                        confirmButtonText: "OK",

//                        closeOnConfirm: true,
//                        closeOnCancel: true
//                    },
//                        function (isConfirm) {
//                            if (isConfirm) {
//                                $("#DigiCerti").hide();
//                                window.location.href = "/OrganisationLogin/OrganisationDetails";

//                            }
//                            else {

//                            }
//                        }
//                    );
//                }
//                else if (result == "Attempt2") {
//                    $("#DigiCerti").hide();
//                    //swal("Please Enter Valid UserID OR Passward");
//                    swal({
//                        title: "Incorrect UserID Or Password",
//                        text: "You Have Only 1 Attempts Remaining",
//                        type: "warning",
//                        showCancelButton: false,
//                        confirmButtonColor: "#DD6B55",
//                        confirmButtonText: "OK",

//                        closeOnConfirm: true,
//                        closeOnCancel: true
//                    },
//                        function (isConfirm) {
//                            if (isConfirm) {
//                                $("#DigiCerti").hide();
//                                window.location.href = "/OrganisationLogin/OrganisationDetails";

//                            }
//                            else {

//                            }
//                        }
//                    );
//                }
//                else if (result == "Locked") {
//                    $("#DigiCerti").hide();
//                    //swal("Please Enter Valid UserID OR Passward");
//                    swal({
//                        title: "You’ve reached the maximum login attempts .",
//                        text: "Your Account Has Been Locked",
//                        text: "Please Contact Admin For Help!",
//                        type: "warning",
//                        showCancelButton: false,
//                        confirmButtonColor: "#DD6B55",
//                        confirmButtonText: "OK",

//                        closeOnConfirm: true,
//                        closeOnCancel: true
//                    },
//                        function (isConfirm) {
//                            if (isConfirm) {
//                                $("#DigiCerti").hide();
//                                window.location.href = "/OrganisationLogin/OrganisationDetails";

//                            }
//                            else {

//                            }
//                        }
//                    );
//                }
//                else {
//                    $("#DigiCerti").hide();
//                    //swal("Please Enter Valid UserID OR Passward");
//                    swal({
//                        title: "Please Enter Valid UserID OR Password",

//                        type: "warning",
//                        showCancelButton: false,
//                        confirmButtonColor: "#DD6B55",
//                        confirmButtonText: "OK",

//                        closeOnConfirm: true,
//                        closeOnCancel: true
//                    },
//                        function (isConfirm) {
//                            if (isConfirm) {
//                                $("#DigiCerti").hide();
//                                window.location.href = "/OrganisationLogin/OrganisationDetails";

//                            }
//                            else {

//                            }
//                        }
//                    );
//                    //window.location.href = "/OrganisationLogin/OrganisationDetails";

//                }

//            }

//        });
//    };

//};
//###########//
function AgentLogin() {

    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    debugger;
    var unm = $("#UserId").val();
    const textToEncrypt = $("#Password").val();
    const encryptedPassword = encryptText(textToEncrypt, encryptionKey, IV);

    if ($("#UserId").val() == "" && $("#Password").val() == "") {
        $("#DigiCerti").hide();
        swal("Please Enter Valid UserID Or Password");
        return false;
    }
    else if ($("#UserId").val() == '') {
        $("#DigiCerti").hide();
        swal("Please Enter Valid UserID");
        return false;
    }
    else if ($("#Password").val() == '') {
        $("#DigiCerti").hide();
        swal("Please Enter Valid Password ");
        return false;
    }
    else if (textToEncrypt.indexOf(' ') != -1) {
        $("#DigiCerti").hide();
        swal("Password should not contain spaces.");
        return false;
    }
    else if ($("#UserCaptchaCode").val() == "" || $("#UserCaptchaCode").val() == null || $("#UserCaptchaCode").val() == "undefined") {
        swal("Please Enter Code Given In a Picture.");
        //$('#WrongCaptchaError').text('Please Enter Code Given In a Picture.').show();
        $('#UserCaptchaCode').val("");
        $('#UserCaptchaCode').focus();
        $("#otp_div").css("display", "none");
        $("#DigiCerti").hide();
        return false;
    }
    else {
        var result = ValidateCaptcha();
        if (result == false) {
            IsAllowed = false;
            $('#WrongCaptchaError').text("Captcha value doesn't match").show();
            $('#UserCaptchaCode').val("");
            $("#otp_div").css("display", "none");
            CreateCaptcha();
            $('#UserCaptchaCode').focus().select();
            $("#DigiCerti").hide();
        }
        else {
            IsAllowed = true;
            //$('#UserCaptchaCode').val('').attr('place-holder','Enter Captcha - Case Sensitive');
            //CreateCaptcha();
            $('#WrongCaptchaError').fadeOut(100);
            $('#SuccessMessage').fadeIn(500).css('display', 'block').delay(5000).fadeOut(250);

            $.ajax({
                url: '/OrganisationLogin/OrganisationDetails',
                type: 'POST',
                data: {
                    userId: unm,
                    Password: encryptedPassword
                },
                cache: false,
                success: function (result) {
                    debugger;
                    var Message = result.split(',')[0];
                    const textToEncrypt = result.split(',')[1];
                    const encrypMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
                    var LogStatus = result.split(',')[2];
                    var ip = result.split(',')[3];
                    var hostnm = result.split(',')[4];
                    var time = result.split(',')[5];
                    if (Message == "Success") {

                        if (LogStatus == 'true' || LogStatus == 'True') {
                            //swal({
                            //    title: "User Already Logged In ",
                            //    text: "IP Address:"+ ip +" \n Device Name:"+ hostnm +"\n Session Start Time: "+ time +" ",
                            //    confirmButtonClass: "btn-danger",
                            //    confirmButtonText: "ok",
                            //},
                            //    function (isConfirm) {
                            //        if (isConfirm) {
                            //            debugger;
                            //            window.location.href = "/OrganisationLogin/OrganisationDetails";
                            //        }
                            //    })
                            ActivLogin(ip, hostnm, time);

                        }
                        else {
                            //swal("Verified Successfully");
                            //CheckCaptcha();
                            $('#QEMobilecode').hide();
                            QEGetMobileOTP();
                            swal("User Verified Successfully");
                            $("#DigiCerti").hide();
                            $('#QEMobilecode').val(encrypMobileNo);
                            $("#otp_div").show();

                            //window.location.href = "/OrgnisationDashBoard/MainDashboard";
                        }

                    }
                    else if (result == "Attempt1") {
                        $("#DigiCerti").hide();
                        //swal("Please Enter Valid UserID OR Passward");
                        swal({
                            title: "Incorrect UserID Or Password",
                            text: "You Have Only 2 Attempts Remaining",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",

                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $("#DigiCerti").hide();
                                    window.location.href = "/OrganisationLogin/OrganisationDetails";

                                }
                                else {

                                }
                            }
                        );
                    }
                    else if (result == "Attempt2") {
                        $("#DigiCerti").hide();
                        //swal("Please Enter Valid UserID OR Passward");
                        swal({
                            title: "Incorrect UserID Or Password",
                            text: "You Have Only 1 Attempts Remaining",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",

                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $("#DigiCerti").hide();
                                    window.location.href = "/OrganisationLogin/OrganisationDetails";

                                }
                                else {

                                }
                            }
                        );
                    }
                    else if (result == "Locked") {
                        $("#DigiCerti").hide();
                        //swal("Please Enter Valid UserID OR Passward");
                        swal({
                            title: "You’ve reached the maximum login attempts .",
                            text: "Your Account Has Been Locked",
                            text: "Please Contact Admin For Help!",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",

                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $("#DigiCerti").hide();
                                    window.location.href = "/OrganisationLogin/OrganisationDetails";

                                }
                                else {

                                }
                            }
                        );
                    }
                    else if (result == "OTP Based Locked") {
                        $("#DigiCerti").hide();
                        //swal("Please Enter Valid UserID OR Passward");
                        swal({
                            title: "Your Account Has Been Locked",
                            text: "Please try again after 10 minutes.",
                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",

                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $("#DigiCerti").hide();
                                    window.location.href = "/OrganisationLogin/OrganisationDetails";

                                }
                                else {

                                }
                            }
                        );
                    }
                    else if (result == "Invalid Credentials")
                    {
                        $("#DigiCerti").hide();
                        //swal("Please Enter Valid UserID OR Passward");
                        swal({
                            title: "Please Enter Valid UserID OR Password",

                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",

                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $("#DigiCerti").hide();
                                    window.location.href = "/OrganisationLogin/OrganisationDetails";

                                }
                                else {

                                }
                            }
                        );
                    }
                    else {
                        $("#DigiCerti").hide();
                        //swal("Please Enter Valid UserID OR Passward");
                        swal({
                            title: "Please Enter Valid UserID OR Password",

                            type: "warning",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "OK",

                            closeOnConfirm: true,
                            closeOnCancel: true
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    $("#DigiCerti").hide();
                                    window.location.href = "/OrganisationLogin/OrganisationDetails";

                                }
                                else {

                                }
                            }
                        );
                        //window.location.href = "/OrganisationLogin/OrganisationDetails";

                    }

                }

            });
        };

    };

};
//###########//
var cd;
var IsAllowed = false;
$(document).ready(function () {
    CreateCaptcha();
});

// Create Captcha
function CreateCaptcha() {
    //$('#InvalidCapthcaError').hide();
    var alpha = new Array('A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
    $('#UserCaptchaCode').val('');
    var i;
    for (i = 0; i < 6; i++) {
        var a = alpha[Math.floor(Math.random() * alpha.length)];
        var b = alpha[Math.floor(Math.random() * alpha.length)];
        var c = alpha[Math.floor(Math.random() * alpha.length)];
        var d = alpha[Math.floor(Math.random() * alpha.length)];
        var e = alpha[Math.floor(Math.random() * alpha.length)];
        var f = alpha[Math.floor(Math.random() * alpha.length)];
    }
    cd = a + ' ' + b + ' ' + c + ' ' + d + ' ' + e + ' ' + f;
    $('#CaptchaImageCode').empty().append('<canvas id="CapCode" class="capcode" width="300" height="80"></canvas>')

    var c = document.getElementById("CapCode"),
        ctx = c.getContext("2d"),
        x = c.width / 2,
        img = new Image();

    img.src = "https://webdevtrick.com/wp-content/uploads/captchaback.jpg";
    img.onload = function () {
        var pattern = ctx.createPattern(img, "repeat");
        ctx.fillStyle = pattern;
        ctx.fillRect(0, 0, c.width, c.height);
        ctx.font = "46px Roboto Slab";
        ctx.fillStyle = '#dc3545';
        ctx.textAlign = 'center';
        ctx.setTransform(1, -0.12, 0, 1, 0, 15);
        ctx.fillText(cd, x, 55);
    };
}

// Validate Captcha
function ValidateCaptcha() {
    var string1 = removeSpaces(cd);
    var string2 = removeSpaces($('#UserCaptchaCode').val());
    if (string1 == string2) {
        return true;
    }
    else {
        return false;
    }
}

// Remove Spaces
function removeSpaces(string) {
    return string.split(' ').join('');
}


// Check Captcha
function CheckCaptcha() {
    var result = ValidateCaptcha();
    if ($("#UserCaptchaCode").val() == "" || $("#UserCaptchaCode").val() == null || $("#UserCaptchaCode").val() == "undefined") {
        //$('#WrongCaptchaError').text('Please Enter Code Given In a Picture.').show();
        $('#UserCaptchaCode').val("");
        $('#UserCaptchaCode').focus();
        $("#otp_div").css("display", "none");
    } else {
        if (result == false) {
            IsAllowed = false;
            $('#WrongCaptchaError').text("Captcha value doesn't match").show();
            $('#UserCaptchaCode').val("");
            $("#otp_div").css("display", "none");
            CreateCaptcha();
            $('#UserCaptchaCode').focus().select();
        }
        else {
            IsAllowed = true;
            //$('#UserCaptchaCode').val('').attr('place-holder','Enter Captcha - Case Sensitive');
            //CreateCaptcha();
            $('#WrongCaptchaError').fadeOut(100);
            $('#SuccessMessage').fadeIn(500).css('display', 'block').delay(5000).fadeOut(250);
            $("#otp_div").css("display", "block");
            $("#otp_div").show();
            $('#UserCaptchaCode').val("");
            swal("User Verified Successfully");
            //$('#QEMobilecode').hide();
            QEGetMobileOTP();
            //window.location.href = "/OrganisationLogin/OrganisationDetails";
        }
    }
}
function encryptText(textToEncrypt, encryptionKey, IV) {
    const key = CryptoJS.enc.Utf8.parse(encryptionKey);
    const iv = CryptoJS.enc.Utf8.parse(IV);
    const encrypted = CryptoJS.TripleDES.encrypt(textToEncrypt, key, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return encrypted.toString();
}
function decryptText(textToDecrypt, encryptionKey, IV) {
    const key = CryptoJS.enc.Utf8.parse(encryptionKey);
    const iv = CryptoJS.enc.Utf8.parse(IV);
    const decrypted = CryptoJS.TripleDES.decrypt(textToDecrypt, key, {
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });
    return decrypted.toString(CryptoJS.enc.Utf8);
}

function test() {
    AgentLogin();
}
function QEMobileAuth() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();
    //if ($('#QEMobileNo').val() == '') {
    //    swal("Enter Valid Mobile Number");
    //$('#QEEmailbaknew1').hide();
    $('#DigiCerti').hide();
    $('#DIGIbag').hide();
    //} else {
    //$("body").css('overflow', 'auto');
    $("#DigiCerti").hide();
    //$('#QEEmailbaknew1').show();
    $('#Mobileform').show();

    $('#Mobilecode').val
    //}


}
function ActivLogin(ip, hostnm, time) {
    debugger;
    swal({
        title: "User Already Logged In ",
        
        confirmButtonClass: "btn-danger",
        showCancelButton: true,
        confirmButtonText: "Close This Session",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                debugger;

                swal({
                    title: "Previous User Is Logout Now,Login Again",
                    confirmButtonClass: "btn-danger",
                    confirmButtonText: "ok",

                },
                    function () {
                        $.ajax({
                            url: '/OrganisationLogin/Logout',
                            type: 'GET',
                            cache: false,
                            success: function (result) {
                                debugger;
                                if (result == "Logout") {

                                    window.location.href = "/OrganisationLogin/OrganisationDetails";
                                }
                            }
                        })

                    }

                )
            }
            else {
                debugger;

                window.location.href = "/OrganisationLogin/OrganisationDetails";
            }
        });
}






