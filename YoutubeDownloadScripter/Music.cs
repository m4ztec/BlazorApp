using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloadScripter;

public static class Music
{
    public static async Task GetMusicInPlaylist(string playlistUrl, string? outputPath = null)
    {
        var youtube = new YoutubeClient();

        var videosInPlaylist = await youtube.Playlists.GetVideosAsync(playlistUrl);

        foreach (var video in videosInPlaylist)
        {
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id.Value);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            var title = video.Title.WithoutInvalidChars();
            var videoFileName = $"{title}.{streamInfo.Container}";
            var filePath = outputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "Data", videoFileName);

            await youtube.Videos.Streams.DownloadAsync(streamInfo, filePath);
        }
    }
}
