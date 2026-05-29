using System;
using System.Collections.Generic;
using System.Text;

namespace VideoService.Application.DTOs;

public record VideoUploadRequest(
    string Title,
    string? Description,
    int CourseId,
    int LessonId
);
