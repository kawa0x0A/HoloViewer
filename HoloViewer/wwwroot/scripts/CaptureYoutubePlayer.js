/*async*/ function Capture() {
    var canvas = document.createElement('canvas');
    var video = document.querySelector('video');
    var ctx = canvas.getContext('2d');

    if (video.offsetTop >= 0) {
        // 動画再生中
        canvas.width = parseInt(video.offsetWidth);
        canvas.height = parseInt(video.offsetHeight);

        ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
    }
    else {
        // 配信前などの待機状態
        var divElement = document.querySelector('.ytp-offline-slate-background');

        canvas.width = divElement.offsetWidth;
        canvas.height = divElement.offsetHeight;

        const backgroundImage = (divElement.style && divElement.style.backgroundImage) || '';
        const url = backgroundImage.replace(/^url\(["']?/, '').replace(/["']?\)$/, '');

        var img = document.createElement('img');

        img.crossOrigin = "anonymous";
        img.src = url;

        // 画像の読み込みが完了してからキャンバスに描画 (読み込み待ちをしないと描画がキャンバスに反映されない)
        // webview2からのスクリプト呼び出しがawaitで終わってしまう問題を解決できないのでコメントアウト
        //await new Promise(resolve => { const id = setInterval(() => { if (img.complete) { resolve(); alert("load"); clearInterval(id); } }, 100) });

        ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
    }

    return canvas.toDataURL();
}

Capture();
