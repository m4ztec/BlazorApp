using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;
using Microsoft.Extensions.Hosting;
using RazorConsole.Core;
using YoutubeDownloadScripter.Components;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args)
    .UseRazorConsole<YoutubeDownloader>();

builder.ConfigureServices(services =>
{
    services.Configure<ConsoleAppOptions>(options =>
    {
        options.AutoClearConsole = false;
        options.EnableTerminalResizing = true;
    });
});

var app = builder.Build();

await app.RunAsync();

// --------------------------------------

static async Task GetMusicInPlaylist(string playlistUrl, string? outputPath = null)
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

static async Task GetVideoInPlaylist(string playlistUrl, string? outputPath = null)
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
    
public static class Utils
{
    extension(string stringToClean)
    {
        public string WithoutInvalidChars()
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                stringToClean = stringToClean.Replace(c.ToString(), "_");
            }
            return stringToClean;
        }
    }
}