
$(document).ready(function () {
    $("#CameraOpenViewAddrs").hide();
    $("#CameraOpenViewMainAddrs").hide();
    $("#select").hide();
    //###IDENTITY###//
    var DocMainType = "I";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails').html(result);

        },
        error: function (err) {
            debugger;
            // alert(err.statusText);
        }
    });
    debugger;
    DocMainType = "CA";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    DocMainType = "DL";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    DocMainType = "PP";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    DocMainType = "VI";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    debugger;
    DocMainType = "IAPVD";

    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });

    //###ADDRESS###//

    debugger;
    DocMainType = "CA";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails_Add').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    DocMainType = "DL";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails_Add').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    DocMainType = "PP";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails_Add').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    DocMainType = "VI";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails_Add').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });
    debugger;
    DocMainType = "CADL";

    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails_Add').html(result);

        },
        error: function (err) {
            debugger;
            //  alert(err.statusText);
        }
    });


    debugger;
    DocMainType = "SI";
    $.ajax({
        url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
        type: "GET",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: {},
        async: false,
        success: function (result) {
            debugger;

            $('#docdetails_Sign').html(result);

        },
        error: function (err) {
            debugger;
            // alert(err.statusText);
        }
    });
});

function deleteDoc(e, param) {
    debugger;
    swal({
        title: "Are you sure?",
        text: "Do you want to Delete Document?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            debugger;
            var id = e.id;

            // var idData = id.split('-');

            if (isConfirm) {
                $.ajax({
                    url: '/DocumentDetails/deleteDocuments?docId=' + id,
                    type: 'GET',
                    data: '',
                    cache: false,
                    success: function (result) {
                        debugger;
                        if (result == 1) {
                            swal('Document Deleted');
                            //###IDENTITY###//
                            //###IDENTITY###//
                            var DocMainType = "I";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    // alert(err.statusText);
                                }
                            });
                            debugger;
                            DocMainType = "CA";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            DocMainType = "DL";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            DocMainType = "PP";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            DocMainType = "VI";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            debugger;
                            DocMainType = "IAPVD";

                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });

                            //##address##//

                            //###ADDRESS###//

                            debugger;
                            DocMainType = "CA";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails_Add').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            DocMainType = "DL";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails_Add').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            DocMainType = "PP";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails_Add').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            DocMainType = "VI";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails_Add').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                            debugger;
                            DocMainType = "CADL";

                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;

                                    $('#docdetails_Add').html(result);

                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });

                            DocMainType = "SI";
                            $.ajax({
                                url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                                type: "GET",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: {},
                                async: false,
                                success: function (result) {
                                    debugger;
                                    $('#docdetails_Sign').html(result);
                                },
                                error: function (err) {
                                    debugger;
                                    //  alert(err.statusText);
                                }
                            });
                        }


                    }
                });
            }
            else {
                swal("Cancelled", " Cancelled your request  :)", "error");
            }
        })

};
function OpenPOIcamera() {
    debugger;
    $('#CameraPOIView').hide();
    //$("#DigiCertiDD").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
    //$("#DigiCertiDD").show();
    //$("#Digil1").css("display","block");
    $("#DIGIbagDD").show();
    $('#POICam').val('POICam');
    $('#CameraOpenViewMainAddrs').append($('#CameraOpenViewAddrs'));

    if ($("#ddl_idProof").val() == '') {
        $("#Digil1").css("display", "none");

        swal("Please Select Identity Document");
        return false();
    }
    $.ajax({
        // url: '/KYCImageCapture/Opencamera',
        url: '/DocumentDetails/DocOpenCamera1?DocType=' + "DOCPOI",
        type: 'GET',
        cache: false,
        success: function (result) {
            debugger;
            $("#Digil1").css("display", "none");
            $('#CameraPOIView').show();
            $('#CameraPOIView').html(result);
            $('#DigiCertiDD').hide();
            $('#DIGIbagDD').hide();
            // $("#Digil1").css("display", "none");

        }
    });
}

