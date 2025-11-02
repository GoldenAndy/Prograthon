$(document).ready(function () {

    cargarLaboratorios();

    $("#btnNuevo").click(function () {
        limpiarFormulario();
        $("#tituloModal").text("Nuevo Laboratorio");
        $("#modalLaboratorio").modal("show");
    });

    $("#btnGuardar").click(function () {
        const idVal = $("#Laboratorio_Id").val();

        const lab = {
            Laboratorio_Id: idVal ? parseInt(idVal, 10) : null,
            Nombre: $("#Nombre").val().trim(),
            Capacidad: parseInt($("#Capacidad").val(), 10),
            Responsable: $("#Responsable").val().trim()
        };
        if (!idVal) delete lab.Laboratorio_Id

        const url = idVal ? "/Laboratorio/Editar" : "/Laboratorio/Crear";

        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(lab),
            contentType: "application/json; charset=utf-8",
            success: function (r) {
                mostrarToast("Operación exitosa", r.mensaje || "Listo.", "success");
                $("#modalLaboratorio").modal("hide");
                cargarLaboratorios();
            },
            error: function (xhr) {
                let msg = "No se pudo guardar el laboratorio.";
                try {
                    const resp = xhr.responseJSON || JSON.parse(xhr.responseText);
                    if (resp && (resp.mensaje || resp.detalle)) {
                        msg = (resp.mensaje || msg) + (resp.detalle ? `\n${resp.detalle}` : "");
                    }
                } catch { /* ignore */ }
                mostrarToast("Error", msg, "danger");
            }
        });
    });

    function cargarLaboratorios() {
        $.get("/Laboratorio/ObtenerTodos", function (data) {
            let filas = "";
            data.forEach(lab => {
                filas += `
                    <tr>
                        <td>${lab.laboratorio_Id}</td>
                        <td>${lab.nombre}</td>
                        <td>${lab.capacidad}</td>
                        <td>${lab.responsable}</td>
                        <td>
                            <button class="btn btn-sm text-white me-1 btnEditar"
                                    data-id="${lab.laboratorio_Id}"
                                    style="background-color:#FFC400;">
                                <i class="bi bi-pencil-square"></i>
                            </button>
                            <button class="btn btn-sm text-white btnEliminar"
                                    data-id="${lab.laboratorio_Id}"
                                    style="background-color:#C70000;">
                                <i class="bi bi-trash"></i>
                            </button>
                        </td>
                    </tr>`;
            });
            $("#tablaLaboratorios tbody").html(filas);
        });
    }

    $(document).on("click", ".btnEditar", function () {
        const id = $(this).data("id");
        limpiarFormulario();
        $("#tituloModal").text("Cargando...");
        $("#modalLaboratorio").modal("show");

        $.get(`/Laboratorio/ObtenerPorId?id=${id}`, function (lab) {
            if (lab) {
                $("#Laboratorio_Id").val(lab.laboratorio_Id);
                $("#Nombre").val(lab.nombre);
                $("#Capacidad").val(lab.capacidad);
                $("#Responsable").val(lab.responsable);
                $("#tituloModal").text("Editar Laboratorio");
            } else {
                mostrarToast("Error", "No se pudo cargar el laboratorio.", "danger");
                $("#modalLaboratorio").modal("hide");
            }
        }).fail(function () {
            mostrarToast("Error", "Error al obtener el laboratorio.", "danger");
            $("#modalLaboratorio").modal("hide");
        });
    });


    let idAEliminar = null;
    $(document).on("click", ".btnEliminar", function () {
        idAEliminar = $(this).data("id");
        const fila = $(this).closest("tr");
        const nombre = fila.find("td:nth-child(2)").text();
        $("#confirmText").text(`¿Deseas eliminar "${nombre}"?`);
        $("#modalConfirmarEliminar").modal("show");
    });

    $("#btnConfirmarEliminar").click(function () {
        if (!idAEliminar) return;

        $.post("/Laboratorio/Eliminar?id=" + idAEliminar, function (r) {
            $("#modalConfirmarEliminar").modal("hide");

            if (r.exito) {
                mostrarToast("Éxito", r.mensaje, "success");
                cargarLaboratorios();
            } else if (/(reserv|curso|asociad)/i.test(r.mensaje)) {
                mostrarToast("Acción bloqueada", r.mensaje, "warning");
            } else {
                mostrarToast("Error", r.mensaje || "No se pudo eliminar.", "danger");
            }
        }).fail(function () {
            $("#modalConfirmarEliminar").modal("hide");
            mostrarToast("Error", "No se pudo eliminar el laboratorio.", "danger");
        });
    });

    function limpiarFormulario() {
        $("#Laboratorio_Id").val("");
        $("#Nombre").val("");
        $("#Capacidad").val("");
        $("#Responsable").val("");
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
});
