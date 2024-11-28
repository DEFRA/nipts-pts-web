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
    initializeNumberOnlyFields();
    checkCookie();
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
    var name = ".AspNetCore.Culture";
    var value = "c=en-GB|uic=en-GB";
    var now = new Date();
    now.setTime(now.getTime + 1000*36000);
    document.cookie = name + "=" + value + ";expires " + now.toUTCString() + ";path=/";
}

function setLocaleCy() {
    var name = ".AspNetCore.Culture";
    var value = "c=cy|uic=cy";
    var now = new Date();
    now.setTime(now.getTime + 1000 * 36000);
    document.cookie = name + "=" + value + ";expires " + now.toUTCString() + ";path=/";
}

$("#localeEn").on("click", function () {
    setLocaleEn();
});

$("#localeCy").on("click", function () {
    setLocaleCy();
});

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function printWithStyles() {
    // Remove any existing print styles
    const existingStyles = document.getElementById('print-specific-styles');
    if (existingStyles) {
        existingStyles.remove();
    }

    // Create style element
    const printStyles = document.createElement('style');
    printStyles.setAttribute('type', 'text/css');
    printStyles.setAttribute('id', 'print-specific-styles');

    const printCss = `
        @media print {
            /* Base reset */
            * {
                -webkit-print-color-adjust: exact !important;
                print-color-adjust: exact !important;
                color-adjust: exact !important;
            }

            /* Hide print nav */
            .pet-print-download-nav {
                display: none !important;
            }

            /* Global box sizing */
            .print-section {
                box-sizing: border-box !important;
                margin-bottom: 15px !important;
                page-break-inside: avoid !important;
                background-color: white !important;
            }

            /* Application section */
            .print-section.application {
                border: 1px solid #000000 !important;
                width: 100% !important;
                padding: 10px !important;
            }

            /* Side by side container */
            .print-row {
                display: flex !important;
                justify-content: space-between !important;
                gap: 20px !important;
                margin-bottom: 15px !important;
                width: 100% !important;
            }

            /* Microchip and Pet details sections */
            .print-section.microchip,
            .print-section.pet-details {
                border: 1px solid #000000 !important;
                flex: 1 !important;
                padding: 10px !important;
            }

            /* Pet owner section */
            .print-section.owner {
                border: 1px solid #000000 !important;
                width: 100% !important;
                padding: 10px !important;
            }

            /* Firefox specific fixes */
            @-moz-document url-prefix() {
                .print-section {
                    position: relative !important;
                }

                .print-section::after {
                    content: '' !important;
                    position: absolute !important;
                    top: 0 !important;
                    left: 0 !important;
                    right: 0 !important;
                    bottom: 0 !important;
                    border: 1px solid #000000 !important;
                    pointer-events: none !important;
                }

                /* Ensure flex layout works in Firefox */
                .print-row {
                    display: -moz-box !important;
                    -moz-box-pack: justify !important;
                }

                .print-section.microchip,
                .print-section.pet-details {
                    -moz-box-flex: 1 !important;
                    width: 48% !important;
                }
            }

            /* Header styles */
            .print-section h3 {
                margin-top: 0 !important;
                margin-bottom: 10px !important;
            }

            /* Content alignment */
            .govuk-grid-column-full {
                padding: 0 !important;
            }
        }
    `;

    // Add styles
    if (printStyles.styleSheet) {
        printStyles.styleSheet.cssText = printCss;
    } else {
        printStyles.appendChild(document.createTextNode(printCss));
    }

    // Add to document
    document.head.appendChild(printStyles);

    // Function to restructure content
    function restructureForPrint() {
        const sections = document.querySelectorAll('.govuk-grid-row');
        let row = null;

        sections.forEach(section => {
            const heading = section.querySelector('h3')?.textContent?.toLowerCase() || '';

            // Skip if already processed
            if (section.classList.contains('print-section')) return;

            if (heading.includes('application')) {
                section.classList.add('print-section', 'application');
            }
            else if (heading.includes('microchip')) {
                if (!row) {
                    row = document.createElement('div');
                    row.className = 'print-row';
                    section.parentNode.insertBefore(row, section);
                }
                section.classList.add('print-section', 'microchip');
                row.appendChild(section);
            }
            else if (heading.includes('pet details')) {
                section.classList.add('print-section', 'pet-details');
                if (row) {
                    row.appendChild(section);
                }
            }
            else if (heading.includes('owner')) {
                section.classList.add('print-section', 'owner');
            }
        });
    }

    // Apply structure
    restructureForPrint();

    // Print with delay
    setTimeout(() => {
        window.print();
    }, 250);

    // Cleanup after printing
    setTimeout(() => {
        printStyles.remove();
    }, 2000);
}

