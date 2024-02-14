document.addEventListener('DOMContentLoaded', function () {
    loadData();
}, false);


function loadData() {      
    var text = '{}';    

    fetch('/Academia/Institutions?handler=InstitutionSummary')
        .then((response) => response.json())
        .then((data) => updateMap(data));
    return JSON.parse(text);
}


function updateMap(data)
{
    for (const state in data) {
        simplemaps_usmap_mapdata.state_specific[state] = data[state];
    }
    simplemaps_usmap.load();

}

simplemaps_usmap.hooks.click_state = function (id) {
    console.log(id);
    console.log(simplemaps_usmap_mapdata.state_specific[id].name);
    var state = simplemaps_usmap_mapdata.state_specific[id].name;
    var element = document.getElementById(state);
    var sibDiv = element.nextElementSibling;
    element.ariaExpanded = "true";
    sibDiv.ariaHidden = "false";
    element.scrollIntoView({ behavior: "smooth" });
}
