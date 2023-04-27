$(function () {

    $("#CarIn").submit(function (event) {
        console.log("Entro");
        // Detenemos el evento submit para evitar que el formulario se envíe de forma tradicional
        event.preventDefault();

        // Obtenemos los datos del formulario
        var datos = $(this).serialize();
        
        // Enviamos los datos utilizando AJAX
        $.ajax({
            type: "POST",
            url: $(this).attr("action"),
            data: datos,
            success: function (respuesta) {
                // Aquí procesamos la respuesta del servidor
                alert(respuesta);
            },
            error: function () {
                // Aquí manejamos errores de la solicitud
                alert("Error al enviar los datos");
            }
        });
    });



});