function OpenPOAcamera() {
    debugger;
    $("#Digil1").css("display", "none");
    //$("#DigiCertiDD").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
    //$("#DigiCertiDD").show();
    $("#DIGIbagDD").show();
    $('#POACam').val('POACam');
    if ($("#ddl_corrAdd").val() == '') {
        $("#Digil1").css("display", "none");

        swal("Please Select Valid Address Document");
        $('#DigiCertiDD').hide();
        $('#DIGIbagDD').hide();
        return false();
    }
    // $('#CameraOpenViewMainAddrs').append($('#CameraOpenViewAddrs'));
    $.ajax({
        url: '/DocumentDetails/DocOpenCamera2?DocType=' + "DOCPOA",
        type: 'GET',
        cache: false,
        success: function (result) {
            debugger;
            if (this.lastStream) {
                this.lastStream.getTracks().forEach(track => track.stop())
            }
            $('#CameraPOIView').empty();
            $('#CameraPOAView').show();
            $('#CameraPOAView').html(result);
            $('#DigiCertiDD').hide();
            $('#DIGIbagDD').hide();

        }
    });
}
function OpenSigncamera() {
    debugger;
    //$("#DigiCertiDD").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
    //$("#DigiCertiDD").show();
    $("#DIGIbagDD").show();
    $('#SignCam').val('SignCam');
    //$('#CameraOpenViewMainAddrs').append($('#CameraOpenViewAddrs'));
    $.ajax({
        url: '/DocumentDetails/DocOpenCamera3?DocType=' + "DOCCamSign",
        type: 'GET',
        cache: false,
        success: function (result) {
            debugger;
            $('#CameraSignView').show();
            $('#CameraSignView').html(result);
            $('#DigiCertiDD').hide();
            $('#DIGIbagDD').hide();

        }
    });
}
function deleteDocDetails1(e, param) {
    swal({
        title: "Are you sure?",
        text: "Do you want to Delete Document?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            var id = e.id;
            var idData = id.split('-');
            if (isConfirm) {
                $.ajax({
                    url: '/DocumentDetails/deleteDocument?docId=' + idData[0],
                    type: 'GET',
                    data: '',
                    cache: false,
                    success: function (result) {
                        debugger;
                        var splitRslt = result.split('-');
                        if (splitRslt[0] >= 0) {
                            swal("Delete", "Document Deleted Successfully");
                            $('#' + idData[0]).empty();
                            $('#' + idData[1]).prop('disabled', false);
                        }
                    }
                });
            }
            else {
                swal("Cancelled", " Cancell your request  :)", "error");
            }
        })
};
$(document).ready(function () {

    $("#fileInput").on("change", function () {

        //alert("Working");
        $("#Digil1").css("display", "block");
        $("body").attr('read-only', true);
        //$('#overlay_disabl').show();

        setTimeout(function () {
            //Code here
            hello();

        }, 1500);

    });
});
function hello() {
    var fileInput = document.getElementById('fileInput');
    var idproof = document.getElementById("ddl_idProof").value;

    if (idproof == "--Select--" || idproof == "") {
        swal("please Select Identity Document");
    }

    //Iterating through each files selected in fileInput
    var formdata = new FormData();
    for (i = 0; i < fileInput.files.length; i++) {

        var sfilename = fileInput.files[i].name;
        var sfilesize = fileInput.files[i].size;
        var POItype = $('#ddl_idProof option:selected').text();
        let srandomid = Math.random().toString(36).substring(7);

        formdata.append(sfilename, fileInput.files[i]);

        var markup = "<tr id='" + srandomid + "'><td>" + sfilename + "</td><td>" + sfilesize + "</td><td>" + POItype + "</td><td><a href='#' onclick='DeleteFile(\"" + srandomid + "\",\"" + sfilename +
            "\")'><span class='glyphicon glyphicon-remove red'></span></a></td><td><a href='#' onclick='OpenFile(\"" + srandomid + "\",\"" + sfilename +
            "\")'><span class='glyphicon glyphicon-remove red'></span></a></td></tr>";
    }
    $('#fileInput').val('');
    $('#overlay_disabl').hide();


    var idname = $('#ddl_idProof option:selected').text();
    var docvalue = $('#ddl_idProof').val();
    formdata.append('POIvalue', idname);
    formdata.append("ddl_idProof", docvalue);
    formdata.append("DocMainType", "DOCPOI");

    if ($("#ddl_idProof").val() == '') {
        $("#Digil1").css("display", "none");

        swal("Please Select Identity Document");
        return false();
    }
    $('#overlay_disabl').show();
    $.ajax({
        url: '/DocumentDetails/UploadFiles1',
        type: "POST",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: formdata,
        async: false,
        success: function (result) {
            debugger;
            $('#overlay_disabl').show();

            if (result.indexOf("CropPdf") > -1) {
                var splitRslt = result.split('-');
                var id = splitRslt[1];
                var DocMainType = "I";
                $.ajax({
                    url: '/DocumentDetails/CropPdfDocument',
                    type: 'Get',
                    data: {
                        custid: id,
                        DocMainType: DocMainType
                    },
                    success: function (result) {
                        debugger;
                        $('#Viewport_disable1').hide();
                        $('#Viewport_show').hide();
                        $('#edit_disabl').hide();
                        $("#l2Digi").hide();
                        $('#overlay_disabl').hide();


                        if (result == 'Document not available on DMS.Kindly scan document and upload manually.') {
                            swal(result);
                            $("#Digil1").css("display", "none");
                            //$("#l2Digi").hide();
                            $('#overlay_disabl').hide();
                        }
                        else
                            //swal("Ok");
                            $('#CropPdfForm').html(result);
                        $('#edit_disabl').hide();
                        //$("#l2Digi").hide();
                        $("#Digil1").css("display", "none");
                        $("body").attr('read-only', false);
                        $('#overlay_disabl').hide();
                    }
                })
            }
            else if (result != "") {
                $("#Digil1").css("display", "none");
                $("body").attr('read-only', false);
                swal({
                    title: result,
                    //text: "Delete Confirmation?",
                    type: "success",
                    showCancelButton: false,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                });
                //###IDENTITY###//
                var DocMainType = "I";
                $.ajax({
                    url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                    type: "GET",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: {},
                    async: false,
                    success: function (result) {
                        debugger;

                        $('#docdetails').html(result);

                    },
                    error: function (err) {
                        debugger;
                        // alert(err.statusText);
                    }
                });
                debugger;
                DocMainType = "CA";
                $.ajax({
                    url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                    type: "GET",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: {},
                    async: false,
                    success: function (result) {
                        debugger;

                        $('#docdetails').html(result);

                    },
                    error: function (err) {
                        debugger;
                        //  alert(err.statusText);
                    }
                });
                DocMainType = "DL";
                $.ajax({
                    url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                    type: "GET",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: {},
                    async: false,
                    success: function (result) {
                        debugger;

                        $('#docdetails').html(result);

                    },
                    error: function (err) {
                        debugger;
                        //  alert(err.statusText);
                    }
                });
                DocMainType = "PP";
                $.ajax({
                    url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                    type: "GET",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: {},
                    async: false,
                    success: function (result) {
                        debugger;

                        $('#docdetails').html(result);

                    },
                    error: function (err) {
                        debugger;
                        //  alert(err.statusText);
                    }
                });
                DocMainType = "VI";
                $.ajax({
                    url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                    type: "GET",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: {},
                    async: false,
                    success: function (result) {
                        debugger;

                        $('#docdetails').html(result);

                    },
                    error: function (err) {
                        debugger;
                        //  alert(err.statusText);
                    }
                });
                debugger;
                DocMainType = "IAPVD";

                $.ajax({
                    url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                    type: "GET",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: {},
                    async: false,
                    success: function (result) {
                        debugger;

                        $('#docdetails').html(result);

                    },
                    error: function (err) {
                        debugger;
                        //  alert(err.statusText);
                    }
                });



            }
            $("#Digil1").css("display", "none");
            $("body").attr('read-only', false);
        },

        error: function (err) {
            $("#Digil1").css("display", "none");
            $("body").attr('read-only', false);
            swal(err.statusText);
        }
    });
    $("html, body").animate({
        scrollTop: $(
            'html, body').get(0).scrollHeight
    }, 30);
    $(window).scrollTop($('#UTMAuditGridAD').position().top);

};


