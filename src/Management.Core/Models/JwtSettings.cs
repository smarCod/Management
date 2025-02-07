


namespace Management.Core.Models;


public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Key { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}
