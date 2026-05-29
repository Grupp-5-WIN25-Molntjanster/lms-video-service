using System;
using System.Collections.Generic;
using System.Text;

namespace VideoService.Application.Services;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string blobPath, Stream content, string contentType, CancellationToken ct = default);
    Task<string> GetReadUrlAsync(string blobPath, CancellationToken ct = default);
    Task<bool> DeleteAsync(string blobPath, CancellationToken ct = default);
}
