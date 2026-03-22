using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace Adapter.RestApi.AspNetCore.Authentication;

internal sealed class JwtBearerConfigurationOptions(IConfiguration configuration)
    : IConfigureNamedOptions<JwtBearerOptions>
{
    private const string ConfigurationSectionName = "Authentication";

    public void Configure(JwtBearerOptions options)
    {
        configuration.GetSection(ConfigurationSectionName).Bind(options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}
