using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

//var playlistUrl = "https://www.youtube.com/playlist?list=PL-0Oq4MxhbVDIE677b8Wy1CLCd_kaj57z";
var playlistUrl = "https://www.youtube.com/playlist?list=PL-0Oq4MxhbVB_2LfeABQ78Mjor4InBs6i";

var youtube = new YoutubeClient();

var videosInPlaylist = await youtube.Playlists.GetVideosAsync(playlistUrl);

foreach (var video in videosInPlaylist)
{
    var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id.Value);
    var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
    var title = RemoveInvalidChars(video.Title);
    var videoFileName = $"{title}.{streamInfo.Container}";
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", videoFileName);

    await youtube.Videos.Streams.DownloadAsync(streamInfo, filePath);
}
static string RemoveInvalidChars(string stringToClean)
{
    string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

    foreach (char c in invalid)
    {
        stringToClean = stringToClean.Replace(c.ToString(), "_");
    }
    return stringToClean;
}