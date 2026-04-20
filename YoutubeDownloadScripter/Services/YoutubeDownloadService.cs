using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDownloadScripter.Services;

public class YoutubeDownloadService()
{
    private YoutubeClient Client { get; set; } = new YoutubeClient();

    public async Task GetSingleMusicAsync(string videoUrl, string? outputPath = null)
    {
        var video = await Client.Videos.GetAsync(videoUrl);
        var streamManifest = await Client.Videos.Streams.GetManifestAsync(video.Id.Value);
        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
        var title = video.Title.WithoutInvalidChars();
        var videoFileName = $"{title}.{streamInfo.Container}";
        var filePath = outputPath ?? Path.Combine(Directory.GetCurrentDirectory(), videoFileName);

        await Client.Videos.Streams.DownloadAsync(streamInfo, filePath);
    }

    public async Task GetMusicInPlaylistAsync(string playlistUrl, string? outputPath = null)
    {
        var videosInPlaylist = await Client.Playlists.GetVideosAsync(playlistUrl);

        foreach (var video in videosInPlaylist)
        {
            var streamManifest = await Client.Videos.Streams.GetManifestAsync(video.Id.Value);
            var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
            var title = video.Title.WithoutInvalidChars();
            var videoFileName = $"{title}.{streamInfo.Container}";
            var filePath = outputPath ?? Path.Combine(Directory.GetCurrentDirectory(), videoFileName);

            await Client.Videos.Streams.DownloadAsync(streamInfo, filePath);
        }
    }
}
