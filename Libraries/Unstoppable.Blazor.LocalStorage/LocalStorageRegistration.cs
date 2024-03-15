using Microsoft.Extensions.DependencyInjection;

namespace Unstoppable.Blazor;

public static class LocalStorageRegistration
{
  public static void AddLocalStorage(this IServiceCollection services) => services.AddScoped<LocalStorage>();
}