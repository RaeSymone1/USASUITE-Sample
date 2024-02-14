$(document).ready(function () {

    $('#upload-doc-name').keyup(function () {
        $("#doc-name-error").hide();
        $("#doc-type-error").hide();
    });

    $('#btn-add-document').click(function (event) {
        $('#resultToUpload').click();
        return false;
    });

    $("#resultToUpload").change(function () {
        //get the file name and open new modal
        var filename = $('input[type=file]').val().replace(/.*(\/|\\)/, '');
        var docName = filename.replace(/\.[^.$]+$/, '');
        $("#upload-doc-name").val(docName);
        $('#addDocumentUpload').modal();
    });

    $('.delete-doc').click(function (event) {
        $('#confirmDeleteModal').modal();
        var docID = $(this).data("id");
        $("#docid").val(docID);
        //console.log(docID);
        return false;
    });

    $('#btn-cancel-delete').click(function (event) {
        $.modal.close();
    });
    $('#btn-confirm-delete').click(function (event) {
        $(".loader").show();
        $("#delete-doc-form").submit();
    });

    $('#btn-upload-document').click(function (event) {
        var doc = $("#resultToUpload").prop("files");
        var docName = $("#upload-doc-name").val();
        var studID = $("#StudID").val();
        var docType = $("input[type='radio']:checked").val();
        
        var isValid = validate(docName, docType);
        if (isValid == true) {
            $("#btn-upload-document").prop("disabled", true);
            $(".loader").show();
            var formData = new FormData();
            formData.append("Document", doc[0]);
            formData.append("Name", docName);
            formData.append("DocumentType", docType);
            var url = "?handler=Upload";
            $.ajax({
                type: 'POST',
                url: url,
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (data) {
                    if (data.status != "success") {
                        $.modal.close();
                        $("#errors").text(data.status);
                        $("#errors").show();
                        $('body').scrollTo('#errors');
                    }
                    else {
                        location.reload();
                    }

                },
                error: function (errorText, textStatus, xhr) {
                    console.log("Error!");
                }
            });
        }
    });

    $('#btn-admin-upload-document').click(function (event) {
        var doc = $("#resultToUpload").prop("files");
        var docName = $("#upload-doc-name").val();
        var studID = $("#StudentID").val();
        var docType = $("input[type='radio']:checked").val();

        var isValid = validate(docName, docType);
        if (isValid == true) {
            $("#btn-upload-document").prop("disabled", true);
            $(".loader").show();
            var formData = new FormData();
            formData.append("Document", doc[0]);
            formData.append("Name", docName);
            formData.append("DocumentType", docType);
            formData.append("StudentID", studID)
            var url = "?handler=Upload";
            $.ajax({
                type: 'POST',
                url: url,
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                success: function (data) {
                    if (data.status != "success") {
                        $.modal.close();
                        $("#errors").text(data.status);
                        $("#errors").show();
                        $('body').scrollTo('#errors');
                    }
                    else {
                        location.reload();
                    }

                },
                error: function (errorText, textStatus, xhr) {
                    console.log("Error!");
                }
            });
        }
    });
});

function validate(name, type) {
    //return (!str || str.length === 0);
    var rg = new RegExp("^[A-Za-z0-9? ,_-]+$");
    if (!name || name.length === 0) {
        $("#doc-name-error").text("Document Name is required.");
        $("#doc-name-error").show();
        return false;
    }
    else if (!rg.test(name)) {
        $("#doc-name-error").text("Document Name contains invalid characters.");
        $("#doc-name-error").show();
        return false;
    }
    else if (type == "undefined" || !type) {
        $("#doc-type-error").text("Document Type is required.");
        $("#doc-type-error").show();
        return false;
    }
    return true;
}