var autocomplete = [];
google.maps.event.addDomListener(window, 'load', function () {    
    var addrInput = document.getElementsByClassName('address-autocomplete');
    for (let i = 0; i < addrInput.length; i++) {
        autocomplete[i] = new google.maps.places.Autocomplete(addrInput[i], { types: ['geocode'] });
        if (autocomplete[i] != undefined) {
            autocomplete[i].__elementForm = addrInput[i].form;
            autocomplete[i].setFields(['address_component']);
            autocomplete[i].addListener('place_changed', () => { fillInAddress(i); });
        }
    }
});

function fillInAddress(index) {    
    var place = autocomplete[index].getPlace();
    var __elementForm = autocomplete[index].__elementForm;
    var __formId = "#" + __elementForm.id + " ";
    var street_number = "";
    var route = "";
    for (var i = 0; i < place.address_components.length; i++) {
        var addressType = place.address_components[i].types[0];
        if (addressType == "street_number") {
            street_number = place.address_components[i]["short_name"];
        }
        if (addressType == "route") {
            route = place.address_components[i]["long_name"];
        }
        if (addressType == "locality") {
            var city = place.address_components[i]["long_name"];
            setInputVal(__formId, "city-autocomplete", city);
        }
        if (addressType == "postal_code") {
            var postal_code = place.address_components[i]["long_name"];
            setInputVal(__formId, "zipcode-autocomplete", postal_code);
        }
        if (addressType == "administrative_area_level_1") {
            var state = place.address_components[i]["short_name"];
            var select = $(__formId + ".state-autocomplete");
            select.val(state);
            select.trigger('change');
        }
    }
    var address = street_number == "" ? route : street_number + " " + route;
    setInputVal(__formId, "address-autocomplete", address)
    var input = $(__formId + ".address-autocomplete");
    input.trigger('input');
}

function setInputVal(__formId, inputclassname, value) {
    var input = $(__formId + "." + inputclassname);
    input.val(value);
    input.trigger('input');
}