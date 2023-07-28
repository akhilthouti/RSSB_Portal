$(document).ready(function () {
    $("#initialDepositChequeid").change(function () {
        if (this.checked) {
            $("#initialDepositChequeNo").show();
            $("#initialDepositCheque").show();
            $("#IDAmount").hide();
            $("#InitialDepositAmount").hide();
            $("#initialDepositCashid").prop("checked", false);
        }
    });
    $("#initialDepositCashid").change(function () {
        if (this.checked) {
            $("#IDAmount").show();
            $("#InitialDepositAmount").show();
            $("#initialDepositChequeNo").hide();
            $("#initialDepositCheque").hide();
            $("#initialDepositChequeid").prop("checked", false);
        }
    });
});
$(document).ready(function () {
    $("#initialDepositChequeid1").change(function () {
        if (this.checked) {
            $("#initialDepositChequeNo1").show();
            $("#initialDepositCheque1").show();
            $("#IDAmount1").hide();
            $("#InitialDepositAmount1").hide();
            $("#initialDepositCashid1").prop("checked", false);
        }
    });
    $("#initialDepositCashid1").change(function () {
        if (this.checked) {
            $("#IDAmount1").show();
            $("#InitialDepositAmount1").show();
            $("#initialDepositChequeNo1").hide();
            $("#initialDepositCheque1").hide();
            $("#initialDepositChequeid1").prop("checked", false);
        }
    });
});
$(document).ready(function () {
    $("#initialDepositChequeid2").change(function () {
        if (this.checked) {
            $("#initialDepositChequeNo2").show();
            $("#initialDepositCheque2").show();
            $("#IDAmount2").hide();
            $("#InitialDepositAmount2").hide();
            $("#initialDepositCashid2").prop("checked", false);
        }
    });
    $("#initialDepositCashid2").change(function () {
        if (this.checked) {
            $("#IDAmount2").show();
            $("#InitialDepositAmount2").show();
            $("#initialDepositChequeNo2").hide();
            $("#initialDepositCheque2").hide();
            $("#initialDepositChequeid2").prop("checked", false);
        }
    });
});
$(document).ready(function () {
    $("#initialDepositChequeid3").change(function () {
        if (this.checked) {
            $("#initialDepositChequeNo3").show();
            $("#initialDepositCheque3").show();
            $("#IDAmount3").hide();
            $("#InitialDepositAmount3").hide();
            $("#initialDepositCashid3").prop("checked", false);
        }
    });
    $("#initialDepositCashid3").change(function () {
        if (this.checked) {
            $("#IDAmount3").show();
            $("#InitialDepositAmount3").show();
            $("#initialDepositChequeNo3").hide();
            $("#initialDepositCheque3").hide();
            $("#initialDepositChequeid3").prop("checked", false);
        }
    });
});
$(document).ready(function () {
    $("#initialDepositChequeid4").change(function () {
        if (this.checked) {
            $("#initialDepositChequeNo4").show();
            $("#initialDepositCheque4").show();
            $("#IDAmount4").hide();
            $("#InitialDepositAmount4").hide();
            $("#initialDepositCashid4").prop("checked", false);
        }
    });
    $("#initialDepositCashid4").change(function () {
        if (this.checked) {
            $("#IDAmount4").show();
            $("#InitialDepositAmount4").show();
            $("#initialDepositChequeNo4").hide();
            $("#initialDepositCheque4").hide();
            $("#initialDepositChequeid4").prop("checked", false);
        }
    });
});
$(document).ready(function () {
    $("#JointYes, #JointNo").change(function () {
        $("#JointYes, #JointNo").not(this).prop("checked", false);
    });
});
$(document).ready(function () {
    $('#dwnPDFAccount').prop('disabled', true);
});
$('#FormSave').click(function () {
    $('#dwnPDFAccount').prop('disabled', false);
});
function pdfAccount(e) {
    debugger;
    $("html,body").animate({ scrollTop: 0 }, 600);
    $("html,body").animate({ scrollTop: 0 }, 600);
    var url = window.location.protocol + "//" + window.location.host + "/CustomerAccount/DownloadAccpdf";
    window.open(url, '_blank');
}
function onNomineeClick(check) {
    debugger;
    if (check == 1) {
        debugger;
        $('#NomineeAdd').show();
        $('#NomineeDetails').show();
        $('#NomineeYes').attr("checked", true);
        $('#NomineeNo').attr("checked", false);
    }
    else if (check == 2) {
        $('#NomineeAdd').show();
        $('#NomineeDetails').show();
        $('#NomineeYes').attr("checked", true);
        $('#NomineeNo').attr("checked", true);
        $('#NomineeYes').removeAttr(true);
    }
}

