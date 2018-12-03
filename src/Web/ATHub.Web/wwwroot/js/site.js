function initYouTubeModal() {
    $('.video').each(function () {
        let src = $(this).data("src");
        let id = $(this).attr('id');
        $(this).click(function () {
            let modal = $('#myModal');
            modal.on('show.bs.modal', function (e) {
                $('#details').val(id);
                $("#video").attr('src', src + "?rel=0&amp;showinfo=0&amp;modestbranding=1&amp;autoplay=1");
            });
            $('#myModal').on('hide.bs.modal', function (b) {
                $("#video").attr('src', src);
            });
        });

    });
};