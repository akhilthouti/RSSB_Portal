$(document).ready(function () {
    $('#SaveQEData').prop('disabled', true);

});
$(document).ready(function () {

    $('#CustReKYC').hide();
    $('#ExistCustomerDiv').hide();

    $('#RekycDiv').hide();
    if ($("#OffEkyc").prop("checked") == true) {
        $('#OffVerification').show();
    }
    else {
        $('#OffVerification').hide();
    }
    if ($("#OnEkyc").prop("checked") == true) {
        $('#CustEkyc').show();
    }
    else {
        $('#CustEkyc').hide();
    }
    if ('@ViewBag.RejectId' != '') {
        $('#SaveQEData').attr("disabled", true);
        //$('#ClearQEData').attr("disabled", true);
        $('#Aadharverificationtype').prop("checked", false);
        $('#OffEkyc').prop("checked", false);
    }

    $("#PanCard").click(function () {
        debugger;
        if ($(this).is(":checked")) {
            $('#Verificationtxt1').show();
            //$('#Verificationtxt2').s()
            $('#Verificationtxt3').show();
            $('#Verificationtxt7').show();
            $('#Verificationtxt8').show();

            $('#DrivingLic').attr("checked", false);
            $('#FetchDigi').show();
            $('#CustDigilockerVerify').hide();
        }
        else {
            $('#Verificationtxt1').hide();
            $('#Verificationtxt3').hide();
            $('#Verificationtxt7').hide();
            $('#Verificationtxt8').hide();

            $('#FetchDigi').hide();
            $('#CustDigilockerVerify').hide();
            //    $('#btnPDF').show();
            //    $('#btnPDFWithout').show();
        }
    });
    $("#DrivingLic").click(function () {
        debugger;
        if ($(this).is(":checked")) {
            $('#Verificationtxt2').show();
            $('#Verificationtxt4').show();
            //$('#Verificationtxt1').show();
            $('#PanCard').attr("checked", false);
            $('#FetchDigi').show();
        }
        else {
            $('#Verificationtxt2').hide();
            $('#Verificationtxt4').hide();
            $('#FetchDigi').hide();
            //    $('#btnPDF').show();
            //    $('#btnPDFWithout').show();
        }
    });
    $("#Aadharxml").click(function () {
        debugger;
        if ($(this).is(":checked")) {
            $('#Verificationtxt5').show();
            //$('#Verificationtxt2').show();
            $('#Aadharxml').attr("checked", false);
            $('#FetchDigi').show();
        }
        else {
            $('#Verificationtxt5').hide();
            $('#FetchDigi').hide();
            //    $('#btnPDF').show();
            //    $('#btnPDFWithout').show();
        }
    });
});
function numericOnly(id) {
    debugger;
    // Allow: backspace, delete, tab, escape, enter and .
    var inputtxt = $('#' + id).val();
    var numbers = /^[6-9][0-9]{0,9}$/;
    if (!numbers.test(inputtxt)) {
        alert('Only Numeric Numbers Can Be Enter ');
        $('#' + id).val('');
        document.form1.text1.focus();
        return true;
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
function alphabetOnly(id) {
    debugger;
    // Allow: backspace, delete, tab, escape, enter and .
    var inputtxt = $('#' + id).val();
    var alpha = /^[A-Z-a-z]+$/;
    if (!alpha.test(inputtxt)) {
        alert('Only Alphabet Can Be Enter ');
        $('#' + id).val('');
        document.form1.text1.focus();
        return true;
    }

}  


function closeForm() {
    debugger;
    $('#Emailbak').hide();
    $('#Mobileform').hide();
}


function GetMobileOTP() {
    debugger;
    var MobileNumber = $('#Mobileotp').val();

    if (MobileNumber == '') {
        swal("Enter valid Mobile and EmailId Number...");
        return false;
    }
    $.ajax({
        url: '/KYCQuickEnroll/MobileOTPSent',
        type: 'POST',
        data: {
            MobileNumber: MobileNumber
        },
        cache: false,
        success: function (result) {
            debugger;
            if (result != null) {
                swal(result);
                $("#MobileGetOtp").html('RESEND OTP');

            } else {
                swal('OTP does not send.try again..');
            }
            // $('#DigitCaptureImage').html(result);
        }
    });
}
function MobileOTPAuthenticate() {
    debugger;
    var OTP = $('#Mobileauth').val();
    var EmailId = $('#txtEmailId').val();

    if (OTP == '') {
        swal("Enter valid OTP...");
        return false;
    }
    if (OTP.length > 6) {
        swal("Enter 6 digit Valid OTP...");
        $('#Mobileauth').val('');
        return false;
    }
    $.ajax({
        url: '/KYCQuickEnroll/MobileOTPAutheticate',
        type: 'GET',
        data: {
            OTP: OTP,
            EmailId: EmailId
        },
        cache: false,
        success: function (result) {
            debugger;
            if (result == "Athenticate Successfully... ") {
                swal(result);
                swal({
                    title: "Authenticate Successfully... ",
                    confirmButtonClass: "btn-danger",
                    confirmButtonText: "Ok",

                },
                    function () {

                        $('#Mobileform').hide();
                        $('#Emailbak').hide();
                        var url = window.location.protocol + "//" + window.location.host + '/KYCQuickEnroll/downloadpdf'
                        window.open(url, '_blank');
                    });
            } else {
                swal(result);
            }
        }
    });

}
function XMLURL(e) {
    debugger;
    $("#steps").hide();
    //window.open('https://resident.uidai.gov.in/offlineaadhaar/', "_newtab")
    window.open('https://resident.uidai.gov.in/offline-kyc', "_newtab")
}
function XMLcaptcha(obj) {// $('#XMLcaptcha').click(function () {
    debugger;
    $("#captchaLoader").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
    $("#captchaLoader").show();
    $('#overlay_disabl').show();
    $.ajax({


        url: "/KYCQuickEnroll/offlineXmlCaptcha",
        type: 'POST',
        cache: false,
        processData: false,
        contentType: false,
        // data: data,
        success: function (result) {
            debugger;
            swal(result);
            $('#captcha').val('');
            $("#captchaLoader").hide();
            $('#overlay_disabl').hide();
            document.getElementById('Captchapic').src = result.captchaimg;

        }
        //}
    });

}
function regsubmit(e) {

    debugger;


    $.ajax({
        url: '/KYCQuickEnroll/DigilockerlFlag',
        type: 'POST',
        cache: false,
        data: {}
    });
    swal("Submitted Successfully");
    window.location.href = '/KYCQuickEnroll/DigitalQuickEnrollment';

}
function submitPanVerification(e) {
    debugger;

    if ($("#PanTitle").val() == '') {
        swal("Enter Title");
        //event.preventDefault();
        return false;
    }
    else if ($("#NSDL_firstname").val() == '') {
        swal("Enter FirstName");
        //event.preventDefault();
        return false;
    }
    else if ($("#NSDL_middlename").val() == '') {
        swal("Enter MiddleName");
        //event.preventDefault();
        return false;
    }
    else if ($("#NSDL_lastname").val() == '') {
        swal("Enter LastName");
        //event.preventDefault();
        return false;
    }
    else {
        $.ajax({
            url: '/KYCQuickEnroll/DigilockerlFlag',
            type: 'POST',
            cache: false,
            data: {}
        });
        window.location.href = '/KYCQuickEnroll/DigitalQuickEnrollment';
    }
}
function AadharForm() {
    debugger;
    if ($("#AXML").prop("checked") == true) {
        // $('#xmlConstentmsg').show();
        $('#KYC').show();
        $('#me').show();
        $('#myself').show();
        $(window).scrollTop($('#myself').position().top);

    }
    else {
        //$('#xmlConstentmsg').hide();
        $('#KYC').show();

    }
    if ($("#AQRCODE").prop("checked") == true) {
        $('#KYCQRCodeVerify').show();
        $(window).scrollTop($('#KYCQRCodeVerify').position().top);
    }
    else {
        $('#KYCQRCodeVerify').hide();
    }
    if ($("#OCR").prop("checked") == true) {
        $('#OCRDiv_id').show();
        $(window).scrollTop($('#OCRDiv_id').position().top);

    }
    else {
        $('#OCRDiv_id').hide();
    }
    if ($("#ScanQR").prop("checked") == true) {
        $('#KYCQRCodeScanVerify').show();
    }
    else {
        $('#KYCQRCodeScanVerify').hide();
    }
    if ($("#AXML1").prop("checked") == true) {
        $('#AadharCaptcha').show();

        //$('#KYCXMLVerify').show();
    }
    else {
        $('#AadharCaptcha').hide();
        // $('#KYCXMLVerify').hide();
    }
}
function agreeconsent() {
    debugger;
    $('#KYCXMLVerify').show();
    $(window).scrollTop($('#KYCXMLVerify').position().top);
}
function disagrconsent() {
    debugger;
    // swal("Disagree");

    swal({
        title: 'Are you sure?',
        text: "You want to cancelled the process!",
        type: 'warning',
        showCancelButton: true,

        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Cancel it!'
    },
        function (isConfirm) {
            if (isConfirm) {
                //$('#myself').hide();
                $('#KYC').hide();
                $('#xmlConstentmsg').hide();
                $('#OffVerification').hide();
                $('#AXML').prop("checked", false);
                $('#OffEkyc').prop("checked", false);
                $('#KYCXMLVerify').hide();

                $('#disagree').prop("checked", false);
                $('#agr').prop("checked", false);

            }
            else {
                $('#disagree').prop("checked", false);

            }
        });
}
function AadharKYC() {
    debugger;
    if ($("#OffEkyc").prop("checked") == true) {
        $('#OffVerification').show();
        $(window).scrollTop($('#OffVerification').position().top);
    }
    else {
        $('#OffVerification').hide();
    }
    if ($("#OnEkyc").prop("checked") == true) {
        $('#CustEkyc').show();
    }
    else {
        $('#CustEkyc').hide();
    }
}

function AadharKYC() {
    debugger;
    if ($("#OffEkyc").prop("checked") == true) {
        $('#OffVerification').show();
        $(window).scrollTop($('#OffVerification').position().top);
    }
    else {
        $('#OffVerification').hide();
    }
    if ($("#OnEkyc").prop("checked") == true) {
        $('#CustEkyc').show();
    }
    else {
        $('#CustEkyc').hide();
    }
}

function agreeconsentXmlme() {
    debugger;
    $('#AadharCaptcha').show();


}
function disagreeconsentXmlme() {
    debugger;
    // swal("Disagree");

    swal({
        title: 'Are you sure?',
        text: "You want to cancelled the process!",
        type: 'warning',
        showCancelButton: true,

        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Cancel it!'
    },
        function (isConfirm) {
            if (isConfirm) {
                $('#me').hide();
                $('#KYC').hide();
                $('#xmlConstentmsgMe').hide();
                $('#OffVerification').hide();
                $('#AXML').prop("checked", false);
                $('#OffEkyc').prop("checked", false);
                $('#AadharCaptcha').hide();

                $('#disagreeme').prop("checked", false);
                $('#agreeMe').prop("checked", false);

                $('#AadharCaptcha').hide();
                $('#otpxml').hide();
                $('#captchaDiv').show();
                $.ajax({


                    url: "/KYCQuickEnroll/offlineXmlCaptcha",
                    type: 'POST',
                    cache: false,
                    processData: false,
                    contentType: false,
                    // data: data,
                    success: function (result) {
                        $("#captchaLoader").hide();
                        $('#overlay_disabl').hide();
                        document.getElementById('Captchapic').src = result.captchaimg;
                        $('#captcha').val("");
                    }
                    //}
                });

            }
            else {
                $('#disagreeme').prop("checked", false);

            }
        });

}
function submitforpan(e) {

    debugger;
    swal("Submitted Successfully");

    window.location.href = '/KYCQuickEnroll/DigitalQuickEnrollment';
}


function EmailOTPAuthenticate() {
    debugger;
    $("#QEEmailbaknew1").css("display", "none");
    var OTP = $('#QEemailauth').val();
    if (OTP == '') {
        swal("Enter valid OTP...");
        return false;
    }
    if (OTP.length > 6) {
        swal("Enter 6 digit Valid OTP...");
        $('#QEemailauth').val('');
        return false;
    }
    $.ajax({
        url: '/KYCQuickEnroll/EmailOTPServices',
        type: 'GET',
        data: {
            OTPforCheck: OTP
        },
        cache: false,
        success: function (result) {
            debugger;
            if (result == "success") {
                $('#QEemailform').hide();
                $('#QEEmailGetOtp').hide();
                swal("Verified Successfully");
                $('#QEisEmailVerify').show();
                $('#QEisEmailVerifyCheckbox').show();
                document.getElementById("QEEmailId").readOnly = true;
                $('#Emailbaknew1').hide();

            }
            else if (result == "Failed") {
                swal("Enter valid OTP");
                $('#QEisEmailVerify').hide();
                $('#QEisEmailVerifyCheckbox').hide();
                $('#QEEmailbaknew1').show();
            }
            else {
                swal(result);
                $('#QEEmailbaknew1').hide();
                $('#QEemailauth').val('');

            }
        }
    });

}
function QEOTPAuth() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();
    if ($('#QEEmailId').val() == '') {
        debugger;
        swal("Enter Valid EmailId");
        $('#QEEmailbaknew1').hide();
        $('#DigiCerti').hide();
        $('#DIGIbag').hide();
        //$('#Emailbak').show();
    } else {
        if ($("#QEEmailId").val() != '') {
            debugger;
            var email = $("#QEEmailId").val();
            if (IsEmail(email) == false) {
                debugger;
                swal("Enter Valid EmailId");
                $('#QEEmailbaknew1').show();
                $('#DigiCerti').hide();
                $('#DIGIbag').hide();
                return false;
            } else {
                //$("body").css('overflow', 'auto');
                $('#DigiCerti').hide();
                $('#QEEmailbaknew1').show();
                $('#QEemailform').show();

                $('#QEemailcode').val($('#QEEmailId').val());

            }
        }

    }


}

function QEMobileAuth() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();
    var number = $('#QEMobileNo').val();
    var regex = /^[6-9]\d{9}$/;
    if ($('#QEMobileNo').val() == '') {
        swal("Enter Valid Mobile Number");
        $('#QEEmailbaknew1').hide();
        $('#DigiCerti').hide();
        $('#DIGIbag').hide();
        
        $('#QEMobileNo').val('');
    }
    else {
        if (regex.test(number)) {
            $('#QEEmailbaknew1').show();
            $("#DigiCerti").hide();
            $('#QEMobileform').show();
            $('#QEMobilecode').val($('#QEMobileNo').val());
            return true;
        } else {
            swal("Invalid mobile number");
            $("#DigiCerti").hide();
            $('#QEMobileNo').val('');
            return false; // Invalid mobile number
        }

    }
}
function onRadioChange(obj) {
    debugger;
    if (obj.id == 'Re-Kyc') {
        debugger;
        $('#New').prop("checked", false);
        $('#JointAccountID').prop("checked", false);
        $('#CustReKYC').show();
        $('#imp').show();
        $('#rekyc_doc').show();
        $('#Panverificationtype').hide();
        $('#pn1').hide();
        $('#jointAccountDIV').hide();

        $('#PanCard').hide();
        $('#PanDigi').hide();

        $('#DrivingLic').hide();
        $('#DrivingDigi').hide();

        $('#QEAuthMobileGetOtp').hide();
        $('#SaveQEData').prop('disabled', false);

        //$("#QEVTypeTextboxId").removeAttr("onblur");
        //$("QEVTypeTextboxId").prop("onblur", null).off("click");
        //$("QEVTypeTextboxId").prop("blur", null).off("click");

        //$("#QEMobileNo").removeAttr("onblur");
        //$("QEMobileNo").prop("onblur", null).off("click");
        //$("QEMobileNo").prop("blur", null).off("click");
    }

    else if (obj.id == 'New') {
        debugger;
        $('#Re-Kyc').prop("checked", false);
        $('#JointAccountID').prop("checked", false);
        $('#Panverificationtype').show();
        $('#jointAccountDIV').hide();
        $('#mainDIVid').show();
        $('#HideVerificationType').show();
        $('#pn1').show();

        $('#CustReKYC').hide();
        $('#QEEmailGetOtp').show();
        $('#QEAuthMobileGetOtp').show();
        $("#middlenamere").hide();
        $("#QEFirstName").prop("disabled", "disabled");
        $("#QELastName").removeAttr("disabled");
        $("#QEMobileNo").removeAttr("disabled");
        $("#QEEmailId").removeAttr("disabled");
        $("#VTYPE").removeAttr("disabled");
        $("#QEVTypeTextboxId").removeAttr("disabled");
        $("#QEFirstName").removeAttr("disabled");

    }
    else if (obj.id == 'JointAccountID') {
        debugger;
        $('#New').prop("checked", false);
        $('#Re-Kyc').prop("checked", false);
        swal({
            title: 'You Already Have an Account',
            text: "Please Click On Yes",
            type: 'warning',
            showCancelButton: true,

            confirmButtonColor: '#d33',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No'

        },

            function (isConfirm) {
                if (isConfirm) {

                    $('#imp').hide();
                    $('#jointAccountDIV').show();
                    $('#CustReKYC').show();
                    $('#mainDIVid').hide();
                    $('#HideVerificationType').hide();

                }
                else {
                    $('#imp').hide();
                    $('#jointAccountDIV').hide();
                    $('#mainDIVid').show();
                    //$('#CustReKYC').show();

                    $('#HideVerificationType').show();
                }

            });
        //$('#New').prop("checked",false);
        // $('#Re-Kyc').prop("checked",false);
        // $('#imp').hide();
        // $('#jointAccountDIV').show();





    }
    else {
        window.location.reload();
    }
};
function GetJointAccountCustomer() {
    debugger;

    var Accountno = $('#cbsJointAccountID').val();

    $.ajax({
        url: '/KYCQuickEnroll/jointAccountCustomer?AccountNO=' + Accountno,
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            debugger;
            if (result == "API is not Working") {
                swal("Services Are not Working Please Try After Sometime!")
                $("#cbsJointAccountID").val('');
            }
            else if (result != null) {
                window.location.href = "/DataVerify/SummerySheetDetails";
            }
            else {
                swal("Your Data Is pre");
            }


        }
    });

}

