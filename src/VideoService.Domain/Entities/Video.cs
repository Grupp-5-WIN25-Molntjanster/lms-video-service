using System;
using System.Collections.Generic;
using System.Text;

namespace VideoService.Domain.Entities;

public class Video
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string BlobPath { get; set; } = string.Empty;
    public string ContentType { get; set; } = "video/mp4";
    public long FileSizeBytes { get; set; }
    public int CourseId { get; set; }
    public int LessonId { get; set; }
    public DateTime CreatedAt { get; set; }
}