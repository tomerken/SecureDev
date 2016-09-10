$("#submitBtn").on("click", function (event) {
    event.preventDefault();
    search();
}); 

//search
function search() {
    var petName = $("#petName").val();
    $.get("/home/search?name=" + petName, function (data) {
        buildTablePets(data);
    });
}

//build table of pets 
function buildTablePets(data) {
    if (data != '[]') {
        var innerTable = "<table class='table table-striped table-hover table-bordered'><thead><tr><th>name</th><th>price</th></tr></thead><tbody>";
        var jsonStr = JSON.parse(data);

        for (x in jsonStr) {
            innerTable += "<tr><td>" + jsonStr[x].PetName + "</td><td>" + jsonStr[x].Price + "</td></tr>"

        }
        innerTable += "</tbody></table>";

        $("#petsTable").html(innerTable);
    }
    else
        $("#petsTable").html("No results were found for your search");
}