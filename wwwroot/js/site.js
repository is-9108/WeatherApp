// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function selectArea() {
    var todoufuken = document.getElementById("todoufuken").value;
    alert(todoufuken);
    $.ajax({
        type: "POST",
        url: "/api/GetCity/getCities",
        data: { id: todoufuken },
        success: function (data) {
            // 成功時の処理
            alert("Success");
        },
        error: function (xhr, status, error) {
            // エラー時の処理
            alert("Error: " + error);
        }
    });
}
