$(document).ready(function () {
    getPetTypes();
    getPetNames("Dog");
    getPetPrice("Amstaf");
    zero();
});

/* ===== event handling ===== */

$('#petTypes').on('change', function () {
    getPetNames($(this).val()); 
});

$('#petNames').on('change', function () {
    getPetPrice($(this).val());
});

$('#Amount').on('change', function () {
    updateTotalPrice($(this).val());
});

/*============================*/



/* ===== get pet types ===== */

function getPetTypes() {
    $.get("/Shop/GetStage1PetTypes", function (data) {
        buildPetTypesDropDownList(data);
        zero();
    });
}

//build ddl of pets 
function buildPetTypesDropDownList(data) {
    if (data != '[]') {
        var innerTable = "";
        var jsonStr = JSON.parse(data);
        for (x in jsonStr) {
            innerTable += "<option value=" + jsonStr[x] + ">" + jsonStr[x] + "</option>"
        }

        $("#petTypes").html(innerTable);
    }
    else
        $("#petTypes").html("No results were found for your search");
}

/*===========================*/



/* ===== get pet names ===== */

function getPetNames(petType) {
    $.get("/Shop/GetStage1PetNames?petType=" + petType, function (data) {
        buildPetNamesDropDownList(data);
        zero();
    });
}

function buildPetNamesDropDownList(data) {
    if (data != '[]') {
        var innerTable = "";
        var jsonStr = JSON.parse(data);
        var i = 0;
        for (x in jsonStr) {
            //to take the first value of the petNames ddl
            if (i == 0){
                i++;
                getPetPrice(jsonStr[x]);
            }
            
            innerTable += "<option value=" + jsonStr[x] + ">" + jsonStr[x] + "</option>"

        }

        $("#petNames").html(innerTable);
    }
    else
        $("#petNames").html("No results were found for your search");
}

/*===========================*/



/* ===== Get pet price ===== */

function getPetPrice(petName) {
    $.get("/Shop/GetStage1PetPrice?petName=" + petName, function (data) {
        buildPetPrice(data);        
        zero();
    });
}

function buildPetPrice(data) {
    var jsonStr = JSON.parse(data);
    $("#price").text(jsonStr[0]);
}

/* ========================= */


/* ======= GENERAL ========= */

function updateTotalPrice(amount) {
    //get price
    var price = $("#price").text();
    //multiple the amount with the price
    var totalPrice = amount * price;
    if (totalPrice < 0)
        $("#totalPrice").text(0);
    else
        //insert the value into the label of total price
        $("#totalPrice").text(totalPrice);
}

function zero() {
    updateTotalPrice(0);
    $("#Amount").val(0);
}