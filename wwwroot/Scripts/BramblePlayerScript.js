$(document).ready(FnLoadStartUpElements());

function FnLoadStartUpElements() {
    FnRenderSongList();
    FnShowDescButton();
}
function FnRenderSongList() {
    $('#recommended-song-list').load("/BramblePlayer/RenderSongList");
}

function FnShowDescButton() {
    var description = document.getElementById('desc-text')
    if ((description).innerText.length <= 38) {
        $('#view-desc-button').hide();
    }
}

function FnResizeDescBox(div, desc1, desc2) {
    var currentDescText = document.getElementById(div[0].id).innerText;
    if (currentDescText === desc1) {
        div[0].innerText = desc2;
    }
    else {
        div[0].innerText = desc1;
    }
}

var commentSection = document.getElementById('comment-input-div')
var commentButton = commentSection.getElementsByTagName(commentSection.children[1].tagName)[0];
commentButton.addEventListener("click", function () {
    FnSubmitCommet();
}, false);


function FnSubmitCommet() {
    var commentInput = commentSection.getElementsByTagName(commentSection.children[0].tagName)[0];
    if (commentInput.value != '') {
        $.ajax({
            type: "POST",
            url: "SubmitComment",
            data: { comment: commentInput.value },
            dataType: "json",
            async: true,
            success: function (response) {
                if (response.success) {
                    commentInput.value = '';
                    commentInput.classList.add('error-text-input-box');
                    commentInput.attributes.getNamedItem("placeholder").value = "Comment posted...!";
                }
                else {
                    commentInput.value = '';
                    commentInput.classList.add('error-text-input-box');
                    commentInput.attributes.getNamedItem("placeholder").value = "Error trying to comment...";
                }
            }
        });
    }
    else {
        $('#' + commentInput.id).prop('placeholder', 'Please input comment...');
        commentInput.classList.add('error-text-input-box');
    }
}
