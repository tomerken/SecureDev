$(document).ready(function () {
    getInformation();
});

//search
function getInformation() {
    $.get("/Information/GetInfo", function (data) {
        buildTableInfos(data);
    });
}

//build table of pets 
function buildTableInfos(data) {
    if (data != '[]') {
        var innerTable = "<table class='table table-striped table-hover table-bordered'><thead><tr><th>First name</th><th>Last name</th><th>Pet type</th><th>Pet Name</th></tr></thead><tbody>";
        var jsonStr = JSON.parse(data);

        for (x in jsonStr) {
            innerTable += "<tr><td>" + jsonStr[x].FirstName + "</td><td>" + jsonStr[x].LastName + "</td><td>" + jsonStr[x].PetName + "</td><td>" + jsonStr[x].PetType + "</td></tr>"

        }
        innerTable += "</tbody></table>";

        $("#infoTable").html(innerTable);
    }
    else
        $("#infoTable").html("No results were found for your search");
}