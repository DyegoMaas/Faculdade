require(
    [
        "knockout",
        "knockout-amd-helpers"
    ], 
    function (ko) {
        ko.bindingHandlers.module.baseDir = "modules";
        ko.bindingHandlers.module.templateProperty = "embeddedTemplate";

        ko.amdTemplateEngine.defaultPath = "templates";
    	ko.amdTemplateEngine.defaultSuffix = ".tmpl.html";
    	ko.amdTemplateEngine.defaultRequireTextPluginName = "text";
    }
);