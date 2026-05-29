namespace VideoService.Api.OpenApi;

public static class OpenApiConfiguration
{
    public static IServiceCollection AddVideoOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<VideoDocumentTransformer>();
            options.AddSchemaTransformer<VideoSchemaTransformer>();
        });
        return services;
    }
}