//-----Address Upload-------
$(document).ready(function () {

    $("#corrAdd").on("change", function () {

        //alert("Working");
        $("#Digil1").css("display", "block");
        $("body").attr('read-only', true);
        //$('#overlay_disabl').show();

        setTimeout(function () {
            //Code here
            hello1();

        }, 1500);

    });
});

function hello1() {
    var fileInput = document.getElementById('corrAdd');
    var Addproof = document.getElementById("ddl_corrAdd").value;

    if (Addproof == "--Select--" || Addproof == "") {
        swal("please Select Address Document");
    }
    //Iterating through each files selected in fileInput
    var formdata = new FormData();
    for (i = 0; i < corrAdd.files.length; i++) {

        var sfilename = corrAdd.files[i].name;
        var sfilesize = corrAdd.files[i].size;
        var POItype = $('#ddl_corrAdd option:selected').text();
        let srandomid = Math.random().toString(36).substring(7);

        formdata.append(sfilename, corrAdd.files[i]);

        var markup = "<tr id='" + srandomid + "'><td>" + sfilename + "</td><td>" + sfilesize + "</td><td>" + POItype + "</td><td><a href='#' onclick='DeleteFile(\"" + srandomid + "\",\"" + sfilename +
            "\")'><span class='glyphicon glyphicon-remove red'></span></a></td><td><a href='#' onclick='OpenFile(\"" + srandomid + "\",\"" + sfilename +
            "\")'><span class='glyphicon glyphicon-remove red'></span></a></td></tr>"; // Binding the file name

    }
    //chkatchtbl();
    $('#corrAdd').val('');
    //$('#ddl_corrAdd' ).text('');
    $("#Addloder").hide();
    $('#overlay_disabl').hide();


    var idname = $('#ddl_corrAdd option:selected').text();
    var docvalue = $('#ddl_corrAdd').val();
    formdata.append('POIvalue', idname);
    formdata.append("ddl_idProof", docvalue);
    formdata.append("DocMainType", "DOCPOA");

    if ($("#ddl_corrAdd").val() == '') {
        $("#Digil1").css("display", "none");

        swal("Please Select Address Document");
        return false();
    }
    $.ajax({
        url: '/DocumentDetails/UploadFiles1',
        type: "POST",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: formdata,
        async: false,
        success: function (result) {
            debugger;
            $("#Digil1").css("display", "none");
            $("#okloader").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
            $("#okloader").show();
            $('#overlay_disabl').show();
            swal({
                title: result,
                type: "success",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ok",
                closeOnConfirm: false
            });
            $("#okloader").hide();

            //###ADDRESS###//

            debugger;
            DocMainType = "CA";
            $.ajax({
                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                type: "GET",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: {},
                async: false,
                success: function (result) {
                    debugger;

                    $('#docdetails_Add').html(result);

                },
                error: function (err) {
                    debugger;
                    //  alert(err.statusText);
                }
            });
            DocMainType = "DL";
            $.ajax({
                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                type: "GET",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: {},
                async: false,
                success: function (result) {
                    debugger;

                    $('#docdetails_Add').html(result);

                },
                error: function (err) {
                    debugger;
                    //  alert(err.statusText);
                }
            });
            DocMainType = "PP";
            $.ajax({
                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                type: "GET",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: {},
                async: false,
                success: function (result) {
                    debugger;

                    $('#docdetails_Add').html(result);

                },
                error: function (err) {
                    debugger;
                    //  alert(err.statusText);
                }
            });
            DocMainType = "VI";
            $.ajax({
                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                type: "GET",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: {},
                async: false,
                success: function (result) {
                    debugger;

                    $('#docdetails_Add').html(result);

                },
                error: function (err) {
                    debugger;
                    //  alert(err.statusText);
                }
            });
            debugger;
            DocMainType = "CADL";

            $.ajax({
                url: '/DocumentDetails/GetDocumentsForGridView1?DocMainType=' + DocMainType,
                type: "GET",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: {},
                async: false,
                success: function (result) {
                    debugger;

                    $('#docdetails_Add').html(result);

                },
                error: function (err) {
                    debugger;
                    //  alert(err.statusText);
                }
            });


        },
        error: function (err) {
            $("#Digil1").css("display", "none");
            swal(err.statusText);
        }
    });
    $("html, body").animate({
        scrollTop: $(
            'html, body').get(0).scrollHeight
    }, 2000);
    $(window).scrollTop($('#AddSign').position().top);

}


