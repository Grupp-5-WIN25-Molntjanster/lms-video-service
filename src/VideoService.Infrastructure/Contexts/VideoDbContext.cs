using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VideoService.Domain.Entities;


namespace VideoService.Infrastructure.Contexts;

public class VideoDbContext(DbContextOptions<VideoDbContext> options) : DbContext(options)
{
    public DbSet<Video> Videos => Set<Video>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Title).HasMaxLength(200).IsRequired();
            entity.Property(v => v.BlobPath).HasMaxLength(500).IsRequired();
            entity.Property(v => v.ContentType).HasMaxLength(50).IsRequired();
            
            // One video per lesson
            entity.HasIndex(v => new { v.CourseId, v.LessonId }).IsUnique();
        });
    }
}