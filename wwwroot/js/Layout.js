
$(document).ready(function () {
    $("#btnExpand").on("click", function () {
        $(".side-bar").addClass("enter");
        $(".opacity-container").css("visibility", "visible");
    });

    $('#exitMenu').on("click", function () {
        $("#expandedBarRelatorios").removeClass("enter");
        $("#expandedBarCadastros").removeClass("enter");
        $("#expandedBarChecklists").removeClass("enter");
        $("#expandedBarDashboards").removeClass("enter");
        $("#iconExpandRelatorios").css("transform", "rotate(360deg)");
        $("#iconExpandChecklists").css("transform", "rotate(360deg)");
        $("#iconExpandCadastros").css("transform", "rotate(360deg)");
        $("#iconExpandDashboards").css("transform", "rotate(360deg)");
        $("#btnCadastros").css("color", "");
        $("#btnChecklists").css("color", "");
        $("#btnRelatorios").css("color", "");
        $("#btnDashboards").css("color", "");
        $("#btnRelatorios").css("background-color", "");
        $("#btnCadastros").css("background-color", "");
        $("#btnChecklists").css("background-color", "");
        $("#btnDashboards").css("background-color", "");
        $(".side-bar").removeClass("enter");
        $(".opacity-container").css("visibility", "hidden");
        $("#btnExpand").css("visibility", "visible");
    });

    $("#btnCadastros").on("click", function () {
        $("#btnExpand").css("visibility", "hidden");
        $("#expandedBarChecklists").removeClass("enter");
        $("#expandedBarRelatorios").removeClass("enter");
        $("#expandedBarDashboards").removeClass("enter");
        $("#expandedBarCadastros").addClass("enter");
        $("#iconExpandCadastros").css("transform", "rotate(180deg)");
        $("#iconExpandRelatorios").css("transform", "rotate(360deg)");
        $("#iconExpandChecklists").css("transform", "rotate(360deg)");
        $("#iconExpandDashboards").css("transform", "rotate(360deg)");
        $("#btnCadastros").css("background-color", "rgb(20,20,20)");
        $("#btnChecklists").css("background-color", "");
        $("#btnRelatorios").css("background-color", "");
        $("#btnDashboards").css("background-color", "");
        $("#btnCadastros").css("color", "white");
        $("#btnChecklists").css("color", "");
        $("#btnRelatorios").css("color", "");
        $("#btnDashboards").css("color", "");
        $(".opacity-container").css("visibility", "visible");
    });

    $("#exitCasdastros").on("click", function () {
        $("#btnExpand").css("visibility", "visible");
        $("#btnCadastros").css("color", "");
        $("#btnCadastros").css("background-color", "");
        $("#iconExpandCadastros").css("transform", "rotate(360deg)");
        $("#expandedBarCadastros").removeClass("enter");
    });

    $("#btnRelatorios").on("click", function () {
        $("#btnExpand").css("visibility", "hidden");
        $("#expandedBarCadastros").removeClass("enter");
        $("#expandedBarChecklists").removeClass("enter");
        $("#expandedBarDashboards").removeClass("enter");
        $("#expandedBarRelatorios").addClass("enter");
        $("#iconExpandRelatorios").css("transform", "rotate(180deg)");
        $("#iconExpandCadastros").css("transform", "rotate(360deg)");
        $("#iconExpandChecklists").css("transform", "rotate(360deg)");
        $("#iconExpandDashboards").css("transform", "rotate(360deg)");
        $("#btnRelatorios").css("background-color", "rgb(20,20,20)");
        $("#btnChecklists").css("background-color", "");
        $("#btnCadastros").css("background-color", "");
        $("#btnDashboards").css("background-color", "");
        $("#btnRelatorios").css("color", "white");
        $("#btnChecklists").css("color", "");
        $("#btnCadastros").css("color", "");
        $("#btnDashboards").css("color", "");
        $(".opacity-container").css("visibility", "visible");
    });

    $("#exitRelatorios").on("click", function () {
        $("#btnExpand").css("visibility", "visible");
        $("#btnRelatorios").css("color", "");
        $("#btnRelatorios").css("background-color", "");
        $("#iconExpandRelatorios").css("transform", "rotate(360deg)");
        $("#expandedBarRelatorios").removeClass("enter");
    });

    $("#btnChecklists").on("click", function () {
        $("#btnExpand").css("visibility", "hidden");
        $("#expandedBarRelatorios").removeClass("enter");
        $("#expandedBarCadastros").removeClass("enter");
        $("#expandedBarDashboards").removeClass("enter");
        $("#expandedBarChecklists").addClass("enter");
        $("#iconExpandChecklists").css("transform", "rotate(180deg)");
        $("#iconExpandRelatorios").css("transform", "rotate(360deg)");
        $("#iconExpandCadastros").css("transform", "rotate(360deg)");
        $("#iconExpandDashboards").css("transform", "rotate(360deg)");
        $("#btnChecklists").css("background-color", "rgb(20,20,20)");
        $("#btnCadastros").css("background-color", "");
        $("#btnRelatorios").css("background-color", "");
        $("#btnDashboards").css("background-color", "");
        $("#btnChecklists").css("color", "white");
        $("#btnCadastros").css("color", "");
        $("#btnRelatorios").css("color", "");
        $("#btnDashboards").css("color", "");
        $(".opacity-container").css("visibility", "visible");       
    });

    $("#exitChecklists").on("click", function () {
        $("#btnExpand").css("visibility", "visible");
        $("#btnChecklists").css("color", "");
        $("#btnChecklists").css("background-color", "");
        $("#iconExpandChecklists").css("transform", "rotate(360deg)");
        $("#expandedBarChecklists").removeClass("enter");
    });

    $("#btnDashboards").on("click", function () {
        $("#btnExpand").css("visibility", "hidden");
        $("#expandedBarChecklists").removeClass("enter");
        $("#expandedBarRelatorios").removeClass("enter");
        $("#expandedBarCadastros").removeClass("enter");
        $("#expandedBarDashboards").addClass("enter");
        $("#iconExpandDashboards").css("transform", "rotate(180deg)");
        $("#iconExpandRelatorios").css("transform", "rotate(360deg)");
        $("#iconExpandChecklists").css("transform", "rotate(360deg)");
        $("#iconExpandCadastros").css("transform", "rotate(360deg)");
        $("#btnDashboards").css("background-color", "rgb(20,20,20)");
        $("#btnChecklists").css("background-color", "");
        $("#btnRelatorios").css("background-color", "");
        $("#btnCadastros").css("background-color", "");
        $("#btnDashboards").css("color", "white");
        $("#btnChecklists").css("color", "");
        $("#btnRelatorios").css("color", "");
        $("#btnCadastros").css("color", "");
        $(".opacity-container").css("visibility", "visible");
    });

    $("#exitDashboards").on("click", function () {
        $("#btnExpand").css("visibility", "visible");
        $("#btnDashboards").css("color", "");
        $("#btnDashboards").css("background-color", "");
        $("#iconExpandDashboards").css("transform", "rotate(360deg)");
        $("#expandedBarDashboards").removeClass("enter");
    });
});
