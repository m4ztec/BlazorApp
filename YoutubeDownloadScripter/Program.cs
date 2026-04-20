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

    services.AddSingleton<YoutubeDownloader>();
});

var app = builder.Build();

await app.RunAsync();

// --------------------------------------

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