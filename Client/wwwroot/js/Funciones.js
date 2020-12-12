function menuClick() {
    $("#wrapper").toggleClass("toggled");
}
function hoverImages() {
    $('[data-bs-hover-animate]')
        .mouseenter(function () { var elem = $(this); elem.addClass('animated ' + elem.attr('data-bs-hover-animate')) })
        .mouseleave(function () { var elem = $(this); elem.removeClass('animated ' + elem.attr('data-bs-hover-animate')) });
}
function CustomConfirm(titulo, mensaje, tipo) {
    return new Promise((resolve) => {
        Swal.fire({
            title: titulo,
            text: mensaje,
            icon: tipo,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Confirmar!'
        }).then((result) => {
            if (result.value) {
                resolve(true);
            }
            else {
                resolve(false);
            }
        });
    });
}