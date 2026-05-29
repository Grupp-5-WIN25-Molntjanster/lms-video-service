using Microsoft.AspNetCore.Mvc;
using VideoService.Application.DTOs;
using VideoService.Application.Services;
namespace VideoService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideosController(VideoManager manager) : ControllerBase
{
    [HttpGet("{id:int}")]
    [EndpointSummary("Get video by ID")]
    [EndpointDescription("Returns video metadata with a SAS URL for streaming")]
    [ProducesResponseType(typeof(VideoResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await manager.GetByIdAsync(id, ct);
        if (result is null) return NotFound();
        return Ok(result);
    }


    [HttpGet("by-course-lesson")]
    [EndpointSummary("Get video by course and lesson")]
    [EndpointDescription("Returns the video for a specific course + lesson combo")]
    [ProducesResponseType(typeof(VideoResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCourseLesson([FromQuery] int courseId, [FromQuery] int lessonId, CancellationToken ct)
    {
        var result = await manager.GetByCourseAndLessonAsync(courseId, lessonId, ct);
        if (result is null) return NotFound();
        return Ok(result);
    }


    [HttpPost]
    [EndpointSummary("Upload a new video")]
    [EndpointDescription("Uploads an MP4 file to Azure Blob Storage and saves metadata")]
    [ProducesResponseType(typeof(VideoResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload([FromForm] VideoUploadRequest request, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest("File is required");
        await using var stream = file.OpenReadStream();
        var result = await manager.UploadAsync(request, stream, file.ContentType, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }


    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete a video")]
    [EndpointDescription("Deletes video from both Blob Storage and database")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await manager.DeleteAsync(id, ct);
        if (!deleted) return NotFound();
        return NoContent();
    }
}