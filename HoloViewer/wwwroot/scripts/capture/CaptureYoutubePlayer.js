/*async*/ function Capture() {
    var canvas = document.createElement('canvas');
    var video = document.querySelector('video');
    var ctx = canvas.getContext('2d');

    canvas.width = parseInt(video.offsetWidth);
    canvas.height = parseInt(video.offsetHeight);

    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

    return canvas.toDataURL();
}

Capture();
