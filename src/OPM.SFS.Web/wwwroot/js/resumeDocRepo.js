$(document).ready(function () {
    $('#upload-doc-name').keyup(function () {
        $("#doc-name-error").hide();
        $("#doc-type-error").hide();
    });


    $('#btn-add-resume').click(function (event) {
        $('#addResumeModal').modal();
        return false;
    });
    $('#lnkUploadResume').click(function (event) {
        $('#resultToUpload').click();
    });
    $('.delete-doc').click(function (event) {
        $('#confirmDeleteModal').modal();
        var docID = $(this).data("id");
        $("#docid").val(docID);
        console.log(docID);
        return false;
    });
    $('#btn-cancel-delete').click(function (event) {
        $.modal.close();
    });
    $('#btn-confirm-delete').click(function (event) {
        $(".loader").show();
        $("#delete-doc-form").submit();
    });

    $('#btn-upload-resume').click(function (event) {
        var resume = $("#resultToUpload").prop("files");
        var url = "?handler=Upload";
        var docName = $("#upload-doc-name").val();
        var formData = new FormData();
        formData.append("resume", resume[0]);
        formData.append("name", docName);
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
    });
    $("#resultToUpload").change(function () {
        //get the file name and open new modal
        var filename = $('input[type=file]').val().replace(/.*(\/|\\)/, '');
        var docName = filename.replace(/\.[^.$]+$/, '');
        $.modal.close();
        $("#upload-doc-name").val(docName);
        $('#addResumeUpload').modal();
    });

    $(document).on("click", ".set-shareable-resume", function () {
        var id = $(this).data("id");
        console.log(id);
        
        if ($(this).is(':checked')) {
            console.log("do some work!");
            $.getJSON(`?handler=ShareResume&docID=${id}`, (data) => {
                console.log(data);
            });
        }
    });

});

function validate(name) {
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
    return true;
}