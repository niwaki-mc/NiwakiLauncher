using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace NiwakiLauncher.Utils;

/// <summary>
/// The FileHelper class provides methods to work with files.
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Initializes a file if it does not exist.
    /// </summary>
    /// <param name="filePath">The path of the file to initialize.</param>
    public static void InitFileIfNotExists(string filePath)
    {
        string? dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        if (File.Exists(filePath)) return;
        
        FileStream fs = File.Create(filePath);
        fs.Close();
    }

    /// <summary>
    /// Downloads a file from a web server with progress tracking.
    /// </summary>
    /// <param name="baseUrl">The base URL of the web server.</param>
    /// <param name="fileName">The name of the file to download.</param>
    /// <param name="baseLocalPath">The base local path where the file will be saved.</param>
    /// <param name="relativePath">The relative path inside the base local path where the file will be saved.</param>
    /// <param name="reportProgress">An action to report the progress of the download. The action takes two parameters: the number of bytes copied and the total number of bytes.</param>
    /// <returns>A Task representing the completion of the download operation.</returns>
    public static async Task DownloadFileFromWebServerWithProgress(string baseUrl, string fileName,
        string baseLocalPath, string relativePath, Action<long, long> reportProgress)
    {
        string filePath = Path.Combine(baseLocalPath, relativePath, fileName);
        string? dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        using HttpClient client = new HttpClient();
        using HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/files/{fileName}");
        await using Stream contentStream = await (await client.SendAsync(req)).Content.ReadAsStreamAsync(),
            stream = new FileStream(filePath, FileMode.Create,
                FileAccess.Write, FileShare.None, 8192, true);
        long totalBytes = contentStream.Length;
        long bytesCopied = 0L;
        byte[] buffer = new byte[8192];
        int bytesRead;
        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await stream.WriteAsync(buffer, 0, bytesRead);
            bytesCopied += bytesRead;
            reportProgress(bytesCopied, totalBytes);
        }
    }


    /// <summary>
    /// Extracts and deletes a zip file.
    /// </summary>
    /// <param name="basePath">The base path where the zip file is located.</param>
    /// <param name="relPath">The relative path to the zip file.</param>
    /// <param name="zipName">The name of the zip file.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ExtractAndDeleteZip(string basePath, string relPath, string zipName)
    {
        string pathZip = Path.Combine(basePath, relPath, zipName);
        
        
        if (File.Exists(pathZip))
        {
            string extractPath = Path.Combine(basePath, relPath);
            await Task.Run(() => ZipFile.ExtractToDirectory(pathZip, extractPath, true));
        }
        
        File.Delete(pathZip);
    }
}