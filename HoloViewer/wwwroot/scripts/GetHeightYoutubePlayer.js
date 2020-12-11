function GetHeight() {
    var video = document.querySelector('video');
    var height = 0;

    if (video.offsetTop >= 0) {
        // 動画再生中
        height = parseInt(video.offsetHeight);
    }
    else {
        // 配信前などの待機状態
        var divElement = document.querySelector('.ytp-offline-slate-background');

        height = divElement.offsetHeight;
    }

    return height;
}

GetHeight();
