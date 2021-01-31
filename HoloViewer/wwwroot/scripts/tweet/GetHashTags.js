function GetHashTags() {
    let aTags = document.querySelectorAll("a");
    let hashTags = Array();
    let hashTag = "https://www.youtube.com/hashtag/";

    for (let i = 0; i < aTags.length; i++) {
        if (aTags[i].href.startsWith(hashTag)) {
            hashTags.push(aTags[i].href.substring(hashTag.length));
        }
    }

    return hashTags;
}

GetHashTags();
