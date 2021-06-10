using System;
using System.Linq;
using Leads.Domain.Contracts.v1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Leads.Library
{
    public static class GlobalExtensionMethods
    {
        /// <summary>
        /// Install services from assembly.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void InstallServicesFromAssemblyV1<T>(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(T).Assembly.ExportedTypes.Where(t => typeof(IInstaller).IsAssignableFrom(t)
                                                            && !t.IsInterface
                                                            && !t.IsAbstract)
                                                            .Select(Activator.CreateInstance)
                                                            .Cast<IInstaller>()
                                                            .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }


        /// <summary>
        /// Convert nullable int to string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToNValue(this int? value)
            => value.HasValue
            ? value.ToString()
            : string.Empty;
    }
}