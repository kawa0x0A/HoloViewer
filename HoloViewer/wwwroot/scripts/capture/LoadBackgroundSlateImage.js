var img = document.createElement('img');

function LoadImage() {
    var divElement = document.querySelector('.ytp-offline-slate-background');

    const backgroundImage = (divElement.style && divElement.style.backgroundImage) || '';
    const url = backgroundImage.replace(/^url\(["']?/, '').replace(/["']?\)$/, '');

    img.crossOrigin = "anonymous";
    img.src = url;
}

LoadImage();
