
function Aadharvalidation(id) {
    debugger;
    // Allow: backspace, delete, tab, escape, enter and .
    var inputtxt = $('#' + id).val();
    var alpha = /^[2-9]{1}[0-9]{11}$/;
    if (!alpha.test(inputtxt)) {
        alert('Please Enter Valid Aadhar Data ');
        $('#' + id).val('');
        document.form1.text1.focus();
        return true;
    }

}
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
function GetOTP() {
    debugger;
    var EmailId = $('#emailcodeOTP').val();
    $.ajax({
        url: '/KYCQuickEnroll/EmailOTPSent',
        type: 'POST',
        data: {
            EmailId: EmailId
        },
        cache: false,
        success: function (result) {
            debugger;
            if (result != null) {
                swal(result);
            } else {
                swal('OTP does not send.try again..');
            }
            // $('#DigitCaptureImage').html(result);
        }
    });

}
function GetPanVerifyData() {
    debugger;
    //$("#DigiCerti1").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
    //$("#DigiCerti1").show();
    //$("#DIGIbag1").show();
    $.ajax({
        url: '/KYCQuickEnroll/GetPanVerification?PanNo=' + '@Model.Digi_PAN',
        type: 'GET',
        cache: false,
        processData: false,
        contentType: false,
        success: function (result) {
            debugger;
            if (result != null) {
                $("#DigiCerti1").hide();
                $("#DIGIbag1").hide();
                $('#digi_firstname').val('');
                $('#digi_middlename').val('');
                $('#digi_lastname').val('');
                $('#digi_DOB').val('');
                $('#digi_address1').val('');
                $('#digi_address2').val('');
                $('#digi_address3').val('');
                $('#digi_Gender').val('');
                $('#digi_pincode').val('');


                $('#digi_firstname').val(result.digi_FirstName);
                $('#digi_middlename').val(result.digi_MiddleName);
                $('#digi_lastname').val(result.digi_LastName);
                $("#getPanData1").addClass("green");


                $('#EditPanPerDetail').show();
                $('#EditPerDetail').hide();
                //document.getElementById("digi_firstname").disabled = true;
                //document.getElementById("digi_middlename").disabled = true;
                //document.getElementById("digi_lastname").disabled = true;
                //document.getElementById("digi_DOB").disabled = true;
                //document.getElementById("digi_Gender").disabled = true;

            }
        }
    })
};