function AutoPopulate() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $('#QEFirstName').val($('#FirstNameReKyc').val());
    $('#QELastName').val($('#LastNameReKyc').val());
    $('#QEMobileNo').val($('#MobileReKyc').val());
    $('#QEEmailId').val($('#EmailIdReKyc').val());
    var Data = $("#rekyc_doc").val();
    if (Data == "Pan Number") {
        $("#DigiCerti").hide();
        $('#VTYPE').val("P1");
        $('#QEVTypeTextboxId').val($('#cbsId').val());
    }
    $("#DigiCerti").hide();
    $("#CustReKYC").hide();


}

function RkycMPAC() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    var Data = $("#rekyc_doc").val();
    if (Data == "Pan Number") {

        PanKyc();

    }
    else if (Data == "Mobile Number") {

        kyc1();

    }
    else {

        Custno();

    }
}
function kyc1() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    var CBSValue;
    CBSValue = $('#cbsId').val();

    $.ajax({
        url: '/KYCQuickEnroll/GetCBSData2?selectedValue=' + $("#rekyc_doc").val() + '&appType=' + $("#cbsId").val(),
        type: 'POST',
        data: {
            CBSValue: CBSValue,
        },
        cache: false,
        success: function (data) {
            $("#DigiCerti").hide();
            if (data == "Mobile No does Not Exists.") {
                swal(data);
            }
            else if (data != 'Server Error Occurred') {
                swal({
                    title: "Are you sure You want to do your KYC again?",
                    text: "Please Confirm..!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes",
                    cancelButtonText: "No",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            $('#myModalContent').html(data);
                            $('#myModal').show();
                        }
                        else {
                            $('#myModal').hide();

                        }
                    }
                );
            }
            else {
                swal(data);
            }

        }

    });

}
function Custno() {

    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    var CBSValue;
    CBSValue = $('#cbsId').val();

    $.ajax({
        url: '/KYCQuickEnroll/ReKycCust?selectedValue=' + $("#rekyc_doc").val() + '&appType=' + $("#cbsId").val(),
        type: 'POST',
        data: {
            CBSValue: CBSValue,
        },
        cache: false,
        success: function (data) {

            $("#DigiCerti").hide();
            if (data == "Account Not Found ") {
                swal(data);
                $('#cbsId').val('');
            }
            else if (data != 'Server Error Occurred') {
                swal({
                    title: "Are you sure You want to do your KYC again?",
                    text: "Please Confirm..!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes",
                    cancelButtonText: "No",
                    closeOnConfirm: true,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            $('#myModalContent').html(data);
                            $('#myModal').show();
                        }
                        else {
                            $('#myModal').hide();

                        }
                    }
                );
            }
            else {
                swal(data);
            }

        }

    });

}
function EXT() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    var CBSValue;
    CBSValue = $('#cbsId').val();

    if (CBSValue == null || CBSValue == "") {
        //$("#ReKYCLoader").hide();
        //$('#overlay_disabl').hide();
        swal("Please Enter Valid Entry");
    }

    else {
        $.ajax({
            url: '/KYCQuickEnroll/GetCBSData?selectedValue=' + $("#rekyc_doc").val() + '&appType=' + $("#cbsId").val(),
            //url: '/KYCQuickEnroll/GetCBSData?CBSValue=' + CBSValue,
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                debugger;
                //swal(result);
                if (result == "Failed") {
                    swal("Customer not found in CBS data");
                    document.getElementById("#RekycDiv").style.display = "none";
                }

                if (result.FirstNameReKyc != "") {
                    swal("Data Fetched Successfully");
                    $("#DigiCerti").hide();
                    $('#RekycDiv').show();
                    $("#QEFirstName").prop('disabled', false);
                    $("#QELastName").prop('disabled', false);
                    $("#QEMobileNo").prop('disabled', false);
                    $("#QEEmailId").prop('disabled', false);
                    $("#VTYPE").prop('disabled', false);
                    $("#QEVTypeTextboxId").prop('disabled', false);

                    $('#LblFirstName').addClass('active');
                    $("#FirstNameReKyc").val(result.firstNameReKyc);
                    $('#LblMiddleName').addClass('active');
                    $("#MiddleNameReKyc").val(result.middleNameReKyc);
                    if (result.GenderReKyc == 'F') {
                        $("#ReKycgender2").prop('checked', true);
                    } else {
                        $("#ReKycgender1").prop('checked', true);
                    }

                    $('#LblLastName').addClass('active');
                    $("#LastNameReKyc").val(result.lastNameReKyc);
                    $("#MobileReKyc").val(result.mobileReKyc);
                    $("#EmailIdReKyc").val(result.emailIdReKyc);
                    $("#LastNameReKyc").val(result.lastNameReKyc);
                    $("#ReKyc_addressline1").val(result.reKyc_AddressLine1);
                    $("#ReKyc_dressline2").val(result.reKyc_AddressLine2);
                    $("#ReKyc_dressline3").val(result.reKyc_AddressLine3);
                    $("#ReKyc_City").val(result.reKyc_City);
                    $("#ReKyc_AnnualRevenue").val(result.reKyc_AnnualRevenue);
                    $("#ReKyc_Occupation").val(result.reKyc_Occupation);
                    $("#ReKyc_PinCode").val(result.reKyc_PinCode);
                    $("#DateOfBirthReKyc").val(result.dateOfBirthReKyc);
                    if (result.ReKyc_AADHA == "AADHA") {
                        $('#Aadhaar_Card').attr("checked", true);
                        $('#Aadharverificationtype').attr("checked", true);
                    }
                    if (result.ReKyc_PANR == "PANR") {
                        $('#Pan_Card').attr("checked", true);
                        //$('#DigiLocker').attr("checked", true);
                        $('#Panverificationtype').attr("checked", true);
                        //$('#CustPanVerify').show();
                    }
                    if (result.ReKyc_ELECD == "ELECD") {
                        $('#Voter_Card').attr("checked", true);
                        $('#DigiLocker').attr("checked", true);
                    }
                    if (result.ReKyc_PASSP == "PASSP") {
                        $('#Passport').attr("checked", true);
                        $('#DigiLocker').attr("checked", true);
                    }
                    if (result.ReKyc_DRLIC == "DRLIC") {
                        $('#Driving_licence').attr("checked", true);
                        $('#DigiLockerAadharverificationtype').attr("checked", true);
                    }
                    $("html, body").animate({
                        scrollTop: $(
                            'html, body').get(0).scrollHeight
                    }, 2000);
                    $(window).scrollTop($('#imageSign').position().top);
                }
                else if (result.CBSStatus == "Record Already  Exists") {
                    $("#DigiCerti").hide();
                    $("#DIGIbag").hide();
                    $("#blackbg2").hide();
                    $("#ReKYCLoader").hide();
                    $('#overlay_disabl').hide();
                    swal(result.CBSStatus);
                    $("#filexml").val('');
                }
                else {
                    $("#DigiCerti").hide();
                    $("#DIGIbag").hide();
                    $("#blackbg2").hide();
                    $("#ReKYCLoader").hide();
                    $('#overlay_disabl').hide();

                    swal(result);
                }

                if (result.TFForVerif == true) {
                    $("#imageSign").show();
                    $("#imageSignNot").hide();


                }
                else {
                    $("#imageSignNot").show();
                    $("#imageSign").hide();


                }
            }
        });

    }




    $("#DigiCerti").hide();

}

