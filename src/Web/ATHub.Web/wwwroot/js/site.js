function initYouTubeModal() {
    let done = false;
    let player = createPlayer('videoPlayer', '', onPlayerReady, onPlayerStateChange, '390', '640');
    $('.video').each(function () {
        let src = $(this).data("src");
        let id = $(this).attr('id');
        $(this).click(function () {
            let modal = $('#myModal');
            modal.on('show.bs.modal', () => {
                console.log(player);
                player.loadVideoById(src, 0, "large");
                $('#details').val(id);

            });
            $('#myModal').on('hide.bs.modal', function (b) {
                stopVideo();
            });


        });


    });
    function onPlayerReady(event) {
        console.log('Play video')
        event.target.playVideo();
    }

    function onPlayerStateChange(event) {
        if (event.data == YT.PlayerState.PLAYING && !done) {
            //setTimeout(stopVideo, 6000);
            console.log(event);
            done = true;
        } else if (event.data == YT.PlayerState.ENDED) {
            console.log('Ended event :)');

        }
    }
    function stopVideo() {
        player.stopVideo();
    }
};

function initPlaylistPlayer() {
    let videoCollection = $('.playlistVideo');
    let currentIndex = 0;

    if (videoCollection.length > 0) {

        let player = createPlayer('videoPlayerMyPlaylist', videoCollection.first().data('src'), onPlayerReady, onPlayerStateChange, '400', '500');

        videoCollection.each(function () {
            let videoLink = $(this).data('src');
            let id = $(this).attr('id');
            $(this).click(function () {
                player.loadVideoById(videoLink, 0, "large");
                currentIndex = id;

            });
        });

        function onPlayerReady(event) {

            event.target.playVideo();

        }

        function onPlayerStateChange(event) {
          
            if (event.data == YT.PlayerState.ENDED) {
                let index = ++currentIndex % videoCollection.length;
                console.log(index);
                videoCollection[index].click();
                console.log(videoCollection[index]);
            }
            
        }
    }
    function stopVideo() {
        player.stopVideo();
    }

};

function initDetailsPlayer(videoId) {

    let done = false;
    let player = createPlayer('videoPlayerDetailsPage', videoId, onPlayerReady, onPlayerStateChange, '390', '640');
    //player.loadVideoById(videoId, 0, "large");

    function onPlayerReady(event) {
        
        event.target.playVideo();
    }

    function onPlayerStateChange(event) {
       
    }
    function stopVideo() {
        player.stopVideo();
    }

}
function createPlayer(domElementId, videoId, onReady, onStateChange, height, width) {

    return new YT.Player(domElementId, {
        height: height,
        width: width,
        videoId: videoId,
        events: {
            'onReady': onReady,
            'onStateChange': onStateChange
        }
    });
}

function search() {
    let autocmoplete = new Awesomplete(document.getElementById('search'), { list: [] });
    $('#search').keyup(function () {
        let input = $(this).val();
        if (input.length >= 3) {
            $.ajax({
                url: "/Home/GetSearchValue",
                type: "GET",
                data: { searchParam: $(this).val() },
                success: function (data) {
                    let list = data.map((i) => { return i.name; });
                    autocmoplete.list = list;

                }
            })
        }
    });
}

