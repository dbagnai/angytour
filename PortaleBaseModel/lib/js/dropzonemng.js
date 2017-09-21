"use strict";


function onDragOver(e) {
   
    myDZ.css('background-color', 'orange');
}

function onDragEnter(e) {
}

function onDragLeave(e) {
    myDZ.css('background-color', 'lightyellow');
}

function onDrop(e) {
    myflist = {
        generic: [],
        images: [],
        files: []
    }
    myDZ.css('background-color', 'lightyellow');
    e.preventDefault();
}

// re-create dropzone div
//$dropzoneContainer.append($('<div id="dropzone" class="dropzone"></div>'));

// create dropzone
//$dropzoneContainer.find('#dropzone').dropzone({ url: '/upload', paramName: 'file_name' });
var myDZ = null;

var myflist = {
    generic: [],
    images: [],
    files: []
};
var fext = {
    docx: ".docx",
    jpg: ".jpg",
    png: ".png",
    gif: ".gif",
    pdf: ".pdf"
};


function startDropZone() {
     
    var tipologialocal = "";
    var idselectedlocal = "";
    

    if (typeof $("#hidTipologia") !== 'undefined' && $("#hidTipologia").length>0 ) tipologialocal = $("#hidTipologia")[0].value;
    if (typeof $("#hidIdselected") !== 'undefined' && $("#hidIdselected").length > 0) idselectedlocal = $("#hidIdselected")[0].value;

    if (myDZ != null) {
        myDZ = null;
    }
    var dzcontainer = $("#dzContainer");
    dzcontainer.empty();
    dzcontainer.append($('<div id="dZUpload" class="dropzone dropzone-backcloud" style="background-color:#eee;min-height:370px"><div class="dz-default"></div></div>'));
    Dropzone.autoDiscover = false;
   // myDZ = new Dropzone("div#dZUpload", { url: "/file/post"});
    //or $("div#dZUpload").dropzone({ url: "/file/post" });

    myDZ = $("#dZUpload");
    myDZ.dropzone({
        url: "/lib/hnd/filehnd.ashx",
        addRemoveLinks: true,
        dictDefaultMessage: "",
        createImageThumbnails: false,
        previewTemplate: document.getElementById('preview-template').innerHTML,
        parallelUploads: 1,//Attenzione mettendo parallel uploads , vanno asincroni e se arrivano nello stesso momento il db potrebbe essere aggiorato male!
        maxFiles:10,
        //autoQueue : false,
        //acceptedFiles: 'image/*,application/pdf,application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/docx,application/pdf,text/plain,application/msword,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        sending: function (file, xhr, formData) {
            //var detailID = $("#dtID").text();
            //formData.append('detailid', detailID);
            formData.append('tipologia', tipologialocal);
            formData.append('idselected', idselectedlocal);
            formData.append('filepath', file.fullPath);
            formData.append('q', 'store');
            var isValidFile = true;
            var sz = file.name.toLowerCase();
            myflist.generic.push(sz);

            //var szfp = file.fullPath.toLowerCase();
            //if (szfp.includes("undefined")) {
            //    isValid = false;
            //}

            //if (!isValidFile) {
            //    if (myDZ.length)
            //        myDZ[0].dropzone.removeFile(file);
            //}

        },
        uploadprogress: function (file, done) {
            return true;
        },
        complete: function (file, done) {
            return true;
        },
        queuecomplete: function () {
            //alert("Riepilogo files \r\n" +
            //    "generic : " + myflist.generic.join() + "\r\n" 
            //)
            if (typeof aggiornaview !== 'undefined') { aggiornaview(); }


            return true;
        },
        success: function (file, response) {

            var m = response.split('|');
            if (m.length > 0) {
                if (m[0] == "ALERT") {
                    var sErr = "";
                    if (m.length > 1) sErr = m[1];
                    file.previewElement.classList.add("dz-error");
                    alert('Errore caricamento file : ' + sErr);
                    return;
                }
            }
            var imgName = response;
            file.previewElement.classList.add("dz-success");
            console.log("Successfully uploaded :" + imgName);
            //var detailID = $("#dtID").text();
            //refreshJoint(detailID);
            file.previewElement.remove();//TOGLIE L'ELEMENTO CARICATO'

        },
        error: function (file, response) {
            file.previewElement.classList.add("dz-error");
            alert(file.name.toLowerCase() + ': Errore caricamento file  ' + response);
            
           
        }
    });

    //		$("#dZUpload").on("addedfile", function(file) {

    //		}});

    $('div.dz-default.dz-message > span').show();
   // $('div.dz-default.dz-message').css({ 'opacity': 1, 'background-image': 'none' });

    //closeModal();
    myDZ.on('dragover', onDragOver);
    myDZ.on('dragenter', onDragEnter);
    myDZ.on('dragleave', onDragLeave);
    myDZ.on('drop', onDrop);


}
