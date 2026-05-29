using System;
using System.Collections.Generic;
using System.Text;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using VideoService.Application.Services;



namespace VideoService.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobContainerClient _container;

    public BlobStorageService(IConfiguration config)
    {
        var conn = config["Azure:BlobStorage:ConnectionString"]
            ?? throw new InvalidOperationException("Azure:BlobStorage:ConnectionString is not configured");
        var containerName = config["Azure:BlobStorage:ContainerName"] ?? "course-videos";
        _container = new BlobContainerClient(conn, containerName);
        _container.CreateIfNotExists();
    }

    public async Task<string> UploadAsync(string blobPath, Stream content, string contentType, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(blobPath);
        await blob.UploadAsync(content, overwrite: true, ct);
        // Set content type
        await blob.SetHttpHeadersAsync(new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = contentType }, cancellationToken: ct);
        return await GetReadUrlAsync(blobPath, ct);
    }

    public Task<string> GetReadUrlAsync(string blobPath, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(blobPath);
        var sas = blob.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
        return Task.FromResult(sas.ToString());
    }

    public async Task<bool> DeleteAsync(string blobPath, CancellationToken ct = default)
    {
        var blob = _container.GetBlobClient(blobPath);
        return await blob.DeleteIfExistsAsync(cancellationToken: ct);
    }
}
