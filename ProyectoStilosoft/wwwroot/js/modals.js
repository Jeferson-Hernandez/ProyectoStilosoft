mostrarModalG = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-lg .modal-body').html(res)
            $('#form-modal-lg .modal-title').html(title)
            $('#form-modal-lg').modal('show');
        }
    })
}


mostrarModalS = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-sm .modal-body').html(res)
            $('#form-modal-sm .modal-title').html(title)
            $('#form-modal-sm').modal('show');
        }
    })
}