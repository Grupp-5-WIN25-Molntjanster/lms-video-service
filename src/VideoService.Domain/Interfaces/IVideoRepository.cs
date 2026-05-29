using System;
using System.Collections.Generic;
using System.Text;
using VideoService.Domain.Entities;


namespace VideoService.Domain.Interfaces;

public interface IVideoRepository
{
    Task<Video?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Video?> GetByCourseAndLessonAsync(int courseId, int lessonId, CancellationToken ct = default);
    Task<Video> AddAsync(Video video, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
