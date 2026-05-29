using System;
using System.Collections.Generic;
using System.Text;

namespace VideoService.Application.DTOs;

public record VideoResult(
    int Id,
    string Title,
    string? Description,
    string Url,
    string ContentType,
    long FileSizeBytes,
    int CourseId,
    int LessonId,
    DateTime CreatedAt
);
