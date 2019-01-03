function initYouTubeModal() {
    $('.video').each(function () {
        let src = $(this).data("src");
        let id = $(this).attr('id');

        $(this).click(function () {
            let modal = $('#myModal');
            console.log(`src: ${src} id: ${id}`)
            modal.on('show.bs.modal', () => {

                $('#details').val(id);
                $("#video").attr('src', src + "?rel=0&amp;showinfo=0&amp;modestbranding=1&amp;autoplay=1");
            });
            $('#myModal').on('hide.bs.modal', function (b) {

                $("#video").attr('src', src);
            });
        });

    });
};

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
        success: successFunction

    })
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