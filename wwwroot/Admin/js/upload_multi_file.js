$(document).ready(function () {
    $('#ful').on('change', function () {
        var files = $(this)[0].files;

        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            var fileReader = new FileReader();
            fileReader.onload = (function (fileParams) {
                return function (event) {
                    var str = '<div class="col-sm-4">' +
                        '<img class="img-thumbnail js-file-image" style="width: 100%; height: 100%">' +
                                           
                        '</div>';
                    $('.js-file-list').append(str);
                    var imageSrc = event.target.result;                   
                    $('.js-file-image').last().attr('src', imageSrc);                 
                  
                };
            })(file);
            fileReader.readAsDataURL(file);
        }
    });
});