function DataAccountTypeSave() {
    debugger;
    if ($('#regularType').is(":checked") == false && $('#CosmoPremiumType').is(":checked") == false && $('#CosmoSalaryType').is(":checked") == false && $('#CosmoRoyaleType').is(":checked") == false && $('#CosmoPremiumPlusType').is(":checked") == false && $('#BSBDAtype').is(":checked") == false && $('#CosmoYouthType').is(":checked") == false && $('#CosmoPremiumSalaryType').is(":checked") == false && $('#otherType').is(":checked") == false) {
        swal("select Account type");
    }
    else {
        var ChkregularType = false;
        var ChkCosmoPremiumType = false;
        var ChkCosmoSalaryType = false;
        var ChkCosmoRoyaleType = false;
        var ChkCosmoPremiumPlusType = false;
        var ChkBSBDAtype = false;
        var ChkCosmoYouthType = false;
        var ChkCosmoPremiumSalaryType = false;
        var ChkotherType = false;
        if ($("#regularType").prop('checked') == true) {
            ChkregularType = true;
        }
        if ($("#CosmoPremiumType").prop('checked') == true) {
            ChkCosmoPremiumType = true;
        }
        if ($("#CosmoSalaryType").prop('checked') == true) {
            ChkCosmoSalaryType = true;
        }
        if ($("#CosmoRoyaleType").prop('checked') == true) {
            ChkCosmoRoyaleType = true;
        }
        if ($("#CosmoPremiumPlusType").prop('checked') == true) {
            ChkCosmoPremiumPlusType = true;
        }
        if ($("#BSBDAtype").prop('checked') == true) {
            ChkBSBDAtype = true;
        }
        if ($("#CosmoYouthType").prop('checked') == true) {
            ChkCosmoYouthType = true;
        }
        if ($("#CosmoPremiumSalaryType").prop('checked') == true) {
            ChkCosmoPremiumSalaryType = true;
        }
        if ($("#otherType").prop('checked') == true) {
            ChkotherType = true;
        }
        var cls =
        {
            "Regular": ChkregularType,
            "CosmoPremium": ChkCosmoPremiumType,
            "CosmoSalary": ChkCosmoSalaryType,
            "CosmoRoyale": ChkCosmoRoyaleType,
            "CosmoPremiumPlus": ChkCosmoPremiumPlusType,
            "BSBDA": ChkBSBDAtype,
            "CosmoYouth": ChkCosmoYouthType,
            "CosmoPremiumSalary": ChkCosmoPremiumSalaryType,
            "other": ChkotherType
        }
        $.post("/CustomerAccount/CustomerAccForm?check=" + 'SaveAccount', cls).done(function (result) {
            $("#docIdDiv").html("");
            $('#overlay_disabl').hide();
            if (result == 'Session Expired') {
                window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
            }
            else if (result == "Success") {
                swal("Data saved successfully");
                document.getElementById("SavePersonal2").classList.add("bg-green");
                $('#SavePersonal3').prop('disabled', false);
            }
            else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
            }
            else {
                swal("Try Again");
            }
        });
    }
}
function DataEbankSave() {
    debugger;
    if ($('#CosmoVisaDebitCardType').is(":checked") == false && $('#CosmoRupayCardType').is(":checked") == false && $('#UPIType').is(":checked") == false && $('#InternetBankingType').is(":checked") == false && $('#IMPSType').is(":checked") == false) {
        swal("select ebank services");
    }
    else {
        var ChkCosmoVisaDebitCardType = false;
        var ChkCosmoRupayCardType = false;
        var ChkUPIType = false;
        var ChkInternetBankingType = false;
        var ChkIMPSType = false;
        if ($("#CosmoVisaDebitCardType").prop('checked') == true) {
            ChkCosmoVisaDebitCardType = true;
        }
        if ($("#CosmoRupayCardType").prop('checked') == true) {
            ChkCosmoRupayCardType = true;
        }
        if ($("#UPIType").prop('checked') == true) {
            ChkUPIType = true;
        }
        if ($("#InternetBankingType").prop('checked') == true) {
            ChkInternetBankingType = true;
        }
        if ($("#IMPSType").prop('checked') == true) {
            ChkIMPSType = true;
        }
        var cls =
        {
            "CosmoRupayCard": ChkCosmoRupayCardType,
            "CosmoVisaDebitCard": ChkCosmoVisaDebitCardType,
            "UPIType": ChkUPIType,
            "InternetBankingType": ChkInternetBankingType,
            "IMPSType": ChkIMPSType
        }

        $.post("/CustomerAccount/CustomerAccForm?check=" + 'SaveEbank' + '&UPIType=' + ChkUPIType + '&InternetBankingType=' + ChkInternetBankingType + '&IMPSType=' + ChkIMPSType, cls).done(function (result) {
            $("#docIdDiv").html("");
            $('#overlay_disabl').hide();
            if (result == 'Session Expired') {
                window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
            }
            else if (result == "Success") {
                swal("Data saved successfully");
                document.getElementById("SavePersonal3").classList.add("bg-green");
                $('#SavePersonal4').prop('disabled', false);
            }
            else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
            }
            else {
                swal("Try Again");
            }
        });
    }
}
function DataFacilitySave() {
    var ChkCarLoanType = false;
    var ChkConsumerLoanType = false;
    var ChkHomeLoanType = false;
    var ChkBusinessLoanType = false;
    var ChkEducationLoanType = false;
    var ChkNewsPaperType = false;
    var ChkStaffType = false;
    var ChkRelativeFriendType = false;
    var ChkAdvertiseType = false;
    var ChkOtherCreditFacilityType = false;
    if ($("#CarLoanType").prop('checked') == true) {
        ChkCarLoanType = true;
    }
    if ($("#ConsumerLoanType").prop('checked') == true) {
        ChkConsumerLoanType = true;
    }
    if ($("#HomeLoanType").prop('checked') == true) {
        ChkHomeLoanType = true;
    }
    if ($("#BusinessLoanType").prop('checked') == true) {
        ChkBusinessLoanType = true;
    }
    if ($("#EducationLoanType").prop('checked') == true) {
        ChkEducationLoanType = true;
    }
    if ($("#NewsPaperType").prop('checked') == true) {
        ChkNewsPaperType = true;
    }
    if ($("#StaffType").prop('checked') == true) {
        ChkStaffType = true;
    }
    if ($("#RelativeFriendType").prop('checked') == true) {
        ChkRelativeFriendType = true;
    }
    if ($("#AdvertiseType").prop('checked') == true) {
        ChkAdvertiseType = true;
    }
    if ($("#OtherCreditFacilityType").prop('checked') == true) {
        ChkOtherCreditFacilityType = true;
    }
    var cls =
    {
        "CarLoan": ChkCarLoanType,
        "ConsumerLoan": ChkConsumerLoanType,
        "HomeLoan": ChkHomeLoanType,
        "BusinessLoan": ChkBusinessLoanType,
        "EducationLoan": ChkEducationLoanType,
        "NewsPaper": ChkNewsPaperType,
        "Staff": ChkStaffType,
        "RelativeFriend": ChkRelativeFriendType,
        "Advertise": ChkAdvertiseType,
        "OtherCreditFacility": ChkOtherCreditFacilityType
    }
    $.post("/CustomerAccount/CustomerAccForm?check=" + 'SaveCredit', cls).done(function (result) {
        $("#docIdDiv").html("");
        $('#overlay_disabl').hide();
        if (result == 'Session Expired') {
            window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
        }
        else if (result == "Success") {
            swal("Data saved successfully");
            document.getElementById("SavePersonal4").classList.add("bg-green");
            $('#SaveNominee').prop('disabled', false);
        }
        else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
            window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
        }
        else {
            swal("Try Again");
        }
    });
}
function DataNomineeSave() {
    debugger;
    var ChkNomineeYes = false;
    var ChkNomineeNo = false;
    if ($("#NomineeYes").prop('checked') == true) {
        ChkNomineeYes = true;
    }
    if ($("#NomineeNo").prop('checked') == true) {
        ChkNomineeNo = true;
    }
    if ($("#NomineePrefixid").val() == '') {
        swal("Please Enter Nominee Prefix Name");
        return false;
    }
    if ($("#NomineeFNameid").val() == '') {
        swal("Please Enter Nominee First Name");
        return false;
    }
    if ($("#NomineeMNameid").val() == '') {
        swal("Please Enter Nominee Middle Name");
        return false;
    }
    if ($("#NomineeLNameid").val() == '') {
        swal("Please Enter Nominee Last Name");
        return false;
    }
    if ($("#NomineeAge").val() == '') {
        swal("Please Enter Nominee DOB");
        return false;
    }
    if ($("#NomineeRelation").val() == '') {
        swal("Please Enter Relation");
        return false;
    }
    if ($("#Nominee_ADDRESS_1id").val() == '') {
        swal("Please Enter Address1");
        return false;
    }
    if ($("#Nominee_ADDRESS_2id").val() == '') {
        swal("Please Enter Address2");
        return false;
    }
    if ($("#Nominee_ADDRESS_3id").val() == '') {
        swal("Please Enter Address3");
        return false;
    }
    if ($("#Nominee_CITYid").val() == '') {
        swal("Please Enter City");
        return false;
    }
    if ($("#Nominee_STATEid").val() == '') {
        swal("Please Select State");
        return false;
    }
    if ($("#Nominee_Pincodeid").val() == '') {
        swal("Please Enter Pincode");
        return false;
    }
    if ($("#Nominee_COUNTRYid").val() == '') {
        swal("Please Select Country");
        return false;
    }
    if ($("#Nominee_Pincodeid").val().length < '6') {
        swal("Pincode shuold be 6 digit");
        return false;
    }
    var cls =
    {
        "NomineePrefix": $("#NomineePrefixid").val(),
        "NomineeFName": $("#NomineeFNameid").val(),
        "NomineeMName": $("#NomineeMNameid").val(),
        "NomineeLName": $("#NomineeLNameid").val(),
        "Nominee_ADDRESS_1": $("#Nominee_ADDRESS_1id").val(),
        "Nominee_ADDRESS_2": $("#Nominee_ADDRESS_2id").val(),
        "Nominee_ADDRESS_3": $("#Nominee_ADDRESS_3id").val(),
        "Nominee_CITY": $("#Nominee_CITYid").val(),
        "Nominee_Pincode": $("#Nominee_Pincodeid").val(),
        "Nominee_STATE": $("#Nominee_STATEid").val(),
        "Nominee_COUNTRY": $("#Nominee_COUNTRYid").val(),
        "NomineeForAccountYes": ChkNomineeYes,
        "NomineeForAccountNo": ChkNomineeNo,
        "NomineeAge": $("#NomineeAge").val(),
        "NomineeRelation": $("#NomineeRelation").val()
    }
    $.post("/CustomerAccount/CustomerAccForm?check=" + 'SaveNominee', cls).done(function (result) {
        $("#docIdDiv").html("");
        $('#overlay_disabl').hide();
        if (result == 'Session Expired') {
            window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
        }
        else if (result == "Success") {
            swal("Data saved successfully");
            $("#SaveNominee").addClass("bg-green");
            $('#FormSave').prop('disabled', false);
        }
        else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
            window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
        }
        else {
            swal("Try Again");
        }
    });
}
function DataFormSave() {
    debugger;
    var cls =
    {
        "ABCcell_OfficerName": $("#ABCcell_OfficerNameid").val(),
        "ABCcell_TicketNo": $("#ABCcell_TicketNoid").val()
    }
    $.post("/CustomerAccount/CustomerAccForm?check=" + 'FormSave', cls).done(function (result) {
        $("#docIdDiv").html("");
        $('#overlay_disabl').hide();
        if (result == 'Session Expired') {
            window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
        }
        else if (result == "Success") {
            swal({
                title: "Your KYC Has been Completed Successfully",
                type: 'success',
                showCancelButton: false,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Okay',
                allowEscapeKey: false,
                allowOutsideClick: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        document.getElementById("FormSave").style.backgroundColor = "green";
                        document.getElementById("Agentlitab5").classList.add("active_digi_caf_doc_ipv_sum_sav");
                        $("#Agentlitab5").addClass("active_digi_caf_doc_ipv_sum_sav");
                        $("#FormSave").addClass("green");
                    }
                    else {
                        $('#dwnPDFAccount').prop('disabled', true);

                    }
                }
            );

        }
        else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
            window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
        }
        else {
            swal("Try Again");
        }
    });
}
function onJointClick(check) {
    debugger;
    if (check == 1) {
        debugger;
        $('#FirstHolder').show();
        $('#Add2ndJointBtn').show();
        $('#JointYes').prop("checked", true);
        $('#JointNo').prop("checked", false);
        $('#JointNo').removeAttr(true);
        $('#JointYes').prop("checked", false);
        $('#JointNo').prop("checked", true);
    }
    else if (check == 2) {
        $('#FirstHolder').hide();
        $('#Add2ndJointBtn').hide();
        $('#NomineeYes').attr("checked", true);
        $('#NomineeNo').attr("checked", true);
        $('#NomineeYes').removeAttr(true);
    }
}
function DataAdd2ndJoint() {
    $('#Joint2nd').show();
    $('#SecondHolder').show();

    $('#Add2ndJointBtn').hide();
    $('#Add3rdJointBtn').show();
}
function DataAdd3rdJoint() {
    $('#Joint3rd').show();
    $('#ThirdHolder').show();
    $('#Add2ndJointBtn').hide();
    $('#Add3rdJointBtn').hide();
    $('#Add4thJointBtn').show();
}
function DataAdd4thJoint() {
    $('#FourthHolder').show();
    $('#Add3rdJointBtn').hide();
    $('#Add4thJointBtn').hide();
}
function MainAccData1() {
    debugger;
    var url = '/CustomerAccount/NomineecustData?IDtype=' + $('#MainACCDetails1').val() + '&IDnumber=' + $('#MainSearchid1').val();
    $.ajax({
        url: url,
        type: "GET",
        success: function (result) {
            debugger;
            if (result != 'Not Found') {
                $('#NomineeFNameid').val(result.split('#')[0]);
                $('#NomineeMNameid').val(result.split('#')[1]);
                $('#NomineeLNameid').val(result.split('#')[2]);
                $('#MainapplicantCustIDid1').val(result.split('#')[3]);
                swal({
                    title: "Are you sure?You want to add Main Holder",
                    text: result.split('#')[0] + " " + result.split('#')[1] + " " + result.split('#')[2],//"You will not be able to recover this imaginary file!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, I am sure!',
                    cancelButtonText: "No, cancel it!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            if ($('#MainapplicantCustIDid1').val() != ($('#firstapplicantCustIDid').val() || $('#SecondapplicantCustIDid').val() || $('#ThirdapplicantCustIDid').val())) {
                                var cls =
                                {
                                    "NomineeFName": $("#NomineeFNameid").val(),
                                    "NomineeMName": $("#NomineeMNameid").val(),
                                    "NomineeLName": $("#NomineeLNameid").val(),
                                    "MainHolderapplicantCustID": $('#MainapplicantCustIDid1').val(),
                                    "MainHolderVerifyCustData": $('#MainACCDetails1').val(),
                                    "MainHolderSearchType": $('#MainSearchid1').val()
                                }
                                $.post("/CustomerAccount/CustomerAccForm?check=" + 'SavePersonal' + '&JointID=' + 0, cls).done(function (result) {
                                    $("#docIdDiv").html("");
                                    $('#overlay_disabl').hide();
                                    if (result == 'Session Expired') {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else if (result == "Success") {
                                        swal("Data saved successfully");
                                    }
                                    else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else {
                                        swal("Try Again");
                                    }
                                });
                            }
                            else {
                                swal("Already exist holder", " :)", "error");
                                $('#NomineeFNameid').val("");
                                $('#NomineeMNameid').val("");
                                $('#NomineeLNameid').val("");
                                $('#MainapplicantCustIDid1').val("");
                                $('#MainACCDetails1').val("");
                                $('#MainSearchid1').val("");
                            }
                        }
                        else {
                            swal("Cancelled", " :)", "error");
                            $('#NomineeFNameid').val("");
                            $('#NomineeMNameid').val("");
                            $('#NomineeLNameid').val("");
                            $('#MainapplicantCustIDid1').val("");
                            $('#MainACCDetails1').val("");
                            $('#MainSearchid1').val("");
                        }
                    });
            }
            else {
                swal(result);
            }
        }
    })
}
function MainAccDataReset1() {
    $('#NomineeFNameid').val("");
    $('#NomineeMNameid').val("");
    $('#NomineeLNameid').val("");
    $('#MainapplicantCustIDid1').val("");
    $('#MainACCDetails1').val("");
    $('#MainSearchid1').val("");
}
function MainAccData() {
    debugger;
    var url = '/CustomerAccount/custData?IDtype=' + $('#MainACCDetails').val() + '&IDnumber=' + $('#MainSearchid').val();
    $.ajax({
        url: url,
        type: "GET",
        success: function (result) {
            debugger;
            if (result != 'Not Found') {
                $('#MainapplicantFNameid').val(result.split('#')[0]);
                $('#MainapplicantMNameid').val(result.split('#')[1]);
                $('#MainapplicantLNameid').val(result.split('#')[2]);
                $('#MainapplicantCustIDid').val(result.split('#')[3]);
                swal({
                    title: "Are you sure?You want to add Main Holder",
                    text: result.split('#')[0] + " " + result.split('#')[1] + " " + result.split('#')[2],//"You will not be able to recover this imaginary file!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, I am sure!',
                    cancelButtonText: "No, cancel it!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {

                        if (isConfirm) {
                            if ($('#MainapplicantCustIDid').val() != ($('#firstapplicantCustIDid').val() || $('#SecondapplicantCustIDid').val() || $('#ThirdapplicantCustIDid').val())) {
                                var cls =
                                {
                                    "MainHolderapplicantFName": $('#MainapplicantFNameid').val(),
                                    "MainHolderapplicantMName": $('#MainapplicantMNameid').val(),
                                    "MainHolderapplicantLName": $('#MainapplicantLNameid').val(),
                                    "MainHolderapplicantCustID": $('#MainapplicantCustIDid').val(),
                                    "MainHolderVerifyCustData": $('#MainACCDetails').val(),
                                    "MainHolderSearchType": $('#MainSearchid').val()
                                }
                                $.post("/CustomerAccount/CustomerAccForm?check=" + 'SavePersonal' + '&JointID=' + 0, cls).done(function (result) {
                                    $("#docIdDiv").html("");
                                    $('#overlay_disabl').hide();
                                    if (result == 'Session Expired') {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else if (result == "Success") {
                                        swal("Data saved successfully");
                                    }
                                    else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else {
                                        swal("Try Again");
                                    }
                                });
                            }
                            else {
                                swal("Already exist holder", " :)", "error");
                                $('#MainapplicantFNameid').val("");
                                $('#MainapplicantMNameid').val("");
                                $('#MainapplicantLNameid').val("");
                                $('#MainapplicantCustIDid').val("");
                                $('#MainACCDetails').val("");
                                $('#MainSearchid').val("");
                            }
                        }
                        else {
                            swal("Cancelled", " :)", "error");
                            $('#MainapplicantFNameid').val("");
                            $('#MainapplicantMNameid').val("");
                            $('#MainapplicantLNameid').val("");
                            $('#MainapplicantCustIDid').val("");
                            $('#MainACCDetails').val("");
                            $('#MainSearchid').val("");
                        }
                    });
            }
            else {
                swal(result);
            }
        }
    })
}
function MainAccDataReset() {
    $('#MainapplicantFNameid').val("");
    $('#MainapplicantMNameid').val("");
    $('#MainapplicantLNameid').val("");
    $('#MainapplicantCustIDid').val("");
    $('#MainACCDetails').val("");
    $('#MainSearchid').val("");
}
function FirstHolderAccData() {
    debugger;
    var url = '/CustomerAccount/custData?IDtype=' + $('#FirstACCDetails').val() + '&IDnumber=' + $('#FirstSearchid').val();
    $.ajax({
        url: url,
        type: "GET",
        success: function (result) {
            debugger;
            if (result != 'Not Found') {
                $('#firstapplicantFNameid1').val(result.split('#')[0]);
                $('#firstapplicantMNameid1').val(result.split('#')[1]);
                $('#firstapplicantLNameid1').val(result.split('#')[2]);
                $('#firstapplicantCustIDid1').val(result.split('#')[3]);
                swal({
                    text: result.split('#')[0] + " " + result.split('#')[1] + " " + result.split('#')[2],//"You will not be able to recover this imaginary file!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, I am sure!',
                    cancelButtonText: "No, cancel it!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            if ($('#firstapplicantCustIDid1').val() != ($('#firstapplicantCustIDid').val() || $('#SecondapplicantCustIDid').val() || $('#ThirdapplicantCustIDid').val())) {
                                var cls =
                                {
                                    "firstapplicantFNAME1": $('#firstapplicantFNameid1').val(result.split('#')[0]),
                                    "firstapplicantMNAME1": $('#firstapplicantMNameid1').val(result.split('#')[1]),
                                    "firstapplicantLNAME1": $('#firstapplicantLNameid1').val(result.split('#')[2]),
                                    "firstapplicantCustID1": $('#firstapplicantCustIDid1').val(result.split('#')[3]),
                                    "FirstHolderVerifyCustData1": $('#FirstACCDetails').val(),
                                    "FirstHolderSearchType1": $('#FirstSearchid').val()
                                }
                                $.post("/CustomerAccount/CustomerAccForm?check=" + 'joint1', cls).done(function (result) {
                                    $("#docIdDiv").html("");
                                    $('#overlay_disabl').hide();
                                    if (result == 'Session Expired') {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else if (result == "Success") {
                                        swal("Data saved successfully");
                                    }
                                    else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else {
                                        swal("Try Again");
                                    }
                                });
                            }
                            else {
                                swal("Already exist holder", " :)", "error");
                                $('#firstapplicantFNameid1').val("");
                                $('#firstapplicantMNameid1').val("");
                                $('#firstapplicantLNameid1').val("");
                                $('#firstapplicantCustIDid1').val("");
                                $('#FirstACCDetails').val("");
                                $('#FirstSearchid').val("");
                            }
                        }
                        else {
                            swal("Cancelled", " :)", "error");
                            $('#firstapplicantFNameid1').val("");
                            $('#firstapplicantMNameid1').val("");
                            $('#firstapplicantLNameid1').val("");
                            $('#firstapplicantCustIDid1').val("");
                            $('#FirstACCDetails').val("");
                            $('#FirstSearchid').val("");
                        }
                    });
            }
            else {
                swal(result);
            }
        }
    })
}
function FirstHolderAccDataReset() {
    $('#firstapplicantFNameid1').val("");
    $('#firstapplicantMNameid1').val("");
    $('#firstapplicantLNameid1').val("");
    $('#firstapplicantCustIDid1').val("");
    $('#FirstACCDetails').val("");
    $('#FirstSearchid').val("");
}
function SecondHolderAccData() {
    debugger;
    var url = '/CustomerAccount/custData?IDtype=' + $('#SecondACCDetails').val() + '&IDnumber=' + $('#SecondSearchid').val();
    $.ajax({
        url: url,
        type: "GET",
        success: function (result) {
            debugger;
            if (result != 'Not Found') {
                $('#SecondapplicantFNameid2').val(result.split('#')[0]);
                $('#SecondapplicantMNameid2').val(result.split('#')[1]);
                $('#SecondapplicantLNameid2').val(result.split('#')[2]);
                $('#SecondapplicantCustIDid2').val(result.split('#')[3]);
                swal({
                    text: result.split('#')[0] + " " + result.split('#')[1] + " " + result.split('#')[2],//"You will not be able to recover this imaginary file!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, I am sure!',
                    cancelButtonText: "No, cancel it!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            if ($('#firstapplicantCustIDid1').val() != ($('#firstapplicantCustIDid').val() || $('#SecondapplicantCustIDid').val() || $('#ThirdapplicantCustIDid').val())) {
                                var cls =
                                {
                                    "SecondapplicantFNAME2": $('#SecondapplicantFNameid2').val(result.split('#')[0]),
                                    "SecondapplicantMNAME2": $('#SecondapplicantMNameid2').val(result.split('#')[1]),
                                    "SecondapplicantLNAME2": $('#SecondapplicantLNameid2').val(result.split('#')[2]),
                                    "SecondapplicantCustID2": $('#SecondapplicantCustIDid2').val(result.split('#')[3]),
                                    "SecondHolderVerifyCustData2": $('#SecondACCDetails').val(),
                                    "SecondHolderSearchType2": $('#SecondSearchid').val()
                                }
                                $.post("/CustomerAccount/CustomerAccForm?check=" + 'joint2', cls).done(function (result) {
                                    $("#docIdDiv").html("");
                                    $('#overlay_disabl').hide();
                                    if (result == 'Session Expired') {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else if (result == "Success") {
                                        swal("Data saved successfully");
                                    }
                                    else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else {
                                        swal("Try Again");
                                    }
                                });
                            }
                            else {
                                swal("Already exist holder", " :)", "error");
                                $('#SecondapplicantFNameid2').val("");
                                $('#SecondapplicantMNameid2').val("");
                                $('#SecondapplicantLNameid2').val("");
                                $('#SecondapplicantCustIDid2').val("");
                                $('#SecondACCDetails').val("");
                                $('#SecondSearchid').val("");
                            }
                        }
                        else {
                            swal("Cancelled", " :)", "error");
                            $('#SecondapplicantFNameid2').val("");
                            $('#SecondapplicantMNameid2').val("");
                            $('#SecondapplicantLNameid2').val("");
                            $('#SecondapplicantCustIDid2').val("");
                            $('#SecondACCDetails').val("");
                            $('#SecondSearchid').val("");
                        }
                    });
            }
            else {
                swal(result);
            }
        }
    })
}
function SecondHolderAccDataReset() {
    $('#SecondapplicantFNameid2').val("");
    $('#SecondapplicantMNameid2').val("");
    $('#SecondapplicantLNameid2').val("");
    $('#SecondapplicantCustIDid2').val("");
    $('#SecondACCDetails').val("");
    $('#SecondSearchid').val("");
}
function ThirdHolderAccData() {
    debugger;
    var url = '/CustomerAccount/custData?IDtype=' + $('#ThirdACCDetails').val() + '&IDnumber=' + $('#ThirdSearchid').val();
    $.ajax({
        url: url,
        type: "GET",
        success: function (result) {
            debugger;

            if (result != 'Not Found') {
                $('#ThirdapplicantFNameid3').val(result.split('#')[0]);
                $('#ThirdapplicantMNameid3').val(result.split('#')[1]);
                $('#ThirdapplicantLNameid3').val(result.split('#')[2]);
                $('#ThirdapplicantCustIDid3').val(result.split('#')[3]);
                swal({
                    text: result.split('#')[0] + " " + result.split('#')[1] + " " + result.split('#')[2],//"You will not be able to recover this imaginary file!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, I am sure!',
                    cancelButtonText: "No, cancel it!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            if ($('#ThirdapplicantCustIDid3').val() != ($('#firstapplicantCustIDid').val() || $('#SecondapplicantCustIDid').val() || $('#ThirdapplicantCustIDid').val())) {
                                var cls =
                                {
                                    "ThirdapplicantFNAME3": $('#ThirdapplicantFNameid3').val(result.split('#')[0]),
                                    "ThirdapplicantMNAME3": $('#ThirdapplicantMNameid3').val(result.split('#')[1]),
                                    "ThirdapplicantLNAME3": $('#ThirdapplicantLNameid3').val(result.split('#')[2]),
                                    "ThirdapplicantCustID3": $('#ThirdapplicantCustIDid3').val(result.split('#')[3]),
                                    "ThirdHolderVerifyCustData3": $('#ThirdACCDetails').val(),
                                    "ThirdHolderSearchType3": $('#ThirdSearchid').val()
                                }
                                $.post("/CustomerAccount/CustomerAccForm?check=" + 'joint3', cls).done(function (result) {
                                    $("#docIdDiv").html("");
                                    $('#overlay_disabl').hide();
                                    if (result == 'Session Expired') {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else if (result == "Success") {
                                        swal("Data saved successfully");
                                    }
                                    else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else {
                                        swal("Try Again");
                                    }
                                });
                            }
                            else {
                                swal("Already exist holder", " :)", "error");
                                $('#ThirdapplicantFNameid3').val("");
                                $('#ThirdapplicantMNameid3').val("");
                                $('#ThirdapplicantLNameid3').val("");
                                $('#ThirdapplicantCustIDid3').val("");
                                $('#ThirdACCDetails').val("");
                                $('#ThirdSearchid').val("");
                            }
                        }
                        else {
                            swal("Cancelled", " :)", "error");
                            $('#ThirdapplicantFNameid3').val("");
                            $('#ThirdapplicantMNameid3').val("");
                            $('#ThirdapplicantLNameid3').val("");
                            $('#ThirdapplicantCustIDid3').val("");
                            $('#ThirdACCDetails').val("");
                            $('#ThirdSearchid').val("");
                        }
                    });
            }
            else {
                swal(result);
            }
        }
    })
}
function ThirdHolderAccDataReset() {
    $('#ThirdapplicantFNameid3').val("");
    $('#ThirdapplicantMNameid3').val("");
    $('#ThirdapplicantLNameid3').val("");
    $('#ThirdapplicantCustIDid3').val("");
    $('#ThirdACCDetails').val("");
    $('#ThirdSearchid').val("");
}
function FourthHolderAccData() {
    debugger;
    var url = '/CustomerAccount/custData?IDtype=' + $('#FourthACCDetails').val() + '&IDnumber=' + $('#FourthSearchid').val();
    $.ajax({
        url: url,
        type: "GET",
        success: function (result) {
            debugger;
            if (result != 'Not Found') {
                $('#FourthapplicantFNameid4').val(result.split('#')[0]);
                $('#FourthapplicantMNameid4').val(result.split('#')[1]);
                $('#FourthapplicantLNameid4').val(result.split('#')[2]);
                $('#FourthapplicantCustIDid4').val(result.split('#')[3]);
                swal({
                    text: result.split('#')[0] + " " + result.split('#')[1] + " " + result.split('#')[2],//"You will not be able to recover this imaginary file!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, I am sure!',
                    cancelButtonText: "No, cancel it!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            if ($('#firstapplicantCustIDid1').val() != ($('#firstapplicantCustIDid').val() || $('#SecondapplicantCustIDid').val() || $('#ThirdapplicantCustIDid').val())) {
                                var cls =
                                {
                                    "FourthapplicantFNAME4": $('#FourthapplicantFNameid4').val(result.split('#')[0]),
                                    "FourthapplicantMNAME4": $('#FourthapplicantMNameid4').val(result.split('#')[1]),
                                    "FourthapplicantLNAME4": $('#FourthapplicantLNameid4').val(result.split('#')[2]),
                                    "FourthapplicantCustID4": $('#FourthapplicantCustIDid4').val(result.split('#')[3]),
                                    "FourthHolderVerifyCustData4": $('#FourthACCDetails').val(),
                                    "FourthHolderSearchType4": $('#FourthSearchid').val()
                                }
                                $.post("/CustomerAccount/CustomerAccForm?check=" + 'joint4', cls).done(function (result) {
                                    $("#docIdDiv").html("");
                                    $('#overlay_disabl').hide();
                                    if (result == 'Session Expired') {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else if (result == "Success") {
                                        swal("Data saved successfully");
                                    }
                                    else if (result.includes("window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn'")) {
                                        window.location.href = '/AccountLogin/AccountDetails?strLoginDetail=LogIn';
                                    }
                                    else {
                                        swal("Try Again");
                                    }
                                });
                            }
                            else {
                                swal("Already exist holder", " :)", "error");
                                $('#FourthapplicantFNameid4').val("");
                                $('#FourthapplicantMNameid4').val("");
                                $('#FourthapplicantLNameid4').val("");
                                $('#FourthapplicantCustIDid4').val("");
                                $('#FourthACCDetails').val("");
                                $('#FourthSearchid').val("");
                            }
                        }
                        else {
                            swal("Cancelled", " :)", "error");
                            $('#FourthapplicantFNameid4').val("");
                            $('#FourthapplicantMNameid4').val("");
                            $('#FourthapplicantLNameid4').val("");
                            $('#FourthapplicantCustIDid4').val("");
                            $('#FourthACCDetails').val("");
                            $('#FourthSearchid').val("");
                        }
                    });
            }
            else {
                swal(result);
            }
        }
    })
}
function FourthHolderAccDataReset() {
    $('#FourthapplicantFNameid4').val("");
    $('#FourthapplicantMNameid4').val("");
    $('#FourthapplicantLNameid4').val("");
    $('#FourthapplicantCustIDid4').val("");
    $('#FourthACCDetails').val("");
    $('#FourthSearchid').val("");
}
function numericOnly(id) {
    debugger;
    var inputtxt = $('#' + id).val();
    var numbers = /^[0-9]+$/;
    if (!numbers.test(inputtxt)) {
        alert('Only Numeric Numbers Can Be Enter ');
        $('#' + id).val('');
        document.form1.text1.focus();
        return true;
    }

}
function alphabetOnly(id) {
    debugger;
    var inputtxt = $('#' + id).val();
    var alpha = /^[A-Z-a-z]+$/;
    if (!alpha.test(inputtxt)) {
        alert('Only Alphabet Can Be Enter ');
        $('#' + id).val('');
        document.form1.text1.focus();
        return true;
    }
} 