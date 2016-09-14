$(document).ready(function () {
    //getPetNames("dog");
    //getPetPrice("amstaf");
});







/* ===== event handling ===== */

$('#SelectPetType').on('change', function () {
    getPetNames($(this).val()); 
});

$('#SelectPetName').on('change', function () {
    getPetPrice($(this).val());
});


/*============================*/


/* ===== get pet names ===== */

function getPetNames(petType) {
    $.get("/Shop/GetStage1PetNames?petType=" + petType, function (data) {
        //buildPetNamesDropDownList(data);
        var subItems = "<option value='none'>Select Pet Name</option>";
        $.each(data, function (index, item) {
            subItems += "<option value='" + item.Value + "'>" + item.Text + "</option>"
        });
        $("#SelectPetName").html(subItems)
    });
}




/* ===== Get pet price ===== */

function getPetPrice(petName) {
    $.get("/Shop/GetStage1PetPrice?petName=" + petName, function (data) {
        //buildPetPrice(data);
        //var jsonStr = JSON.parse(data);
        $("#SelectPrice").val(data)
    });
}



