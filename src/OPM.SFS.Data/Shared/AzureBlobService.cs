using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Shared
{
    public interface IAzureBlobService
    {
        public Task<string> UploadDocumentStreamAsync(IFormFile theFile, int userID, string userType);
        public Task<FileStreamResult> DownloadDocumentStreamAsync(string filepath, string filename);
        public Task DeleteDocumentAsync(string filepath);
        public Task<string> CheckStorageMalwareTags(string filename);
    }

    public class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration _appSettings;
        private readonly ILogger<AzureBlobService> _logger;
        private readonly ScholarshipForServiceContext _efDB;

        public AzureBlobService(IConfiguration appSettings, ILogger<AzureBlobService> logger, IServiceProvider serviceProvider)
        {
            _appSettings = appSettings;
            _logger = logger;
            _efDB = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ScholarshipForServiceContext>(); 
        }

        public async Task DeleteDocumentAsync(string filepath)
        {
            var blobServiceClient = new BlobServiceClient(
                    new Uri(_appSettings["Azure:BlobServiceUri"]),
                      new DefaultAzureCredential());

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_appSettings["Azure:BlobContainerName"]);
            BlobClient blobClient = containerClient.GetBlobClient(filepath);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<FileStreamResult> DownloadDocumentStreamAsync(string filepath, string filename)
        {
            var blobServiceClient = new BlobServiceClient(
                    new Uri(_appSettings["Azure:BlobServiceUri"]),
                      new DefaultAzureCredential());

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_appSettings["Azure:BlobContainerName"]);
            BlobClient blobClient = containerClient.GetBlobClient(filepath);
            var docStream = await blobClient.OpenReadAsync();
            var theFile = new FileStreamResult(docStream, GetMimeType(Path.GetExtension(filepath)));
            theFile.FileDownloadName = $"{filename}{Path.GetExtension(filepath)}";
            return theFile;
        }

        public async Task<string> UploadDocumentStreamAsync(IFormFile theFile, int userID, string userType)
        {
            var blobServiceClient = new BlobServiceClient(
                    new Uri(_appSettings["Azure:BlobServiceUri"]),
                      new DefaultAzureCredential());

            var fileName = Path.GetFileName(theFile.FileName);
            var fileType = Path.GetExtension(fileName);
            var newFilePath = $"{GetSubFolder()}/{Convert.ToString(Guid.NewGuid())}{fileType}";

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_appSettings["Azure:BlobContainerName"]);
            containerClient.CreateIfNotExists();

            BlobClient blobClient = containerClient.GetBlobClient(newFilePath);
            using Stream stream = theFile.OpenReadStream();
            await blobClient.UploadAsync(stream);
            await QueueForScanResult(newFilePath, userType, userID);
            return newFilePath;
        }

        public async Task<string> CheckStorageMalwareTags(string filename)
        {
            StringBuilder result = new StringBuilder();
            var blobServiceClient = new BlobServiceClient(
                    new Uri(_appSettings["Azure:BlobServiceUri"]),
                      new DefaultAzureCredential());
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_appSettings["Azure:BlobContainerName"]);
            BlobClient blobClient = containerClient.GetBlobClient(filename);
            try
            {
                Response<GetBlobTagResult> tagsResponse = await blobClient.GetTagsAsync();
                foreach (KeyValuePair<string, string> tag in tagsResponse.Value.Tags)
                {                   
                    _logger.LogInformation($"AzureStorage: Tags for {filename} - {tag.Key}={tag.Value}");
                    result.Append($"{tag.Key}-{tag.Value}");
                }
                return result.ToString();              
            }
            catch ( Exception ex )
            {
                _logger.LogError($"AzureStorage: Error getting blob index tags: {ex.Message}");
            }
            return result.ToString();
        }

        private async Task QueueForScanResult(string filepath, string userType, int userID)
        {
            _efDB.DocumentScanQueue.Add(new Data.DocumentScanQueue() { DateQueued = DateTime.UtcNow, FilePath = filepath, QueuedBy = $"{userType}-{userID}" });
            await _efDB.SaveChangesAsync();
        }

        private string GetSubFolder()
        {
            Random rand = new Random();
            int subfolder = rand.Next(0, 20);
            return subfolder.ToString();
        }

        public string GetMimeType(string filetype)
        {
            switch (filetype.ToLower().Replace(".", ""))
            {
                case "pdf":
                    return "application/pdf";
                case "doc":
                    return "application/msword";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
                case "jpg":
                    return "image/jpeg";
                case "pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case "ppt":
                    return "application/vnd.ms-powerpoint";
                default:
                    break;
            }
            return "";
        }
    }
}
