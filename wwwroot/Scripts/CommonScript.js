
//#region Partial View Loads



function FnDirect(controllerName, viewName, id, idVal) {
    var url = "/" + controllerName + "/" + viewName;
    if (id !== "" && id != undefined) {
        url = url + "?" + id + "=" + idVal;
    }
    window.location.href = url;
}