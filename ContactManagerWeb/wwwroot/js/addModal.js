$(function () {
    // boostrap 4 load modal example from docs
    $('#modal-container').on('show.bs.modal', function (e) {
        var button = $(e.relatedTarget); // Button that triggered the modal
        var url = button.attr("href");
        var modal = $(this);
        // note that this will replace the content of modal-content ever time the modal is opened
        modal.find('.modal-content').load(url, function() {});
    });
});