function closeemail() {
    $('#emailform').hide();
    $('#Emailbaknew1').hide();
    $("#DigiCerti").hide();
}
function closeQEMobile() {
    $("#DigiCerti").hide();
    $("#DIGIbag").hide();
    $('#emailform').hide();
    $('#Emailbaknew1').hide();
}
function QEcloseemail() {
    $("#DigiCerti").hide();
    $("#DIGIbag").hide();
    $('#QEemailform').hide();
    $('#QEEmailbaknew1').hide();
}
function closeQEMobile() {
    $("#DigiCerti").hide();
    $("#DIGIbag").hide();
    $('#QEMobileform').hide();
    $('#QEEmailbaknew1').hide();
}
function QEGetMobileOTP() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();

    const textToEncrypt = $('#QEMobilecode').val();
    const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
    $.ajax({
        url: '/KYCQuickEnroll/QEMoblieOTPSend',
        type: 'POST',
        data: {
            MobileNumber: encryptedMobileNo
        },
        cache: false,
        success: function (result) {
            debugger;
            $("#DigiCerti").hide();
            $("#DIGIbag").hide();
            if (result != null) {
                debugger;
                if (result == "Index was outside the bounds of the array.") {
                    swal("Enter Valid Mobile Number");
                }
                else { 
                swal(result);
                $("#QEMobileGetOtp").html('RESEND OTP');
                $("#QEMobileGetOtp").prop('disabled', true);
                setTimeout(function () {
                    $("#QEMobileGetOtp").prop('disabled', false); // enable the button after 5 seconds
                }, 600000);
                    }
            }
            else {
                swal('OTP does not send.try again..');
            }
        }
    });

}