//Signature
$(document).ready(function () {

    $("#AddSign").on("change", function () {

        //alert("Working");
        $("#Digil1").css("display", "block");
        //$('#overlay_disabl').show();

        setTimeout(function () {
            //Code here
            hello2();

        }, 1500);

    });
});




function hello2() {
    var fileInput = document.getElementById('AddSign');
    //Iterating through each files selected in fileInput
    var formdata = new FormData();
    for (i = 0; i < AddSign.files.length; i++) {

        var sfilename = AddSign.files[i].name;
        var sfilesize = AddSign.files[i].size;
        var POItype = $('#ddl_corrAdd option:selected').text();
        let srandomid = Math.random().toString(36).substring(7);

        formdata.append(sfilename, AddSign.files[i]);

        var markup = "<tr id='" + srandomid + "'><td>" + sfilename + "</td><td>" + sfilesize + "</td><td>" + POItype + "</td><td><a href='#' onclick='DeleteFile(\"" + srandomid + "\",\"" + sfilename +
            "\")'><span class='glyphicon glyphicon-remove red'></span></a></td><td><a href='#' onclick='OpenFile(\"" + srandomid + "\",\"" + sfilename +
            "\")'><span class='glyphicon glyphicon-remove red'></span></a></td></tr>";
    }
    //chkatchtbl();
    $('#AddSign').val('');
    $("#Docsign").hide();
    $('#overlay_disabl').hide();

    var idname = $('#ddl_corrAdd option:selected').text();
    var docvalue = 'Signature';
    formdata.append('POIvalue', idname);
    formdata.append("ddl_idProof", docvalue);
    formdata.append("DocMainType", "SI");
    $.ajax({
        url: '/DocumentDetails/UploadFiles1',
        type: "POST",
        contentType: false, // Not to set any content header
        processData: false, // Not to process data
        data: formdata,
        async: false,
        success: function (result) {
            debugger;
            if (result == "Signature Uploaded Successfully") {
                swal("Signature Uploaded Successfully");
            }
            $("#oksign").html("<div style='position: fixed;z-index: 5000;float: left;right: 50%;height: 50px;width: 50px;top:40%;'><img class='ajax-loader' src='../Images/loader/Dual Ring-1s-200px.gif' height='60px' width='60px' align='middle' alt='ajaxloadergif' /></div>");
            $("#oksign").show();
            $('#overlay_disabl').show();
            if (result != "") {
                $("#Digil1").css("display", "none");
                //$('#dividtemp').append(result);
                swal(result);


                swal({
                    title: result,
                    //text: "Delete Confirmation?",
                    type: "success",
                    showCancelButton: false,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                },

                );






                var cutomerdetailid = '@ViewBag.CustomerDetailsId';
                var DocMainType = "SI";
                //var formdataAdd = new FormData();
                $.ajax({
                    url: '/DocumentDetails/GetDocumentsForGridView?DocMainType=' + DocMainType,
                    type: "GET",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: {},
                    async: false,
                    success: function (result) {
                        debugger;
                        $("#Digil1").css("display", "none");
                        $('#docdetails_Sign').html(result);
                        $("#oksign").hide();
                        $('#overlay_disabl').hide();
                        //},
                        //error: function (err) {
                        //    $("#Digil1").css("display", "none");
                        //    swal(err.statusText);
                    }
                });

            }
        },
        error: function (err) {
            $("#Digil1").css("display", "none");
            alert(err.statusText);
            $("#oksign").hide();
            $('#overlay_disabl').hide();
        }
    });
}


$(document).ready(function () {
    $("#btncropNew").click(function () {
        debugger;
        $.ajax({

            url: '/DocumentDetails/CropDoc',
            type: 'Get',
            success: function (result) {
                debugger;
                if (result == 'Document not available on DMS.Kindly scan document and upload manually.') {
                    swal('If');
                    //swal(result);
                }
                else {
                    swal('Else');
                    //swal(result);
                    $('#dividtemp').append(result);


                }
            }
        });
    });
});
function DeleteFile(Fileid, FileName) {
    formdata.delete(FileName)
    $("#" + Fileid).remove();
    chkatchtbl();
}
function OpenFile(Fileid, FileName) {
    formdata.get(FileName)
    $("#" + Fileid).loadImage();
    // chkatchtbl();
}
function chkatchtbl() {
    if ($('#FilesList tr').length > 1) {
        $("#FilesList").css("visibility", "visible");
    } else {
        $("#FilesList").css("visibility", "hidden");
    }
}
    