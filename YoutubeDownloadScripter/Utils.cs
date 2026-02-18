using System;
using System.Security.Cryptography.X509Certificates;

namespace YoutubeDownloadScripter;

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
