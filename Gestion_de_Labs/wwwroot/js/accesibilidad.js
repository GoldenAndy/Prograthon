$(document).ready(function () {

    $("#btnAumentarFuente").on("click", function () {
        let fs = parseFloat($("html").css("font-size"));
        $("html").css("font-size", (fs + 1) + "px");
    });

    $("#btnDisminuirFuente").on("click", function () {
        let fs = parseFloat($("html").css("font-size"));
        $("html").css("font-size", Math.max(fs - 1, 10) + "px");
    });

    $("#btnContraste").on("click", function () {
        $("body").toggleClass("modo-alto-contraste");
        anunciar("Modo de alto contraste activado");
    });


    const ariaLive = $("#ariaLiveRegion");

    window.anunciar = function (mensaje) {
        ariaLive.text("");
        setTimeout(() => ariaLive.text(mensaje), 50);
    };


    $(document).on("keydown", function (e) {
        if (e.key === "F2") {
            $("body").toggleClass("modo-alto-contraste");
            anunciar("Modo de alto contraste activado mediante F2");
            e.preventDefault();
        }
    });



    $(document).on("keydown", function (e) {

        // CTRL + +
        if (e.ctrlKey && (e.key === "+" || e.key === "=")) {
            let fs = parseFloat($("html").css("font-size"));
            $("html").css("font-size", (fs + 1) + "px");
            anunciar("Aumento de tamaño de letra");
            e.preventDefault();
        }

        // CTRL + -
        if (e.ctrlKey && e.key === "-") {
            let fs = parseFloat($("html").css("font-size"));
            $("html").css("font-size", Math.max(fs - 1, 10) + "px");
            anunciar("Disminución de tamaño de letra");
            e.preventDefault();
        }
    });

});
