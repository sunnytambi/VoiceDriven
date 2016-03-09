//// 2. This code loads the IFrame Player API code asynchronously.
//var tag = document.createElement('script');

//tag.src = "https://www.youtube.com/iframe_api";
//var firstScriptTag = document.getElementsByTagName('script')[0];
//firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);

//// 3. This function creates an <iframe> (and YouTube player)
////    after the API code downloads.
//var player;
//function onYouTubeIframeAPIReady() {
//    player = new YT.Player('player', {
//        height: '390',
//        width: '640',
//        videoId: 'M7lc1UVf-VE',
//        events: {
//            'onReady': onPlayerReady,
//            'onStateChange': onPlayerStateChange
//        }
//    });
//}

//// 4. The API will call this function when the video player is ready.
//function onPlayerReady(event) {
//    event.target.playVideo();
//}

//// 5. The API calls this function when the player's state changes.
////    The function indicates that when playing a video (state=1),
////    the player should play for six seconds and then stop.
//var done = false;
//function onPlayerStateChange(event) {
//    if (event.data == YT.PlayerState.PLAYING && !done) {
//        setTimeout(stopVideo, 6000);
//        done = true;
//    }
//}
//function stopVideo() {
//    player.stopVideo();
//}

// After the API loads, call a function to enable the search box.
function handleAPILoaded() {
    $('#search-button').attr('disabled', false);
}

// Search for a specified string.
function search() {
    var q = $('#query').val();
    var request = gapi.client.youtube.search.list({
        q: q,
        part: 'snippet'
    });

    request.execute(function (response) {
        var str = JSON.stringify(response.result);
        $('#search-container').html('<pre>' + str + '</pre>');
    });
}