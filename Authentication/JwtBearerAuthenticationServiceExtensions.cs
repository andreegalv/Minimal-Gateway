namespace SigalNET.Gateway.Authentication
{
    public static class JwtBearerAuthenticationServiceExtensions
    {
        public static IServiceCollection AddJwtBearerAuthenticationServices(this IServiceCollection services)
        {
            services.AddAuthentication()
                    .AddJwtBearer();
            services.AddOptions<ConfigureJwtBearerOptions>();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AuthorizedUser", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
            });

            return services;
        }
    }
}
