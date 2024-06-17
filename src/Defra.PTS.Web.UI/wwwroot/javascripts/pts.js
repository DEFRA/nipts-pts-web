//
// For guidance on how to add JavaScript see:
// https://prototype-kit.service.gov.uk/docs/adding-css-javascript-and-images
//

$(document).ready(function () {

    const breedDDL = document.querySelector('#SelectedBreed');
    if (breedDDL) {
        accessibleAutocomplete.enhanceSelectElement({
            selectElement: document.querySelector('#SelectedBreed'),
            autoselect: true,
            showNoOptionsFound: false,
            minLength: 2,
        });
    }

    initializeShared();
    initializePetMicrochip();
    initializeNumberOnlyFields()
});

// initializeShared()
function initializeShared() {
    // maxLength for number input types
    document.querySelectorAll('input[type=number]').forEach(input => {
        input.oninput = (event) => {
            if (input.value.length > input.maxLength) {
                input.value = input.value.slice(0, input.maxLength);
            }
        };
    });

}

// PetMicrochip
function initializePetMicrochip() {
    var $IdentificationTypeRadio = $('input[type=radio][name=Microchipped]');
    if ($IdentificationTypeRadio.is(':checked')) {
        handleMicrochipOptiondChange($('input[type=radio][name=Microchipped]:checked').val());
    }
    else {
        handleMicrochipOptiondChange('');
    }

    $('input[type=radio][name=Microchipped]').change(function () {
        handleMicrochipOptiondChange(this.value);
    });
}

function handleMicrochipOptiondChange(selValue) {

    if (selValue == 'Yes') {
        $("#conditional-MicrochipNumber").show();
    }
    else {
        $("#conditional-MicrochipNumber").hide();
    }
}

// Evidence
$("#UploadButtonContainer").hide();
$("#fileToUploadError").hide();

// PetBreed
var $BreedTypeRadio = $('input[type=radio][name=BreedType]');
if ($BreedTypeRadio.is(':checked')) {
    handleBreedTypeChange($('input[type=radio][name=BreedType]:checked').val());
}
else {
    handleBreedTypeChange('');
}

$('input[type=radio][name=BreedType]').change(function () {
    handleBreedTypeChange(this.value);
});
function handleBreedTypeChange(selValue) {

    if (selValue == 'PurebredOrPedigree') {
        $("#conditional-PurebredOrPedigree").show();
        $("#conditional-MixedBreedOrUnknown").hide();
    }
    else if (selValue == 'MixedBreedOrUnknown') {
        $("#conditional-PurebredOrPedigree").hide();
        $("#conditional-MixedBreedOrUnknown").show();
    }
    else {
        $("#conditional-PurebredOrPedigree").hide();
        $("#conditional-MixedBreedOrUnknown").hide();
    }
}

// PetFeature
var $HasUniqueFeatureRadio = $('input[type=radio][name=HasUniqueFeature]');
if ($HasUniqueFeatureRadio.is(':checked')) {
    handleHasUniqueFeatureChange($('input[type=radio][name=HasUniqueFeature]:checked').val());
}
else {
    handleHasUniqueFeatureChange('');
}

$('input[type=radio][name=HasUniqueFeature]').change(function () {
    handleHasUniqueFeatureChange(this.value);
});
function handleHasUniqueFeatureChange(selValue) {

    if (selValue == 'Yes') {
        $("#conditional-FeatureDescription").show();
    }
    else if (selValue == 'No') {
        $("#conditional-FeatureDescription").hide();
    }
    else {
        $("#conditional-FeatureDescription").hide();
    }
}

// PetAge
var $KnowDoBRadio = $('input[type=radio][name=KnowDoB]');
if ($KnowDoBRadio.is(':checked')) {
    handleKnowDoBChange($('input[type=radio][name=KnowDoB]:checked').val());
}
else {
    handleKnowDoBChange('');
}

$('input[type=radio][name=KnowDoB]').change(function () {
    handleKnowDoBChange(this.value);
});
function handleKnowDoBChange(selValue) {

    if (selValue == 'Yes') {
        $("#conditional-DateOfBirth").show();
        $("#conditional-Age").hide();
    }
    else if (selValue == 'No') {
        $("#conditional-DateOfBirth").hide();
        $("#conditional-Age").show();
    }
    else {
        $("#conditional-DateOfBirth").hide();
        $("#conditional-Age").hide();
    }
}


function initializeNumberOnlyFields() {
    // Allow users to enter numbers only 
    $(".numeric-only").bind('keypress', function (e) {
        if (e.keyCode == '9' || e.keyCode == '16') {
            return;
        }
        var code;
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;
        if (e.which == 46)
            return false;
        if (code == 8 || code == 46)
            return true;
        if (code < 48 || code > 57)
            return false;
    });

    // Disable paste
    $(".numeric-only").bind("paste", function (e) {
        e.preventDefault();
    });

    $(".numeric-only").bind('mouseenter', function (e) {
        var val = $(this).val();
        if (val != '0') {
            val = val.replace(/[^0-9]+/g, "")
            $(this).val(val);
        }
    });
}

//locale
function setLocaleEn() {
    var name = "locale";
    var value = "en";
    var now = new Date();
    now.setTime(now.getTime + (1 * 60 * 60 * 1000));
    document.cookie = name + "=" + value + ";expires " + now.toUTCString() + ";path=/";
}

function setLocaleCy() {
    var name = "locale";
    var value = "cy";
    var now = new Date();
    now.setTime(now.getTime + (1 * 60 * 60 * 1000));
    document.cookie = name + "=" + value + ";expires " + now.toUTCString() + ";path=/";
}

$("#localeEn").on("click", function () {
    setLocaleEn();
});

$("#localeCy").on("click", function () {
    setLocaleCy();
});