function newCommentEvent(videObject) {
    if (videObject) {
        let newCommentForm = $('#add_new_comment');
        newCommentForm.on('submit', (e) => {
            e.preventDefault();
            let contentTextArea = $('textarea#content').val();
            console.log(contentTextArea);
            requester('/Videos/Comments/Add', 'POST', { content: contentTextArea, id: videObject }, (data) => {
                if (data.success) {
                    newCommentForm.reset;
                    $('textarea#content').val('');
                    comments(videObject);
                }
            })
        });
    }
}
function comments(videObject) {
    let parrent = $('#all_comment');
    parrent.html('');
    requester(`/Videos/Videos/Comments`, 'GET', { id: videObject }, (data) => {

        console.log($('#loggedInUser').html());
        for (comment of data) {

            parrent.append(`
        <div class="col-sm-8">

            <div class="post-heading">

                <div class="pull-left meta">
                    <div id="uploaderName${comment.id}" class="title h5">
                        <a href="#"><b>${comment.uploaderName}</b></a>
                        made a post.`);
            if (comment.uploaderName + '!' === $('#loggedInUser').html()) {
                $(`#uploaderName${comment.id}`).append(`<a id="${comment.id}_edit"><u>Edit</u></a>
                        <a id="${comment.id}_delete"><u>Delete</u></a>`);
            }
            $(`#uploaderName${comment.id}`).append(`
</div>
                    <h6 class="text-muted time">${comment.date}</h6>
                </div>
            </div>
            <div id="post-description-${comment.id}"class="post-description">
                <p  id="comment-text-${comment.id}">${comment.text}</p>
            </div>
        </div>`);

            let commentId = comment.id;
            $(`#${commentId}_edit`).on('click', () => {
                let text = $(`#comment-text-${commentId}`).text();
                let postDesc = $(`#post-description-${commentId}`);
                postDesc.html(`
        <textarea name="content" id="${commentId}_content" style="width:50%;padding:2%;font-size:1.2em;background-color:silver;" class="form-control" rows="3"></textarea>
        <p class="lead">
            <button id="${commentId}_save" class="btn btn-secondary my-2 my-sm-0">Save</button>
        </p>`);

                $(`#${commentId}_content`).val(text);

                $(`#${commentId}_save`).on('click', () => {
                    let ctn = $(`textarea#${commentId}_content`).val();
                    ;
                    requester('/Videos/Comments/Edit', 'POST', { content: ctn, id: `${commentId}` }, (data) => {

                        comments(videObject);

                    })
                });
            });
            $(`#${commentId}_delete`).on('click', () => {
                requester('/Videos/Comments/Delete', 'POST', { id: `${commentId}` }, (data) => {

                    if (data.success) {
                        comments(videObject);
                    }

                })
            })
        }
    });

}

function initialSideBarEvents() {
    $(".sidebar-dropdown > a").click(function () {
        $(".sidebar-submenu").slideUp(200);
        if ($(this).parent().hasClass("active")) {
            $(".sidebar-dropdown").removeClass("active");
            $(this).parent().removeClass("active");
        } else {
            $(".sidebar-dropdown").removeClass("active");
            $(this).next(".sidebar-submenu").slideDown(200);
            $(this).parent().addClass("active");
        }

    });

    $("#toggle-sidebar").click(function () {
        $(".page-wrapper").toggleClass("toggled");
    });
}

function requester(url, method, data, successFunction) {
    $.ajax({
        url: url,
        type: method,
        data: data,
        success: successFunction,
        error: function (request, status, error) {
            alert("Something went wrong!");
        }

    })
}

function editVideoModalEvent() {
    $('.btn-success').each(function () {
        let id = $(this).attr('id');

        $(this).click(function () {
            requester("/Administrator/Videos/EditVideo", "GET", { 'id': id }, (model) => {
                $('#name').val(model.name);
                $('#link').val(model.link);
                $('#description').val(model.description);
                $('#videoId').val(id);
                let categorySelect = $('#category');
                for (categoryName of model.categoryNames) {
                    if (model.category != categoryName) {
                        categorySelect.append(`<option>${categoryName}</option>`)
                    } else {
                        categorySelect.append(`<option selected=true>${categoryName}</option>`)
                    }
                }
            });

        });
    });
}

function initNewNavBArEvents() {

    var trigger = $('.hamburger'),
        overlay = $('.overlay'),
        isClosed = false;

    trigger.click(function () {
        hamburger_cross();
    });

    function hamburger_cross() {

        if (isClosed == true) {
            overlay.hide();
            trigger.removeClass('is-open');
            trigger.addClass('is-closed');
            isClosed = false;
        } else {
            overlay.show();
            trigger.removeClass('is-closed');
            trigger.addClass('is-open');
            isClosed = true;
        }
    }

    $('[data-toggle="offcanvas"]').click(function () {
        $('#wrapper').toggleClass('toggled');
    });
}