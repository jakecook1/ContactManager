const cloudName = 'dvqkgk1y6';
const unsignedUploadPreset = 'wx6loj9o';

$("#uploadFile").click(function(e) {
    e.preventDefault();
    $("#fileUploader").click();
});

var handleFile = function(files) {
    for (var i = 0; i < files.length; i++) {
        uploadFile(files[i]);
    }
};

$(".deleteImage").on('click', function(e) {
    e.preventDefault();
    $(this).parents("div.img-wrapper:first").hide();
    applyDeletedImage();
});

function uploadFile(file) {
    var formData = getFormData(file);

    $.ajax({
        xhr: function(){ return addEvents(); },
        url: `https://api.cloudinary.com/v1_1/${cloudName}/image/upload`,
        data: formData,
        processData: false,
        contentType: false,
        type: 'POST',
        beforeSend: function(){ $(".progress").removeClass("d-none"); },
        complete: function(){ $(".progress").addClass("d-none"); }
    }).done(function(res) {
        let secureUrl = res.secure_url;

        let publicId = res.public_id;
        let version = res.version;

        addImage(secureUrl, publicId);
        applyDetails(publicId, version);
    });
}

function getFormData(file) {
    var fd = new FormData();

    fd.append('upload_preset', unsignedUploadPreset);
    fd.append('tags', 'browser_upload'); // Optional - add tag for image admin in Cloudinary
    fd.append('file', file);

    return fd;
}

function addEvents() {
    var xhr = new window.XMLHttpRequest();

    //Upload progress
    xhr.upload.addEventListener("progress", function (e) {
        var progress = Math.round((e.loaded * 100.0) / e.total);
        $("#progress").width(`${progress}%`);
    }, false);

    return xhr;
}

function applyDetails(publicId, version) {
    $("#UploadDetails").val(publicId + "#$%" + version);
}

function applyDeletedImage() {
    $("#DeletedImage").val("true");
}

function addImage(secureUrl, publicId) {
    var img = getImage(secureUrl, publicId);

    var divWrapper = document.createElement('div');
    divWrapper.className = "img-wrapper p-1";

    divWrapper.appendChild(img);

    $("#gallery").html(divWrapper);
}

function getImage(secureUrl, public_id) {
    // Create a thumbnail of the uploaded image, with 150px width
    var tokens = secureUrl.split('/');
    tokens.splice(-2, 0, 'h_100,c_scale');

    var img = new Image();

    img.src = tokens.join('/');
    img.alt = public_id;
    img.className = "img-fluid img-thumbnail";

    return img;
}