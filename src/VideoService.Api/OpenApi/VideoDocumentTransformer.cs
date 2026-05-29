using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
namespace VideoService.Api.OpenApi;

public class VideoDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Video API",
            Version = "v1",
            Description = "An API for managing course videos stored in Azure Blob Storage.",
            Contact = new OpenApiContact
            {
                Name = "Video API Support",
                Url = new Uri("https://github.com/alnils"),
            }
        };
        return Task.CompletedTask;
    }
}