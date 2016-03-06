function MostrarPDF() {
    //window.open(url, _blank);
    var popup = window.open('C:/Users/jonforero/Documents/1234.pdf', _blank, '{1}');
    popup.focus();
}
//Función que permite solo Números
function ValidaSoloNumeros() {
    if ((event.keyCode < 48) || (event.keyCode > 57))
        event.returnValue = false;
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