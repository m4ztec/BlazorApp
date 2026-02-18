using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloadScripter;

public class Video
{
    public static async Task GetVideoInPlaylist(string playlistUrl, string? outputPath = null)
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
    
    public static async Task GetVideo(string videoUrl, string? outputPath = null)
    {
         var youtube = new YoutubeClient();

        var video = await youtube.Videos.GetAsync(videoUrl);

        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id.Value);
        var streamInfo = streamManifest.GetVideoStreams().Where(a => a.Container == Container.Mp4).GetWithHighestVideoQuality();

        var title = video.Title.WithoutInvalidChars();
        var videoFileName = $"{title}.{streamInfo.Container}";
        var filePath = outputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "Data", videoFileName);

        await youtube.Videos.Streams.DownloadAsync(streamInfo, filePath);
    }
}
