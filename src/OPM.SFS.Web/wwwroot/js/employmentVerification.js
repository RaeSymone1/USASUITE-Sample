const fileinputStuff = document.getElementById("Data_UploadedDocument")
if (fileinputStuff !== null) {
    fileinputStuff.addEventListener('change', (e) => {
        document.getElementById("frmEVFSubmit").submit();
    });
}

document.getElementById("lnkCancelEVF").addEventListener("click", cancelEVF);
document.getElementById("lnkCancelEvfYes").addEventListener("click", closeModal);
document.getElementById("lnkCancelEvfNo").addEventListener("click", closeModal);

$(document).ready(function () {
    var txtTraining = $('#txtTraining');
    var textCounterTraining = $('#text-counter-Training');
    var Training = $('#Training');
    var CurrentPositionEndDate = $('#CurrentPositionEndDate');
    var NewPositionSection = $('#NewPositionSection');
    var SubmitEVF = $("#SubmitEVF");
    var DocumentUploaded = $(".evf-document-display")
    txtTraining.keyup(function () {
        textCounterTraining.text(this.value.length);
    });

    function updateTrainingVisibility() {
        Training.toggle(YesTraining.is(':checked'));
    }

    var YesTraining = $('.evf-question-remedialTraining').first().change(updateTrainingVisibility);
    var NoTraining = $('.evf-question-remedialTraining').last().change(updateTrainingVisibility);
    updateTrainingVisibility();

    var YesCurrentPosition = $('.evf-question-currentPosition').first().change(function () {
        CurrentPositionEndDate.toggle(NoCurrentPosition.is(':checked'));
        NewPositionSection.toggle(NoCurrentPosition.is(':checked'));
        updateSubmitEVF();
    });

    var NoCurrentPosition = $('.evf-question-currentPosition').last().change(function () {
        CurrentPositionEndDate.toggle(this.checked);
        NewPositionSection.toggle(this.checked);
        SubmitEVF.prop('disabled',true)
    });

    YesCurrentPosition.change();

    $(".lnkDeleteEvfDocument").click(function () {
        var fid = $(this).data("fid");
        $("#" + fid).modal();
    });

    $(".lnkCancelModal").click(closeModal);

    function updateSubmitEVF() {
        SubmitEVF.prop('disabled', !(YesCurrentPosition.is(':checked') || NoCurrentPosition.is(':checked')) || !(YesTraining.is(':checked') || NoTraining.is(':checked')) || !(DocumentUploaded.length > 0));
    }

    YesNewPosition = $('.evf-question-hasNewCommmitment').first().change(updateSubmitEVF);
    NoNewPosition = $('.evf-question-hasNewCommmitment').last().change(updateSubmitEVF);
    updateSubmitEVF();
});

function cancelEVF() {
    $('#cancelEvfModal').modal();
}

function closeModal() {
    $.modal.close();
}

function confirmDelete() {
    $('#deleteEvfDocumentModal').modal();
}