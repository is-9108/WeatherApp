// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function selectArea() {
    var todoufuken = document.getElementById("todoufuken").value;
    $.ajax({
        type: "POST",
        url: "/api/GetCity/getCities",
        data: { id: todoufuken },
        success: function (data) {
            var citySelect = document.getElementById("cities");
            data.cities.forEach((city) => console.log(city));
            data.cities.forEach(function (city) {
                var option = document.createElement("option");
                option.value = city.name;
                option.text = city.name;
                citySelect.appendChild(option);
            });
            // 成功時の処理
            alert("Success");
        },
        error: function (xhr, status, error) {
            // エラー時の処理
            alert("Error: " + error);
        }
    });
}
