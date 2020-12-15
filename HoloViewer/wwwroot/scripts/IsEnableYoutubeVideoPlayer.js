function IsEnableYoutubeVideoPlayer() {
    var video = document.querySelector('video');

    return (video.offsetTop >= 0);
}

IsEnableYoutubeVideoPlayer();
