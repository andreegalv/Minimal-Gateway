namespace SigalNET.Gateway.Authentication
{
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using SigalNET.Gateway.Secret;

    public class ConfigureJwtBearerOptions : IConfigureOptions<JwtBearerOptions>
    {
        public ConfigureJwtBearerOptions(IJwtAuthenticationFile authFile, ISecretConfigurationFile secretFile, IHostEnvironment hostEnviroment)
        {
            this._authFile = authFile;
            this._secretFile = secretFile;
            this._hostEnviroment = hostEnviroment;
        }

        private readonly IJwtAuthenticationFile _authFile;

        private readonly ISecretConfigurationFile _secretFile;

        private readonly IHostEnvironment _hostEnviroment;

        public void Configure(JwtBearerOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            options.RequireHttpsMetadata = this._hostEnviroment.IsProduction();
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = this._authFile.Issuer,
                ValidAudience = this._authFile.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._secretFile.Jwt)),
            };
        }
    }
}
