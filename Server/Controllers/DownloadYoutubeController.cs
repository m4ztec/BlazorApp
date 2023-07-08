using Microsoft.AspNetCore.Mvc;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace BlazorApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DownloadYoutubeController : ControllerBase
{
    [HttpGet("GetAudioFile/{videoId}")]
    public async Task<IActionResult> GetAudioFile(string videoId)
    {
        //var videoId = "dQw4w9WgXcQ";
        var youtube = new YoutubeClient();
        var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
        var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
        var video = await youtube.Videos.GetAsync(videoId);
        var videoFileName = $"{video.Title}.{streamInfo.Container}";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", videoFileName);
        await youtube.Videos.Streams.DownloadAsync(streamInfo, filePath);

        var memory = new MemoryStream();
        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        return File(memory, "video/mp4", Path.GetFileName(filePath));
    }

    [HttpGet("GetVideoName/{videoId}")]
    public async Task<IActionResult> GetVideoName(string videoId)
    {
        var youtube = new YoutubeClient();
        var video = await youtube.Videos.GetAsync(videoId);
        return Ok($"{video.Title}.mp4");
    }
}
