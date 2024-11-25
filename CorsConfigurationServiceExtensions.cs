namespace SigalNET.Gateway
{
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using SigalNET.Gateway.Configuration;

    public static class CorsConfigurationServiceExtensions
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IWebHostEnvironment environment, string basePath)
        {
            ArgumentNullException.ThrowIfNull(environment);

            // Load cors configuration json, no mandatory for startup, since Cors is controlled by runtime code.
            CorsConfiguration? corsConfiguration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("cors.json", true, true)
                .AddJsonFile($"cors.{environment.EnvironmentName}.json", true, true)
                .Build()
                .Get<CorsConfiguration>();

            if (corsConfiguration != null && corsConfiguration.Cors?.Count > 0)
            {
                services.AddSingleton(corsConfiguration);

                foreach (var corsKey in corsConfiguration.Cors)
                {
                    services.AddCors(conf =>
                    {
                        conf.AddPolicy(corsKey.Name, p =>
                        {
                            AddOriginsPolcity(p, corsKey);
                            AddMethodsPolicy(p, corsKey);
                            AddHeadersPolicy(p, corsKey);

                            if (corsKey.ExposedHeaders?.Count > 0)
                            {
                                p.WithExposedHeaders(corsKey.ExposedHeaders.ToArray());
                            }
                        });

                        if (!string.IsNullOrEmpty(corsConfiguration.Default))
                        {
                            conf.DefaultPolicyName = corsConfiguration.Default;
                        }
                    });
                }
            }

            return services;
        }

        public static void UseCorsConfiguration(this WebApplication app)
        {
            ArgumentNullException.ThrowIfNull(app);
            CorsConfiguration? corsConfiguration = app.Services.GetService<CorsConfiguration>();

            if (!string.IsNullOrEmpty(corsConfiguration?.Default))
            {
                app.UseCors(corsConfiguration.Default);
            }
        }

        private static void AddOriginsPolcity(CorsPolicyBuilder policy, CorsKeyConfiguration corsKey)
        {
            if (corsKey.Origins.Count > 0)
            {
                if (corsKey.AllowAnyOrigin())
                {
                    policy.AllowAnyOrigin();
                    return;
                }

                policy.WithOrigins(corsKey.Origins.ToArray());
            }
        }

        private static void AddMethodsPolicy(CorsPolicyBuilder policy, CorsKeyConfiguration corsKey)
        {
            if (corsKey.Methods.Count > 0)
            {
                if (corsKey.AllowAnyMethod())
                {
                    policy.AllowAnyMethod();
                    return;
                }

                policy.WithMethods(corsKey.Methods.ToArray());
            }
        }

        private static void AddHeadersPolicy(CorsPolicyBuilder policy, CorsKeyConfiguration corsKey)
        {
            if (corsKey.Headers.Count > 0)
            {
                if (corsKey.AllowAnyHeader())
                {
                    policy.AllowAnyHeader();
                    return;
                }

                policy.WithHeaders(corsKey.Headers.ToArray());
            }
        }
    }
}