function OTPQEMobileAuthenticate() {
    debugger;
    $("#DIGIbag").show();
    var OTP = $('#QEMobileauth').val();
    const textToEncrypt = $('#QEMobilecode').val();
    const encryptedMobileNo = encryptText(textToEncrypt, encryptionKey, IV);
    $(".hello").hide();
    $(".hell").css("display", "block");
    //swal("Authenticate Successfully... ");
    $("#QEMobileform").hide();
    $("#QEEmailbaknew1").css("display", "none");
    $('#QEMobileauth').val('');
    $("")
    if (OTP == '') {
        swal("Enter valid OTP...");
        $('#QEisMobileVerify').hide();
        $('#QEAuthMobileGetOtp').show();
        return false;
    }
    if (OTP.length > 6) {
        swal("Enter 6 digit Valid OTP...");
        $('#QEMobileauth').val('');
        $('#QEisMobileVerify').hide();
        $('#QEAuthMobileGetOtp').show();
        return false;
    }
    $.ajax({
        //url: '/KYCQuickEnroll/OTPAutheticate',
        url: '/KYCQuickEnroll/QEMobileOTPAuthenticate',
        type: 'GET',
        data: {
            OTP: OTP,
            MobileNumber: encryptedMobileNo
        },
        cache: false,
        success: function (result) {
            debugger;
            $("#DIGIbag").hide();
            if (result == "Verified Successfully") {
                swal(result);
                $('#QEMobileNo').prop("readonly", "readonly");
                $("#DigiCerti").hide();
                $("#DIGIbag").hide();
                $('#Emailbaknew1').hide();
                $('#QEMobileform').hide();
                $('#EmailGetOtp').hide();
                //$('#QEisMobileVerify').show();
                $('#QEMobileVerify').show();
                $('#SaveQEData').prop('disabled', false);
                $('#QEAuthMobileGetOtp').hide();



            } else {
                swal(result);
                $('#Emailbaknew1').hide();
                $('#emailauth').val('');
                $('#QEMobileVerify').hide();
                $('#QEisMobileVerify').hide();
                $('#QEAuthMobileGetOtp').show();
            }

        }
    });

}
function GetOTP() {
    debugger;
    $("#DIGIbag").show();
    var EmailId = $('#emailcode').val();
    $.ajax({
        url: '/KYCQuickEnroll/EmailOTPSent',
        type: 'POST',
        data: {
            EmailId: EmailId
        },
        cache: false,
        success: function (result) {
            debugger;
            $("#DIGIbag").hide();
            if (result != null) {
                swal(result);
            } else {
                swal('OTP does not send.try again..');
            }
            // $('#DigitCaptureImage').html(result);
        }
    });

}
function QEGetOTP() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();
    var EmailId = $('#QEemailcode').val();
    $.ajax({
        url: '/KYCQuickEnroll/QEEmailOTPSent',
        type: 'POST',
        data: {
            EmailId: EmailId

        },
        cache: false,
        success: function (result) {
            debugger;
            $("#DigiCerti").hide();
            $("#DIGIbag").hide();
            if (result != null) {
                swal(result);
                $("#QEEmailGetOtp1").html('RESEND OTP');
                $("#QEEmailGetOtp1").prop('disabled', true);
                setTimeout(function () {
                    $("#QEEmailGetOtp1").prop('disabled', false); // enable the button after 5 seconds
                }, 10000);
            } else {
                swal('OTP does not send.try again..');
            }
            // $('#DigitCaptureImage').html(result);
        }
    });

}
function EmailOTPServices() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    var Email = $('#QEemailcode').val();
    var digits = '0123456789';
    let OTP = '';
    for (let i = 0; i < 6; i++) {
        OTP += digits[Math.floor(Math.random() * 10)];
    }
    $.ajax({
        url: '/KYCQuickEnroll/EmailOTPServices?Email=' + Email + '&OTP=' + OTP,
        type: 'GET',
        cache: false,
        success: function (result) {
            debugger;
            $("#DigiCerti").hide();
            if (result == "Successfully Sent") {

                swal("Successfully Sent")
            }
            else {
                swal("Something Went Wrong Try Again?");
            }
        }
    });

}




