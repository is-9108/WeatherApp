function selectArea() {
    var todoufuken = document.getElementById("todoufuken").value;
    var citySelect = document.getElementById("cities");
    for (var i = citySelect.options.length - 1; i >= 0; i--) {
        citySelect.remove(i);
    }
    $.ajax({
        type: "POST",
        url: "/api/GetCity/getCities",
        data: { id: todoufuken },
        success: function (data) {
            data.cities.forEach((city) => console.log(city));
            data.cities.forEach(function (city) {
                var option = document.createElement("option");
                option.value = city;
                option.text = city;
                citySelect.appendChild(option);
            });
        },
        error: function (xhr, status, error) {
            // エラー時の処理
            alert("Error: " + error);
        }
    });
}
function getLocate() {
    var todoufuken = document.getElementById("todoufuken").value;
    var city = document.getElementById("cities").value;
    $.ajax({
        type: "POST",
        url: "/api/GetCity/getLocate",
        data: { todoufuken:todoufuken, city: city },
        success: function (data) {
            // 成功時の処理
            window.location.href = "/HOME";
        },
        error: function (xhr, status, error) {
            // エラー時の処理
            alert("Error: " + error);
        }
    });
}
