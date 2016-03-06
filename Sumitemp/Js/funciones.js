//Oculta los mensajes de error cuando clic en cajas de texto
function hideOnKeyPress() {
    document.getElementById('LblMsj').style.display = 'none';
}

//Función que permite solo Números
function ValidaSoloNumeros(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

//Función que permite solo Números y Letras
function txNombres() {
    if ((event.keyCode != 32) && (event.keyCode < 65) || (event.keyCode > 90) && (event.keyCode < 97) || (event.keyCode > 122))
        event.returnValue = false;
}

//Función que permite solo Letras, espacio, ctr v, ctr c, ctr x
function ValidaSoloLetras(e) {
        tecla = (document.all) ? e.keyCode : e.which;
        if (tecla == 8) return true; // backspace
        if (tecla == 32) return true; // espacio
        if (e.ctrlKey && tecla == 86) { return true; } //Ctrl v
        if (e.ctrlKey && tecla == 67) { return true; } //Ctrl c
        if (e.ctrlKey && tecla == 88) { return true; } //Ctrl x

        patron = /[a-zA-Z]/; //patron

        te = String.fromCharCode(tecla);
        return patron.test(te); // prueba de patron
}