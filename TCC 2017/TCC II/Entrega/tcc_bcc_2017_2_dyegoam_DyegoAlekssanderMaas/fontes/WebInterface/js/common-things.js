var PAGE_URL = "http://" + window.location.hostname +  ":8001/smart-things/plugs/plug-manager";
var BACKEND_URL = "http://" + window.location.hostname +  ":8001/smart-things/plugs";

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if ( (charCode > 31 && charCode < 48) || charCode > 57) {
        return false;
    }
    return true;
}

function _get(url, success) {
  console.log(url);
  $.ajax({
      url: url,
      type: "GET",
      crossDomain: true,
      data: "",
      success: success,
      error: function (xhr, status) {
          console.log(xhr);
          alert("Algo não saiu conforme o esperado...");
      }
  });
}

function _post(url, request, success) {
  console.log(url);
  $.ajax({
      url: url,
      type: "POST",
      crossDomain: true,
      data: request,
      dataType: "json",
      success: success,
      error: function (xhr, status) {
          console.log(xhr);
          alert("Algo não saiu conforme o esperado...");
      }
  });
}

function _renderTemplate(templateId, targetId, viewData, callback) {
  console.log('printing something into ' + targetId);
  var template = $("#" + templateId).html();
  Mustache.parse(template);

  var rendered = Mustache.render(template, viewData);
  $("#" + targetId).append(rendered);

  if (callback)
      callback();
}