function checkCookie() {
    let culture = getCookie(".AspNetCore.Culture");
    if (culture == "") {
        setLocaleEn();
    } 
}

/**
 * Initialise component
 *
 * Radios can be associated with a 'conditionally revealed' content block – for
 * example, a radio for 'Phone' could reveal an additional form field for the
 * user to enter their phone number.
 *
 * These associations are made using a `data-aria-controls` attribute, which is
 * promoted to an aria-controls attribute during initialisation.
 *
 * We also need to restore the state of any conditional reveals on the page (for
 * example if the user has navigated back), and set up event handlers to keep
 * the reveal in sync with the radio state.
 */
Radios.prototype.init = function () {
    // Check that required elements are present
    if (!this.$module || !this.$inputs) {
        return
    }

    var $module = this.$module;
    var $inputs = this.$inputs;

    nodeListForEach($inputs, function ($input) {
        var targetId = $input.getAttribute('data-aria-controls');

        // Skip radios without data-aria-controls attributes, or where the
        // target element does not exist.
        if (!targetId || !document.getElementById(targetId)) {
            return
        }

        // Promote the data-aria-controls attribute to a aria-controls attribute
        // so that the relationship is exposed in the AOM
        $input.setAttribute('aria-controls', targetId);
        $input.removeAttribute('data-aria-controls');
    });

    // When the page is restored after navigating 'back' in some browsers the
    // state of form controls is not restored until *after* the DOMContentLoaded
    // event is fired, so we need to sync after the pageshow event in browsers
    // that support it.
    window.addEventListener(
        'onpageshow' in window ? 'pageshow' : 'DOMContentLoaded',
        this.syncAllConditionalReveals.bind(this)
    );

    // Although we've set up handlers to sync state on the pageshow or
    // DOMContentLoaded event, init could be called after those events have fired,
    // for example if they are added to the page dynamically, so sync now too.
    this.syncAllConditionalReveals();

    // Handle events
    $module.addEventListener('click', this.handleClick.bind(this));
};

/**
 * Sync the conditional reveal states for all radio buttons in this $module.
 *
 * @deprecated Will be made private in v5.0
 */
Radios.prototype.syncAllConditionalReveals = function () {
    nodeListForEach(this.$inputs, this.syncConditionalRevealWithInputState.bind(this));
};

/**
 * Sync conditional reveal with the input state
 *
 * Synchronise the visibility of the conditional reveal, and its accessible
 * state, with the input's checked state.
 *
 * @deprecated Will be made private in v5.0
 * @param {HTMLInputElement} $input - Radio input
 */
Radios.prototype.syncConditionalRevealWithInputState = function ($input) {
    var targetId = $input.getAttribute('aria-controls');
    if (!targetId) {
        return
    }

    var $target = document.getElementById(targetId);
    if ($target && $target.classList.contains('govuk-radios__conditional')) {
        var inputIsChecked = $input.checked;

        $input.setAttribute('aria-expanded', inputIsChecked.toString());
        $target.classList.toggle('govuk-radios__conditional--hidden', !inputIsChecked);
    }
};

/**
 * Click event handler
 *
 * Handle a click within the $module – if the click occurred on a radio, sync
 * the state of the conditional reveal for all radio buttons in the same form
 * with the same name (because checking one radio could have un-checked a radio
 * in another $module)
 *
 * @deprecated Will be made private in v5.0
 * @param {MouseEvent} event - Click event
 */
Radios.prototype.handleClick = function (event) {
    var $component = this;
    var $clickedInput = event.target;

    // Ignore clicks on things that aren't radio buttons
    if (!($clickedInput instanceof HTMLInputElement) || $clickedInput.type !== 'radio') {
        return
    }

    // We only need to consider radios with conditional reveals, which will have
    // aria-controls attributes.
    var $allInputs = document.querySelectorAll('input[type="radio"][aria-controls]');

    var $clickedInputForm = $clickedInput.form;
    var $clickedInputName = $clickedInput.name;

    nodeListForEach($allInputs, function ($input) {
        var hasSameFormOwner = $input.form === $clickedInputForm;
        var hasSameName = $input.name === $clickedInputName;

        if (hasSameName && hasSameFormOwner) {
            $component.syncConditionalRevealWithInputState($input);
        }
    });
};