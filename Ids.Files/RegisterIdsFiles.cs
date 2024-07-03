using Ids.Files.Foundations;
using Microsoft.Extensions.DependencyInjection;

namespace Ids.Files;

public static class RegisterIdsFiles
{
    public static IServiceCollection AddFileService(this IServiceCollection services,
        string connectionStringName)
    {
        services.AddScoped<IFileService>(s => new FileService(connectionStringName));
        return services;
    }
}