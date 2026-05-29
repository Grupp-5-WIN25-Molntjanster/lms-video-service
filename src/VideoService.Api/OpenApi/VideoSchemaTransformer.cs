using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using System.Text.Json.Nodes;
using VideoService.Application.DTOs;
namespace VideoService.Api.OpenApi;

public class VideoSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;
        if (type == typeof(VideoResult))
        {
            schema.Example = new JsonObject
            {
                ["id"] = 1,
                ["title"] = "Introduction to Algebra",
                ["description"] = "First lesson in the algebra course",
                ["url"] = "https://...blob...?.sv=...&se=...",
                ["contentType"] = "video/mp4",
                ["fileSizeBytes"] = 52428800,
                ["courseId"] = 5,
                ["lessonId"] = 3,
                ["createdAt"] = "2026-05-29T12:00:00Z"
            };
        }
        else if (type == typeof(VideoUploadRequest))
        {
            schema.Example = new JsonObject
            {
                ["title"] = "Introduction to Algebra",
                ["description"] = "First lesson in the algebra course",
                ["courseId"] = 5,
                ["lessonId"] = 3
            };
        }
        return Task.CompletedTask;
    }
}