function Capture() {
    var canvas = document.createElement('canvas');
    var ctx = canvas.getContext('2d');

    var divElement = document.querySelector('.ytp-offline-slate-background');

    canvas.width = divElement.offsetWidth;
    canvas.height = divElement.offsetHeight;

    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);

    return canvas.toDataURL();
}

Capture();
