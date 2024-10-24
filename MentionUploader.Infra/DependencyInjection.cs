using MentionUploader.Application.Interfaces;
using MentionUploader.Infra.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace MentionUploader.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services)
    {
        services.AddScoped<IFileUploader, GcpFileUploader>();
        return services;
    }
}