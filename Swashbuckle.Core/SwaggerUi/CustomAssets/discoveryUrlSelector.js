window.onload = function () {
    if ($('input.download-url-input=') != null) {
        var url = swashbuckleConfig.rootUrl;
        var style = 'style="width:100%;border:2px solid #547f00;font-weight:normal;font-family:Arial;"';
        var select = '<select class="download-url-input" ' + style + ' >';
        swashbuckleConfig.discoveryPaths.forEach(function (path) {
            select += '<option>' + url + "/" + path + '</option>';
        });
        select += '</select>';
        $('.download-url-input').outerHTML = select;
    }
}