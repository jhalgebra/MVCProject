function showModal(title, content, okText, cancelText, okEventHandler) {
    var modal = $("#modal");
    $(".modal-title").text(title);
    $(".modal-body").html(content);

    var footerButtons = $(".modal-footer > button");

    var okButton = $(footerButtons[0]);
    var cancelButton = $(footerButtons[1]);

    okButton.text(okText);
    if (cancelText == null || cancelText.length == 0)
        cancelButton.hide(0);
    else
        cancelButton.text(cancelText);

    okButton.click(okEventHandler === undefined ? hideModal : okEventHandler);

    modal.modal("show");
}

function hideModal() {
    $("#modal").modal("hide");
}