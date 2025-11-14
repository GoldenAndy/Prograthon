$(document).ready(function () {

    cargarReservas();
    cargarUsuarios();
    cargarLaboratorios();

    $("#btnNuevaReserva").click(function () {
        limpiarFormularioReserva();
        $("#tituloModalReserva").text("Nueva Reserva");
        $("#modalReserva").modal("show");
    });

    $("#btnGuardarReserva").click(function () {
        const idVal = $("#Reserva_Id").val();

        const reserva = {
            Reserva_Id: idVal ? parseInt(idVal, 10) : null,
            Usuario_Id: parseInt($("#Usuario_Id").val(), 10),
            Laboratorio_Id: parseInt($("#Laboratorio_Id").val(), 10),
            Fecha: $("#Fecha").val(),
            Hora: $("#Hora").val()
        };

        if (!reserva.Usuario_Id || !reserva.Laboratorio_Id ||
            !reserva.Fecha || !reserva.Hora) {
            mostrarToast("Error", "Todos los campos son obligatorios.", "danger");
            return;
        }

        const url = idVal ? "/Reserva/Editar" : "/Reserva/Crear";

        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(reserva),
            contentType: "application/json; charset=utf-8",
            success: function (r) {
                mostrarToast("Éxito", r.mensaje || "Operación completada.", "success");
                $("#modalReserva").modal("hide");
                cargarReservas();
            },
            error: function (xhr) {
                let msg = "No se pudo guardar la reserva.";
                try {
                    const resp = xhr.responseJSON;
                    if (resp && resp.mensaje) msg = resp.mensaje;
                } catch { }
                mostrarToast("Error", msg, "danger");
            }
        });
    });


    function cargarReservas() {
        $.get("/Reserva/ObtenerTodos", function (data) {
            let filas = "";

            data.forEach(r => {
                const fecha = r.fecha ? r.fecha : "N/A";
                const hora = r.hora ? r.hora : "N/A";

                filas += `
                    <tr>
                        <td>${r.usuario?.nombre ?? "—"}</td>
                        <td>${r.laboratorio?.nombre ?? "—"}</td>
                        <td>${formatearFecha(fecha)}</td>
                        <td>${formatearHora(hora)}</td>
                        <td class="text-nowrap">
                            <button class="btn btn-warning btn-sm me-1 btnEditarReserva"
                                    data-id="${r.reserva_Id}">
                                <i class="bi bi-pencil-square"></i>
                            </button>
                            <button class="btn btn-danger btn-sm btnEliminarReserva"
                                    data-id="${r.reserva_Id}"
                                    data-nombre="${r.usuario?.nombre ?? "Reserva"}">
                                <i class="bi bi-trash"></i>
                            </button>
                        </td>
                    </tr>`;
            });

            $("#tablaReservas tbody").html(filas);
        });
    }


    $(document).on("click", ".btnEditarReserva", function () {
        const id = $(this).data("id");

        limpiarFormularioReserva();
        $("#tituloModalReserva").text("Cargando...");
        $("#modalReserva").modal("show");

        $.get(`/Reserva/ObtenerPorId?id=${id}`, function (r) {

            $("#Reserva_Id").val(r.reserva_Id);
            $("#Usuario_Id").val(r.usuario_Id);
            $("#Laboratorio_Id").val(r.laboratorio_Id);
            $("#Fecha").val(r.fecha);
            $("#Hora").val(r.hora);

            $("#tituloModalReserva").text("Editar Reserva");

        }).fail(() => {
            mostrarToast("Error", "No se pudo cargar la reserva.", "danger");
            $("#modalReserva").modal("hide");
        });
    });


    let idAEliminar = null;

    $(document).on("click", ".btnEliminarReserva", function () {
        idAEliminar = $(this).data("id");
        const nombre = $(this).data("nombre");

        $("#textoEliminarReserva").text(`¿Deseas eliminar la reserva de "${nombre}"?`);
        $("#modalEliminarReserva").modal("show");
    });

    $("#btnEliminarReservaConfirmado").click(function () {
        if (!idAEliminar) return;

        $.post(`/Reserva/Eliminar?id=${idAEliminar}`, function (r) {

            $("#modalEliminarReserva").modal("hide");

            if (r.exito) {
                mostrarToast("Éxito", r.mensaje, "success");
                cargarReservas();
            } else {
                mostrarToast("Error", r.mensaje || "No se pudo eliminar.", "danger");
            }

        }).fail(() => {
            mostrarToast("Error", "No se pudo eliminar la reserva.", "danger");
        });
    });


    function cargarUsuarios() {
        $.get("/Reserva/ListarUsuarios", function (data) {
            let options = '<option value="">Seleccione...</option>';

            data.forEach(u => {
                options += `<option value="${u.usuario_Id}">${u.nombre}</option>`;
            });

            $("#Usuario_Id").html(options);
        });
    }


    function cargarLaboratorios() {
        $.get("/Reserva/ListarLaboratorios", function (data) {
            let options = '<option value="">Seleccione...</option>';

            data.forEach(l => {
                options += `<option value="${l.laboratorio_Id}">${l.nombre}</option>`;
            });

            $("#Laboratorio_Id").html(options);
        });
    }


    function limpiarFormularioReserva() {
        $("#Reserva_Id").val("");
        $("#Usuario_Id").val("");
        $("#Laboratorio_Id").val("");
        $("#Fecha").val("");
        $("#Hora").val("");
    }

    function mostrarToast(titulo, mensaje, tipo) {
        const colores = {
            success: "#0033A0",
            danger: "#C70000",
            warning: "#FFC400"
        };

        const toast = $(`
            <div class="toast align-items-center text-white border-0"
                 style="background-color:${colores[tipo]};
                        position: fixed; top: 20px; right: 20px; z-index: 1055;"
                 role="alert">
                <div class="d-flex">
                    <div class="toast-body">
                        <strong>${titulo}</strong><br>${mensaje}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto"
                            data-bs-dismiss="toast"></button>
                </div>
            </div>
        `);

        $("body").append(toast);
        const bsToast = new bootstrap.Toast(toast[0]);
        bsToast.show();
        toast.on("hidden.bs.toast", () => toast.remove());
    }

    function formatearFecha(fecha) {
        if (!fecha) return "—";
        const p = fecha.split("-");
        return `${p[2]}/${p[1]}/${p[0]}`;
    }

    function formatearHora(hora) {
        if (!hora) return "—";
        return hora.substring(0, 5);
    }

});
