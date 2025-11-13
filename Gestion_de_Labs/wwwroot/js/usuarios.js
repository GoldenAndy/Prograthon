$(document).ready(function () {

    // ================================
    //  INICIALIZAR
    // ================================
    cargarUsuarios();

    // ================================
    //  NUEVO USUARIO
    // ================================
    $("#btnNuevoUsuario").click(function () {
        limpiarFormularioUsuario();
        $("#tituloModalUsuario").text("Nuevo Usuario");
        $("#modalUsuario").modal("show");
    });

    // ================================
    //  GUARDAR (CREAR / EDITAR)
    // ================================
    $("#btnGuardarUsuario").click(function () {

        const idVal = $("#Usuario_Id").val();

        const usuario = {
            Usuario_Id: idVal ? parseInt(idVal, 10) : null,
            Nombre: $("#Nombre").val().trim(),
            Correo: $("#Correo").val().trim(),
            Tipo_Usuario_Id: parseInt($("#Tipo_Usuario_Id").val(), 10)
        };

        if (!idVal) delete usuario.Usuario_Id;

        const url = idVal ? "/Usuarios/Editar" : "/Usuarios/Crear";

        $.ajax({
            url: url,
            type: "POST",
            data: JSON.stringify(usuario),
            contentType: "application/json; charset=utf-8",
            success: function (r) {
                mostrarToast("Operación exitosa", r.mensaje || "Listo", "success");
                $("#modalUsuario").modal("hide");
                cargarUsuarios();
            },
            error: function (xhr) {
                let msg = "No se pudo guardar el usuario.";
                try {
                    const resp = xhr.responseJSON || JSON.parse(xhr.responseText);
                    if (resp && resp.mensaje) msg = resp.mensaje;
                } catch { }
                mostrarToast("Error", msg, "danger");
            }
        });

    });

    // ================================
    //  CARGAR LISTA
    // ================================
    function cargarUsuarios() {
        $.get("/Usuarios/ObtenerTodos", function (data) {

            let filas = "";
            data.forEach(u => {

                const tipoTexto =
                    u.tipo_Usuario_Id === 1 ? "Estudiante" :
                        u.tipo_Usuario_Id === 2 ? "Profesor" : "Otro";

                filas += `
                    <tr>
                        <td>${u.usuario_Id}</td>
                        <td>${u.nombre}</td>
                        <td>${u.correo}</td>
                        <td>${tipoTexto}</td>
                        <td class="text-nowrap">

                            <button class="btn btn-sm btn-warning me-1 btnEditarUsuario"
                                    data-id="${u.usuario_Id}">
                                <i class="bi bi-pencil"></i>
                            </button>

                            <button class="btn btn-sm btn-danger btnEliminarUsuario"
                                    data-id="${u.usuario_Id}"
                                    data-nombre="${u.nombre}">
                                <i class="bi bi-trash"></i>
                            </button>

                        </td>
                    </tr>`;
            });

            $("#tablaUsuarios tbody").html(filas);
        });
    }

    // ================================
    //  EDITAR
    // ================================
    $(document).on("click", ".btnEditarUsuario", function () {

        const id = $(this).data("id");

        limpiarFormularioUsuario();
        $("#tituloModalUsuario").text("Cargando...");
        $("#modalUsuario").modal("show");

        $.get("/Usuarios/ObtenerPorId?id=" + id, function (u) {

            $("#Usuario_Id").val(u.usuario_Id);
            $("#Nombre").val(u.nombre);
            $("#Correo").val(u.correo);
            $("#Tipo_Usuario_Id").val(u.tipo_Usuario_Id);

            $("#tituloModalUsuario").text("Editar Usuario");
        })
            .fail(function () {
                mostrarToast("Error", "No se pudo cargar el usuario.", "danger");
                $("#modalUsuario").modal("hide");
            });

    });

    // ================================
    //  ELIMINAR (CONFIRMACIÓN)
    // ================================
    let idAEliminar = null;

    $(document).on("click", ".btnEliminarUsuario", function () {
        idAEliminar = $(this).data("id");
        const nombre = $(this).data("nombre");
        $("#textoEliminarUsuario").text(`¿Deseas eliminar a "${nombre}"?`);
        $("#modalEliminarUsuario").modal("show");
    });

    $("#btnEliminarUsuarioConfirmado").click(function () {

        if (!idAEliminar) return;

        $.post("/Usuarios/Eliminar?id=" + idAEliminar, function (r) {

            $("#modalEliminarUsuario").modal("hide");

            if (r.exito) {
                mostrarToast("Éxito", r.mensaje, "success");
                cargarUsuarios();
            } else if (/(reserv|asociad)/i.test(r.mensaje)) {
                mostrarToast("Acción bloqueada", r.mensaje, "warning");
            } else {
                mostrarToast("Error", r.mensaje || "No se pudo eliminar.", "danger");
            }

        })
            .fail(function () {
                $("#modalEliminarUsuario").modal("hide");
                mostrarToast("Error", "Error al eliminar el usuario.", "danger");
            });

    });

    // ================================
    //  FUNCIONES AUXILIARES
    // ================================
    function limpiarFormularioUsuario() {
        $("#Usuario_Id").val("");
        $("#Nombre").val("");
        $("#Correo").val("");
        $("#Tipo_Usuario_Id").val("");
    }

    function mostrarToast(titulo, mensaje, tipo) {

        const colores = {
            success: "#0d6efd",
            danger: "#dc3545",
            warning: "#ffc107"
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