function OTPAuthenticate() {
    debugger;
    var OTP = $('#emailauth').val();
    if (OTP == '') {
        swal("Enter valid OTP...");
        return false;
    }
    $.ajax({
        url: '/KYCQuickEnroll/OTPAutheticate',
        type: 'GET',
        data: {
            OTP: OTP
        },
        cache: false,
        success: function (result) {
            debugger;
            if (result == "Athenticate Successfully... ") {
                swal(result);
                $('#Emailbaknew1').hide();
                $('#emailformOTP').hide();
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
function OTPAuth() {
    debugger;

    if ($('#emailId').val() == '') {
        swal("Enter Valid EmailId");
        $('#Emailbaknew1').show();
        //$('#Emailbak').show();
    } else {
        $('#Emailbaknew1').show();
        $('#Emailbak').show();
        $('#emailformOTP').show();

        $('#emailcodeOTP').val($('#emailId').val());
    }


}
function Panvalidation(id) {
    debugger;
    // Allow: backspace, delete, tab, escape, enter and .
    var inputtxt = $('#' + id).val();

    var numbers = /([A-Z]){5}([0-9]){4}([A-Z]){1}$/;
    if (!numbers.test(inputtxt)) {
        alert('Please Enter Valid PAN');
        $('#' + id).val('');
        document.form1.text1.focus();
        return true;
    }

}
function SendPhotoOfCamera(e) {
    debugger;
    //$("#DigiCerti1").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
    //$("#DigiCerti1").show();
    //$("#DIGIbag1").show();
    var base64I;
    var imageInnerHtml;
    var b;
    var resultvalues;
    var resultvalues1;
    if ($('#base64img').val() == null) {
        swal("Please Capture Your Live Photo");
    }
    else {
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


            } else {

                if ($("#SubTitleId").val() == '') {
                    swal("Enter Sub-Title");
                    //event.preventDefault();
                    return false;
                }
                else if ($("#digi_firstname").val() == '') {
                    swal("Enter Firstname");
                    //event.preventDefault();
                    return false;
                }

                else if ($("#digi_middlename").val() == '') {
                    swal("Enter Middlename");
                    //event.preventDefault();
                    return false;
                }

                else if ($("#digi_lastname").val() == '') {
                    swal("Enter Lastname");
                    //event.preventDefault();
                    return false;
                }

                else if ($("#digi_DOB").val() == '') {
                    swal("Enter DOB");
                    //event.preventDefault();
                    return false;
                }

                else if ($("#digi_Gender").val() == '') {
                    swal("Select Gender");
                    //event.preventDefault();
                    return false;
                }
                else if ($("#CasteId").val() == '') {
                    swal("Select Caste");
                    //event.preventDefault();
                    return false;
                }
                else if ($("#ReligionId").val() == '') {
                    swal("Select Region");
                    //event.preventDefault();
                    return false;
                }
                else if ($("#maritalstatusId").val() == '') {
                    swal("Select Marital Status");
                    //event.preventDefault();
                    return false;
                }
                else if ($("#emailId").val() == '') {
                    swal("Enter EmailID");
                    //e.preventDefault();
                    return false;
                }
                else if ($("#mobileNo").val() == '') {
                    swal("Enter MobileNo");
                    //e.preventDefault();
                    return false;
                }
                else if ($("#MobileDetail").val() == '') {
                    swal("Enter Mobile Details");
                    //event.preventDefault();
                    return false;
                }
                else if ($("#digi_address1").val() == '') {
                    swal("Enter permanent Address Line 1");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_address2").val() == '') {
                    swal("Enter permanent Address Line 2");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_address3").val() == '') {
                    swal("Enter permanent Address Line 3");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_city").val() == '') {
                    swal("Enter permanent City");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_pincode").val() == '') {
                    swal("Enter permanent Pincode");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#Add_ddlState").val() == '') {
                    swal("Enter permanent State");

                    //e.preventDefault();
                    return false;

                }
                
                else if ($("#Add_ddlCountry").val() == '') {
                    swal("Enter permanent Country");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#ResidenceId").val() == '') {
                    swal("Select Residence");

                    //e.preventDefault();
                    return false;

                }

                else if ($("#ResidenceDocument").val() == '') {
                    swal("Select Residence Document");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#ResidentialStatusid").val() == '') {
                    swal("Enter Residential Status");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#residenceYNId").val() == '') {
                    swal("Select Residence Y/N");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_permAddress1").val() == '') {
                    swal("Enter Correspondence Address Line 1");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_permAddress2").val() == '') {
                    swal("Enter Correspondence Address Line 2");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_permAddress3").val() == '') {
                    swal("Enter Correspondence Address Line 3");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_permCity").val() == '') {
                    swal("Enter Correspondence City");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_PERM_pincode").val() == '') {
                    swal("Enter Correspondence City");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#PERM_ddlState").val() == '') {
                    swal("Enter Correspondence State");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#PERM_ddlCountry").val() == '') {
                    swal("Select Correspondence Country");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#digi_pan").val() == '') {
                    swal("Enter Pan No");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#PhoneBanking").val() == '') {
                    swal("Select PhoneBanking");

                    //e.preventDefault();
                    return false;

                }
                else if ($("#AMLRating").val() == '') {
                    swal("Select AMLRating");

                    //e.preventDefault();
                    return false;

                }
                
                swal("Capture photo");
                //event.preventDefault();
                return false;

            }
        } else {

            if ($("#SubTitleId").val() == '') {
                swal("Enter Sub-Title");
                //event.preventDefault();
                return false;
            }
            else if ($("#digi_firstname").val() == '') {
                swal("Enter Firstname");
                //event.preventDefault();
                return false;
            }

            else if ($("#digi_middlename").val() == '') {
                swal("Enter Middlename");
                //event.preventDefault();
                return false;
            }

            else if ($("#digi_lastname").val() == '') {
                swal("Enter Lastname");
                //event.preventDefault();
                return false;
            }

            else if ($("#digi_DOB").val() == '') {
                swal("Enter DOB");
                //event.preventDefault();
                return false;
            }

            else if ($("#digi_Gender").val() == '') {
                swal("Select Gender");
                //event.preventDefault();
                return false;
            }
            else if ($("#CasteId").val() == '') {
                swal("Select Caste");
                //event.preventDefault();
                return false;
            }
            else if ($("#ReligionId").val() == '') {
                swal("Select Region");
                //event.preventDefault();
                return false;
            }
            else if ($("#maritalstatusId").val() == '') {
                swal("Select Marital Status");
                //event.preventDefault();
                return false;
            }
            else if ($("#emailId").val() == '') {
                swal("Enter EmailID");
                //e.preventDefault();
                return false;
            }
            else if ($("#mobileNo").val() == '') {
                swal("Enter MobileNo");
                //e.preventDefault();
                return false;
            }
            else if ($("#MobileDetail").val() == '') {
                swal("Enter Mobile Details");
                //event.preventDefault();
                return false;
            }
            else if ($("#digi_address1").val() == '') {
                swal("Enter permanent Address Line 1");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_address2").val() == '') {
                swal("Enter permanent Address Line 2");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_address3").val() == '') {
                swal("Enter permanent Address Line 3");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_city").val() == '') {
                swal("Enter permanent City");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_pincode").val() == '') {
                swal("Enter permanent Pincode");

                //e.preventDefault();
                return false;

            }
            else if ($("#Add_ddlState").val() == '') {
                swal("Enter permanent State");

                //e.preventDefault();
                return false;

            }

            else if ($("#Add_ddlCountry").val() == '') {
                swal("Enter permanent Country");

                //e.preventDefault();
                return false;

            }
            else if ($("#ResidenceId").val() == '') {
                swal("Select Residence");

                //e.preventDefault();
                return false;

            }

            else if ($("#ResidenceDocument").val() == '') {
                swal("Select Residence Document");

                //e.preventDefault();
                return false;

            }
            else if ($("#ResidentialStatusid").val() == '') {
                swal("Enter Residential Status");

                //e.preventDefault();
                return false;

            }
            else if ($("#residenceYNId").val() == '') {
                swal("Select Residence Y/N");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_permAddress1").val() == '') {
                swal("Enter Correspondence Address Line 1");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_permAddress2").val() == '') {
                swal("Enter Correspondence Address Line 2");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_permAddress3").val() == '') {
                swal("Enter Correspondence Address Line 3");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_permCity").val() == '') {
                swal("Enter Correspondence City");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_PERM_pincode").val() == '') {
                swal("Enter Correspondence City");

                //e.preventDefault();
                return false;

            }
            else if ($("#PERM_ddlState").val() == '') {
                swal("Enter Correspondence State");

                //e.preventDefault();
                return false;

            }
            else if ($("#PERM_ddlCountry").val() == '') {
                swal("Select Correspondence Country");

                //e.preventDefault();
                return false;

            }
            else if ($("#digi_pan").val() == '') {
                swal("Enter Pan No");

                //e.preventDefault();
                return false;

            }
            else if ($("#PhoneBanking").val() == '') {
                swal("Select PhoneBanking");

                //e.preventDefault();
                return false;

            }
            else if ($("#AMLRating").val() == '') {
                swal("Select AMLRating");

                //e.preventDefault();
                return false;

            }
            swal("Capture photo");
            //event.preventDefault();
            return false;
        }

        if ($("#SubTitleId").val() == '') {
            swal("Enter Sub-Title");
            //event.preventDefault();
            return false;
        }
        else if ($("#digi_firstname").val() == '') {
            swal("Enter Firstname");
            //event.preventDefault();
            return false;
        }

        else if ($("#digi_middlename").val() == '') {
            swal("Enter Middlename");
            //event.preventDefault();
            return false;
        }

        else if ($("#digi_lastname").val() == '') {
            swal("Enter Lastname");
            //event.preventDefault();
            return false;
        }

        else if ($("#digi_DOB").val() == '') {
            swal("Enter DOB");
            //event.preventDefault();
            return false;
        }

        else if ($("#digi_Gender").val() == '') {
            swal("Select Gender");
            //event.preventDefault();
            return false;
        }
        else if ($("#CasteId").val() == '') {
            swal("Select Caste");
            //event.preventDefault();
            return false;
        }
        else if ($("#ReligionId").val() == '') {
            swal("Select Region");
            //event.preventDefault();
            return false;
        }
        else if ($("#maritalstatusId").val() == '') {
            swal("Select Marital Status");
            //event.preventDefault();
            return false;
        }
        else if ($("#emailId").val() == '') {
            swal("Enter EmailID");
            //e.preventDefault();
            return false;
        }
        else if ($("#mobileNo").val() == '') {
            swal("Enter MobileNo");
            //e.preventDefault();
            return false;
        }
        else if ($("#MobileDetail").val() == '') {
            swal("Enter Mobile Details");
            //event.preventDefault();
            return false;
        }
        else if ($("#digi_address1").val() == '') {
            swal("Enter Permanent Address Line 1");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_address2").val() == '') {
            swal("Enter Permanent Address Line 2");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_address3").val() == '') {
            swal("Enter Permanent Address Line 3");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_city").val() == '') {
            swal("Enter Permanent City");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_pincode").val() == '') {
            swal("Enter Permanent Pincode");

            //e.preventDefault();
            return false;

        }
        else if ($("#Add_ddlState").val() == '') {
            swal("Enter Permanent State");

            //e.preventDefault();
            return false;

        }

        else if ($("#Add_ddlCountry").val() == '') {
            swal("Enter Permanent Country");

            //e.preventDefault();
            return false;

        }
        else if ($("#ResidenceId").val() == '') {
            swal("Select Residence");

            //e.preventDefault();
            return false;

        }

        else if ($("#ResidenceDocument").val() == '') {
            swal("Select Residence Document");

            //e.preventDefault();
            return false;

        }
        else if ($("#ResidentialStatusid").val() == '') {
            swal("Enter Residential Status");

            //e.preventDefault();
            return false;

        }
        else if ($("#residenceYNId").val() == '') {
            swal("Select Residence Y/N");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_permAddress1").val() == '') {
            swal("Enter Correspondence Address Line 1");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_permAddress2").val() == '') {
            swal("Enter Correspondence Address Line 2");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_permAddress3").val() == '') {
            swal("Enter Correspondence Address Line 3");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_permCity").val() == '') {
            swal("Enter Correspondence City");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_PERM_pincode").val() == '') {
            swal("Enter Correspondence City");

            //e.preventDefault();
            return false;

        }
        else if ($("#PERM_ddlState").val() == '') {
            swal("Enter Correspondence State");

            //e.preventDefault();
            return false;

        }
        else if ($("#PERM_ddlCountry").val() == '') {
            swal("Select Correspondence Country");

            //e.preventDefault();
            return false;

        }
        else if ($("#digi_pan").val() == '') {
            swal("Enter Pan No");

            //e.preventDefault();
            return false;

        }
        else if ($("#PhoneBanking").val() == '') {
            swal("Select PhoneBanking");

            //e.preventDefault();
            return false;

        }
        else if ($("#AMLRating").val() == '') {
            swal("Select AMLRating");

            //e.preventDefault();
            return false;

        }

        else if (i = 0) {
            swal("Enter Mandatory Details");
            //e.preventDefault();
            return false;
        }




        //$("#DigiCerti1").hide();
        //$("#DIGIbag1").hide();
        $('#digiphotoid').val(b);
        $('#latitudelongitude_id').val(resultvalues);
        $('#Prediction_id').val(resultvalues1);
        $("html, body").animate({
            scrollTop: $(
                'html, body').get(0).scrollHeight
        }, 2000);
        // $(window).scrollTop($('#orgresubmit').position().top);


        url = '/KYCQuickEnroll/CheckUser?digi_pan=' + $('#digi_pan').val();
        $.ajax({
            url: url,
            type: 'Post',

            success: function (result) {
                debugger;
                if (result == "Submitted Successfully") {



                    swal("Submitted Successfully")
                    $("#DigiForm").submit();

                }
                else {
                    alert(result);
                    window.location.href = '@Url.Action("CustomerQuickEnrollment", "KYCQuickEnroll")'
                }

            }
        });
    }
}
