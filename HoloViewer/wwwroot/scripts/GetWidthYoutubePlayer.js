function GetWidth() {
    var video = document.querySelector('video');
    var width = 0;

    if (video.offsetTop >= 0) {
        // 動画再生中
        width = parseInt(video.offsetWidth);
    }
    else {
        // 配信前などの待機状態
        var divElement = document.querySelector('.ytp-offline-slate-background');

        width = divElement.offsetWidth;
    }

    return width;
}

GetWidth();
