$(document).ready(function () {
    $("input[id$='cboxUsuario']").change(function () {

        if ($("input[id$='cboxUsuario']").prop("checked")) {
            $("#usuarioAuto").css('display', 'block');            
        }
        else {
            $("#usuarioAuto").css('display', 'none');
        }
    });
});

function CargarCalendario() {
    $(".jqCalendar").datepicker({
        showOn: "button",
        buttonImage: "../js/images/calendar.gif",
        buttonImageOnly: true,
        buttonText: "Seleccione la fecha",
        changeMonth: true,
        changeYear: true,
        dateFormat: "yy/mm/dd"
    });
}

// mensaje de alerta
function CargarMensaje(data) {
    alert(data);
}

function Cargando(e) {
    $(".loader").fadeOut("slow");
}

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

//Función que permite solo Números
function ValidaSoloNumerosFecha(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (e.keyCode == 47) return true; // tab
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
        if (e.keyCode == 9) return true; // tab
        if (tecla == 32) return true; // espacio
        if (e.ctrlKey && tecla == 86) { return true; } //Ctrl v
        if (e.ctrlKey && tecla == 67) { return true; } //Ctrl c
        if (e.ctrlKey && tecla == 88) { return true; } //Ctrl x

        patron = /[a-zA-Z]/; //patron

        te = String.fromCharCode(tecla);
        return patron.test(te); // prueba de patron
}