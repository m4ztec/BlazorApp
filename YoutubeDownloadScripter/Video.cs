using VideoLibrary;
using YoutubeDLSharp;
using YoutubeDLSharp.Options;
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

    public static async Task<RunResult<string>> GetVideo(string videoUrl, string? outputPath = null)
    {
        string ytDlpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "yt-dlp.exe");

        if (!File.Exists(ytDlpPath))
        {
            Console.WriteLine("Downloading yt-dlp...");
            await DownloadYtDlp(ytDlpPath);
        }

        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
Console.WriteLine($"Videos will be saved to: {downloadsPath}");
        var yt = new YoutubeDL
        {
            YoutubeDLPath = ytDlpPath,
            OutputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
            OutputFileTemplate = "%(title)s.%(ext)s"
        };

        var options = new OptionSet
        {
            Format = "bestaudio/best",
            ExtractAudio = true,
            AudioFormat = AudioConversionFormat.Best,
            //Proxy = "https://inv.riverside.rocks",
        AgeLimit = 99,

            //AudioMultistreams  = true
        };
        var hi = await yt.RunVideoDownload(videoUrl, overrideOptions: options);
        var hi2 =  hi.Success;

        return hi; 
    }
    
    static async Task DownloadYtDlp(string savePath)
    {
        string ytDlpUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";

        using var client = new HttpClient();
        var response = await client.GetAsync(ytDlpUrl);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsByteArrayAsync();
        await File.WriteAllBytesAsync(savePath, content);
    }
}
