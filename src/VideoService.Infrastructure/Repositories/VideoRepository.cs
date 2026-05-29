using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VideoService.Domain.Entities;
using VideoService.Domain.Interfaces;
using VideoService.Infrastructure.Contexts;


namespace VideoService.Infrastructure.Repositories;

public class VideoRepository(VideoDbContext db) : IVideoRepository
{

    public async Task<Video?> GetByIdAsync(int id, CancellationToken ct = default)
        => await db.Videos.FindAsync([id], ct);

    public async Task<Video?> GetByCourseAndLessonAsync(int courseId, int lessonId, CancellationToken ct = default)
        => await db.Videos.FirstOrDefaultAsync(v => v.CourseId == courseId && v.LessonId == lessonId, ct);

    public async Task<List<Video>> GetByCourseIdAsync(int courseId, CancellationToken ct = default)
        => await db.Videos.Where(v => v.CourseId == courseId).ToListAsync(ct);

    public async Task<Video> AddAsync(Video video, CancellationToken ct = default)
    {
        db.Videos.Add(video);
        await db.SaveChangesAsync(ct);
        return video;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var video = await db.Videos.FindAsync([id], ct);
        if (video is null) return false;
        db.Videos.Remove(video);
        await db.SaveChangesAsync(ct);
        return true;
    }
}