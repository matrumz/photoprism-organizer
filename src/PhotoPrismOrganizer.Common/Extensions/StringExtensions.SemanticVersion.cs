using NuGet.Versioning;

namespace PhotoPrismOrganizer.Common.Extensions;

public static partial class StringExtensions
{

    public static bool IsValidSemanticVersion(this string? version) =>
        !string.IsNullOrWhiteSpace(version) && SemanticVersion.TryParse(version, out _);

    public static bool IsValidSemanticVersionOrNull(this string? version) =>
        version is null || SemanticVersion.TryParse(version, out _);

    public static SemanticVersion ParseSemanticVersion(this string version) =>
        SemanticVersion.Parse(version);

    public static SemanticVersion? ParseSemanticVersionOrNull(this string? version) =>
        version is null ? null : SemanticVersion.Parse(version);

    public static SemanticVersion ParseSemanticVersionOrDefault(this string? version, string defaultVersion) =>
        SemanticVersion.TryParse(version ?? defaultVersion, out var parsedVersion)
            ? parsedVersion
            : SemanticVersion.Parse(defaultVersion);

}