function OTPAuthenticate() {
    debugger;
    $("#DIGIbag").show();
    var OTP = $('#emailauth').val();
    if (OTP == '') {
        swal("Enter valid OTP...");
        return false;
    }
    $.ajax({
        //url: '/KYCQuickEnroll/OTPAutheticate',
        url: '/KYCQuickEnroll/OTPAutheticate',
        type: 'GET',
        data: {
            OTP: OTP
        },
        cache: false,
        success: function (result) {
            debugger;
            $("#DIGIbag").hide();
            if (result == "Athenticate Successfully... ") {
                swal(result);
                $('#Emailbaknew1').hide();
                $('#emailform').hide();
                $('#EmailGetOtp').hide();
                $('#isEmailVerify').show();


            } else {
                swal(result);
                $('#Emailbaknew1').hide();
                $('#emailauth').val('');

            }
            // $('#DigitCaptureImage').html(result);
        }
    });

}
function Fethch() {
    debugger;
    var DocType1 = $('#DocType').val();


    if ($("#PanCard").prop('checked') == true) {
        var Verificationtxt3 = $('#Verificationtxt3').val();
        if (Verificationtxt3 == "") {
            swal("Please Enter Pan No");
            return false;
        }
        else{


        }
    }

    if ($("#DrivingLic").prop('checked') == true) {
        var Verificationtxt4 = $('#Verificationtxt4').val();
        
        var pattern = /^[a-zA-Z0-9]*$/;
        if (Verificationtxt4 == "") {
            swal("enter valid Driving Licence no");
            return false;
        }
        else if (!pattern.test(Verificationtxt4)) {
            swal("Special characters are not allowed.");
            return false; // Prevent form submission or any further action
        }
        else if (Verificationtxt4.indexOf(' ') != -1) {
            $("#DigiCerti").hide();
            swal("Password should not contain spaces.");
            return false;
        }
    }

    var Verificationtxt3 = $('#Verificationtxt3').val();
    var Verificationtxt4 = $('#Verificationtxt4').val();
    var Verificationtxt6 = $('#Verificationtxt6').val();
    var Verificationtxt8 = $('#Verificationtxt8').val();

    //var PanCard = $('#PanCard').val();
    //var DrivingLic = $('#DrivingLic').val();
    //var Aadharxml = $('#Aadharxml').val();

    var lfckv1 = document.getElementById("PanCard").checked;
    var lfckv2 = document.getElementById("DrivingLic").checked;
    var lfckv3 = document.getElementById("Aadharxml").checked;


    window.location.href = '/KYCQuickEnroll/FetchDoc?DocType1=' + DocType1 + '&Verificationtxt3=' + Verificationtxt3 + '&Verificationtxt4=' + Verificationtxt4 + '&Verificationtxt6=' + Verificationtxt6 + '&Verificationtxt8=' + Verificationtxt8 + '&DrivingLic=' + lfckv2 + '&PanCard=' + lfckv1 + '&Aadharxml=' + lfckv3;

}
function PanVerificationforPan() {
    debugger;
    $("#DigiCerti").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#DigiCerti").show();
    $("#DIGIbag").show();
    $("#blackbg2").show();
    if ($('#NSDL_pannumber').val() != "") {
        $.ajax({
            url: '/KYCQuickEnroll/NSDLPanVerification?PanNo=' + $('#NSDL_pannumber').val(),
            type: 'POST',
            cache: false,
            processData: false,
            contentType: false,
            success: function (result) {
                debugger;
                if (result != null) {
                    //NSDL_firstname  NSDL_middlename NSDL_lastname
                    $('#NSDL_firstname').val(result.nsdL_FirstName);
                    $('#NSDL_middlename').val(result.nsdL_MiddleName);
                    $('#NSDL_lastname').val(result.nsdL_LastName);
                    $('#PanTitle').val(result.nsdL_PanTitle);
                    //$('#nameprintedonPan').val(result.NSDL_NamePrintedOnPan);
                    if (result.nsdL_Result == "Aadhaar Seeding is Successful") {
                        swal("Aadhaar Seeding is Successful");
                        //return false;
                    }
                    else {
                        swal("Aadhaar Seeding is UnSuccessful");
                    }
                    let text = $('#QEFirstName').val();
                    let l = $('#NSDL_firstname').val();
                    let mm = text.toUpperCase();
                    //alert(mm);
                    if (mm != $('#NSDL_firstname').val()) {
                        /*alert("Name not matching as per quick enrollment.");*/


                    }
                    $("#DigiCerti").hide();
                    $("#DIGIbag").hide();
                    $("#blackbg2").hide();
                    swal(result.NSDL_Result);
                    $('#goForNextCAF').show();
                    $('#goForNextCAF1').show();
                    $('#goForNextCAF2').show();
                }
            }
        });
    }
    else {
        $("#DigiCerti").hide();
        $("#DIGIbag").hide();
        $("#blackbg2").hide();
        swal("Enter Pan Number");
    }
}
function getOtp(obj) {

    var EKYC_Length = $("#aadhaartext").val().length;

    //if (EKYC_Length == 12) {
    debugger;

    $("#loading").html("<div style='position: fixed;z-index: 5000;float: right;right: 49%;height: 100px;width: 100px;top:41%'><i class='fa fa-pulse fa-fw dgldr ldrdesign' style='height:70px;width:70px;border-radius:50%;border-bottom:5px dotted grey;border-top:5px dotted black;border-right:5px dotted black;border-left:5px dotted black;height:80px;width:80px;position:absolute;margin: 20% 50%;'></i></div>");
    $("#overlay_disabl").show();

    $("#otpButton").hide();
    $("#Resendbtn").show();

    $.ajax({
        url: '/EKYC/GetOTPWRTAdhaar?AadhaarNumber=' + $('#aadhaartext').val() + '&customerID=' + $('#custAadharOtp').val(),
        type: 'GET',
        cache: false,
        success: function (result) {

            debugger;

            swal(result.split('-')[0]);

            document.getElementById("transactionIdEKYC").value = result.split('-')[1];
            document.getElementById("otpButton").disabled = true;
            document.getElementById("aadhaartext").disabled = true;
            $("#overlay_disabl").hide();
            $("#loading").html("");
            $("#loadingImg1").html("");
            document.getElementById("throughOTP").disabled = false;
            $('#throughOTP').find('*').attr('disabled', false);
            $('#radiobutton').find('*').attr('disabled', false);
        }
    });
}
function ResetOtp() {
    debugger;
    document.getElementById("otpButton").disabled = false;
    document.getElementById("aadhaartext").disabled = false;
    document.getElementById("AgreeF").checked = false;
    document.getElementById("DisagreeF").checked = false;

    $("#otpButton").show();
    $('#afterAgreeFinger').hide();
    $('#resetbtn').hide();
    $('#EkycViaOpt').hide();

    $('#aadhaartext').val("");
    $('#otptext').val("");
    $('#Load_resultOtp').hide();
}

