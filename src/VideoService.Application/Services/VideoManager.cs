using VideoService.Application.DTOs;
using VideoService.Domain.Entities;
using VideoService.Domain.Interfaces;


namespace VideoService.Application.Services;

public class VideoManager(IVideoRepository repo, IBlobStorageService blob)
{


    public async Task<VideoResult?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var video = await repo.GetByIdAsync(id, ct);
        if (video is null) return null;
        return await ToResultAsync(video, ct);
    }

    public async Task<VideoResult?> GetByCourseAndLessonAsync(int courseId, int lessonId, CancellationToken ct = default)
    {
        var video = await repo.GetByCourseAndLessonAsync(courseId, lessonId, ct);
        if (video is null) return null;
        return await ToResultAsync(video, ct);
    }

    public async Task<VideoResult> UploadAsync(VideoUploadRequest request, Stream fileStream, string contentType, CancellationToken ct = default)
    {
        var blobPath = $"{request.CourseId}/{Guid.NewGuid():N}.mp4";
        var url = await blob.UploadAsync(blobPath, fileStream, contentType, ct);
        var video = new Video
        {
            Title = request.Title,
            Description = request.Description,
            BlobPath = blobPath,
            ContentType = contentType,
            FileSizeBytes = fileStream.Length,
            CourseId = request.CourseId,
            LessonId = request.LessonId,
            CreatedAt = DateTime.UtcNow
        };
        var saved = await repo.AddAsync(video, ct);
        return new VideoResult(saved.Id, saved.Title, saved.Description, url,
            saved.ContentType, saved.FileSizeBytes, saved.CourseId, saved.LessonId, saved.CreatedAt);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var video = await repo.GetByIdAsync(id, ct);
        if (video is null) return false;
        await blob.DeleteAsync(video.BlobPath, ct);
        return await repo.DeleteAsync(id, ct);
    }

    private async Task<VideoResult> ToResultAsync(Video video, CancellationToken ct = default)
    {
        var url = await blob.GetReadUrlAsync(video.BlobPath, ct);
        return new VideoResult(video.Id, video.Title, video.Description, url,
            video.ContentType, video.FileSizeBytes, video.CourseId, video.LessonId, video.CreatedAt);
    }
}