function captureimage() {
    debugger;
    $.ajax({
        url: '/KYCImageCapture/Opencamera',
        type: 'GET',
        cache: false,
        success: function (result) {
            debugger;
            $('#DigitCaptureImage').html(result);
            $("html, body").animate({
                scrollTop: $(
                    'html, body').get(0).scrollHeight
            }, 2000);
            $(window).scrollTop($('#orgresubmit').position().top);
        }
    });
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
function CCVerifyOTP() {
    debugger;
    const textToEncrypt;
    var OTP;
    var checkid = $(this).attr('id');
    if ($("#CustOTP_id").val() != null && $("#CustOTP_id").val() != "") {
        textToEncrypt = $("#CustMobileNo_id").val();
        const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
        OTP = $("#CustOTP_id").val();
    }
    else if ($("#UserOTP_id").val() != null && $("#UserOTP_id").val() != "") {
        textToEncrypt = $('#UserMobileNo_id').val();
        const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
        OTP = $("#UserOTP_id").val();
    }
    else {
        swal("Enter OTP");
    }
    $.ajax({
        url: "/DataVerify/VerifyGenerateOTP?MbileNo=" + encryptedMobileNo + "&OTP=" + OTP + "&Verifyname=CCVerifyOTP",//+ "&BCPhoto=" + null.......,
        type: "Get",
        cache: false,
        success: function (result) {
            debugger;
            if (result == "OTP Validated Successfully..") {
                debugger;
                $('#SMobileVerify1').show();
                $('#SisMobileVerify1').show();
                $('#PhotoAgent').show();
                $('#CameraDiv').show();
                swal(result);
                $("#CustMobileNoButton_id").prop('disabled', true);
                $("#CVerifyButton").prop('disabled', true);
            }
            else {
                swal(result);

            }
        }
    })
}
function CustCVerifyOTP() {
    debugger;
    const textToEncrypt;
    var OTP;
    var checkid = $(this).attr('id');
    if ($("#CustOTP_id").val() != null && $("#CustOTP_id").val() != "") {
        textToEncrypt = $("#CustMobileNo_id").val();
        const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
        OTP = $("#CustOTP_id").val();
    }
    else if ($("#UserOTP_id").val() != null && $("#UserOTP_id").val() != "") {
        textToEncrypt = $('#UserMobileNo_id').val();
        const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
        OTP = $("#UserOTP_id").val();
    }
    else {
        swal("Enter OTP");
    }
    $.ajax({
        url: "/DataVerify/VerifyGenerateOTP?MbileNo=" + encryptedMobileNo + "&OTP=" + OTP + "&Verifyname=CCVerifyOTP",//+ "&BCPhoto=" + null.......,
        type: "Get",
        cache: false,
        success: function (result) {
            debugger;
            if (result == "OTP Validated Successfully..") {
                debugger;
                $('#SMobileVerify1').show()
                $('#SisMobileVerify1').show();
                swal(result);
                $("#CustMobileNoButton_id").prop('disabled', true);
                $("#CVerifyButton").prop('disabled', true);


                $('#CustCAFbtndownload').show();
            }
            else {
                swal(result);

            }
        }
    })
}
function CustMobileOTPGenerate() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();
    const textToEncrypt = $("#CustMobileNo_id").val();
    const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
    $.ajax({
        url: "/DataVerify/SendOtp?MbileNo=" + encryptedMobileNo,
        type: "Get",
        cache: false,
        success: function (result) {
            debugger;
            $("#DigiCerti").hide();
            $("#DIGIbag").hide();
            if (result == "OTP Generated Successfully...!" || result == "OK") {
                debugger;
                swal("OTP Generated Successfully..");
                $('#custOtpDiv').show();
                // $("#CustMobileNoButton_id").prop('Send OTP', 'RESEND OTP');
                $("#CustMobileNoButton_id").html('RESEND OTP');
                $("#CustMobileNoButton_id").prop('disabled', true);
                setTimeout(function () {
                    $("#CustMobileNoButton_id").prop('disabled', false); // enable the button after 5 seconds
                }, 10000);
            }
            else {
                swal(result);
            }
        }
    })
}

function SendPhotoOfCamera(e) {
    debugger;
    var base64I;
    var imageInnerHtml;
    var b;
    var resultvalues;
    var resultvalues1;

    if ($('#DigitCaptureImage').is(':visible') == true) {
        // if (document.getElementById('livephoto').src != '') {
        if ($('#results').is(':visible') == true) {
            if (document.getElementById('results').innerHTML != "Your captured image will appear here...") {
                base64I = (document.getElementById("results").innerHTML);
                imageInnerHtml = base64I.split(",");
                b = imageInnerHtml[1];
                b = b.substring(0, b.length - 2);
                resultvalues = demo.textContent;
                resultvalues1 = demo1.textContent;


            }


        }
        swal("Capture photo");
        return false;

    }
}
$('#digiphotoid').val(b);
$('#latitudelongitude_id').val(resultvalues);
$('#Prediction_id').val(resultvalues1);
$("html, body").animate({
    scrollTop: $(
        'html, body').get(0).scrollHeight
}, 2000);
function UserMobileOTPGenerate() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();
    const textToEncrypt = $("#UserMobileNo_id").val();
    const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
    $.ajax({
        url: "/DataVerify/SendOtp?MbileNo=" + encryptedMobileNo,
        type: "Get",
        cache: false,
        success: function (result) {
            debugger;
            $("#DigiCerti").hide();
            $("#DIGIbag").hide();
            if (result == "OTP Generated Successfully...!" || result == "OK") {
                debugger;
                swal("OTP Generated Successfully..");
                $('#custOtpDiv').show();
                $("#UserMobileNoButton_id").html('RESEND OTP');
                $("#UserMobileNoButton_id").prop('disabled', true);
                setTimeout(function () {
                    $("#UserMobileNoButton_id").prop('disabled', false); // enable the button after 5 seconds
                }, 10000);
            }
            else {
                swal(result);
            }
        }
    })
}