function onAgreeClickFinger(check) {
    debugger;
    var prim = document.getElementById("AgreeF");
    var secn = document.getElementById("DisagreeF");

    if (check == 1) {
        debugger;
        if ($("#aadhaartext").val() == "") {   //|| $('#aadhaartext').val().length < 12
            swal("Enter Aadhaar number")
            $('#AgreeF').attr("checked", false);
            $('#DisagreeF').attr("checked", false);
            return false;
        }
        else {
            //$('#overlay_disabl').show();
            //$('#ConfirmPhysical').show();
            debugger;
            var url = '/EKYC/consentRegister?Aadhar=' + $("#aadhaartext").val() + '&act=Agree';
            $.ajax({
                url: url,
                type: 'GET',
                cache: false,
                success: function (result) {
                    if (result == "logout") {
                        window.location.href = '/AccountLogin/AccountLogin';
                    }
                    else if (result == 'Customer Already Authenticated') {
                        swal(result);
                    }
                }
            })
            $("#Resendbtn").hide();
            $('#afterAgreeFinger').show();
            $('#resetbtn').show();
            $('#EkycViaOpt').show();
            $('#EkycThrowOtp').hide();
            $('#AgreeF').attr("checked", true);
            $('#DisagreeF').attr("checked", false);
            secn.checked = false;
        }
    }
    else if (check == 2) {
        $('#overlay_disabl').show();
        $('#ConfirmDisagree').show();
        $('#afterAgreeFinger').hide();
        $('#resetbtn').hide();
        $('#EkycThrowOtp').hide();
        $('#EkycViaOpt').hide();
        prim.checked = false;
        $('#AgreeF').attr("checked", false);
        $('#DisagreeF').attr("checked", true);
        //$('#fingerprintfgfg').hide();
    }
}
function AutoPopulate1() {
    $('#QEFirstName').val($('#ExistFirstName').val());
    $('#QELastName').val($('#ExistLastName').val());
    $('#QEMobileNo').val($('#ExistMobileNo').val());
    $('#QEEmailId').val($('#ExistEmail').val());
    $("#QEVTypeTextboxId").removeAttr("onblur");
    $("QEVTypeTextboxId").prop("onblur", null).off("click");
    $("QEVTypeTextboxId").prop("blur", null).off("click");

    $("#QEMobileNo").removeAttr("onblur");
    $("QEMobileNo").prop("onblur", null).off("click");
    $("QEMobileNo").prop("blur", null).off("click");
    $("#ExistCustomerDiv").hide();


}
function Dedupe() {
    debugger;
    var MobileNumber = $('#QEMobileNo').val();

    $.ajax({
        url: '/KYCQuickEnroll/Dedupe',
        type: 'POST',
        data: {
            MobileNumber: MobileNumber,
        },
        cache: false,
        success: function (data) {
            swal({
                title: "The account no already exists with this Mobile No. Are you sure you want to continue Opening a new Account?",
                text: "Please Confirm..!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {

                        $('#myModalContent').html(data);
                        $('#myModal').show();
                    }
                    else {
                        $('#myModal').hide();
                    }
                }
            );
        }

    });
}