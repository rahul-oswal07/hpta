using HPTA.Common.Models;

namespace HPTA.Common.Configurations;

public class ApplicationSettings
{
    public string MasterDbConnectionString { get; set; } = null!;

    public string OpenAIUrl { get; set; } = null!;

    public DevCentralConfig DevCentralConfig { get; set; }

    public EmailClientConfig EmailClientConfig { get; set; }
}
