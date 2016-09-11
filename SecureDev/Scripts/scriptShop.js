$(document).ready(function () {
    getPetTypes();
});



//search
function getPetTypes() {
    $.get("/Shop/GetStage1PetsTypes", function (data) {
        buildDropDownList(data);
    });
}

//build table of pets 
function buildDropDownList(data) {
    if (data != '[]') {
        var innerTable = "<select>";
        var jsonStr = JSON.parse(data);
        alert(jsonStr);
        for (x in jsonStr) {
            innerTable += "<option value=" + jsonStr[x] + ">" + jsonStr[x] + "</option>"

        }
        innerTable += "</select>";

        $("#petTypeDDL").html(innerTable);
    }
    else
        $("#petTypeDDL").html("No results were found for your search");
}